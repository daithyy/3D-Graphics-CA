using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    //store effect parameters
    public class Material
    {
        public virtual void SetEffectParameters(Effect effect) { }
        public virtual void Update() { }
    }

    public class PointLightMaterial : Material
    {
        public Color LightColor { get; set; }
        public Color AmbientColor { get; set; }
        public Color DiffuseColor { get; set; }
        public Vector3 Position { get; set; }
        public Texture2D Texture { get; set; }
        public float Attenuation { get; set; }
        public Texture2D NormalMap { get; set; }

        public PointLightMaterial()
        {
            LightColor = Color.White;
            DiffuseColor = Color.White;
            AmbientColor = Color.Black;
            Position = new Vector3(0, 5, 0);
            Attenuation =40;
        }

        public override void SetEffectParameters(Effect effect)
        {
            effect.Parameters["LightColor"].SetValue(LightColor.ToVector3());
            effect.Parameters["AmbientColor"].SetValue(AmbientColor.ToVector3());
            effect.Parameters["DiffuseColor"].SetValue(DiffuseColor.ToVector3());
            effect.Parameters["Position"].SetValue(Position);
            effect.Parameters["ModelTexture"].SetValue(Texture);
            effect.Parameters["NormalTexture"].SetValue(NormalMap);
            effect.Parameters["Attenuation"].SetValue(Attenuation);

            base.SetEffectParameters(effect);
        }
    }

    public class DirectionalLightMaterial : Material
    {
        public Color LightColor { get; set; }
        public Color AmbientColor { get; set; }
        public Color DiffuseColor { get; set; }
        public Vector3 Direction { get; set; }

        public Texture2D Texture { get; set; }

        public DirectionalLightMaterial()
        {
            LightColor = Color.White;
            DiffuseColor = Color.White;
            AmbientColor = Color.Black;
            Direction = new Vector3(0, 0, 1);
        }

        public override void SetEffectParameters(Effect effect)
        {
            effect.Parameters["LightColor"].SetValue(LightColor.ToVector3());
            effect.Parameters["AmbientColor"].SetValue(AmbientColor.ToVector3());
            effect.Parameters["DiffuseColor"].SetValue(DiffuseColor.ToVector3());
            effect.Parameters["Direction"].SetValue(Direction);
            effect.Parameters["ModelTexture"].SetValue(Texture);

            base.SetEffectParameters(effect);
        }
    }

    public class TextureMaterial : Material
    {
        public Color Color { get; set; }
        public Texture2D Texture { get; set; }
        public Texture2D Overlay { get; set; }
        public float Time { get; set; }

        public TextureMaterial(Color color, Texture2D texture, Texture2D overlay) : base()
        {
            Color = color;
            Texture = texture;
            Overlay = overlay;
        }

        public override void SetEffectParameters(Effect effect)
        {
            effect.Parameters["Color"].SetValue(Color.ToVector3());
            effect.Parameters["ModelTexture"].SetValue(Texture);
            effect.Parameters["OverlayTexture"].SetValue(Overlay);
            effect.Parameters["Time"].SetValue(Time);

            base.SetEffectParameters(effect);
        }
    }

    public class ColorMaterial : Material
    {
        public Color Color { get; set; }
        public Color AltColor { get; set; }
        public bool DrawAlt { get; set; }

        public override void SetEffectParameters(Effect effect)
        {
            effect.Parameters["Color"].SetValue(Color.ToVector3());
            effect.Parameters["AltColor"].SetValue(AltColor.ToVector3());
            effect.Parameters["DrawAlt"].SetValue(DrawAlt);

            base.SetEffectParameters(effect);
        }

        public ColorMaterial() : base()
        {
            Color = Color.White;
            AltColor = Color.LawnGreen;
            DrawAlt = true;
        }
    }


}
