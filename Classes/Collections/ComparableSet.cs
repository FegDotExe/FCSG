using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace FCSG.Collections
{
    /// <summary>
    /// A high-perfomance set of unique values which uses <see cref="IComparable.CompareTo(object?)"/> to determine uniqueness.
    /// </summary>
    /// <typeparam name="T">A comparable type</typeparam>
    public class ComparableSet<T> where T: IComparable<T>
    {
        private List<T> list;

        public ComparableSet()
        {
            list = new List<T>();
        }

        /// <summary>
        /// Try adding a value to the set, if not already present.
        /// </summary>
        /// <param name="value">The value to be added</param>
        /// <returns>Whether the value was correctly added or not.</returns>
        public bool Add(T value)
        {
            int output=Array.BinarySearch(list.ToArray(), value);

            if (output >= 0)
            {
                return false;
            }
            else
            {
                int insertPosition = ~output; //Bitwise one complement of the output value.
                list.Insert(insertPosition, value);
                return true;
            }
        }

        /// <summary>
        /// Test if this set contains a certain value.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <returns>Whether the set contains the given value or not.</returns>
        public bool Contains(T value)
        {
            int output = Array.BinarySearch(list.ToArray(), value);
            return output >= 0;
        }

        public override string ToString()
        {
            string output = "{";
            int i = 0;
            foreach(T value in list)
            {
                if (i != list.Count - 1)
                {
                    output += value.ToString() + ",";
                }
                else
                {
                    output += value.ToString()+ "}";
                }

                i++;
            }

            return output;
        }
    }
}
