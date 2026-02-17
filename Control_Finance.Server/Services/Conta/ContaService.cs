using Control_Finance.Server.Data;
using Control_Finance.Server.DTO;
using Control_Finance.Server.Enums;
using Control_Finance.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace Control_Finance.Server.Services.Conta
{
    public class ContaService(AppDbContext context) : IContasService
    {
        private readonly AppDbContext _context = context;
        public async Task<ResultRequisitions> CreateContaAsync(string nome, TipoConta tipo_conta, int fkIdUser, bool ativo)
        {
            try
            {
                var conta = new Contas
                {
                    Id = 0,
                    Titulo = nome,
                    Tipo_Conta = (TipoConta)tipo_conta,
                    Fk_Id_User = fkIdUser,
                    Ativo = true
                };
                _context.Contas.Add(conta);
                await _context.SaveChangesAsync();
                return new ResultRequisitions
                {
                    Success = true,
                    Code = (int)ResultsRequests.Success,
                    Message = "Conta criada com sucesso"
                };
            }
            catch (Exception ex)
            {
                return new ResultRequisitions
                {
                    Success = false,
                    Code = (int)ResultsRequests.BadRequest,
                    Message = $"Erro ao criar conta: {ex.Message}",
                    Data = ex
                };
            }
        }
        public async Task<ResultRequisitions> UpdateContaByIdAsync(int id, string nome, TipoConta tipo_conta, bool ativo)
        {
            try
            {
                var conta = await _context.Contas.FindAsync(id);
                if (conta == null)
                {
                    return new ResultRequisitions
                    {
                        Success = false,
                        Code = (int)ResultsRequests.NotFound,
                        Message = "Conta não encontrada"
                    };
                }
                conta.Titulo = nome;
                conta.Tipo_Conta = (TipoConta)tipo_conta;
                conta.Ativo = ativo;
                await _context.SaveChangesAsync();
                return new ResultRequisitions
                {
                    Success = true,
                    Code = (int)ResultsRequests.Success,
                    Message = "Conta atualizada com sucesso"
                };   
            }
            catch (Exception ex)
            {
                return new ResultRequisitions
                {
                    Success = false,
                    Code = (int)ResultsRequests.BadRequest,
                    Message = $"Erro ao atualizar conta: {ex.Message}",
                    Data = ex
                };
            }
        }
        public async Task<ResultRequisitions> DeleteContaByIdAsync(int id)
        {
            try
            {
                var conta = await _context.Contas.FindAsync(id);
                if (conta == null)
                {
                    return new ResultRequisitions
                    {
                        Success = false,
                        Code = (int)ResultsRequests.NotFound,
                        Message = "Conta não encontrada"
                    };
                }
                conta.Ativo = false;
                await _context.SaveChangesAsync();
                return new ResultRequisitions
                {
                    Success = true,
                    Code = (int)ResultsRequests.Success,
                    Message = "Conta deletada com sucesso"
                };
            }
            catch(Exception ex)
            {
                return new ResultRequisitions
                {
                    Success = false,
                    Code = (int)ResultsRequests.BadRequest,
                    Message = $"Erro ao deletar conta: {ex.Message}",
                    Data = ex
                };
            }
        }
        public async Task<ResultRequisitions> GetContaByIdAsync(int id)
        {
            try
            {
                var contas = await _context.Contas
                                       .Where(c => c.Id == id)
                                       .Select(c => new ContasDTO
                                       {
                                           Id = c.Id,
                                           Titulo = c.Titulo,
                                           Tipo_Conta = (TipoConta)c.Tipo_Conta
                                       })
                                       .FirstOrDefaultAsync();
                if (contas == null)
                {
                    return new ResultRequisitions
                    {
                        Success = false,
                        Code = (int)ResultsRequests.NotFound,
                        Message = "Nenhuma conta encontrada!"
                    };
                }
                else
                {
                    return new ResultRequisitions
                    {
                        Success = true,
                        Code = (int)ResultsRequests.Success,
                        Message = "Conta encontrada com sucesso!",
                        Data = contas
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResultRequisitions
                {
                    Success = false,
                    Code = (int)ResultsRequests.BadRequest,
                    Message = $"Erro ao buscar conta: {ex.Message}",
                    Data = ex
                };
            }
        }
        public async Task<List<ResultRequisitions>> GetAllContasByUserIdAsync(int id)
        {
            try
            {
                var contas = await _context.Contas
                                           .Where(c => c.Fk_Id_User == id)
                                           .ToListAsync();
                if (contas.Count == 0)
                {
                    return [new ResultRequisitions
                    {
                        Success = false,
                        Code = (int)ResultsRequests.NotFound,
                        Message = "Nenhuma conta encontrada para este usuário!"
                    }];
                }
                else
                {
                    return new List<ResultRequisitions>
                    {
                        new ResultRequisitions
                        {
                            Success = true,
                            Code = (int)ResultsRequests.Success,
                            Message = "Contas encontrada para este usuário!",
                            Data = contas.Select(c => new ContasDTO
                            {
                                Id = c.Id,
                                Titulo = c.Titulo,
                                Tipo_Conta = (TipoConta)c.Tipo_Conta
                            }).ToList()
                        }
                    };
                }
            }
            catch(Exception ex)
            {
                return new List<ResultRequisitions>
                {
                    new ResultRequisitions
                    {
                        Success = false,
                        Code = (int)ResultsRequests.BadRequest,
                        Message = $"Erro ao buscar contas: {ex.Message}",
                        Data = ex
                    }
                };
            }
        }
    }
}