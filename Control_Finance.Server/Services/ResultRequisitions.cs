using Control_Finance.Server.Enums;

namespace Control_Finance.Server.Services
{
    public class ResultRequisitions
    {
        public bool Success { get; set; }
        public ResultsRequests Code { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; } = default(object?);
    }
}