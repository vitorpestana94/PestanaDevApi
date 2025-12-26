using System.Data;

namespace PestanaDevApi.Repositories
{
    public class GenericRepository
    {
        protected readonly IDbConnection _db;

        public GenericRepository(IDbConnection dbConnection)
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
