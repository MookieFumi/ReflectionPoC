using System;

namespace ReflectionPoC.Entities
{
    public interface IImportationEntity
    {
        Guid Id { get; set; }
        DateTime CreationDate { get; set; }
    }
}