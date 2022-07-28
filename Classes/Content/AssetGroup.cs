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
        /// <summary>
        /// A class made to be able to convert string to <c>Type</c>s and to <c>ContentType</c>s.
        /// </summary>
        private class TypeConverter
        {
            public string typeString;
            public LoadDelegate loadDelegate;
            public ContentType contentType;

            /// <summary>
            /// Build a new <c>TypeConverter</c>
            /// </summary>
            /// <param name="typeString">The string by which the type will be characterized</param>
            /// <param name="loadDelegate">A delegate which should be called when trying to load an object of the type of this <c>TypeConverter</c></param>
            /// <param name="contentType">The content type which will represent this <c>TypeConverter</c></param>
            public TypeConverter(string typeString, LoadDelegate loadDelegate, ContentType contentType)
            {
                this.typeString = typeString;
                this.loadDelegate = loadDelegate;
                this.contentType = contentType;
            }
        }
        private Dictionary<string, ContentType> assets;

        private static TypeConverter[] typeConverterArray = { //TODO: add new types here.
            new TypeConverter("Texture2D",(string name, ContentManager content)=>content.Load<Texture2D>(name),ContentType.Texture2D),
            new TypeConverter("SpriteFont",(string name, ContentManager content)=>content.Load<SpriteFont>(name),ContentType.Texture2D)
        };

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
            foreach(TypeConverter typeConverter in typeConverterArray)
            {
                if(typeConverter.typeString == typeString)
                {
                    return typeConverter.contentType;
                }
            }

            throw new Exception(typeString+" is not a valid content type.");
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

            foreach(TypeConverter converter in typeConverterArray)
            {
                if (converter.contentType == type)
                {
                    return new ContentElement(name, converter.loadDelegate(name,content));
                }
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
        Texture2D,
        SpriteFont
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
