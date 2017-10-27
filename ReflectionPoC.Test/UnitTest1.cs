using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using EntityFramework.MappingAPI.Extensions;
using FluentAssertions;
using ReflectionPoC.Entities;
using ReflectionPoC.Services;
using Xunit;

namespace ReflectionPoC.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Launch_Generate_Context()
        {
            using (var context = GetNewContext())
            {
                SeedData(context, 100);
                context.SaveChanges();

                context.CompaniesImportations.Count().Should().Be(100);
                context.CustomersImportations.Count().Should().Be(100);

                var type = typeof(CompanyImportation);
                var customerTableName = context.Db(type).TableName;
                customerTableName.Should().Be("_Importation_Companies");

                context.SaveChanges();
            }
        }

        [Fact]
        public void Instance_Of_Type_Retrieving_All_Entities()
        {
            var items = GetImplementationsOfIImportationEntity();
            items.Count().Should().Be(2);
        }

        [Fact]
        public void RemoveData_Method_Remove_All_Items()
        {
            var context = GetNewContext();
            SeedData(context, 100);
            context.SaveChanges();

            var items = GetImplementationsOfIImportationEntity();
            foreach (Type item in items)
            {
                MethodInfo methodInfo = context.GetType().GetMethods().Single(p => p.Name == nameof(DbContext.Set) && p.IsGenericMethod);
                MethodInfo genericMethod = methodInfo.MakeGenericMethod(item);
                var invoke = genericMethod.Invoke(context, null);
                InvokeRemoveData(invoke, item);
            }

            context.SaveChanges();
            context.CompaniesImportations.Count().Should().Be(0);
            context.CustomersImportations.Count().Should().Be(0);
        }

        [Fact]
        public void Bazz()
        {
            var microsoft = CompanyImportation(name: "Microsoft", creationDate: DateTime.Today.Date);
            var apple = CompanyImportation(name: "Apple", creationDate: DateTime.Today.Date);
            var companies = Filter(microsoft, apple);

            companies.Length.Should().Be(0);
        }

        private CompanyImportation[] Filter(params CompanyImportation[] companiesImportations)
        {
            return new List<CompanyImportation>().ToArray();
        }

        private void InvokeRemoveData(object dbSet, Type item)
        {
            MethodInfo methodInfo = typeof(ImportationTablesRemoverService).GetMethods().Single(p => p.Name == nameof(ImportationTablesRemoverService.RemoveData) && p.IsStatic && p.IsGenericMethod);
            var method = methodInfo.MakeGenericMethod(item);
            method.Invoke(null, new[]
            {
                dbSet,
                DateTime.UtcNow.Date
            });
        }

        private static IEnumerable<Type> GetImplementationsOfIImportationEntity()
        {
            var type = typeof(IImportationEntity);
            var items = from t in Assembly.GetAssembly(type).GetTypes()
                        where typeof(IImportationEntity).IsAssignableFrom(t) && t.IsClass
                        select t;
            return items;
        }

        private static OwnContext GetNewContext()
        {
            return new OwnContext();
        }

        private static void SeedData(OwnContext context, int numberOfItems)
        {
            for (var i = 0; i < numberOfItems; i++)
            {
                var company = CompanyImportation(name: $"Company {i}", creationDate: DateTime.Today.Date);
                context.CompaniesImportations.Add(company);

                var customer = CustomerImportation(name: $"Customer {i}", creationDate: DateTime.Today.Date);
                context.CustomersImportations.Add(customer);
            }
        }

        private static CustomerImportation CustomerImportation(string name, DateTime creationDate)
        {
            return new CustomerImportation { Id = Guid.NewGuid(), Name = name, CreationDate = creationDate };
        }

        private static CompanyImportation CompanyImportation(string name, DateTime creationDate)
        {
            return new CompanyImportation { Id = Guid.NewGuid(), Name = name, CreationDate = creationDate };
        }
    }
}
