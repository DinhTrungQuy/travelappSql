using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TravelApp.EntityFrameworkCore;
using TravelAppAPI.Models;

namespace TravelAppAPI.Services
{
    public class RatingServices
    {
        private readonly TravelAppDbContext _context;

        public RatingServices(TravelAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Rating>> GetAsync()
        {
            return await _context.Ratings
                .Include(r => r.User)
                .Include(r => r.Place)
                .ToListAsync();
        }

        public async Task<List<Rating>> GetByPlaceIdAsync(string placeId)
        {
            return await _context.Ratings
                .Include(r => r.User)
                .Include(r => r.Place)
                .Where(r => r.Place.PlaceId == placeId)
                .ToListAsync();
        }

        public async Task<Rating> InsertAsync(Rating rating)
        {
            await _context.Ratings.AddAsync(rating);
            await _context.SaveChangesAsync();
            return rating;
        }

        public async Task UpdateAsync(string id, Rating ratingIn)
        {
            var rating = await _context.Ratings.FindAsync(id);
            if (rating != null)
            {
                _context.Entry(rating).CurrentValues.SetValues(ratingIn);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveAsync(Rating ratingIn)
        {
            var rating = await _context.Ratings.FindAsync(ratingIn.Id);
            if (rating != null)
            {
                _context.Ratings.Remove(rating);
                await _context.SaveChangesAsync();
            }
        }
    }
}
