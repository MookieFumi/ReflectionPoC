using System;

namespace ReflectionPoC.Entities
{
    public class CompanyImportation : IImportationEntity
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
    }
}