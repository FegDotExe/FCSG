using System;
using System.Collections.Generic;
using System.Text;

namespace FCSG
{
    /// <summary>
    /// A vector of two values of chosen type
    /// </summary>
    /// <typeparam name="T1">The type of the first value</typeparam>
    /// <typeparam name="T2">The type of the second value</typeparam>
    public class ValueVector<T1, T2>
    {
        public T1 Value1;
        public T2 Value2;

        public ValueVector(T1 value1, T2 value2)
        {
            Value1 = value1;
            Value2 = value2;
        }
    }
}
