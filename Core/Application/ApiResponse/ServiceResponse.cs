namespace VbtEgitimKampiMVC.Core.Application.ApiResponse
{
    public class ServiceResponse<T> : IServiceResponse<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "Operation completed successfully.";
        public List<string>? NotificationMessages { get; set; }
    }
}
