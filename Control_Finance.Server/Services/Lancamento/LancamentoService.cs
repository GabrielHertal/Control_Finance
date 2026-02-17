using Control_Finance.Server.Data;
using Control_Finance.Server.DTO;
using Control_Finance.Server.Enums;
using Control_Finance.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace Control_Finance.Server.Services.Lancamento
{
    public class LancamentoService(AppDbContext context) : ILancamentoService
    {
        private readonly AppDbContext _context = context;
        public async Task<ResultRequisitions> CreateLancamentoAsync(string titulo, string descricao, decimal valor, DateOnly data_Lancamento, DateOnly? data_Vencimento, int? fk_Id_Categoria,
                                                                    decimal? valor_Pago, DateOnly? data_Pagamento, int fk_Id_User, int tipo_Lancamento, int fk_Id_Conta)
        {
            try
            {
                var lancamento = new Lancamentos
                {
                    Id = 0,
                    Titulo = titulo,
                    Descricao = descricao,
                    Valor = valor,
                    Data_Lancamento = data_Lancamento,
                    Data_Vencimento = data_Vencimento,
                    Fk_Id_Categoria = fk_Id_Categoria,
                    Valor_Pago = valor_Pago,
                    Data_Pagamento = data_Pagamento,
                    Fk_Id_User = fk_Id_User,
                    Tipo_Lancamento = (TipoLancamento)tipo_Lancamento,
                    Fk_Id_Conta = fk_Id_Conta,
                    Ativo = true
                };
                _context.Lancamentos.Add(lancamento);
                await _context.SaveChangesAsync();
                return new ResultRequisitions
                {
                    Success = true,
                    Code = (int)ResultsRequests.Success,
                    Message = "Lançamento criado com sucesso"
                };
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return new ResultRequisitions
                {
                    Success = false,
                    Code = (int)ResultsRequests.BadRequest,
                    Message = "Erro ao criar lançamento",
                    Data = ex
                };
            }
        }
        public async Task<ResultRequisitions> UpdateLancamentoByIdAsync(int id, string titulo, string descricao, decimal valor, DateOnly data_Lancamento, DateOnly? data_Vencimento, int? fk_Id_Categoria,
                                                                        decimal? valor_Pago, DateOnly? data_Pagamento, int fk_Id_User, int tipo_Lancamento, int fk_Id_Conta, bool ativo)
        {
            try
            {
                var lancamento = await _context.Lancamentos.FindAsync(id);
                if (lancamento == null)
                {
                    return new ResultRequisitions
                    {
                        Success = false,
                        Code = (int)ResultsRequests.NotFound,
                        Message = "Lançamento não encontrado"
                    };
                }
                lancamento.Titulo = titulo;
                lancamento.Descricao = descricao;
                lancamento.Valor = valor;
                lancamento.Data_Lancamento = data_Lancamento;
                lancamento.Data_Vencimento = data_Vencimento;
                lancamento.Fk_Id_Categoria = fk_Id_Categoria;
                lancamento.Valor_Pago = valor_Pago;
                lancamento.Data_Pagamento = data_Pagamento;
                lancamento.Fk_Id_User = fk_Id_User;
                lancamento.Tipo_Lancamento = (TipoLancamento)tipo_Lancamento;
                lancamento.Fk_Id_Conta = fk_Id_Conta;
                lancamento.Ativo = ativo;
                await _context.SaveChangesAsync();
                return new ResultRequisitions
                {
                    Success = true,
                    Code = (int)ResultsRequests.Success,
                    Message = "Lançamento atualizado com sucesso"
                };
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return new ResultRequisitions
                {
                    Success = false,
                    Code = (int)ResultsRequests.BadRequest,
                    Message = "Erro ao atualizar lançamento",
                    Data = ex
                };
            }
        }
        public async Task<ResultRequisitions> DeleteLancamentoByIdAsync(int id)
        {
            try
            {
                var lancamento = await _context.Lancamentos.FindAsync(id);
                if (lancamento == null)
                {
                    return new ResultRequisitions
                    {
                        Success = false,
                        Code = (int)ResultsRequests.NotFound,
                        Message = "Lançamento não encontrado"
                    };
                }
                lancamento.Ativo = false;
                await _context.SaveChangesAsync();
                return new ResultRequisitions
                {
                    Success = true,
                    Code = (int)ResultsRequests.Success,
                    Message = "Lançamento deletado com sucesso"
                };
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return new ResultRequisitions
                {
                    Success = false,
                    Code = (int)ResultsRequests.BadRequest,
                    Message = "Erro ao deletar lançamento",
                    Data = ex
                };
            }
        }
        public async Task<ResultRequisitions> GetLancamentoByIdAsync(int id)
        {
            try
            {
                var lancamento = await _context.Lancamentos
                                               .Where(l => l.Id == id)
                                               .Select(l => new LancamentoDTO
                                               {
                                                   Id = l.Id,
                                                   Titulo = l.Titulo,
                                                   Descricao = l.Descricao,
                                                   Valor = l.Valor,
                                                   Data_Lancamento = l.Data_Lancamento,
                                                   Data_Vencimento = l.Data_Vencimento,
                                                   Fk_Id_Categoria = l.Fk_Id_Categoria,
                                                   Valor_Pago = l.Valor_Pago,
                                                   Data_Pagamento = l.Data_Pagamento,
                                                   Fk_Id_User = l.Fk_Id_User,
                                                   Tipo_Lancamento = (TipoLancamento)l.Tipo_Lancamento,
                                                   Fk_Id_Conta = l.Fk_Id_Conta,
                                                   Ativo = l.Ativo
                                               }).FirstOrDefaultAsync();

                return new ResultRequisitions
                {
                    Success = true,
                    Code = (int)ResultsRequests.Success,
                    Message = "Lançamento encontrado com sucesso",
                    Data = lancamento
                };
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return new ResultRequisitions
                {
                    Success = false,
                    Code = (int)ResultsRequests.BadRequest,
                    Message = "Erro ao buscar lançamento",
                    Data = ex
                };
            }
        }
        public async Task<List<ResultRequisitions>> GetAllLancamentosByUserIdAsync(int id)
        {
            try
            {
                var lancamento = await _context.Lancamentos
                                               .Where(l => l.Fk_Id_User == id)
                                               .ToListAsync();
                if (lancamento.Count == 0)
                {
                    return new List<ResultRequisitions>
                    {
                        new ResultRequisitions
                        {
                            Success = false,
                            Code = (int)ResultsRequests.NotFound,
                            Message = "Nenhum lançamento encontrado para esse usuário"
                        }
                    };
                }
                return new List<ResultRequisitions>
                {
                    new ResultRequisitions
                    {
                        Success = true,
                        Code = (int)ResultsRequests.Success,
                        Message = "Lançamentos encontrados com sucesso",
                        Data = lancamento.Select(l => new LancamentoDTO
                        {
                            Id = l.Id,
                            Titulo = l.Titulo,
                            Descricao = l.Descricao,
                            Valor = l.Valor,
                            Data_Lancamento = l.Data_Lancamento,
                            Data_Vencimento = l.Data_Vencimento,
                            Fk_Id_Categoria = l.Fk_Id_Categoria,
                            Valor_Pago = l.Valor_Pago,
                            Data_Pagamento = l.Data_Pagamento,
                            Fk_Id_User = l.Fk_Id_User,
                            Tipo_Lancamento = (TipoLancamento)l.Tipo_Lancamento,
                            Fk_Id_Conta = l.Fk_Id_Conta,
                            Ativo = l.Ativo
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
                        Code = (int)ResultsRequests.BadRequest,
                        Message = "Erro ao buscar lançamentos",
                        Data = ex
                    }
                };
            }
        }
    }
}