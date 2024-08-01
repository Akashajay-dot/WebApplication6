using Google.Apis.Auth;

using System;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Google.Apis.Auth.OAuth2;
using Google;
using WebApplication6.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;



namespace WebApplication6.Controllers.Api
{
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        private readonly AppdbContext _context;

        public AuthController(AppdbContext context)
        {
            _context = context;
        }
        [HttpPost]
        [Route("validate-token")]
        public async Task<IHttpActionResult> ValidateToken([FromBody] TokenDto tokenDto)
        {

            if (string.IsNullOrWhiteSpace(tokenDto.Credential))
            {
                
                return BadRequest(new { message = "Credential must not be empty" });
            }   
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(tokenDto.Credential);
                var user = await _context.Users.SingleOrDefaultAsync(u => u.GoogleId == payload.Subject);

                if (user == null)
                {
                    user = new User
                    {
                        GoogleId = payload.Subject,
                        Name = payload.Name,
                        CreatedON = DateTime.UtcNow,
                        UpdatedON = DateTime.UtcNow,
                        Email=payload.Email,

                    };
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    user.UpdatedON = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }

                return base.Ok(new
                {
                   Isvalid = true,
                   Payload = payload,
                   UserId = user.UserId,
                   category = _context.Categories
                });
            }
            catch (InvalidJwtException e)
            {
                //return Unauthorized(new { message = "Invalid token", error = e.Message });
                var response = Request.CreateResponse(HttpStatusCode.Unauthorized, new
                {
                   
                    message = "Invalid token",
                    error = e.Message
                    
                });

                return ResponseMessage(response);
            }
            catch (DbUpdateException ex)
            {
                // Log the detailed error
                var detailedErrorMessage = ex.InnerException?.InnerException?.Message ?? ex.Message;

                var response = Request.CreateResponse(HttpStatusCode.InternalServerError, new
                {
                    message = "An unexpected error occurred while updating the database.",
                    error = detailedErrorMessage
                });

                return ResponseMessage(response);
            }
            catch (Exception ex)
            {
                // Log unexpected errors
                //return InternalServerError(new { message = "An error occurred", error = ex.Message });
                // return StatusCode(500, new { message = "An error occurred", error = ex.Message });
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError, new
                {
                    message = "An unexpected error occurred.",
                    error = ex.Message  // You can choose to expose ex.Message or keep it generic for security
                });

                return ResponseMessage(response);
            }
        }

        private IHttpActionResult BadRequest(object value)
        {
            throw new NotImplementedException();
        }
    }
    public class TokenDto
    {
        public string Credential { get; set; }
    }

}
