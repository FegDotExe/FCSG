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
            ContentType type;

            if (typeString == "Texture2D")
            {
                type = ContentType.Texture2D;
            }//Add new types here
            else
            {
                throw new ArgumentException();
            }

            Add(name, type);
        }

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


    }

    public enum ContentType
    {
        Texture2D
    }

    public class ContentElement
    {
        public string name;
        public Object value;

        public ContentElement(string name, object value)
        {
            this.name = name;
            this.value = value;
        }
    }
}
