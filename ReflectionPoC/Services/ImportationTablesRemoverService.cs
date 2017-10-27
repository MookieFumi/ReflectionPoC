using System;
using System.Data.Entity;
using ReflectionPoC.Entities;
using System.Linq;

namespace ReflectionPoC.Services
{
    public static class ImportationTablesRemoverService
    {
        public static void RemoveData<T>(DbSet<T> dbSet, DateTime date) where T : class, IImportationEntity
        {
            var items = dbSet.Where(p => p.CreationDate <= date.Date);
            dbSet.RemoveRange(items);
        }
    }
}