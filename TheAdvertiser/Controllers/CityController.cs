using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TheAdvertiser.Models;

namespace TheAdvertiser.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly IAdvertiserRepository advertiserRepository;

        public CityController(IAdvertiserRepository advertiserRepository)
        {
            this.advertiserRepository = advertiserRepository;
        }
        
        [HttpGet("{city}")]
        public string GetBestMatch(string city)
        {
            var match = advertiserRepository
                .GetCities(city)
                .FirstOrDefault();
            return match ?? string.Empty;
        }
    }
}
