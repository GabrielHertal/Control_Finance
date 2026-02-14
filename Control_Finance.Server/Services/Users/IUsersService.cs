using Control_Finance.Server.DTO;
using Control_Finance.Server.Models;

namespace Control_Finance.Server.Services.Users
{
    public interface IUsersService
    {
        Task<ResultRequisitions> CreateUserAsync(string nome, string email, string password, DateOnly dataNascimento, decimal? rendaMensal);
        Task<ResultRequisitions> UpdateUserByIdAsync(int id, string nome, string email, string senha, DateOnly dataNascimento, decimal? rendaMensal);
        Task<ResultRequisitions> DeleteUserByIdAsync(int id);
        Task<ResultRequisitions> GetUserByIdAsync(int id);
        Task<List<ResultRequisitions>> GetAllUsers();
    }
}