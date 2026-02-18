using Control_Finance.Server.DTO;
using Control_Finance.Server.Enums;
using Control_Finance.Server.Services.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Control_Finance.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController(IUsersService user) : ControllerBase
    {
        private readonly IUsersService _user = user;
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDTO registerDTO)
        {
            try
            {
                var result = await _user.CreateUserAsync(registerDTO.Nome, registerDTO.Email, registerDTO.Password, registerDTO.DataNascimento, registerDTO.RendaMensal);
                if (result.Success == true)
                {
                    return result.Code switch
                    {
                        ResultsRequests.Success => Ok(new { result.Message, result.Code }),
                        ResultsRequests.NotFound => NotFound(new { result.Message, result.Code }),
                        ResultsRequests.Conflict => Conflict(new { result.Message, result.Code }),
                        _ => BadRequest(new { Message = "Erro ao atualizar usuário.", ErrorCode = result.Code, Error = result })
                    };
                }
                else
                {
                    return BadRequest(new { result.Message, result.Code });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }
        [HttpGet("GetAllUsers")]
        public async Task<ActionResult> GetAllUsers()
        {
            try
            {
                var users = await _user.GetAllUsers();
                return Ok(users.ToArray());
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Erro interno do servidor: { e.Message}");
            }
        }
        [HttpGet("GetUserById/{id}")]
        public async Task<ActionResult<UsersDTO>> GetUserByIdAsync(int id)
        {
            try
            {
                var user = await _user.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound(new { Message = "Usuário não encontrado." });
                }
                return Ok((UsersDTO)user.Data!);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Erro interno do servidor: {e.Message}");
            }
        }
        [HttpPut("UpdateUser/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDTO updateDTO)
        {
            try
            {
                var result = await _user.UpdateUserByIdAsync(id, updateDTO.Nome, updateDTO.Email, updateDTO.Password!, updateDTO.DataNascimento, updateDTO.RendaMensal);
                if(result.Success)
                {
                    return result.Code switch
                    {
                        ResultsRequests.Success => Ok(new { result.Message, result.Code }),
                        ResultsRequests.NotFound => NotFound(new { result.Message, result.Code }),
                        ResultsRequests.Conflict => Conflict(new { result.Message, result.Code }),
                        _ => BadRequest(new { Message = "Erro ao atualizar usuário.", ErrorCode = result.Code, Error = result })
                    };
                }
                else
                {
                    return BadRequest(new { result.Message, result.Code });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }
        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var result = await _user.DeleteUserByIdAsync(id);
                if(result.Success)
                {
                    return result.Code switch
                    {
                        ResultsRequests.Success => Ok(new { result.Message, result.Code }),
                        ResultsRequests.NotFound => NotFound(new { result.Message, result.Code }),
                        ResultsRequests.Conflict => Conflict(new { result.Message, result.Code }),
                        _ => BadRequest(new { Message = "Erro ao atualizar usuário.", ErrorCode = result.Code, Error = result })
                    };
                }
                else
                {
                    return BadRequest(new { result.Message, result.Code });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }
    }
}
