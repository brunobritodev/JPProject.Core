using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace JPProject.EntityFrameworkCore.Configuration
{
    public static class ContextHelpers
    {
        /// <summary>
        /// Check if data table is exist in application
        /// </summary>
        /// <typeparam name="T">Class of data table to check</typeparam>
        /// <param name="db">DB Object</param>
        private static async Task<bool> CheckTableExists<T>(DbContext db) where T : class
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
