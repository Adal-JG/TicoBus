namespace TicoBus.MAUI.Interfaces
{
    public interface IApiService
    {
        Task<T?> GetAsync<T>(string url);
        Task<T?> PostAsync<T>(string url, object data);
    }
}
