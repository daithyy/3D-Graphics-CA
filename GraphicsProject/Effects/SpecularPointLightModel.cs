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
    public class SpecularPointLightModel : CustomEffectModel
    {
        private string _albedo;
        private string _normal;
        private string _specular;

        public SpecularPointLightModel(string asset, Vector3 position, string albedo, string normal, string specular)
            : base(asset, position)
        {
            _albedo = albedo;
            _normal = normal;
            _specular = specular;
        }

        public override void LoadContent()
        {
            // Load effect first
            CustomEffect = GameUtilities.Content.Load<Effect>("Effects/SpecularPointLight");

            Texture2D texture = GameUtilities.Content.Load<Texture2D>(_albedo);
            Texture2D normal = GameUtilities.Content.Load<Texture2D>(_normal);
            Texture2D specular = GameUtilities.Content.Load<Texture2D>(_specular);

            if (texture == null || normal == null || specular == null)
                throw new NullReferenceException();

            Material = new SpecularPointLightMaterial()
            {
                Texture = texture,
                Normal = normal,
                Specular = specular
            };

            base.LoadContent();
        }

        public override void Update()
        {
            SpecularPointLightMaterial material = ((SpecularPointLightMaterial)Material);

            float _radius = material.Attenuation;
            Color _color = material.LightColor;
            float _speed = 1f;

            DebugEngine.AddBoundingSphere(new BoundingSphere(material.Position, _radius), _color);

            if (InputEngine.IsKeyHeld(Keys.Up))
                ((SpecularPointLightMaterial)Material).Position += new Vector3(0, 0, -_speed);

            if (InputEngine.IsKeyHeld(Keys.Down))
                ((SpecularPointLightMaterial)Material).Position += new Vector3(0, 0, _speed);

            if (InputEngine.IsKeyHeld(Keys.Left))
                ((SpecularPointLightMaterial)Material).Position += new Vector3(-_speed, 0, 0);

            if (InputEngine.IsKeyHeld(Keys.Right))
                ((SpecularPointLightMaterial)Material).Position += new Vector3(_speed, 0, 0);

            if (InputEngine.IsKeyHeld(Keys.PageUp))
                ((SpecularPointLightMaterial)Material).Position += new Vector3(0, _speed, 0);

            if (InputEngine.IsKeyHeld(Keys.PageDown))
                ((SpecularPointLightMaterial)Material).Position += new Vector3(0, -_speed, 0);

            if (InputEngine.IsKeyHeld(Keys.Add))
                ((SpecularPointLightMaterial)Material).Attenuation += _speed * 2;

            if (InputEngine.IsKeyHeld(Keys.Subtract))
                ((SpecularPointLightMaterial)Material).Attenuation -= _speed * 2;

            base.Update();
        }
    }
}
