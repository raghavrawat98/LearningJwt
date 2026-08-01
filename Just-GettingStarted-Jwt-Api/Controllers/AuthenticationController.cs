using Just_GettingStarted_Jwt_Api.Helper;
using Just_GettingStarted_Jwt_Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using BCrypt.Net;
using System.Text.Json;

namespace Just_GettingStarted_Jwt_Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private IAuthService _authService;
        private ICryptHelper _cryptHelper;
        private ITokenHelper _tokenHelper;

        public AuthenticationController(
            ILogger<WeatherForecastController> logger
            ,IAuthService authService
            , ICryptHelper cryptHelper
            , ITokenHelper tokenHelper
            )
        {
            _logger = logger;
            _authService = authService;
            _cryptHelper = cryptHelper;
            _tokenHelper = tokenHelper;
        }

        // 1> Register a User
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterDetails details)
        {
            try
            {
                // 1. Check if User Exists
                bool exists = await _authService.UserExists(details.email);

                if ( exists ) 
                {
                    return BadRequest("User Already Exists");
                }

                // 2. If user not in DB means it's new user, hash the Password (can't store simple password)
                User user = new User(Guid.NewGuid())
                {
                    Email = details.email,
                    HashedPassword = _cryptHelper.HashPassword(details.password)
                };

                bool userAdded = await _authService.AddUser(user);

                if ( !userAdded) 
                {
                    return Problem("User not added something went wrong...");
                }

                return Ok($"User Created with id={user.UserId} email={user.Email}");
            }
            catch (Exception ex) 
            {
                return Problem(ex.Message);
            }
        }


        // 2> Login User
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginUser details)
        {
            try
            {
                // 1. Check if User Exists, if not then error it is.
                bool exists = await _authService.UserExists(details.email);

                if ( !exists)
                {
                    return BadRequest("User doesn't Exists");
                }

                // 2. Now, compare crypted password and see if it matches. If not error.
                User? user = await _authService.GetUser(details.email);

                bool valid = _cryptHelper.VerifyPassword(details.password, user.HashedPassword);

                if ( !valid ) 
                {
                    // Good practice to not reveal if password is wrong or username
                    // as it would help hackers
                    return Unauthorized("Incorrect User Info"); 
                }

                // 3. Create RefreshToken and Access Token
                TheJwtTokens tokens = _tokenHelper.GenerateTokens(user.Email);

                // 4. Put the refresh Token in Database
                // Refresh token can be versioned but seriously that's too complex, here I can't even get this working ...
                await _authService.UpsertRefreshToken(user.Email , tokens.RefreshToken); // I had to replace UserID with email since it's not working

                return Ok(tokens);

            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        // 3> Logout User
        [HttpDelete]
        public async Task<IActionResult> Logout([FromBody] LogoutUser details) 
        {

            try
            {
                // 1. Check if User Exists, if not then error it is.
                bool exists = await _authService.UserExists(details.email);

                if (!exists)
                {
                    return BadRequest("User doesn't Exists");
                }

                // 2. Now, compare refresh token and see if it matches. If not error.
                User? user = await _authService.GetUser(details.email);

                // https://share.google/aimode/ELq9GaT7EYIrGHOJe
                // (user.RefreshToken.Equals(details.refreshToken)) || (String.IsNullOrEmpty(user.RefreshToken)) will throw error
                bool valid = (String.IsNullOrEmpty(user.RefreshToken)) || (user.RefreshToken.Equals(details.refreshToken)) ;

                if (!valid)
                {
                    // Good practice to not reveal if password is wrong or username
                    // as it would help hackers
                    return Unauthorized("Incorrect Logout attempt");
                }

                await _authService.DeleteRefreshToken(user.Email); 

                return Ok($"UserID:{user.UserId} Email:{user.Email} has been logged out.");

            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        // 5> Getting a new Access token with Refresh Token
        [HttpPost]
        public async Task<IActionResult> RefreshToken([FromBody] NewAccessTokenforUserUsingRefreshToken refreshAcess)
        {

            try
            {
                // If no token in request, no new access token can be generated
                if ( string.IsNullOrEmpty(refreshAcess.refreshToken) ) { return Ok("No refresh token provided"); }

                (bool IsValid, string? Email) result = _tokenHelper.VerifyRefreshToken(refreshAcess.refreshToken);

                if ( !result.IsValid) { return Ok("No new access token generated"); }

                // Since the token is valid , we try to get user and check tokens
                User? user = await _authService.GetUser(result.Email);

                if (user == null || !(user.RefreshToken.Equals(refreshAcess.refreshToken) ) ) { return Ok("No new access token generated"); }

                // Now that everything is set, generate new access tokens

                string newAccessToken = _tokenHelper.GenerateAccessToken(user.Email);
                //TheJwtTokens newTokens = new TheJwtTokens(accessToken: newAccessToken, refreshToken: user.RefreshToken);
                return Ok(newAccessToken);

            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

    }
}
