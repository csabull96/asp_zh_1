using System.ComponentModel.DataAnnotations;

namespace TheAdvertiser.Models
{
    public enum ParcelReceivingOption
    {
        [Display(Name = "Delivery")]
        D2D,
        [Display(Name = "Fox post")]
        FoxPost,
        [Display(Name = "F2F pick up")]
        F2F
    }
}
