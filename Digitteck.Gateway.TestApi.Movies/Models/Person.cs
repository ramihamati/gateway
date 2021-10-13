using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Digitteck.Gateway.TestApi.Movies.Models
{
    public class Person : IEquatable<Person>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public bool Equals([AllowNull] Person other)
        {
            if (other is null)
            {
                return false;
            }

            return this.FirstName == other.FirstName && this.LastName == other.LastName;
        }

        public override bool Equals(object obj)
        {
            if (obj is Person person)
            {
                return this.Equals(person);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return $"{FirstName}{LastName}".GetHashCode();
        }

        /// <summary>
        /// The sets are equal and items in the same order
        /// </summary>
        public static bool StructuredEquals(IList<Person> set1, IList<Person> set2)
        {
            if (set1.Count != set2.Count)
            {
                return false;
            }

            for(int i = 0; i < set1.Count;i++)
            {
                if (!set1[i].Equals(set2[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
