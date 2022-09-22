using System;
using System.Collections.Generic;
using System.Text;

namespace FCSG.Collections
{
    public class Dictionary3D<Keys,Value> where Keys : IComparable<Keys>
    {
        private Dictionary<Keys,Dictionary<Keys,Dictionary<Keys,Value>>> dictionary;

        public Dictionary3D()
        {
            dictionary = new Dictionary<Keys, Dictionary<Keys, Dictionary<Keys, Value>>>();
        }

        public Value this[Keys k1, Keys k2, Keys k3]
        {
            get
            {
                if (dictionary.ContainsKey(k1))
                {
                    Dictionary<Keys, Dictionary<Keys, Value>> dictY = dictionary[k1];
                    if (dictY.ContainsKey(k2))
                    {
                        Dictionary<Keys, Value> dictZ = dictY[k2];
                        if (dictZ.ContainsKey(k3))
                        {
                            return dictZ[k3];
                        }
                    }
                }

                throw new KeyNotFoundException();
            }

            set
            {
                if (!dictionary.ContainsKey(k1)) //The y dictionary is already present
                {
                    dictionary.Add(k1, new Dictionary<Keys, Dictionary<Keys, Value>>());
                }

                Dictionary<Keys, Dictionary<Keys, Value>> dictY = dictionary[k1];

                if (!dictY.ContainsKey(k2))
                {
                    dictY.Add(k2, new Dictionary<Keys, Value>());
                }

                Dictionary<Keys,Value> dictZ = dictY[k2];

                dictZ[k3] = value;
            }
        }
    
        public bool ContainsKeys(Keys k1, Keys k2, Keys k3)
        {
            if (dictionary.ContainsKey(k1))
            {
                Dictionary<Keys, Dictionary<Keys, Value>> dictY = dictionary[k1];
                if (dictY.ContainsKey(k2))
                {
                    Dictionary<Keys, Value> dictZ = dictY[k2];
                    if (dictZ.ContainsKey(k3))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    
        private List<KeyValuePair3D<Keys, Value>> GetRange(Keys kA1, Keys kA2, Keys kA3, Keys kB1, Keys kB2, Keys kB3, bool checkX, bool checkY, bool checkZ)
        {
            Dictionary3D<Keys, Value> output = new Dictionary3D<Keys, Value>();

            foreach (KeyValuePair<Keys, Dictionary<Keys, Dictionary<Keys, Value>>> kvpX in dictionary)
            {
                if (!checkX || (checkX && kvpX.Key.CompareTo(kA1) >= 0 && kvpX.Key.CompareTo(kB1) <= 0)) 
                {
                    Dictionary<Keys, Dictionary<Keys, Value>> selectedYDict = dictionary[kvpX.Key];

                    foreach (KeyValuePair<Keys, Dictionary<Keys, Value>> kvpY in selectedYDict)
                    {
                        if (!checkY || (checkY && kvpY.Key.CompareTo(kA2) >= 0 && kvpY.Key.CompareTo(kB2) <= 0))
                        {
                            Dictionary<Keys, Value> selectedZDict = selectedYDict[kvpY.Key];

                            foreach (KeyValuePair<Keys, Value> kvpZ in selectedZDict)
                            {
                                if (!checkZ || (checkZ && kvpZ.Key.CompareTo(kA3) >= 0 && kvpZ.Key.CompareTo(kB3) <= 0))
                                    output[kvpX.Key, kvpY.Key, kvpZ.Key] = kvpZ.Value;
                            }
                        }
                    }
                }
            }

            return output.GetKeyPairs();
        }
    
        public List<KeyValuePair3D<Keys, Value>> GetRange(Keys kA1, Keys kB1, char axis)
        {
            if (axis == 'x')
            {
                return GetRange(kA1, default(Keys), default(Keys), kB1, default(Keys), default(Keys), true, false, false);
            }
            else if(axis == 'y')
            {
                return GetRange(default(Keys), kA1, default(Keys), default(Keys), kB1, default(Keys), false, true, false);
            }
            else if(axis == 'z')
            {
                return GetRange(default(Keys), default(Keys), kA1, default(Keys), default(Keys), kB1, false, false, true);
            }
            else
            {
                throw new ArgumentException("The given axis character (" + axis + ") was neither x, y or z.");
            }
        }

        public List<KeyValuePair3D<Keys, Value>> GetRange(Keys kA1, Keys kA2, Keys kB1, Keys kB2, string axis)
        {
            if (axis.Length != 2)
            {
                throw new ArgumentException("The given axis string should only contain two characters, either x, y or z.");
            }

            if (!axis.Contains('x') && axis.Contains('y') && axis.Contains('z'))
            {
                return GetRange(default(Keys), kA1, kA2, default(Keys), kB1, kB2, axis);
            }else if (axis.Contains('x') && !axis.Contains('y') && axis.Contains('z'))
            {
                return GetRange(kA1, default(Keys), kA2, kB1, default(Keys), kB2, axis);
            }else if(axis.Contains('x') && axis.Contains('y') && !axis.Contains('z'))
            {
                return GetRange(kA1, kA2, default(Keys), kB1, kB2, default(Keys), axis);
            }
            else
            {
                throw new ArgumentException("The given axis string did not contain two characters between x, y and z.");
            }
        }

        public List<KeyValuePair3D<Keys,Value>> GetRange(Keys kAx, Keys kAy, Keys kAz, Keys kBx, Keys kBy, Keys kBz, string axis)
        {
            Keys x1 = default(Keys);
            Keys x2 = default(Keys);
            Keys y1 = default(Keys);
            Keys y2 = default(Keys);
            Keys z1 = default(Keys);
            Keys z2 = default(Keys);

            bool checkX = false;
            bool checkY = false;
            bool checkZ = false;

            if (axis.Contains('x'))
            {
                x1 = kAx;
                x2 = kBx;
                checkX = true;
            }
            if (axis.Contains('y'))
            {
                y1 = kAy;
                y2 = kBy;
                checkY = true;
            }
            if (axis.Contains('z'))
            {
                z1 = kAz;
                z2 = kBz;
                checkZ = true;
            }

            return GetRange(x1, y1, z1, x2, y2, z2, checkX, checkY, checkZ);
        }

        public List<KeyValuePair3D<Keys, Value>> GetKeyPairs()
        {
            List<KeyValuePair3D<Keys, Value>> output = new List<KeyValuePair3D<Keys, Value>>();

            foreach(KeyValuePair<Keys, Dictionary<Keys, Dictionary<Keys, Value>>> kvpX in dictionary)
            {
                foreach(KeyValuePair<Keys, Dictionary<Keys, Value>> kvpY in dictionary[kvpX.Key])
                {
                    foreach(KeyValuePair<Keys,Value> kvpZ in dictionary[kvpX.Key][kvpY.Key])
                    {
                        output.Add(new KeyValuePair3D<Keys, Value>(kvpX.Key,kvpY.Key,kvpZ.Key,kvpZ.Value));
                    }
                }
            }

            return output;
        }
    }

    public class KeyValuePair3D<Keys, ValueType>
    {
        public readonly Keys Key1;
        public readonly Keys Key2;
        public readonly Keys Key3;

        public readonly ValueType Value;

        public KeyValuePair3D(Keys key1, Keys key2, Keys key3, ValueType value)
        {
            Key1 = key1;
            Key2 = key2;
            Key3 = key3;
            Value = value;
        }
    }
}
