using JPProject.Domain.Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace JPProject.EntityFrameworkCore.MigrationHelper
{
    public class DbHealthChecker
    {
        public static async Task TestConnection(DbContext context)
        {
            var maxAttemps = 3;
            var delay = 5000;

            for (int i = 0; i < maxAttemps; i++)
            {
                var canConnect = CanConnect(context);
                if (canConnect)
                {
                    return;
                }
                await Task.Delay(delay);
            }

            // after a few attemps we give up
            throw new DatabaseNotFoundException("Error wating database. Check ConnectionString and ensure database exist");
        }

        private static bool CanConnect(DbContext context)
        {
            try
            {
                context.Database.CanConnect(); // Check the database connection

                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Check if data table is exist in application
        /// </summary>
        /// <typeparam name="T">Class of data table to check</typeparam>
        /// <param name="db">DB Object</param>
        public static async Task<bool> CheckTableExists<T>(DbContext db) where T : class
        {
            try
            {
                await db.Set<T>().AnyAsync();
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}