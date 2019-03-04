using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsProject.Materials
{
    /// <summary>
    /// Stores effect parameters.
    /// </summary>
    public class Material
    {
        public virtual void SetEffectParameters(Effect effect) { }

        public virtual void Update() { }
    }

    public class NormalPointLightMaterial : Material
    {
        public Color LightColor { get; set; }
        public Color AmbientColor { get; set; }
        public Color DiffuseColor { get; set; }
        public Vector3 Position { get; set; }

        public Texture2D Texture { get; set; }
        public Texture2D Normal { get; set; }

        public float Attenuation { get; set; }

        public NormalPointLightMaterial()
        {
            LightColor = Color.White;
            DiffuseColor = Color.White;
            AmbientColor = Color.Black;
            Position = new Vector3(0, 5, 0);
            Attenuation = 10;
        }

        public override void SetEffectParameters(Effect effect)
        {
            if (effect.Parameters["LightColor"] != null)
            {
                effect.Parameters["LightColor"].SetValue(LightColor.ToVector3());
            }

            if (effect.Parameters["AmbientColor"] != null)
            {
                effect.Parameters["AmbientColor"].SetValue(AmbientColor.ToVector3());
            }

            if (effect.Parameters["DiffuseColor"] != null)
            {
                effect.Parameters["DiffuseColor"].SetValue(DiffuseColor.ToVector3());
            }

            if (effect.Parameters["Position"] != null)
            {
                effect.Parameters["Position"].SetValue(Position);
            }

            if (effect.Parameters["ModelTexture"] != null)
            {
                effect.Parameters["ModelTexture"].SetValue(Texture);
            }

            if (effect.Parameters["NormalTexture"] != null)
            {
                effect.Parameters["NormalTexture"].SetValue(Normal);
            }

            if (effect.Parameters["Attenuation"] != null)
            {
                effect.Parameters["Attenuation"].SetValue(Attenuation);
            }

            base.SetEffectParameters(effect);
        }
    }
}
