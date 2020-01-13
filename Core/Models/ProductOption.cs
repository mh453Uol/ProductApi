using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace RefactorThis.Core.Models
{
    public class ProductOption
    {
        public Guid Id { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        [StringLength(9)]
        public string Name { get; set; }

        [Required]
        [StringLength(23)]
        public string Description { get; set; }
        public void Create()
        {
            Id = Guid.NewGuid();
        }
    }
}