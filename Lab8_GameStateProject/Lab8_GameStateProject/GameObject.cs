using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lab8_GameStateProject
{
    class GameObject
    {
        //Public members 
        public Vector3 position { get; set; }
        public Vector3 scale { get; set; }
        public Matrix rotationMatrix { get; set; }
        public Model model { get; set; }
        public BoundingBox aabb { get; set; } 
        public BoundingSphere bSphere { get; set; }
        public float radius { get; set; }

        //Constructor method
        public GameObject()
        {
            position = Vector3.Zero; //(0,0,0)
            scale = Vector3.One; //(1,1,1)
            rotationMatrix = Matrix.Identity; // No rotation
        }

        public Matrix GetObjectWorldMatrix()
        {
            return Matrix.CreateScale(scale) *
                rotationMatrix *
                Matrix.CreateWorld(position,
                Vector3.Forward, Vector3.Up);
        }

        public override string ToString()
        {
            return "Position " + position.ToString() + "\nScale: "
                + scale.ToString() + "\nRotation: "
                + rotationMatrix.ToString();
        }

        public BoundingBox InitAABB()
        {
            Vector3 min = new Vector3(float.MaxValue);
            Vector3 max = new Vector3(float.MinValue);
            foreach(ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart meshpart in mesh.MeshParts)
                {
                    int vStride =
                        meshpart.VertexBuffer.VertexDeclaration.VertexStride;
                    int vBufferSize = meshpart.NumVertices * vStride;

                    float[] vData = new float[vBufferSize /
                        sizeof(float)];
                    meshpart.VertexBuffer.GetData<float>(vData);
                    for (int i = 0; i < vBufferSize / sizeof(float);
                        i += vStride / sizeof(float))
                    {
                        Vector3 vertex = new Vector3(vData[i],
                            vData[i + 1], vData[i + 2]);
                        vertex = Vector3.Transform(vertex,
                            GetObjectWorldMatrix());
                        min = Vector3.Min(min, vertex);
                        max = Vector3.Max(max, vertex);
                    }
                }                
            }
            aabb = new BoundingBox(min, max);
            return aabb;
        }

        public BoundingSphere InitBSphere()
        {
            float radius;
            radius = MathHelper.Max(MathHelper.Max(scale.X, scale.Y),
                scale.Z);
            bSphere = new BoundingSphere(position, radius);

            return bSphere;
        }
    }
}
