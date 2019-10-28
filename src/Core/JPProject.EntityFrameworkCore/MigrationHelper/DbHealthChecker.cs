using Microsoft.EntityFrameworkCore;

namespace JPProject.EntityFrameworkCore.MigrationHelper
{
    public class DbHealthChecker
    {
        public bool TestConnection(DbContext context)
        {

            try
            {
                context.Database.CanConnect();   // Check the database connection

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}