using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace JPProject.Admin.Infra.Data.MigrationHelper
{
    public class DbHealthChecker
    {
        public async Task<bool> TestConnection(DbContext context)
        {

            try
            {
                await context.Database.CanConnectAsync();   // Check the database connection

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}