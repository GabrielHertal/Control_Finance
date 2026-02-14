using Control_Finance.Server.DTO;
using Control_Finance.Server.Enums;
using Control_Finance.Server.Services.Categoria;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Control_Finance.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriasService _categoriaService;
        public CategoriasController(ICategoriasService categoriaService)
        {
            _categoriaService = categoriaService;
        }
        [HttpPost("CreateCategoria")]
        public async Task<IActionResult> CreateCategoria([FromBody] CategoriasDTO categoriaDTO)
        {
            try
            {
                var result = await _categoriaService.CreateCategoriaAsync(categoriaDTO.Nome, categoriaDTO.FkIdUser, true);
                if (result.Success == true)
                {
                    return result.Code switch
                    {
                        200 => Ok(new { result.Message, result.Code }),
                        201 => Created("", new { result.Message, result.Code }),
                        404 => NotFound(new { result.Message, result.Code }),
                        409 => Conflict(new { result.Message, result.Code }),
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
        [HttpPut("UpdateCategoriaById/{id}")]
        public async Task<IActionResult> UpdateCategoriaById(int id, [FromBody] CategoriasDTO categoriaDTO)
        {
            try
            {
                var result = await _categoriaService.UpdateCategoriaByIdAsync(id, categoriaDTO.Nome, categoriaDTO.Ativo);
                if (result.Success == true)
                {
                    return result.Code switch
                    {
                        200 => Ok(new { result.Message, result.Code }),
                        404 => NotFound(new { result.Message, result.Code }),
                        409 => Conflict(new { result.Message, result.Code }),
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
        [HttpGet("GetAllCategoriasByUserId/{id}")]
        public async Task<ActionResult<List<CategoriasDTO>>> GetAllCategorias(int id)
        {
            try
            {
                var categorias = await _categoriaService.GetAllCategoriasByUserIdAsync(id);
                if (categorias.Count == 0)
                {
                    return NotFound(new { Message = "Nenhuma conta encontrada!", Code = ResultsRequests.NotFound });
                }
                return Ok(categorias.ToArray());
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Erro interno do Servidor: {e.Message }");
            }
        }
        [HttpGet("GetCategoriaById/{id}")]
        public async Task<ActionResult<CategoriasDTO>> GetCategoriaByIdAsync(int id)
        {
            try
            {
                var categoria = await _categoriaService.GetCategoriaByIdAsync(id);
                if (categoria.Success == false)
                {
                    return NotFound(new { categoria.Message, categoria.Code });
                }
                return Ok(new { categoria.Message, categoria.Code, categoria.Data});
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Erro interno do servidor: {e.Message}");
            }
        }
        [HttpDelete("DeleteCategoriaById/{id}")]
        public async Task<IActionResult> DeleteCategoriaById(int id)
        {
            try
            {
                var result = await _categoriaService.DeleteCategoriaByIdAsync(id);
                if (result.Success == true)
                {
                    return result.Code switch
                    {
                        200 => Ok(new { result.Message, result.Code }),
                        404 => NotFound(new { result.Message, result.Code }),
                        409 => Conflict(new { result.Message, result.Code }),
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
    }
}