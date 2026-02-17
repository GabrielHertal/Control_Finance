using Control_Finance.Server.Enums;

namespace Control_Finance.Server.Services.Conta
{
    public interface IContasService
    {
        Task<ResultRequisitions> CreateContaAsync(string nome, TipoConta tipo_conta, int fkIdUser, bool ativo);
        Task<ResultRequisitions> UpdateContaByIdAsync(int id, string nome, TipoConta tipo_conta, bool ativo);
        Task<ResultRequisitions> DeleteContaByIdAsync(int id);
        Task<ResultRequisitions> GetContaByIdAsync(int id);
        Task<List<ResultRequisitions>> GetAllContasByUserIdAsync(int id);
    }
}