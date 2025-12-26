using System.Data;

namespace PestanaDevApi.Repositories
{
    public class DefaultRepository
    {
        protected readonly IDbConnection _db;

        public DefaultRepository(IDbConnection dbConnection)
        {
            _db = dbConnection;

            if (_db.State == ConnectionState.Broken || _db.State == ConnectionState.Closed)
                _db.Open();
        }

        public void Dispose()
        {
            if (_db != null)
                _db.Dispose();
        }
    }
}
