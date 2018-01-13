using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCraft.Engine.Abstract;
using MyCraft.Engine.GameObjects;
using OpenTK;

namespace MyCraft.Engine.Ui.Components
{
    public class DebugComponent : UiComponent
    {
        private Player _player;
        private TextComponent _positionText;

        private double _t;

        public DebugComponent(Player player, TextUtil textUtil, Vector2 position) : base(position)
        {
            _player = player;
            _positionText = new TextComponent("", textUtil, position + new Vector2(0, 0));
        }

        public override Vector2[] Vertices => _positionText.Vertices;

        public override int VertexBufferId => _positionText.VertexBufferId;

        public override Vector2[] TextureCoords => _positionText.TextureCoords;

        public override int TextureBufferId => _positionText.TextureBufferId;

        public override int TextureId => _positionText.TextureId;

        public override void Load()
        {
            _positionText.Load();
        }

        public override void Update(FrameUpdateInfo frameInfo)
        {
            base.Update(frameInfo);
            _t += frameInfo.TimeDelta;
            if (_t > 1)
            {
                _t = 0;
                _positionText.Contents = 
                    $"X {_player.Position.X}\n" +
                    $"Y {_player.Position.Y}\n" +
                    $"Z {_player.Position.Z}\n" +
                    $"BLOCK {_player.RaycastHit?.Block?.GetType().Name.ToUpper()}";
            }
        }
    }
}
