using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lab8_GameStateProject
{
    class GameObject
    {
        //Public members 
        public Vector3 Position { get; set; }
        public Vector3 Scale { get; set; }
        public Matrix RotationMatrix { get; set; }
        public Model Model { get; set; }

        //Constructor method
        public GameObject()
        {
            Position = Vector3.Zero; //(0,0,0)
            Scale = Vector3.One; //(1,1,1)
            RotationMatrix = Matrix.Identity; // No rotation
        }

        public Matrix GetObjectWorldMatrix()
        {
            return Matrix.CreateScale(Scale) *
                RotationMatrix *
                Matrix.CreateWorld(Position,
                Vector3.Forward, Vector3.Up);
        }

        public override string ToString()
        {
            return "Position " + Position.ToString() + "\nScale: "
                + Scale.ToString() + "\nRotation: "
                + RotationMatrix.ToString();
        }
    }
}
