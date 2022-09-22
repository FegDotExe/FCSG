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
    public struct ValueVector2<T1, T2>: IEquatable<ValueVector2<T1, T2>>
    {
        public T1 X;
        public T2 Y;

        public ValueVector2(T1 xValue, T2 yValue)
        {
            X = xValue;
            Y = yValue;
        }

        /// <summary>
        /// Determines whether the current object is equal to the given object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(ValueVector2<T1, T2>))
            {
                ValueVector2<T1, T2> valueVector = (ValueVector2<T1, T2>)obj;

                if (!this.X.Equals(valueVector.X)) 
                {
                    return false;
                }
                if (!this.Y.Equals(valueVector.Y))
                {
                    return false;
                }
            }

            return false;
        }

        public bool Equals(ValueVector2<T1,T2> valueVector)
        {
            return X.Equals(valueVector.X) && Y.Equals(valueVector.Y);
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(X);
            hash.Add(Y);
            return hash.ToHashCode();
        }

        public override string ToString()
        {
            return "vv[" + X + "," + Y + "]";
        }
    }
}
