using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphicsProject.Assets;

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
            effect.Parameters["LightColor"]?.SetValue(LightColor.ToVector3());
            effect.Parameters["AmbientColor"]?.SetValue(AmbientColor.ToVector3());
            effect.Parameters["DiffuseColor"]?.SetValue(DiffuseColor.ToVector3());
            effect.Parameters["Position"]?.SetValue(Position);
            effect.Parameters["ModelTexture"]?.SetValue(Texture);
            effect.Parameters["NormalTexture"]?.SetValue(Normal);
            effect.Parameters["Attenuation"]?.SetValue(Attenuation);

            base.SetEffectParameters(effect);
        }
    }

    public class SpecularPointLightMaterial : Material
    {
        public Color LightColor { get; set; }
        public Color AmbientColor { get; set; }
        public Color DiffuseColor { get; set; }
        public Color SpecularColor { get; set; }
        public Vector3 Position { get; set; }

        public Texture2D Texture { get; set; }
        public Texture2D Normal { get; set; }
        public Texture2D Specular { get; set; }

        public float Attenuation { get; set; }
        public float SpecularPower { get; set; }

        public SpecularPointLightMaterial()
        {
            LightColor = Color.White;
            DiffuseColor = Color.White;
            AmbientColor = Color.Black;
            SpecularColor = Color.White;
            
            Position = new Vector3(0, 5, 0);

            Attenuation = 10;
            SpecularPower = 16f;
        }

        public override void SetEffectParameters(Effect effect)
        {
            effect.Parameters["LightColor"]?.SetValue(LightColor.ToVector3());
            effect.Parameters["AmbientColor"]?.SetValue(AmbientColor.ToVector3());
            effect.Parameters["DiffuseColor"]?.SetValue(DiffuseColor.ToVector3());
            effect.Parameters["SpecularColor"]?.SetValue(SpecularColor.ToVector3());
            effect.Parameters["SpecularPower"]?.SetValue(SpecularPower);
            effect.Parameters["Position"]?.SetValue(Position);
            effect.Parameters["ModelTexture"]?.SetValue(Texture);
            effect.Parameters["NormalTexture"]?.SetValue(Normal);
            effect.Parameters["SpecularTexture"]?.SetValue(Specular);
            effect.Parameters["CameraPosition"]?.SetValue(GameRoot.MainCamera.Position);
            effect.Parameters["Attenuation"]?.SetValue(Attenuation);

            base.SetEffectParameters(effect);
        }
    }

    public class MultiplePointLightMaterial : Material
    {
        public Color[] LightColor { get; set; }
        public Color AmbientColor { get; set; }
        public Color DiffuseColor { get; set; }
        public Color SpecularColor { get; set; }

        public Vector3[] Position { get; set; }

        public Texture2D Texture { get; set; }
        public Texture2D AlternateTexture { get; set; }
        public Texture2D Normal { get; set; }
        public Texture2D Specular { get; set; }

        public float[] Attenuation { get; set; }
        public float[] FallOff { get; set; }
        public float SpecularPower { get; set; }
        public float Rotation { get; set; }

        public bool IsAlternateTexture { get; set; }

        public MultiplePointLightMaterial()
        {
            // Default values
            LightColor = new Color[3]
            {
                Color.Red,
                Color.Green,
                Color.Blue
            };

            DiffuseColor = Color.White;
            AmbientColor = Color.Black;
            SpecularColor = Color.White;

            Position = new Vector3[3]
            {
                new Vector3(200, 100, 120),
                new Vector3(0, 100, 120),
                new Vector3(-200, 100, 120)
            };

            Attenuation = new[] {200f, 200f, 200f};
            FallOff = new float[] {2, 2, 2};

            SpecularPower = 16f;

            IsAlternateTexture = false;

            Rotation = 0;
        }

        public override void SetEffectParameters(Effect effect)
        {
            effect.Parameters["LightColor"]?.SetValue(ConvertColors(LightColor));
            effect.Parameters["AmbientColor"]?.SetValue(AmbientColor.ToVector3());
            effect.Parameters["DiffuseColor"]?.SetValue(DiffuseColor.ToVector3());
            effect.Parameters["SpecularColor"]?.SetValue(SpecularColor.ToVector3());
            effect.Parameters["SpecularPower"]?.SetValue(SpecularPower);
            effect.Parameters["Position"]?.SetValue(Position);
            effect.Parameters["ModelTexture"]?.SetValue(Texture);
            effect.Parameters["AlternateTexture"]?.SetValue(AlternateTexture);
            effect.Parameters["NormalTexture"]?.SetValue(Normal);
            effect.Parameters["SpecularTexture"]?.SetValue(Specular);
            effect.Parameters["CameraPosition"]?.SetValue(GameRoot.MainCamera.Position);
            effect.Parameters["Attenuation"]?.SetValue(Attenuation);
            effect.Parameters["FallOff"]?.SetValue(FallOff);
            effect.Parameters["IsAlternate"]?.SetValue(IsAlternateTexture);
            effect.Parameters["Angle"]?.SetValue(Rotation);

            base.SetEffectParameters(effect);
        }

        /// <summary>
        /// Converts Color array to Vector3 array.
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        private Vector3[] ConvertColors(Color[] array)
        {
            Vector3[] lightColors = new Vector3[3];

            for (int i = 0; i < LightColor.Length; i++)
            {
                lightColors[i] = LightColor[i].ToVector3();
            }

            return lightColors;
        }
    }
}
