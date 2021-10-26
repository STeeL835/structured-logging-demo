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

        /// <summary>
        /// Generates a list of random sources with a sprinkle of "broken" ones
        /// </summary>
        [HttpPost("random-sources")]
        public IActionResult GetRandomSources(int amount, float brokenCoefficient = 0.05f)
        {
            var brokenPrefixes = new[] {"example", "demo2", "us.v2", "mock"};

            return Ok(Enumerable.Range(0, amount).Select(_ =>
            {
                var url = _faker.Internet.DomainName();

                return _faker.Random.Bool(brokenCoefficient)
                    ? $"{_faker.PickRandom(brokenPrefixes)}.{url}"
                    : $"{_faker.Internet.DomainWord()}.{url}";
            }));
        }
    }
}