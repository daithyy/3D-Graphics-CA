using GraphicsProject.Materials;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphicsProject.Assets;

namespace GraphicsProject.Effects
{
    public class NormalPointLightModel : CustomEffectModel
    {
        private string _albedo;
        private string _normal;

        public NormalPointLightModel(string asset, Vector3 position, string albedo, string normal) : base(asset,
            position)
        {
            _albedo = albedo;
            _normal = normal;
        }

        public override void LoadContent()
        {
            // Load effect first
            CustomEffect = GameUtilities.Content.Load<Effect>("Effects/NormalPointLight");

            var texture = GameUtilities.Content.Load<Texture2D>(_albedo);
            var normal = GameUtilities.Content.Load<Texture2D>(_normal);

            Material = new NormalPointLightMaterial()
            {
                Texture = texture,
                Normal = normal
            };

            base.LoadContent();
        }

        public override void Update()
        {
            NormalPointLightMaterial material = ((NormalPointLightMaterial) Material);

            float _radius = material.Attenuation;
            Color _color = material.LightColor;
            float _speed = 1f;

            DebugEngine.AddBoundingSphere(new BoundingSphere(material.Position, _radius), _color);

            if (InputEngine.IsKeyHeld(Keys.Up))
                ((NormalPointLightMaterial) Material).Position += new Vector3(0, 0, -_speed);

            if (InputEngine.IsKeyHeld(Keys.Down))
                ((NormalPointLightMaterial) Material).Position += new Vector3(0, 0, _speed);

            if (InputEngine.IsKeyHeld(Keys.Left))
                ((NormalPointLightMaterial) Material).Position += new Vector3(-_speed, 0, 0);

            if (InputEngine.IsKeyHeld(Keys.Right))
                ((NormalPointLightMaterial) Material).Position += new Vector3(_speed, 0, 0);

            if (InputEngine.IsKeyHeld(Keys.PageUp))
                ((NormalPointLightMaterial) Material).Position += new Vector3(0, _speed, 0);

            if (InputEngine.IsKeyHeld(Keys.PageDown))
                ((NormalPointLightMaterial) Material).Position += new Vector3(0, -_speed, 0);

            if (InputEngine.IsKeyHeld(Keys.Add))
                ((NormalPointLightMaterial) Material).Attenuation += _speed * 2;

            if (InputEngine.IsKeyHeld(Keys.Subtract))
                ((NormalPointLightMaterial) Material).Attenuation -= _speed * 2;

            base.Update();
        }
    }
}
