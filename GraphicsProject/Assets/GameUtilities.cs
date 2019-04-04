using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GraphicsProject.Assets
{
    public delegate void ObjectStringIDHandler(string id);

    public static class GameUtilities
    {
        public static GraphicsDevice GraphicsDevice { get; set; }
        public static GameTime Time { get; set; }
        public static ContentManager Content { get; set; }
        public static Random Random { get; set; }
        public static SpriteFont DebugFont { get; set; }

        public static void SetGraphicsDeviceFor3D()
        {
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
        }
    }

}
