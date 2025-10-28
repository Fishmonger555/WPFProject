using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class Publisher : ICloneable
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Publisher name is required.")]
        [StringLength(150, ErrorMessage = "Publisher name cannot be longer than 150 characters.")]
        public string Name { get; set; }

        [StringLength(200, ErrorMessage = "Address cannot be longer than 200 characters.")]
        public string Address { get; set; }

        [StringLength(100, ErrorMessage = "Contact email cannot be longer than 100 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string ContactEmail { get; set; }

        public virtual ICollection<Book> Books { get; set; } = new List<Book>();

        public object Clone()
        {
            return new Publisher
            {
                Id = this.Id,
                Name = this.Name,
                Address = this.Address,
                ContactEmail = this.ContactEmail
            };
        }
    }
}