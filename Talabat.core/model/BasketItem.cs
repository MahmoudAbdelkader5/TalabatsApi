using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.core.model
{
    public class BasketItem
    {

        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Picture URL is required")]
        public string PictureUrl { get; set; } = string.Empty;
        [Required(ErrorMessage = "Brand is required")]

        public string Brand { get; set; } = string.Empty;
        [Required(ErrorMessage = "Type is required")]
        public string Type { get; set; } = string.Empty;
        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
    }
}
