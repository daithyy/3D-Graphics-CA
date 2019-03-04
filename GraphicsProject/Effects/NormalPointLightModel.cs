using Graphics;
using GraphicsProject.Materials;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsProject.Effects
{
    public class NormalPointLightModel : CustomEffectModel
    {
        public NormalPointLightModel(string asset, Vector3 position) : base(asset, position) { }

        public override void LoadContent()
        {
            // Load effect first
            CustomEffect = GameUtilities.Content.Load<Effect>("Effects/NormalPointLight");

            Texture2D texture = GameUtilities.Content.Load<Texture2D>("Textures/Rock_1");
            Texture2D normal = GameUtilities.Content.Load<Texture2D>("Textures/Rock_1_normal_intense");

            Material = new NormalPointLightMaterial()
            {
                Texture = texture,
                Normal = normal
            };

            base.LoadContent();
        }

        public override void Update()
        {
            NormalPointLightMaterial _material = (Material as NormalPointLightMaterial);

            float _radius = _material.Attenuation;
            Color _color = _material.LightColor;
            float _speed = 0.025f;

            DebugEngine.AddBoundingSphere(new BoundingSphere(_material.Position, _radius), _color);

            if (InputEngine.IsKeyHeld(Keys.Up))
                (Material as NormalPointLightMaterial).Position += new Vector3(0, 0, -_speed);

            if (InputEngine.IsKeyHeld(Keys.Down))
                (Material as NormalPointLightMaterial).Position += new Vector3(0, 0, _speed);

            if (InputEngine.IsKeyHeld(Keys.Left))
                (Material as NormalPointLightMaterial).Position += new Vector3(-_speed, 0, 0);

            if (InputEngine.IsKeyHeld(Keys.Right))
                (Material as NormalPointLightMaterial).Position += new Vector3(_speed, 0, 0);

            if (InputEngine.IsKeyHeld(Keys.PageUp))
                (Material as NormalPointLightMaterial).Position += new Vector3(0, _speed, 0);

            if (InputEngine.IsKeyHeld(Keys.PageDown))
                (Material as NormalPointLightMaterial).Position += new Vector3(0, -_speed, 0);

            if (InputEngine.IsKeyHeld(Keys.Add))
                (Material as NormalPointLightMaterial).Attenuation += _speed * 2;

            if (InputEngine.IsKeyHeld(Keys.Subtract))
                (Material as NormalPointLightMaterial).Attenuation -= _speed * 2;

            base.Update();
        }
    }
}
