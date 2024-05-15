using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelApp.EntityFrameworkCore;
using TravelAppAPI.Models;

namespace TravelAppAPI.Services
{
    public class WishlistServices
    {
        private readonly TravelAppDbContext _context;

        public WishlistServices(TravelAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Wishlist>> GetAsync()
        {
            return await _context.Wishlists
                .Include(w => w.User)
                .Include(w => w.Place)
                .ToListAsync();
        }

        public async Task<List<Wishlist>> GetAsync(string userId)
        {
            return await _context.Wishlists
                .Include(w => w.User)
                .Include(w => w.Place)
                .Where(w => w.User.UserId == userId)
                .ToListAsync();
        }

        public async Task<Wishlist> CheckExist(string userId, string placeId)
        {
            return await _context.Wishlists
                .Where(w => w.User.UserId == userId && w.Place.PlaceId == placeId)
                .FirstOrDefaultAsync();
        }

        public async Task<Wishlist> CreateAsync(Wishlist wishlist)
        {
            _context.Wishlists.Add(wishlist);
            await _context.SaveChangesAsync();
            return wishlist;
        }

        public async Task RemoveAsync(Wishlist wishlistIn)
        {
            _context.Wishlists.Remove(wishlistIn);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(string userId, string placeId)
        {
            var wishlist = await _context.Wishlists
                .FirstOrDefaultAsync(w => w.User.UserId == userId && w.Place.PlaceId == placeId);
            if (wishlist != null)
            {
                _context.Wishlists.Remove(wishlist);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveByPlaceIdAsync(string placeId)
        {
            var wishlists = _context.Wishlists.Where(w => w.Place.PlaceId == placeId);
            _context.Wishlists.RemoveRange(wishlists);
            await _context.SaveChangesAsync();
        }
    }
}
