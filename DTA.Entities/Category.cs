using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DTA.Entities
{
    public class Category : IModificationHistory
    {

        public int CategoryId { get; set; }
        public string Title { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime DateCreated { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime DateModified { get; set; }
        [NotMapped]
        public bool IsDirty { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
