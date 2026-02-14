using Control_Finance.Server.DTO;
using Control_Finance.Server.Enums;
using Control_Finance.Server.Models;
using Control_Finance.Server.Services;
using Control_Finance.Server.Services.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Control_Finance.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private ISecurityService _security;
        private readonly SignInManager<AppUsers> _signInManager;
        private readonly UserManager<AppUsers> _userManager;
        private IConfiguration _config;
        public SecurityController(ISecurityService security, SignInManager<AppUsers> signInManager, UserManager<AppUsers> userManager, IConfiguration config)
        {
            _security = security;
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO logindto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(logindto.Email.ToUpper());
                if (user == null)
                {
                    return Unauthorized(new { Message = "Usuário não encontrado com este e-mail!", Code = ResultsRequests.Unauthorized});
                }
                var result = await _signInManager.PasswordSignInAsync(logindto.Email.ToUpper(), logindto.Password, false, false);
                if (!result.Succeeded)
                {
                    return Unauthorized(new { Message = "Senha incorreta!", Code = ResultsRequests.Unauthorized });
                }
                var token = await GenerateTokenJWT(logindto.Email);
                if(token == ResultsRequests.NotFound.ToString()) 
                {
                    return Unauthorized(new { Message = "Erro ao gerar token!", Code = ResultsRequests.Unauthorized});
                }
                var userInformation = _security.GetUserInformationByEmail(logindto.Email);
                if(userInformation == null)
                {
                    return NotFound(new {Message = "Usuário não encontrado!", Code = ResultsRequests.NotFound});
                }
                return Ok(new { token, userInformation.Id, Code = ResultsRequests.Success });
            }
            catch
            {
                throw;
            }
        }
        [HttpGet("GetUserInformation")]
        public async Task<ResultRequisitions?> GetUserInformationClaimms()
        {
            var authToken = Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authToken) || !authToken.StartsWith("Bearer "))
            {
                return null;
            }
            var token = authToken.Substring("Bearer ".Length).Trim();
            var claims = GetClaims(token);
            if (claims.TryGetValue(ClaimTypes.NameIdentifier, out string? id))
            {
                return await _security.GetUserInformationById(Convert.ToInt16(id));
            }
            return null;
        }
        private static Dictionary<string, string> GetClaims(string authToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(authToken) as JwtSecurityToken;
            var claims = jsonToken?.Claims.ToDictionary(claim => claim.Type, claim => claim.Value);
            if (claims != null)
            {
                return claims;
            }
            else
            {
                return [];
            }
        }
        private async Task<string> GenerateTokenJWT(string email)
        {
            try
            {
                var userInformation = await _security.GetUserInformationByEmail(email); 
                if(userInformation == null)
                {
                    return ResultsRequests.NotFound.ToString();
                }
                var user = (SecurityDTO)userInformation.Data!;
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Nome!),
                    new Claim(ClaimTypes.Email, user.Email)
                };

                var issuer = _config["Jwt:Issuer"];
                var audience = _config["Jwt:Audience"];
                var expirationDays = Convert.ToDouble(_config["Jwt:DurationInDays"]);
                var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(issuer, audience, claims, expires: DateTime.Now.AddDays(expirationDays), signingCredentials: credentials);
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch
            {
                throw;
            }
        }
    }
}