using TravelApp.EntityFrameworkCore;
using TravelAppAPI.Model;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TravelAppAPI.Services
{
    public class PlaceServices
    {
        private readonly TravelAppDbContext _context;

        public PlaceServices(TravelAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Place>> GetAsync()
        {
            return await _context.Places.ToListAsync();
        }

        public async Task<Place> GetAsync(string id)
        {
            return await _context.Places.FirstOrDefaultAsync(place => place.PlaceId == id);
        }

        public async Task<Place> CreateAsync(Place place)
        {
            _context.Places.Add(place);
            await _context.SaveChangesAsync();
            return place;
        }

        public async Task UpdateAsync(string id, Place placeIn)
        {
            var place = await GetAsync(id);
            if (place != null)
            {
                _context.Entry(place).CurrentValues.SetValues(placeIn);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateRating(string placeId, double ratingValue)
        {
            var place = await GetAsync(placeId);
            if (place != null)
            {
                place.Rating = ratingValue.ToString();
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveAsync(Place placeIn)
        {
            _context.Places.Remove(placeIn);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(string id)
        {
            var place = await GetAsync(id);
            if (place != null)
            {
                _context.Places.Remove(place);
                await _context.SaveChangesAsync();
            }
        }
    }
}
