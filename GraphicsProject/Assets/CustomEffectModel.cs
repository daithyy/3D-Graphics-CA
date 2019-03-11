﻿using GraphicsProject.Materials;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    public class CustomEffectModel : SimpleModel
    {
        public Effect CustomEffect { get; set; }
        public Material Material { get; set; }

        public CustomEffectModel(string asset, Vector3 position) :
            base("", asset, position)
        { }

        public override void LoadContent()
        {
            //load the model as normal
            base.LoadContent();

            //replace this effect on the model
            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = CustomEffect;
                }
            }
        }

        public override void Draw(Camera camera)
        {
            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect.Parameters["World"].SetValue(BoneTransforms[mesh.ParentBone.Index] * World);
                    part.Effect.Parameters["View"].SetValue(camera.View);
                    part.Effect.Parameters["Projection"].SetValue(camera.Projection);

                    if (Material != null)
                        Material.SetEffectParameters(part.Effect);
                }
                mesh.Draw();
            }
        }
    }
}
