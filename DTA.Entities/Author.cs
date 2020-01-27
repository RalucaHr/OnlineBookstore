using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DTA.Entities
{
    public class Author : IModificationHistory
    {
        public int AuthorId { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime DateCreated { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime DateModified { get; set; }
        [NotMapped]
        public bool IsDirty { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
