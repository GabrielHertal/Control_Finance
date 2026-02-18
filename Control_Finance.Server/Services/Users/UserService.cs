using Control_Finance.Server.Data;
using Control_Finance.Server.DTO;
using Control_Finance.Server.Enums;
using Control_Finance.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Control_Finance.Server.Services.Users
{
    public class UserService(UserManager<AppUsers> userManager, AppDbContext context) : IUsersService
    {
        private readonly AppDbContext _context = context;
        private readonly UserManager<AppUsers> _userManager = userManager;
        public async Task<ResultRequisitions> CreateUserAsync(string nome, string email, string password, DateOnly dataNascimento, decimal? rendaMensal)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(email.ToUpper());
                if (existingUser != null)
                {
                    return new ResultRequisitions
                    {
                        Success = false,
                        Code = ResultsRequests.Conflict,
                        Message = "Email já cadastrado!"
                    };
                }
                var hasher = new PasswordHasher<AppUsers>();
                var newUser = new AppUsers
                {
                    Nome = nome,
                    Email = email,
                    UserName = email,
                    DataNascimento = dataNascimento,
                    RendaMensal = rendaMensal,
                    DataCriacao = DateTime.UtcNow,
                    Ativo = true
                };
                newUser.PasswordHash = hasher.HashPassword(newUser, password);
                var result = await _userManager.CreateAsync(newUser);
                return new ResultRequisitions
                {
                    Success = true,
                    Code = ResultsRequests.Created,
                    Message = "Usuário criado com sucesso!"
                };
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return new ResultRequisitions
                {
                    Success = false,
                    Code = ResultsRequests.BadRequest,
                    Message = "Ocorreu um erro ao criar o usuário!",
                    Data = ex
                };
            }   
        }
        public async Task<ResultRequisitions> UpdateUserByIdAsync(int id, string nome, string email, string senha, DateOnly dataNascimento, decimal? rendaMensal)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if(user == null)
                {
                    return new ResultRequisitions
                    {
                        Success = false,
                        Code = ResultsRequests.NotFound,
                        Message = "Usuário não encontrado!"
                    };
                }
                if (user.Email != email)
                {
                    var emailUser = await _userManager.FindByEmailAsync(email);
                    if (emailUser != null)
                    {
                        return new ResultRequisitions
                        {
                            Success = false,
                            Code = ResultsRequests.Conflict,
                            Message = "Email já cadastrado!"
                        };
                    }
                }
                var hasher = new PasswordHasher<AppUsers>();
                if(string.IsNullOrEmpty(senha))
                {
                    user.PasswordHash = user.PasswordHash!;
                }
                else
                {
                    user.PasswordHash = hasher.HashPassword(user, senha);
                }
                user.Nome = nome;
                user.Email = email;
                user.UserName = email;
                user.NormalizedEmail = email.ToUpper();
                user.DataNascimento = dataNascimento;
                user.RendaMensal = rendaMensal;
                _context.AppUsers.Update(user);
                await _context.SaveChangesAsync();
                return new ResultRequisitions
                {
                    Success = true,
                    Code = ResultsRequests.Success,
                    Message = "Usuário atualizado com sucesso!"
                };
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return new ResultRequisitions
                {
                    Success = false,
                    Code = ResultsRequests.BadRequest,
                    Message = "Ocorreu um erro ao atualizar o usuário!",
                    Data = ex
                };
            }
        }
        public async Task<ResultRequisitions> DeleteUserByIdAsync(int id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if(user == null) 
                return new ResultRequisitions
                {
                    Success = false,
                    Code = ResultsRequests.NotFound,
                    Message = "Usuário não encontrado!"
                };
                user.Ativo = false;
                _context.AppUsers.Update(user);
                await _context.SaveChangesAsync();
                return new ResultRequisitions
                {
                    Success = true,
                    Code = ResultsRequests.Success,
                    Message = "Usuário deletado com sucesso!"
                };
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return new ResultRequisitions
                {
                    Success = false,
                    Code = ResultsRequests.BadRequest,
                    Message = "Ocorreu um erro ao deletar o usuário!",
                    Data = ex
                };
            }
        }
        public async Task<ResultRequisitions> GetUserByIdAsync(int id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if(user == null)
                {
                    return new ResultRequisitions
                    {
                        Success = false,
                        Code = ResultsRequests.NotFound,
                        Message = "Usuário não encontrado!"
                    };
                }
                return new ResultRequisitions
                {
                    Success = true,
                    Code = ResultsRequests.Success,
                    Message = "Usuário encontrado com sucesso!",
                    Data = new UsersDTO
                    {
                        Id = user.Id,
                        Nome = user.Nome,
                        Email = user.Email!,
                        DataNascimento = user.DataNascimento,
                        RendaMensal = user.RendaMensal,
                        DataCriacao = user.DataCriacao
                    }
                };
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return new ResultRequisitions
                {
                    Success = false,
                    Code = ResultsRequests.BadRequest,
                    Message = "Ocorreu um erro ao buscar o usuário!",
                    Data = ex
                };
            }
        }
        public async Task<List<ResultRequisitions>> GetAllUsers()
        {
            try
            {
                var users = await _userManager.Users
                                              .Where(u => u.Ativo == true)
                                              .OrderBy(u => u.Id)
                                              .ToListAsync();
                if(users.Count == 0)
                {
                    return [new ResultRequisitions
                    {
                        Success = false,
                        Code = ResultsRequests.NotFound,
                        Message = "Nenhum usuário encontrado!"
                    }];
                }
                return new List<ResultRequisitions>
                {
                    new ResultRequisitions
                    {
                        Success = true,
                        Code = ResultsRequests.Success,
                        Message = "Usuários encontrados com sucesso!",
                        Data = users.Select(user => new UsersDTO
                        {
                            Id = user.Id,
                            Nome = user.Nome,
                            Email = user.Email!,
                            DataNascimento = user.DataNascimento,
                            RendaMensal = user.RendaMensal,
                            DataCriacao = user.DataCriacao
                        }).ToList()
                    }
                };
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return new List<ResultRequisitions>
                {
                    new ResultRequisitions
                    {
                        Success = false,
                        Code = ResultsRequests.BadRequest,
                        Message = "Ocorreu um erro ao buscar os usuários!",
                        Data = ex
                    }
                };
            }
        }
    }
}