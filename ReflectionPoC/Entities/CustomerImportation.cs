using System;

namespace ReflectionPoC.Entities
{
    public class CustomerImportation : IImportationEntity
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
    }
}