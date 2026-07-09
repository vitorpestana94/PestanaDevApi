using System.Data;

namespace PestanaDevApi.Interfaces.Factories
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
