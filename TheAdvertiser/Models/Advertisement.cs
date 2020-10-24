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
        [Display(Name = "Name of Advertisement")]
        public string Name { get; set; }

        [Required]
        [Range(0, 999999)]
        [Display(Name = "Price")]
        public int Price { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Receiving option")]
        public ParcelReceivingOption ReceivingOption { get; set; }

        public DateTime Created { get; set; }
    }
}
