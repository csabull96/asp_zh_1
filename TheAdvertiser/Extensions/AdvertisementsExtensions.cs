using System.Collections.Generic;
using System.Linq;
using TheAdvertiser.Models;

namespace TheAdvertiser.Extensions
{
    public static class AdvertisementsExtensions
    {
        public static IEnumerable<Advertisement> ApplyDiscount(this IEnumerable<Advertisement> ads, int discountPercentage)
        {
            double multiplier = (double)(100 - discountPercentage) / 100;

            return ads.Select(ad =>
            {
                ad.Price = (int)(ad.Price * multiplier);
                return ad;
            });
        }
    }
}
