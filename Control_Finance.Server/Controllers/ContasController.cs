using Control_Finance.Server.DTO;
using Control_Finance.Server.Enums;
using Control_Finance.Server.Services.Conta;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Control_Finance.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ContasController(IContasService contaService) : ControllerBase
    {
        private readonly IContasService _contaService = contaService;
        [HttpPost("CreateConta")]
        public async Task<ActionResult> CreateConta([FromBody] ContasDTO contaDTO)
        {
            try
            {
                var result = await _contaService.CreateContaAsync(contaDTO.Titulo, contaDTO.Tipo_Conta, contaDTO.Fk_Id_User, true);
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
            catch (Exception e)
            {
                return StatusCode(500, $"Erro interno do servidor: {e.Message}");
            }
        }
        [HttpPut("UpdateContaById/{id}")]
        public async Task<ActionResult> UpdateContaById(int id, [FromBody] ContasDTO contaDTO)
        {
            try
            {
                var result = await _contaService.UpdateContaByIdAsync(id, contaDTO.Titulo, contaDTO.Tipo_Conta, contaDTO.Ativo);
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
            catch (Exception e)
            {
                return StatusCode(500, $"Erro interno do servidor: {e.Message}");
            }
        }
        [HttpDelete("DeleteContaById/{id}")]
        public async Task<ActionResult> DeleteContaById(int id)
        {
            try
            {
                var result = await _contaService.DeleteContaByIdAsync(id);
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
            catch (Exception e)
            {
                return StatusCode(500, $"Erro interno do servidor: {e.Message}");
            }
        }
        [HttpGet("GetContaById/{Id}")]
        public async Task<ActionResult> GetContaById (int Id)
        {
            try
            {
                var result = await _contaService.GetContaByIdAsync(Id);
                if(result.Success == false)
                {
                    return NotFound(new { result.Message, result.Code });
                }
                return Ok(new { result.Message, result.Code, result.Data });
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Erro interno do servidor: {e.Message}");
            }
        }
        [HttpGet("GetAllContasByUserId/{userId}")]
        public async Task<ActionResult> GetAllContasByUserId(int userId)
        {
            try
            {
                var result = await _contaService.GetAllContasByUserIdAsync(userId);
                if(result.Count == 0)    
                {
                    return NotFound(new { Message = "Nenhuma conta encontrada!", Code = ResultsRequests.NotFound });
                }
                return Ok(result.ToArray());
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Erro interno do servidor: {e.Message}");
            }
        }

    }
}