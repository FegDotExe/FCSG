using System;
using System.Collections.Generic;
using System.Text;

namespace FCSG
{
    public class ValueVector3<T1,T2, T3>
    {
        public T1 X;
        public T2 Y;
        public T3 Z;

        public ValueVector3(T1 x, T2 y, T3 z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public ValueVector3() { }

        public override string ToString()
        {
            return "vv[" + X + "," + Y + "," + Z + "]";
        }
    }

    public class ValueVector3<T>: ValueVector3<T, T, T> {
        public ValueVector3(T x, T y, T z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public ValueVector3() { }
    }
}
