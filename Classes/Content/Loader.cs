using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FCSG
{
    public class Loader
    {
        private Dictionary<string, AssetGroup> assetGroups;
        private Microsoft.Xna.Framework.Content.ContentManager content;
        private Dictionary<string, LoadedObject> loadedContent;

        public Loader(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            assetGroups = new Dictionary<string, AssetGroup>();
            this.content = content;
            loadedContent = new Dictionary<string, LoadedObject>();
        }

        /// <summary>
        /// Loads an asset group by its <c>assetName</c> and adds it to the asset groups contained in this loader.
        /// </summary>
        /// <param name="assetGroupName"></param> The assetName of the asset group about to be added.
        public void Add(string assetGroupName)
        {
            AssetGroup newAssetGroup = content.Load<AssetGroup>(assetGroupName);

            assetGroups.Add(assetGroupName, newAssetGroup);
        }

        /// <summary>
        /// Loads all the content contained in the given asset group.
        /// </summary>
        /// <param name="assetGroupName">The <c>assetName</c> of the asset group whose content is to be loaded.</param>
        /// <exception cref="ArgumentException">Thrown if the <c>assetName</c> is not valid.</exception>
        public void LoadAll(string assetGroupName)
        {
            AssetGroup assetGroup = assetGroups[assetGroupName];

            for(int i=0; i<assetGroup.size; i++)
            {
                ContentElement contentElement = assetGroup.LoadIndex(i,content);
                if (loadedContent.ContainsKey(contentElement.name))
                {
                    LoadedObject loadedObject = loadedContent[contentElement.name];
                    loadedObject.loadedTimes++;
                }
                else
                {
                    loadedContent.Add(contentElement.name, new LoadedObject(contentElement.value));
                }
            }
        }
        /// <summary>
        /// Tries to unload all the assets of an assetgroup. If the assets were only loaded through one assetgroup, this will unload them. Otherwise, if they were loaded by more than one assetgroup, this will decrease the <c>loadedTimes</c> value of the loaded object.
        /// </summary>
        /// <param name="assetGroupName">The name of the assetGroup whose content is to be removed</param>
        public void UnloadAll(string assetGroupName)
        {
            AssetGroup assetGroup = assetGroups[assetGroupName];

            for(int i=0; i<assetGroup.size; i++)
            {
                string name = assetGroup.NameByIndex(i);
                if (loadedContent.ContainsKey(name))
                {
                    LoadedObject value=loadedContent[name];
                    value.loadedTimes--;
                    if (value.loadedTimes <= 0)
                    {
                        loadedContent.Remove(name);
                    }
                }
            }
        }

        /// <summary>
        /// Loads the asset group with the given name, adds it to this loader and loads all its content; this function calls <c>Loader.Add()</c> and <c>Loader.LoadAll()</c>
        /// </summary>
        /// <param name="assetGroupName">The <c>assetName</c> of the asset group which is going to be added and whose content is to be loaded.</param>
        public void AddAndLoadAll(string assetGroupName)
        {
            Add(assetGroupName);
            LoadAll(assetGroupName);
        }

        public object this[string str]
        {
            get=>loadedContent[str].value;
        }

        public override string ToString()
        {
            return loadedContent.ToString();
        }

        private class LoadedObject
        {
            public object value;
            public int loadedTimes; //How many times this object has been loaded. The object is unloaded only when this goes down to 0.

            /// <summary>
            /// Constructs a loadedObject with a <c>loadedTimes</c> value of 1.
            /// </summary>
            /// <param name="value"></param>
            public LoadedObject(object value)
            {
                this.value = value;
                this.loadedTimes = 1;
            }

            public override string ToString()
            {
                return value+"*"+loadedTimes;
            }
        }
    }
}
