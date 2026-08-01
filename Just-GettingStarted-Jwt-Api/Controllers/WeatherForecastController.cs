using Just_GettingStarted_Jwt_Api.Helper;
using Just_GettingStarted_Jwt_Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Just_GettingStarted_Jwt_Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ITokenHelper _tokenHelper;

        public WeatherForecastController(ILogger<WeatherForecastController> logger
            , ITokenHelper tokenHelper)
        {
            _logger = logger;
            _tokenHelper = tokenHelper;
        }

        // 4> Protected Route
        [HttpGet]
        public IActionResult GetWeatherForecastProtected()
        {
            try
            {
                // https://share.google/aimode/XseA7Ai4ylJ90t9y5
                if (!Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    return Unauthorized("Authorization header is missing, thus you need to login");
                }

                // // The token looks like Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiJjc19kb3RuZXRfZGV2QHRlc3QuY29tIiwiZXhwIjoxNzg1NjExNDI2fQ.rOHxWYYscEyY8oyEbPHwYDTahIVHPNMX1XBA6ydkuig
                // _logger.LogDebug(authHeader.ToString());

                string inputHead = authHeader.ToString();

                if (!(inputHead.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))) // if it doesn't start with Bearer
                {
                    return BadRequest("Invalid authorization scheme.");
                }

                // so the token we got is following
                string inputToken = inputHead.Substring("Bearer ".Length).Trim();
                
                // Now to verify it
                bool isTokenAuthenticated = _tokenHelper.VerifyAccessToken(inputToken).IsValid;

                if ( !isTokenAuthenticated ) 
                {
                    return Unauthorized("Token Provided is not valid");
                }

                return Ok(Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                }).ToArray());
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

        }

        [HttpGet]
        public IEnumerable<WeatherForecast> GetWeatherForecastPublic()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
