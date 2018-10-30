﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IReadOnlyListLinq.Tests.Unit
{
    public abstract class IReadOnlyListLinqTests
    {
        protected static bool IsEven(int num) => num % 2 == 0;

        protected class AnagramEqualityComparer : IEqualityComparer<string>
        {
            public bool Equals(string x, string y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x == null | y == null) return false;
                int length = x.Length;
                if (length != y.Length) return false;
                using (var en = x.OrderBy(i => i).GetEnumerator())
                {
                    foreach (char c in y.OrderBy(i => i))
                    {
                        en.MoveNext();
                        if (c != en.Current) return false;
                    }
                }
                return true;
            }

            public int GetHashCode(string obj)
            {
                if (obj == null) return 0;
                int hash = obj.Length;
                foreach (char c in obj)
                    hash ^= c;
                return hash;
            }
        }

        protected struct StringWithIntArray
        {
            public string name { get; set; }
            public int?[] total { get; set; }
        }


        protected static List<Func<IReadOnlyList<T>, IReadOnlyList<T>>> IdentityTransforms<T>()
        {
            // All of these transforms should take an enumerable and produce
            // another enumerable with the same contents.
            return new List<Func<IReadOnlyList<T>, IReadOnlyList<T>>>
            {
                e => e,
                e => e.ToArray(),
                e => e.ToList(),
                e => e.Select(i => i),
            };
        }
    }
}
