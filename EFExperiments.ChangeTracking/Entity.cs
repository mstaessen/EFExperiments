using System;

namespace EFExperiments.ChangeTracking
{
    public class Entity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime BirthDate { get; set; }
    }
}