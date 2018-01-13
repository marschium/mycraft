using MyCraft.Engine.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.Core.GameObjects.Components
{
    public class AnimatedModelTexture : Texture
    {
        private double _switchTextureCounter;
        private int _currentTextureIndex;
        private IList<Texture> _textures;

        public AnimatedModelTexture(IList<Texture> textures)
        {
            _textures = textures;
            Coords = textures[0].Coords;
            TextureId = _textures[1].TextureId;
        }

        public double UpdateIntervalMs { get; set; }

        public override void Update(FrameUpdateInfo frameUpdateInfo)
        {
            _switchTextureCounter += frameUpdateInfo.TimeDelta;
            if (_switchTextureCounter > UpdateIntervalMs)
            {
                _switchTextureCounter = 0;
                _currentTextureIndex = _currentTextureIndex + 1 < _textures.Count ? _currentTextureIndex + 1 : 0;
                Coords = _textures[_currentTextureIndex].Coords;
                TextureId = _textures[_currentTextureIndex].TextureId;
                Load();
            }
        }
    }
}
