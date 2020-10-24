using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheAdvertiser.Data;

namespace TheAdvertiser.Models
{
    public class AdvertiserRepository : IAdvertiserRepository
    {
        private readonly ApplicationDbContext appDbContext;

        public AdvertiserRepository(ApplicationDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<Advertisement> GetAd(string uid)
        {
            var ad = await appDbContext.Advertisements.SingleOrDefaultAsync(ad => ad.UID == uid);
            return ad;
        }

        public async Task<IEnumerable<Advertisement>> GetAllAds()
        {
            var ads = await appDbContext.Advertisements.OrderByDescending(ad => ad.Created).ToListAsync();
            return ads;
        }

        public async Task PutAd(Advertisement ad)
        {
            appDbContext.Advertisements.Add(ad);
            await appDbContext.SaveChangesAsync();
        }
    }
}
