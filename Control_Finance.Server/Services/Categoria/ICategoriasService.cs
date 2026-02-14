using Control_Finance.Server.DTO;

namespace Control_Finance.Server.Services.Categoria
{
    public interface ICategoriasService
    {
        Task<ResultRequisitions> CreateCategoriaAsync(string nome, int fkIdUser, bool ativo);
        Task<ResultRequisitions> UpdateCategoriaByIdAsync(int id, string nome, bool ativo);
        Task<ResultRequisitions> DeleteCategoriaByIdAsync(int id);
        Task<ResultRequisitions> GetCategoriaByIdAsync(int id);
        Task<List<ResultRequisitions>> GetAllCategoriasByUserIdAsync(int id);
    }
}