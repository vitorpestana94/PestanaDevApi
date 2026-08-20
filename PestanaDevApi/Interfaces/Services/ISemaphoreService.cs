namespace PestanaDevApi.Interfaces.Services
{
    public interface ISemaphoreService
    {
        Task<T> Work<T>(Func<Task<T>> callBack);
    }
}
