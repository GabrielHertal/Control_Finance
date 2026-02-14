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
        public async Task<ResultRequisitions> CreateContaAsync(string nome, int tipo_conta, int fkIdUser, bool ativo)
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
        public async Task<ResultRequisitions> UpdateContaByIdAsync(int id, string nome, int tipo_conta, bool ativo)
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
        public async Task<ResultRequisitions> DeleteContaByIdAsync(int id)
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
        public async Task<ResultRequisitions> GetContaByIdAsync(int id)
        {
            var contas = await _context.Contas
                                       .Where(c => c.Id == id)
                                       .Select(c => new ContasDTO
                                       {
                                           Id = c.Id,
                                           Titulo = c.Titulo,
                                           Tipo_Conta = (int)c.Tipo_Conta,
                                           Fk_Id_User = c.Fk_Id_User,
                                           Ativo = c.Ativo
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
        public async Task<List<ResultRequisitions>> GetAllContasByUserIdAsync(int id)
        {
            var contas = await _context.Contas
                                       .Where(c => c.Fk_Id_User == id)
                                       .Select(c => new ContasDTO
                                       {
                                           Id = c.Id,
                                           Titulo = c.Titulo,
                                           Tipo_Conta = (int)c.Tipo_Conta,
                                           Fk_Id_User = c.Fk_Id_User,
                                           Ativo = c.Ativo
                                       })
                                       .ToListAsync();
            if(contas.Count == 0)
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
                return [new ResultRequisitions
                {
                    Success = true,
                    Code = (int)ResultsRequests.Success,
                    Message = "Nenhuma conta encontrada para este usuário!",
                    Data = contas.Select(contas => new ContasDTO
                    {
                        Id = contas.Id,
                        Titulo = contas.Titulo,
                        Tipo_Conta = contas.Tipo_Conta,
                        Ativo = contas.Ativo,
                        Fk_Id_User = contas.Fk_Id_User,
                    }).ToList()
                }];
            }
        }
    }
}