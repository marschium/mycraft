using OpenTK;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.Particles
{

    public class Colour
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public byte A { get; set; }

        public uint AsUint()
        {
            return (uint)((A << 24) | (B << 16) | (G << 8) | R);
        }
    }

    public class Particle
    {
        public Vector3 Position { get; set; }

        public Vector3 Velocity { get; set; } = Vector3.Zero;

        public Colour Colour { get; set; }

        public bool Destroyed { get; set; }

        public TimeSpan Lifetime { get; set; }

        public Particle(Vector3 position, Colour colour)
        {
            Position = position;
            Colour = colour;
        }
    }
}
