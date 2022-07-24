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
        private Dictionary<string, Object> loadedContent;

        public Loader(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            assetGroups = new Dictionary<string, AssetGroup>();
            this.content = content;
        }

        /// <summary>
        /// Loads an asset group by its <c>assetName</c> and adds it to the asset groups contained in this loader.
        /// </summary>
        /// <param name="assetGroup"></param> The assetName of the asset group about to be added.
        public void Add(string assetGroup)
        {
            AssetGroup newAssetGroup = content.Load<AssetGroup>(assetGroup);

            assetGroups.Add(assetGroup, newAssetGroup);
        }

        /// <summary>
        /// Loads all the content contained in the given asset group.
        /// </summary>
        /// <param name="assetGroupName"></param> The <c>assetName</c> of the asset group whose content is to be loaded.
        /// <exception cref="ArgumentException"></exception> Thrown if the <c>assetName</c> is not valid.
        public void LoadAll(string assetGroupName)
        {
            AssetGroup assetGroup = assetGroups[assetGroupName];
            if(assetGroup == null)
            {
                throw new ArgumentException("The asset group \""+assetGroupName+"\" does not exist.");
            }

            for(int i=0; i<assetGroup.size; i++)
            {
                ContentElement contentElement = assetGroup.LoadIndex(i,content);
                loadedContent.Add(contentElement.name, contentElement.value);
            }
        }

        public object this[string str]
        {
            get=>loadedContent[str];
        }
    }
}
