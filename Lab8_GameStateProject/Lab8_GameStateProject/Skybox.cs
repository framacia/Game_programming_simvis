using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Lab8_GameStateProject
{
    public class Skybox
    {

        private Model skyBox;
        private TextureCube skyBoxTexture;
        private Effect skyBoxEffect;
        private float size = 5f;

        public Skybox(string skyboxTexture, ContentManager Content)
        {
            skyBox = Content.Load<Model>("skybox\\SkyBoxCube");
            skyBoxTexture = Content.Load<TextureCube>(skyboxTexture);
            skyBoxEffect = Content.Load<Effect>("skybox\\skybox");
        }

        public void Draw(Matrix view, Matrix projection)
        {
            foreach (EffectPass pass in skyBoxEffect.CurrentTechnique.Passes)
            {
                foreach (ModelMesh mesh in skyBox.Meshes)
                {
                    foreach(ModelMeshPart part in mesh.MeshParts)
                    {
                        part.Effect = skyBoxEffect;
                        part.Effect.Parameters["World"].SetValue(
                            Matrix.CreateScale(size));
                        part.Effect.Parameters["View"].SetValue(view);
                        part.Effect.Parameters["Projection"].SetValue(projection);
                        part.Effect.Parameters[
                            "SkyBoxTexture"].SetValue(skyBoxTexture);                        
                    }

                    //Draw the mesh with skybox effect
                    mesh.Draw();
                }
            }
        }
    }
}
