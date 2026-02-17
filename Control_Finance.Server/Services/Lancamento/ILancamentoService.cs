namespace Control_Finance.Server.Services.Lancamento
{
    public interface ILancamentoService
    {
        Task<ResultRequisitions> CreateLancamentoAsync(string titulo, string descricao, decimal valor, DateOnly data_Lancamento, DateOnly? data_Vencimento, int? fk_Id_Categoria
                                                        , decimal? valor_Pago, DateOnly? data_Pagamento, int fk_Id_User, int tipo_Lancamento, int fk_Id_Conta);
        Task<ResultRequisitions> UpdateLancamentoByIdAsync(int id, string titulo, string descricao, decimal valor, DateOnly data_Lancamento, DateOnly? data_Vencimento, int? fk_Id_Categoria
                                                        , decimal? valor_Pago, DateOnly? data_Pagamento, int fk_Id_User, int tipo_Lancamento, int fk_Id_Conta, bool ativo);
        Task<ResultRequisitions> DeleteLancamentoByIdAsync(int id);
        Task<ResultRequisitions> GetLancamentoByIdAsync(int id);
        Task<List<ResultRequisitions>> GetAllLancamentosByUserIdAsync(int id);
    }
}