using Digitteck.Gateway.TestApi.Movies.Controllers;
using Digitteck.Gateway.TestApi.Movies.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IntegrationTests
{
    public class TestingGetTests
    {
        //gate api
        private GatewayFactory webApplicationFactory;
        //movies api

        private HttpClient gateClient;

        [SetUp]
        public void Setup()
        {
            this.webApplicationFactory = new GatewayFactory();
            this.gateClient = this.webApplicationFactory.CreateClient();
        }

        [TearDown]
        public void TearDown()
        {
            this.gateClient?.Dispose();
        }

        [Test]
        public async Task get_all_should_work()
        {
            HttpClient client = this.webApplicationFactory.CreateClient();
            HttpResponseMessage response = await client.GetAsync("/testingget/all");
            string strResponse = await response.Content.ReadAsStringAsync();
            List<Person> persons = JsonConvert.DeserializeObject<List<Person>>(strResponse);
            Assert.IsTrue(Person.StructuredEquals(TestingGetController.Persons, persons));
        }

        [Test]
        public async Task get_list_with_path_placeholder()
        {
            //mathing /testingget/LastName/Doe
            HttpClient client = this.webApplicationFactory.CreateClient();
            HttpResponseMessage response = await client.GetAsync("/testingget/LastName/Doe");
            string strResponse = await response.Content.ReadAsStringAsync();
            List<Person> matchingList = JsonConvert.DeserializeObject<List<Person>>(strResponse);
            List<Person> toMatchList = TestingGetController.Persons.Where(x => x.LastName == "Doe").ToList();
            Assert.IsTrue(Person.StructuredEquals(toMatchList, matchingList));
        }

        /// <summary>
        /// When the config file contains the full query
        ///   Upstream   ->"Template": "/testingget/Query?firstName={firstName}&lastName={lastName}",
        ///   Downstream -> "Template": "/api/TestingGet/Query?firstName={firstName}&lastName={lastName}"
        ///   Request    -> "/api/TestingGet/Query?firstName=John&lastName=Doe"
        /// </summary>
        [Test]
        public async Task get_list_with_query()
        {
            //mathing /testingget/Query?firstName=John&lastName=Doe
            HttpClient client = this.webApplicationFactory.CreateClient();
            HttpResponseMessage response = await client.GetAsync("/testingget/Query?firstName=John&LastName=Doe");
            string strResponse = await response.Content.ReadAsStringAsync();
            List<Person> matchingList = JsonConvert.DeserializeObject<List<Person>>(strResponse);
            List<Person> toMatchList = TestingGetController.Persons.Where(x => x.LastName == "Doe" && x.FirstName == "John").ToList();
            Assert.IsTrue(Person.StructuredEquals(toMatchList, matchingList));
        }

        /// <summary>
        /// When the config file contains the partial query
        ///   Upstream   ->"Template": "/testingget/Query?firstName={firstName}"
        ///   Downstream -> "Template": "/api/TestingGet/Query?firstName={firstName}"
        ///   Request    -> "/api/TestingGet/Query?firstName=John&lastName=Doe"
        /// </summary>
        [Test]
        public async Task get_list_with_partial_query()
        {
            //mathing /testingget/Query?firstName=John&lastName=Doe
            HttpClient client = this.webApplicationFactory.CreateClient();
            HttpResponseMessage response = await client.GetAsync("/testingget/Query2?firstName=John&LastName=Doe");
            string strResponse = await response.Content.ReadAsStringAsync();
            List<Person> matchingList = JsonConvert.DeserializeObject<List<Person>>(strResponse);
            List<Person> toMatchList = TestingGetController.Persons.Where(x => x.LastName == "Doe" && x.FirstName == "John").ToList();
            Assert.IsTrue(Person.StructuredEquals(toMatchList, matchingList));
        }

        /// <summary>
        /// When the config file contains the partial query
        ///   Upstream   ->"Template": "/testingget/ThrowsException"
        ///   Downstream -> "Template": "/api/TestingGet/ThrowsException"
        ///   Request    -> "/api/TestingGet/ThrowsException"
        /// </summary>
        [Test]
        public async Task get_throws_exception()
        {
            //mathing /testingget/Query?firstName=John&lastName=Doe
            HttpClient client = this.webApplicationFactory.CreateClient();
            HttpResponseMessage response = await client.GetAsync("/testingget/throwsexception");
            string strResponse = await response.Content.ReadAsStringAsync();

            Assert.IsTrue(strResponse.Contains(TestingGetController.exceptionMessage));
        }

        /// <summary>
        /// When the config file contains the partial query
        ///   Upstream   -> "Template": "/testingget/ReturnsNull"
        ///   Downstream -> "Template": "/api/TestingGet/ReturnsNull"
        ///   Request    -> "/api/TestingGet/ReturnsNull"
        /// </summary>
        [Test]
        public async Task get_returns_null()
        {
            //mathing /testingget/Query?firstName=John&lastName=Doe
            HttpClient client = this.webApplicationFactory.CreateClient();
            HttpResponseMessage response = await client.GetAsync("/testingget/returnsnull");
            string strResponse = await response.Content.ReadAsStringAsync();

            Assert.AreEqual(string.Empty, strResponse);
        }
    }
}
