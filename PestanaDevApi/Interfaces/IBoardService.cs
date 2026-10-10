namespace PestanaDevApi.Interfaces
{
    public interface IBoardService
    {
        Task CreateBoard(Guid userId);
    }
}
