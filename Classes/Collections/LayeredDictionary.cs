using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace FCSG.Collections
{
    public class LayeredDictionary<IndexType,SideIndexType,DataType>
    {
        /// <summary>
        /// The dictionary which actually holds the values.
        /// </summary>
        private Dictionary<int, DataType> dataDictionary;
        private Dictionary<IndexType, int> mainIndexDictionary;
        private Dictionary<SideIndexType, int> sideIndexDictionary;

        private Dictionary<int, object?[]> internalDictionary; //The index is the int index; the int array is set as follows: int[0]=main id index, int[1]=side id index (null if no side index was registered)

        private int lastObjectId = 0;

        /// <summary>
        /// Build a new layered dictionary.
        /// </summary>
        public LayeredDictionary()
        {
            this.dataDictionary = new Dictionary<int, DataType>();
            this.mainIndexDictionary = new Dictionary<IndexType, int>();
            this.sideIndexDictionary = new Dictionary<SideIndexType, int>();

            this.internalDictionary = new Dictionary<int, object?[]>();
        }

        /// <summary>
        /// Add a value indexed by main index.
        /// </summary>
        /// <param name="mainIndex">The main index</param>
        /// <param name="value">The value to be added</param>
        public void Add(IndexType mainIndex, DataType value)
        {
            RemoveMatches(mainIndex);

            dataDictionary.Add(lastObjectId, value);
            mainIndexDictionary.Add(mainIndex, lastObjectId);
            internalDictionary.Add(lastObjectId, new object?[] { mainIndex, null });

            lastObjectId++;
        }

        /// <summary>
        /// Add a value indexed by both main and side index.
        /// </summary>
        /// <param name="mainIndex">The main index</param>
        /// <param name="sideIndex">The side index</param>
        /// <param name="value">The value to be added</param>
        public void Add(IndexType mainIndex, SideIndexType sideIndex, DataType value)
        {
            RemoveMatches(mainIndex, sideIndex);

            dataDictionary.Add(lastObjectId, value);
            mainIndexDictionary.Add(mainIndex, lastObjectId);
            sideIndexDictionary.Add(sideIndex, lastObjectId);

            internalDictionary.Add(lastObjectId, new object?[] { mainIndex, sideIndex });

            lastObjectId++;
        }

        /// <summary>
        /// Test whether the given index is present in the dictionary.
        /// </summary>
        /// <param name="mainIndex">The index to look for</param>
        /// <returns>true if the index is already in the dictionary, false otherwise.</returns>
        public bool ContainsKey(IndexType mainIndex)
        {
            if (mainIndexDictionary.ContainsKey(mainIndex))
            {
                return dataDictionary.ContainsKey(mainIndexDictionary[mainIndex]);
            }
            return false;
        }

        /// <summary>
        /// Test whether the given index is present in the dictionary.
        /// </summary>
        /// <param name="sideIndex">The index to look for</param>
        /// <returns>true if the index is already in the dictionary, false otherwise.</returns>
        public bool ContainsKey(SideIndexType sideIndex)
        {
            if (sideIndexDictionary.ContainsKey(sideIndex))
            {
                return dataDictionary.ContainsKey(sideIndexDictionary[sideIndex]);
            }
            return false;
        }
    
        public DataType this[IndexType index]
        {
            get
            {
                return dataDictionary[mainIndexDictionary[index]];
            }
        }

        public DataType this[SideIndexType index]
        {
            get
            {
                return dataDictionary[sideIndexDictionary[index]];
            }
        }
    
        /// <summary>
        /// Remove every match indexed with the given main index. This removes main and side index as well as the value.
        /// </summary>
        /// <param name="mainIndex">The main index whose match is to be removed.</param>
        public void RemoveMatches(IndexType mainIndex)
        {
            if (!ContainsKey(mainIndex))
            {
                return;
            }

            int index = mainIndexDictionary[mainIndex];

            object?[] objects = internalDictionary[index];
            internalDictionary.Remove(index);

            mainIndexDictionary.Remove((IndexType)objects[0]);

            if (objects[1] != null)
            {
                sideIndexDictionary.Remove((SideIndexType)objects[1]);
            }

            dataDictionary.Remove(index);
        }

        /// <summary>
        /// Remove every match indexed with the given main index and every match indexed with the given side index. This removes main and side index as well as the value.
        /// </summary>
        /// <param name="mainIndex">The main index whose match is to be removed.</param>
        /// <param name="sideIndex">The side index whose match is to be removed.</param>
        public void RemoveMatches(IndexType mainIndex, SideIndexType sideIndex)
        {
            RemoveMatches(mainIndex);

            if (!ContainsKey(sideIndex))
            {
                return;
            }

            int index = sideIndexDictionary[sideIndex];

            object?[] objects = internalDictionary[index];
            internalDictionary.Remove(index);

            mainIndexDictionary.Remove((IndexType)objects[0]);

            if (objects[1] != null)
            {
                sideIndexDictionary.Remove((SideIndexType)objects[1]);
            }

            dataDictionary.Remove(index);
        }

        /// <summary>
        /// Remove every match indexed with the given side index. This removes main and side index as well as the value.
        /// </summary>
        /// <param name="sideIndex">The main index whose match is to be removed.</param>
        public void RemoveMatches(SideIndexType sideIndex)
        {
            if (!ContainsKey(sideIndex))
            {
                return;
            }

            int index = sideIndexDictionary[sideIndex];
            object?[] objects = internalDictionary[index];
            internalDictionary.Remove(index);

            mainIndexDictionary.Remove((IndexType)objects[0]);

            if (objects[1] != null)
            {
                sideIndexDictionary.Remove((SideIndexType)objects[1]);
            }

            dataDictionary.Remove(index);
        }
    }
}
