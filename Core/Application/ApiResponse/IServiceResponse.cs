namespace VbtEgitimKampiMVC.Core.Application.ApiResponse
{
    public interface IServiceResponse<T>
    {
        T Data { get; set; }
        bool Success { get; set; }
        string Message { get; set; }
        List<string> NotificationMessages { get; set; }
    }
}
