﻿using Microsoft.AspNetCore.Mvc;
using TravelAppAPI.Models;
using TravelAppAPI.Models.Config;
using TravelAppAPI.Models.Dto;
using TravelAppAPI.Services;

namespace TravelAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class RatingController(RatingServices ratingServices, PlaceServices placeServices, BookingServices bookingServices) : ControllerBase
    {

        private readonly RatingServices _ratingServices = ratingServices;
        private readonly PlaceServices _placeServices = placeServices;
        private readonly BookingServices _bookingServices = bookingServices;


        [HttpGet]
        public async Task<ActionResult<string>> GetRating()
        {
            var ratingList = await _ratingServices.GetAsync();
            return Ok(ratingList);
        }
        [HttpGet("{placeId:length(36)}")]
        public async Task<ActionResult<string>> GetRating(string placeId)
        {
            var ratingList = await _ratingServices.GetByPlaceIdAsync(placeId);
            if (ratingList == null)
            {
                return BadRequest("No rating found");
            }
            var rateValue = ratingList.Average(r => r.RatingValue);

            return Ok(rateValue.ToString());
        }
        [HttpPost("{bookingId:length(36)}")]
        public async Task<ActionResult<Rating>> PostRating(RatingInputDto ratingIn, string bookingId)
        {
            var mapper = MapperConfig.Initialize();
            var ratingList = await _ratingServices.GetByPlaceIdAsync(ratingIn.PlaceId);
            var booking = await _bookingServices.GetAsync(bookingId);
            var rating = mapper.Map<Rating>(ratingIn);
            rating.User = booking.User;
            rating.Place = booking.Place;
            booking.Status = 3;
            ratingList.Add(rating);
            var rateValue = Math.Round(ratingList.Average(r => r.RatingValue), 1);
            await _bookingServices.UpdateAsync(bookingId, booking);
            await _ratingServices.InsertAsync(rating);
            await _placeServices.UpdateRating(rating.Place.PlaceId, rateValue);
            return Ok(rating);
        }


    }
}
