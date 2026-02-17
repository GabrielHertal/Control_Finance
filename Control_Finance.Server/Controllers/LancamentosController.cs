using Control_Finance.Server.DTO;
using Control_Finance.Server.Enums;
using Control_Finance.Server.Services.Lancamento;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Control_Finance.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LancamentosController(ILancamentoService lancamentoService) : ControllerBase
    {
        private readonly ILancamentoService _lancamentoService = lancamentoService;
        [HttpPost("CreateLancamento")]
        public async Task<ActionResult> CreateLancamento([FromBody] LancamentoDTO lancamentoDTO)
        {
            try
            {
                var result = await _lancamentoService.CreateLancamentoAsync(lancamentoDTO.Titulo, lancamentoDTO.Descricao!, lancamentoDTO.Valor, lancamentoDTO.Data_Lancamento, lancamentoDTO.Data_Vencimento
                                                                            , lancamentoDTO.Fk_Id_Categoria, lancamentoDTO.Valor_Pago, lancamentoDTO.Data_Pagamento, lancamentoDTO.Fk_Id_User
                                                                            , (int)lancamentoDTO.Tipo_Lancamento, lancamentoDTO.Fk_Id_Conta);
                if (result.Success == true)
                {
                    return result.Code switch
                    {
                        200 => Ok(new { result.Message, result.Code }),
                        201 => Created("", new { result.Message, result.Code }),
                        404 => NotFound(new { result.Message, result.Code }),
                        409 => Conflict(new { result.Message, result.Code }),
                        _ => BadRequest(new { Message = "Erro ao criar lançamento.", ErrorCode = result.Code, Error = result })
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
        [HttpPut("UpdateLancamento")]
        public async Task<ActionResult> UpdateLancamentoById([FromBody] LancamentoDTO lancamentoDTO)
        {
            try
            {
                var result = await _lancamentoService.UpdateLancamentoByIdAsync(lancamentoDTO.Id, lancamentoDTO.Titulo, lancamentoDTO.Descricao!, lancamentoDTO.Valor, lancamentoDTO.Data_Lancamento
                                                                              , lancamentoDTO.Data_Vencimento, lancamentoDTO.Fk_Id_Categoria, lancamentoDTO.Valor_Pago, lancamentoDTO.Data_Pagamento
                                                                              , lancamentoDTO.Fk_Id_User, (int)lancamentoDTO.Tipo_Lancamento, lancamentoDTO.Fk_Id_Conta, lancamentoDTO.Ativo);
                if (result.Success == true)
                {
                    return result.Code switch
                    {
                        200 => Ok(new { result.Message, result.Code }),
                        201 => Created("", new { result.Message, result.Code }),
                        404 => NotFound(new { result.Message, result.Code }),
                        409 => Conflict(new { result.Message, result.Code }),
                        _ => BadRequest(new { Message = "Erro ao atualizar lançamento.", ErrorCode = result.Code, Error = result })
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
        [HttpPatch("DeleteLancamento{id}")]
        public async Task<ActionResult> DeleteLancamentoById(int id)
        {
            try
            {
                var result = await _lancamentoService.DeleteLancamentoByIdAsync(id);
                if (result.Success == true)
                {
                    return result.Code switch
                    {
                        200 => Ok(new { result.Message, result.Code }),
                        201 => Created("", new { result.Message, result.Code }),
                        404 => NotFound(new { result.Message, result.Code }),
                        409 => Conflict(new { result.Message, result.Code }),
                        _ => BadRequest(new { Message = "Erro ao deletar lançamento.", ErrorCode = result.Code, Error = result })
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
        [HttpGet("GetLancamentoById{id}")]
        public async Task<ActionResult> GetLancamentoById(int id)
        {
            try
            {
                var result = await _lancamentoService.GetLancamentoByIdAsync(id);
                if (result.Success == true)
                {
                    return result.Code switch
                    {
                        200 => Ok(new { result.Data, result.Message, result.Code }),
                        201 => Created("", new { result.Data, result.Message, result.Code }),
                        404 => NotFound(new { result.Message, result.Code }),
                        409 => Conflict(new { result.Message, result.Code }),
                        _ => BadRequest(new { Message = "Erro ao obter lançamento.", ErrorCode = result.Code, Error = result })
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
        [HttpGet("GetAllLancamentosByUserId{id}")]
        public async Task<ActionResult<List<LancamentoDTO>>> GetAllLancamentosByUserId(int id)
        {
            try
            {
                var lancamentos = await _lancamentoService.GetAllLancamentosByUserIdAsync(id);
                if (lancamentos.Count == 0)
                {
                    return NotFound(new { Message = "Nenhum lançamento encontrado para este usuário.", Code = ResultsRequests.NotFound });
                }
                else
                {
                    return Ok(lancamentos.ToArray());
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Erro interno do servidor: {e.Message}");
            }
        }
    }  
}