using System;
using System.ComponentModel.DataAnnotations;

namespace TheAdvertiser.Models
{
    public class Advertisement
    {
        [Key]
        public string UID { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        [Required]
        [Range(0, 999999)]
        public int Price { get; set; }
        [Required]
        [StringLength(100)]
        public string Email { get; set; }
        [Required]
        [StringLength(200)]
        public string City { get; set; }
        [Required]
        public ParcelReceivingOption ReceivingOption { get; set; }
    }
}
