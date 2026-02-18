using Control_Finance.Server.Data;
using Control_Finance.Server.DTO;
using Control_Finance.Server.Enums;
using Microsoft.EntityFrameworkCore;

namespace Control_Finance.Server.Services.Security
{
    public class SecurityService : ISecurityService
    {
        private readonly AppDbContext _context;
        public SecurityService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ResultRequisitions> GetUserInformationByEmail(string email)
        {
            try
            {
                var user = await _context.AppUsers
                                         .FirstOrDefaultAsync(u => u.Email == email);
                return new ResultRequisitions
                {
                    Success = true,
                    Code = ResultsRequests.Success,
                    Message = "Informações do usuário recuperadas com sucesso.",
                    Data = new SecurityDTO
                    {
                        Id = user!.Id,
                        Email = user.Email!,
                        Nome = user.Nome
                    }
                };
            }
            catch 
            {
                throw;
            }
        }
        public async Task<ResultRequisitions> GetUserInformationById(int? id)
        {
            try
            {
                var user = await _context.Users
                                         .Where(u => u.Id == id)
                                         .Select(u => new SecurityDTO
                                         {
                                             Id = u.Id,
                                             Nome = u.Nome,
                                             Email = u.Email!,
                                             DataNascimento = u.DataNascimento,
                                             RendaMensal = u.RendaMensal,
                                             DataCriacao = DateOnly.FromDateTime(u.DataCriacao)
                                         })
                                         .FirstOrDefaultAsync();
                if (user == null)
                {
                    return new ResultRequisitions
                    {
                        Success = false,
                        Code = ResultsRequests.NotFound,
                        Message = "Usuário não encontrado."
                    };
                }
                else
                {
                    return new ResultRequisitions
                    {
                        Success = true,
                        Code = ResultsRequests.Success,
                        Message = "Informações do usuário recuperadas com sucesso.",
                        Data = user
                    };
                }
            }
            catch 
            {
                throw;
            }
        }
    }
}
