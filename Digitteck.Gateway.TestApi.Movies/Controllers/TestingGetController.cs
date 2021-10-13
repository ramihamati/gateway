using Digitteck.Gateway.TestApi.Movies.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Digitteck.Gateway.TestApi.Movies.Controllers
{
    [ApiController]
    [Route("api/TestingGet")]
    public class TestingGetController : ControllerBase
    {
        public static List<Person> Persons = new List<Person>
        {
            new Person{FirstName = "John", LastName = "Doe"},
            new Person{FirstName = "Anna", LastName = "Doe"}
        };
        public const string exceptionMessage = "Hey, this is not implemented";

        [HttpGet("All")]
        public List<Person> GetAll()
        {
            return Persons;
        }

        [HttpGet("LastName/{lastName}")]
        public List<Person> GetPersonsByFamilyName(string lastName)
        {
            return Persons.Where(x => x.LastName == lastName).ToList();
        }

        /// <summary>
        /// When the config file contains the full query
        ///   Upstream   ->"Template": "/testingget/Query?firstName={firstName}&lastName={lastName}",
        ///   Downstream -> "Template": "/api/TestingGet/Query?firstName={firstName}&lastName={lastName}"
        ///   Request    -> "/api/TestingGet/Query?firstName=John&lastName=Doe"
        /// </summary>
        [HttpGet("Query")]
        public List<Person> GetPersonWithQuery([FromQuery(Name = "LastName")]string lastName, [FromQuery(Name = "FirstName")]string firstName)
        {
            return Persons.Where(x => x.FirstName == firstName && x.LastName == lastName).ToList();
        }

        /// <summary>
        /// When the config file contains the partial query
        ///   Upstream   ->"Template": "/testingget/Query?firstName={firstName}",
        ///   Downstream -> "Template": "/api/TestingGet/Query?firstName={firstName}"
        ///   Request    -> "/api/TestingGet/Query?firstName=John&lastName=Doe"
        /// </summary>
        [HttpGet("Query2")]
        public List<Person> GetPersonWithQuery2([FromQuery(Name = "LastName")]string lastName, [FromQuery(Name = "FirstName")]string firstName)
        {
            return Persons.Where(x => x.FirstName == firstName && x.LastName == lastName).ToList();
        }

        /// <summary>
        /// When the config file contains the partial query
        ///   Upstream   ->"Template": "/testingget/ThrowsException"
        ///   Downstream -> "Template": "/api/TestingGet/ThrowsException"
        ///   Request    -> "/api/TestingGet/ThrowsException"
        /// </summary>
        [HttpGet("ThrowsException")]
        public List<Person> GetThrowsException()
        {
            throw new NotImplementedException(exceptionMessage);
        }

        /// <summary>
        /// When the config file contains the partial query
        ///   Upstream   -> "Template": "/testingget/ReturnsNull"
        ///   Downstream -> "Template": "/api/TestingGet/ReturnsNull"
        ///   Request    -> "/api/TestingGet/ReturnsNull"
        /// </summary>
        [HttpGet("ReturnsNull")]
        public List<Person> GetReturnsNull()
        {
            return null;
        }
    }
}
