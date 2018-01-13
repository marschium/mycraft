using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Assets
{
    /// <summary>
    /// For now just a collection of file -> texture name -> position
    /// </summary>
    public class TextureInfo
    {
        public IDictionary<string, IDictionary<string, TexturePosition>> TexutrePositions = new Dictionary<string, IDictionary<string, TexturePosition>>
            {
                {
                    "atlas.png", new Dictionary<string, TexturePosition>
                    {
                        { "GrassSide", new TexturePosition(0,0) },
                        { "GrassTop", new TexturePosition(1,0) },
                        { "Dirt", new TexturePosition(0,1) },
                        { "Stone", new TexturePosition(1,1) },
                        { "Water", new TexturePosition(2,0) },
                        { "Leaf", new TexturePosition(2,1) },
                        { "Wood", new TexturePosition(0,2) },
                        { "Outline", new TexturePosition(1,2) },
                        { "Slime", new TexturePosition(2,2) },
                        { "Grass", new TexturePosition(3,0) },
                        { "StoneWall", new TexturePosition(3,1) },
                        { "BurntGrassSide", new TexturePosition(0,3) },
                        { "BurntGrassTop", new TexturePosition(1,3) },
                        { "SlimeFace", new TexturePosition(2,3) },
                        { "WaterSide", new TexturePosition(3,2) },
                        { "WoodPlanks", new TexturePosition(0,4) },
                        { "Sand", new TexturePosition(1,4) },
                        { "DeadPlant", new TexturePosition(2,4) },
                        { "Debug", new TexturePosition(3,4) },
                        { "Cloud", new TexturePosition(0,5) },
                        { "Explosive", new TexturePosition(1,5) },
                        { "Mushroom", new TexturePosition(2,5) },
                        { "MushroomTrunk", new TexturePosition(3,5) },
                        { "SmallMushroom", new TexturePosition(0,6) },
                        { "CactusSide", new TexturePosition(1,6) },
                        { "CactusTop", new TexturePosition(2,6) },

                        { "Fire1", new TexturePosition(0,7) },
                        { "Fire2", new TexturePosition(1,7) },
                        { "Fire3", new TexturePosition(2,7) },
                        { "Fire4", new TexturePosition(3,7) },
                        { "Fire5", new TexturePosition(0,8) },
                        { "Fire6", new TexturePosition(1,8) },
                        { "Fire7", new TexturePosition(2,8) },
                        { "Fire8", new TexturePosition(3,8) },
                    }
                },
                {
                    "slime_maker_menu.png", new Dictionary<string, TexturePosition>
                    {
                        { "Menu", new TexturePosition(0,0) },
                    }
                }
            };

        public IDictionary<string, int> TextureSize = new Dictionary<string, int>
        {
            { "atlas.png", 32 },
            { "slime_maker_menu.png", 128 },
        };
    }

    public struct TexturePosition
    {
        public int X;
        public int Y;

        public TexturePosition(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
