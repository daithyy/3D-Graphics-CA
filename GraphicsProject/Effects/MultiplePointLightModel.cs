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
    public class MultiplePointLightModel : CustomEffectModel
    {
        private string[] _albedo;
        private string _normal;
        private string _specular;

        public MultiplePointLightModel(string asset, Vector3 position, string[] albedo, string normal, string specular)
            : base(asset, position)
        {
            _albedo = albedo;
            _normal = normal;
            _specular = specular;
        }

        public override void LoadContent()
        {
            // Load effect first
            CustomEffect = GameUtilities.Content.Load<Effect>("Effects/MultiplePointLight");

            Texture2D texture = GameUtilities.Content.Load<Texture2D>(_albedo[0]);
            Texture2D alternateTexture = GameUtilities.Content.Load<Texture2D>(_albedo[1]);
            Texture2D normal = GameUtilities.Content.Load<Texture2D>(_normal);
            Texture2D specular = GameUtilities.Content.Load<Texture2D>(_specular);

            if (texture == null || normal == null || specular == null)
                throw new NullReferenceException();

            Material = new MultiplePointLightMaterial()
            {
                Texture = texture,
                AlternateTexture = alternateTexture,
                Normal = normal,
                Specular = specular
            };

            base.LoadContent();
        }

        public override void Update()
        {
            MultiplePointLightMaterial material = ((MultiplePointLightMaterial)Material);

            for (int i = 0; i < material.Position.Length; i++)
            {
                float[] radius = material.Attenuation;
                Color[] color = material.LightColor;
                float[] speed = { 1f, 1f, 1f };

                DebugEngine.AddBoundingSphere(new BoundingSphere(material.Position[i], radius[i]), color[i]);

                if (InputEngine.IsKeyHeld(Keys.Up))
                    ((MultiplePointLightMaterial)Material).Position[i] += new Vector3(0, 0, -speed[i]);

                if (InputEngine.IsKeyHeld(Keys.Down))
                    ((MultiplePointLightMaterial)Material).Position[i] += new Vector3(0, 0, speed[i]);

                if (InputEngine.IsKeyHeld(Keys.Left))
                    ((MultiplePointLightMaterial)Material).Position[i] += new Vector3(-speed[i], 0, 0);

                if (InputEngine.IsKeyHeld(Keys.Right))
                    ((MultiplePointLightMaterial)Material).Position[i] += new Vector3(speed[i], 0, 0);

                if (InputEngine.IsKeyHeld(Keys.PageUp))
                    ((MultiplePointLightMaterial)Material).Position[i] += new Vector3(0, speed[i], 0);

                if (InputEngine.IsKeyHeld(Keys.PageDown))
                    ((MultiplePointLightMaterial)Material).Position[i] += new Vector3(0, -speed[i], 0);

                if (InputEngine.IsKeyHeld(Keys.Add))
                    ((MultiplePointLightMaterial)Material).Attenuation[i] += speed[i] * 2;

                if (InputEngine.IsKeyHeld(Keys.Subtract))
                    ((MultiplePointLightMaterial)Material).Attenuation[i] -= speed[i] * 2;

                if (InputEngine.IsKeyPressed(Keys.Space))
                    ((MultiplePointLightMaterial) Material).IsAlternateTexture =
                        !((MultiplePointLightMaterial) Material).IsAlternateTexture;

                if (InputEngine.IsKeyPressed(Keys.C))
                    ((MultiplePointLightMaterial)Material).LightColor = new[] { Color.White, Color.White, Color.White };
            }

            base.Update();
        }
    }
}
