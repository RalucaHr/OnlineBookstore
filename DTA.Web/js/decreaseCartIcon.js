
//alert("loaded");
function updateCartIcon(bookId) {
    var x = document.getElementById(bookId);
    if (Number(sessionStorage.clickcount) > 0) {
        sessionStorage.clickcount = (sessionStorage.clickcount) - x.value;
    }
    document.getElementById("CartIcon").innerHTML = sessionStorage.clickcount;
    alert("hello");
};