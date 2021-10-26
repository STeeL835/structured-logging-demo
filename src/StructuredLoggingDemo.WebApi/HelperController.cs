using System.Linq;
using Bogus;
using Microsoft.AspNetCore.Mvc;

namespace StructuredLoggingDemo.WebApi
{
    [ApiController]
    [Route("helpers")]
    public class HelperController : Controller
    {
        private Faker _faker = new Faker();

        /// <summary>
        /// Generates a list of cases with random emails for postman runner
        /// </summary>
        [HttpPost("random-emails-data")]
        public IActionResult GetRandomEmails(int amount, float brokenCoefficient = 0.05f)
        {
            return Ok(Enumerable.Range(0, amount).Select(_ => new
            {
                email = _faker.Random.Bool(brokenCoefficient)
                    ? _faker.Internet.Email(provider: "fakeemail.com")
                    : _faker.Internet.Email()
            }));
        }
    }
}