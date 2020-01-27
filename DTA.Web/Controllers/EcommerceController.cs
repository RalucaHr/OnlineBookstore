using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DTA.Entities;
using DTA.Services;
using DTA.Web.Models.Ecommerce;
using PayPal.Api;

namespace DTA.Web.Controllers
{
    public class EcommerceController : Controller
    {
        private GenericRepository<Book> db = new GenericRepository<Book>();


        public ActionResult Index()
        {
            var model = new IndexViewModel()
            {
                Books = db.GetAll().ToList()
            };

            return View(model);
        }


        public ActionResult Cart()
        {
            var cart = CreateOrGetCart();

            return View(cart);
        }



        public ActionResult Add(int bookId, byte? param, int? quantity = 1)
        {
            var book = db.GetAll().FirstOrDefault(x => x.BookId == bookId);
            var cart = CreateOrGetCart();
            var existingItem = cart.CartItems.FirstOrDefault(x => x.BookId == book.BookId);
            if (existingItem != null)
            {
                existingItem.Quantity += (int)quantity;
            }
            else
            {
                cart.CartItems.Add(new CartItem()
                {
                    BookId = book.BookId,
                    Cover = book.Cover,
                    Title = book.Title,
                    Price = book.Price,
                    Quantity = (int)quantity,
                });

            }
            SaveCart(cart);
            if (param == 1)
                return RedirectToAction("Details", "Books", new { id = bookId });
            else
                return RedirectToAction("Index", "Books");
        }

        public ActionResult Delete(int bookId)
        {
            var book = db.GetAll().FirstOrDefault(x => x.BookId == bookId);
            var cart = CreateOrGetCart();
            var existingItem = cart.CartItems.FirstOrDefault(x => x.BookId == book.BookId);

            if (existingItem != null)
            {
                cart.CartItems.Remove(existingItem);
            }

            SaveCart(cart);

            return RedirectToAction("Cart", "Ecommerce");
        }

        public ActionResult Checkout()
        {
            var cart = CreateOrGetCart();

            if (cart.CartItems.Any())
            {
                int shipping = 2;
                var taxRate = 0;

                var subtotal = cart.CartItems.Sum(x => x.Price * x.Quantity);
                var tax = taxRate;
                var total = subtotal + shipping + tax;

                var order = new Entities.Order()
                {
                    OrderDate = DateTime.UtcNow,
                    Subtotal = subtotal,
                    Shipping = shipping,
                    Tax = tax,
                    Total = total,
                    OrderItems = cart.CartItems.Select(x => new OrderItem()
                    {
                        Title = x.Title,
                        Cover = x.Cover,
                        Price = x.Price,
                        Quantity = x.Quantity

                    }).ToList()
                };

                GenericRepository<Entities.Order> dorderDb = new GenericRepository<Entities.Order>();
                dorderDb.Insert(order);
                dorderDb.Save();

                var apiContext = GetApiContext();

                var payment = new Payment
                {
                    intent = "sale",
                    payer = new Payer
                    {
                        payment_method = "paypal"
                    },
                    transactions = new List<Transaction>
                    {
                        new Transaction
                        {
                            description = "Bookstor Shopping Cart",
                            amount = new Amount
                            {
                                currency = "USD",
                                total = (order.Total).ToString(),
                                details = new Details()
                                {
                                    subtotal = order.Subtotal.ToString(),
                                    shipping = order.Shipping.ToString(),
                                    tax = order.Tax.ToString()
                                }
                            },
                            item_list = new ItemList()
                            {
                                items = order.OrderItems.Select(x => new Item()
                                {
                                    description = x.Title,
                                    currency = "USD",
                                    quantity = x.Quantity.ToString(),
                                    price = x.Price.ToString()

                                }).ToList()
                            }
                        }
                    },
                    redirect_urls = new RedirectUrls
                    {
                        return_url = Url.Action("Return", "Ecommerce", null, Request.Url.Scheme),
                        cancel_url = Url.Action("Cancel", "Ecommerce", null, Request.Url.Scheme)

                    }
                };


                var createdPayment = payment.Create(apiContext);

                order.PayPalReference = createdPayment.id;
                dorderDb.Save();

                var approvalUrl = createdPayment.links.FirstOrDefault(
                        x => x.rel.Equals("approval_url", StringComparison.OrdinalIgnoreCase));

                return Redirect(approvalUrl.href);
            }

            return RedirectToAction("Cart");
        }

        public ActionResult Return(string payerId, string paymentId)
        {
            var order = new GenericRepository<Entities.Order>().GetAll().FirstOrDefault(x => x.PayPalReference == paymentId);

            var apiContext = GetApiContext();

            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };

            var payment = new Payment()
            {
                id = paymentId
            };

            var executedPayment = payment.Execute(apiContext, paymentExecution);

            ClearCart();

            return RedirectToAction("Thankyou");

        }

        public ActionResult Cancel()
        {
            return View();
        }

        public ActionResult ThankYou()
        {
            return View();
        }

        private Cart CreateOrGetCart()
        {
            Cart cart = Session["Cart"] as Cart;

            if (cart == null)
            {
                cart = new Cart();
                SaveCart(cart);
            }

            return cart;
        }

        private void ClearCart()
        {
            var cart = new Cart();
            SaveCart(cart);
        }

        private void SaveCart(Cart cart)
        {
            Session["Cart"] = cart;
        }

        private APIContext GetApiContext()
        {
            var config = ConfigManager.Instance.GetProperties();
            var accessToken = new OAuthTokenCredential(config).GetAccessToken();
            var apiContext = new APIContext(accessToken);
            return apiContext;
        }
    }
}
