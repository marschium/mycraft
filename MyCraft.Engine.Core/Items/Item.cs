using MyCraft.Assets;
using MyCraft.Engine.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.Core.Items
{
    /// <summary>
    /// Represents an item held in the inventory
    /// </summary>
    public abstract class Item
    {
        public Item()
        {
        }        

        public virtual TextureName InventoryIconTexture { get; protected set; } = TextureName.Debug;

        // TODO name

        /// <summary>
        /// Owner of the item has used it.
        /// </summary>
        public abstract void Use(GameObject owner);
    }
}
