using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace RefactorThis.Core.Models
{
    public class Product
    {
        public Product()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }

        [Required]
        [StringLength(17)]
        public string Name { get; set; }

        [Required]
        [StringLength(35)]
        public string Description { get; set; }

        [Required]
        [Range(1, (double)decimal.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [Range(1, (double)decimal.MaxValue)]
        public decimal DeliveryPrice { get; set; }

        public void Create()
        {
            Id = Guid.NewGuid();
        }
    }
}