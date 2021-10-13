using System.Collections.Generic;
using Digitteck.Gateway.TestApi.Movies.Models;
using Microsoft.AspNetCore.Mvc;

namespace Digitteck.Gateway.TestApi.Movies.Controllers
{
    [ApiController]
    [Route("api/Movies")]
    public class MoviesControllers : ControllerBase
    {
        public static List<Movie> movies = new List<Movie>
        {
            new Movie{ MovieName = "AmericanGangster" , Rating = 9.7}
        };

        [HttpGet]
        public IEnumerable<Movie> Get()
        {
            return movies;
        }

        //[HttpGet("{movieName}")]
        //public Movie Get(string movieName)
        //{
        //    return movies.Find(x => x.MovieName == movieName);
        //}

        [HttpGet("{movieName}")]
        public Movie GetMovie(string movieName, double rating)
        {
            return movies.Find(x => x.MovieName == movieName && x.Rating == rating);
        }
    }
}
