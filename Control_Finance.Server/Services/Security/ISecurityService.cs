using Control_Finance.Server.DTO;

namespace Control_Finance.Server.Services.Security
{
    public interface ISecurityService
    {
        Task<ResultRequisitions> GetUserInformationById(int? id);
        Task<ResultRequisitions> GetUserInformationByEmail(string email);
    }
}