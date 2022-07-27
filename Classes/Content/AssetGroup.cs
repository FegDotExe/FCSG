using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;


namespace FCSG
{
    public class AssetGroup
    {
        private Dictionary<string, ContentType> assets;

        public int size
        {
            get { return assets.Count; }
        }

        public AssetGroup()
        {
            assets= new Dictionary<string, ContentType>();
        }

        public void Add(string name, ContentType type)
        {
            assets.Add(name, type);
        }

        public void Add(string name, string typeString)
        {
            ContentType type=stringToType(typeString);

            Add(name, type);
        }

        /// <summary>
        /// Convert a string into the corresponding content type.
        /// </summary>
        /// <param name="typeString"></param> The string to be converted.
        /// <returns>The type corresponding to the given string</returns>
        /// <exception cref="ArgumentException"></exception>
        public static ContentType stringToType(string typeString)
        {
            if (typeString == "Texture2D")
            {
                return ContentType.Texture2D;
            }//Add new types here
            else
            {
                throw new Exception(typeString+" is not a valid content type.");
            }
        }

        /// <summary>
        /// Load the content at the given index of this asset group
        /// </summary>
        /// <param name="index"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="Exception"></exception>
        public ContentElement LoadIndex(int index, Microsoft.Xna.Framework.Content.ContentManager content) //Loads content at given index
        {
            if(index<0 || index >= size)
            {
                throw new ArgumentOutOfRangeException();
            }

            string name = assets.ElementAt(index).Key;
            ContentType type=assets.ElementAt(index).Value;

            if(type == ContentType.Texture2D)
            {
                return new ContentElement(name,content.Load<Texture2D>(name));
            }

            throw new Exception("The type "+type+" is unhandled. In order to remove this error, add type handling in AssetGroup.LoadIndex.");
        }

        public string NameByIndex(int index)
        {
            if (index < 0 || index >= size)
            {
                throw new ArgumentOutOfRangeException();
            }
            return assets.ElementAt(index).Key;
        }

        public override string ToString()
        {
            return assets.ToString();
        }
    }

    public enum ContentType
    {
        Texture2D
    }

    public class ContentElement
    {
        public string name;
        public object value;

        public ContentElement(string name, object value)
        {
            this.name = name;
            this.value = value;
        }
    }
}
