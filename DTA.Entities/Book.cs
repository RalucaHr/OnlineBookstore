using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DTA.Entities
{
    public class Book : IModificationHistory
    {
        public int BookId { get; set; }
        public string Cover { get; set; }
        public string Title { get; set; }
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public int ISBN { get; set; }

        [Display(Name = "Year Published")]
        public int YearPublished { get; set; }
        public string Language { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime DateCreated { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime DateModified { get; set; }
        [NotMapped]
        public bool IsDirty { get; set; }


        public int AuthorId { get; set; }

        //[Required]
        public virtual Author Author { get; set; }
        //[Required]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
