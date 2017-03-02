using System;

namespace EFExperiments.ConservationOfOrder
{
    public class Entity
    {
        private static readonly Random Random = new Random();

        private readonly int hashCode = Random.Next();

        public long Id { get; set; }

        public long InsertOrder { get; set; }

        /// <summary>
        /// This is a totally bullshit hashcode but this is just to make sure that the hashcode is not incremental if fields are incremental...
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return hashCode;
        }
    }
}