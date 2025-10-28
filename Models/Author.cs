using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class Author : ICloneable
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Author name is required.")]
        [StringLength(100, ErrorMessage = "Author name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Biography cannot be longer than 500 characters.")]
        public string Biography { get; set; }

        [StringLength(50, ErrorMessage = "Nationality cannot be longer than 50 characters.")]
        public string Nationality { get; set; }

        public virtual ICollection<Book> Books { get; set; } = new List<Book>();

        public object Clone()
        {
            var clone = new Author
            {
                Id = this.Id,
                Name = this.Name,
                Biography = this.Biography,
                Nationality = this.Nationality
            };

            return clone;
        }
    }
}