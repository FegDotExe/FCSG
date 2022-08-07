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
    public class ValueVector2<T1, T2>
    {
        public T1 X;
        public T2 Y;

        public ValueVector2(T1 xValue, T2 yValue)
        {
            X = xValue;
            Y = yValue;
        }

        public ValueVector2(){}

        public override string ToString()
        {
            return "vv[" + X + "," + Y + "]";
        }
    }

    public class ValueVector2<T>: ValueVector2<T, T> {
        public ValueVector2(T xValue, T yValue)
        {
            X = xValue;
            Y = yValue;
        }

        public ValueVector2() { }
    }
}
