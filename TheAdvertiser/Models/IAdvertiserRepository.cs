using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheAdvertiser.Models
{
    public interface IAdvertiserRepository
    {
        Task<Advertisement> GetAd(string uid);
        Task<IEnumerable<Advertisement>> GetAllAds();
        Task PutAd(Advertisement ad);
    }
}
