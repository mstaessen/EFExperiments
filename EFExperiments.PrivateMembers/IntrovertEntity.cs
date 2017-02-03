using System;

namespace EFExperiments.PrivateMembers
{
    public class IntrovertEntity
    {
        /// <summary>
        /// private setters work, read-only properties don't. 
        /// public Guid Id { get; } will make EF say that you don't have a Key defined for the type.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// EF can hydrate private properties.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Entities MUST have parameterless constructors, but they don't have to be public. 
        /// It can be protected, and it can even be private!
        /// 
        /// Note: protected is probably the more pragmatic solution towards testing and other ORMs...
        /// </summary>
        private IntrovertEntity()
        {
            
        }

        public IntrovertEntity(string name) : this()
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public void Rename(string name)
        {
            Name = name;
        }
    }
}