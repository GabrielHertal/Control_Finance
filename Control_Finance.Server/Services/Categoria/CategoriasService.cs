using Control_Finance.Server.Data;
using Control_Finance.Server.DTO;
using Control_Finance.Server.Enums;
using Control_Finance.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace Control_Finance.Server.Services.Categoria
{
    public class CategoriasService(AppDbContext context) : ICategoriasService
    {
        private readonly AppDbContext _context = context;

        public async Task<ResultRequisitions> CreateCategoriaAsync(string nome, int fkIdUser, bool ativo)
        {
            var categoria = new Categorias
            {
                Id = 0,
                Titulo = nome,
                FkIdUser = fkIdUser,
                Ativo = true
            };
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
            return new ResultRequisitions
            {
                Success = true,
                Code = (int)ResultsRequests.Created,
                Message = "Categoria criada com sucesso."
            };
        }
        public async Task<ResultRequisitions> UpdateCategoriaByIdAsync(int id, string nome, bool ativo)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return new ResultRequisitions
                {
                    Success = false,
                    Code = (int)ResultsRequests.NotFound,
                    Message = "Categoria não encontrada."
                };
            }
            categoria.Titulo = nome;
            categoria.Ativo = ativo;
            _context.Categorias.Update(categoria);
            await _context.SaveChangesAsync();
            return new ResultRequisitions
            {
                Success = true,
                Code = (int)ResultsRequests.Success,
                Message = "Categoria atualizada com sucesso."
            };
        }
        public async Task<ResultRequisitions> DeleteCategoriaByIdAsync(int id)
        {
            try
            {
                var categoria = await _context.Categorias.FindAsync(id);
                if (categoria == null)
                {
                    return new ResultRequisitions
                    {
                        Success = false,
                        Code = (int)ResultsRequests.NotFound,
                        Message = "Categoria não encontrada."
                    };
                }
                categoria.Ativo = false;
                _context.Categorias.Update(categoria);
                await _context.SaveChangesAsync();
                return new ResultRequisitions
                {
                    Success = true,
                    Code = (int)ResultsRequests.Success,
                    Message = "Categoria deletada com sucesso."
                };
            }
            catch
            {
                throw;
            }
        }
        public async Task<ResultRequisitions> GetCategoriaByIdAsync(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return new ResultRequisitions
                {
                    Success = false,
                    Code = (int)ResultsRequests.NotFound,
                    Message = "Categoria não encontrada."
                };
            }
            return new ResultRequisitions
            {
                Success = true,
                Code = (int)ResultsRequests.Success,
                Message = "Categoria encontrada com sucesso.",
                Data = new CategoriasDTO
                {
                    Id = categoria.Id,
                    Nome = categoria.Titulo,
                    Ativo = categoria.Ativo,
                    FkIdUser = categoria.FkIdUser
                }
            };
        }
        public async Task<List<ResultRequisitions>> GetAllCategoriasByUserIdAsync(int id)
        { 
            var categorias = await _context.Categorias
                                           .Select(c => new CategoriasDTO
                                           {
                                               Id = c.Id,
                                               Nome = c.Titulo,
                                               Ativo = c.Ativo,
                                               FkIdUser = c.FkIdUser
                                           })
                                           .Where(i => i.FkIdUser == id && i.Ativo == true)
                                           .OrderBy(i => i.Id)
                                           .ToListAsync();
            if(categorias == null)
            {
                return [new ResultRequisitions
                {
                    Success = false,
                    Code = (int)ResultsRequests.NotFound,
                    Message = "Nenhuma categoria encontrada para este usuário."
                }];
            }    
            return [new ResultRequisitions
            {
                Success = true,
                Code = (int)ResultsRequests.Success,
                Message = "Categorias encontradas com sucesso.",
                Data = categorias.Select(categorias => new CategoriasDTO
                {
                    Id = categorias.Id,
                    Nome = categorias.Nome,
                    Ativo = categorias.Ativo,
                    FkIdUser = categorias.FkIdUser
                }).ToList()
            }];
        }
    }
}