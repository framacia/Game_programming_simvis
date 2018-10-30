using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Lab8_GameStateProject
{
    public class PlayState : IGameState 
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ContentManager Content;
        GraphicsDevice graphicsDevice;
        Game1 game;

        //Vectors and matrices
        Vector3 origin;
        Matrix worldMatrix;
        Matrix projectionMatrix;
        Matrix viewMatrix;

        //Camera
        Vector3 cameraLookAt;
        Vector3 camForward;
        Vector3 camSide;
        Vector3 camPosition;
        Vector3 camTarget;
        Matrix cameraRotation;

        //BasicEffect shader
        BasicEffect basicEffect;

        //3D Model
        //Model UFO; // No longer used

        //GameObject List
        List<GameObject> gameObjects;

        //Font
        private SpriteFont font;


        public PlayState() { }

        public void Initialize(Game1 game, ContentManager c,
            GraphicsDeviceManager gdm, GraphicsDevice gd)
        {
            graphics = gdm;
            graphicsDevice = gd;
            Content = c;
            this.game = game;

            origin = new Vector3(0f, 0f, 0f);
            worldMatrix = Matrix.CreateWorld(origin,
                Vector3.Forward, Vector3.Up);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(60f),
                graphicsDevice.DisplayMode.AspectRatio, 1f, 500f);
            cameraLookAt = new Vector3(0f, 0f, 0f);
            viewMatrix = Matrix.CreateLookAt(camPosition,
                camTarget, Vector3.Up);

            basicEffect = new BasicEffect(graphicsDevice);
            basicEffect.VertexColorEnabled = false;
            basicEffect.LightingEnabled = true;

            gameObjects = new List<GameObject>();

            //Setup camera
            camForward = Vector3.Forward; //(0,0,-1)
            camSide = Vector3.Left;
            camPosition = new Vector3(0f, 0f, -5);
            camTarget = camPosition + camForward;
        }

        public void Enter()
        {

        }

        public void Exit()
        {

        }

        public void LoadContent()
        {
            spriteBatch = new SpriteBatch(graphicsDevice);
            font = Content.Load<SpriteFont>("blade_runner");

            gameObjects.Add(new GameObject()
            {
                Model = Content.Load<Model>("UFO"),
                Position = new Vector3(-10f, 0.5f, -10f),
                Scale = new Vector3(0.75f, 0.1f, 0.2f),
            });

            gameObjects.Add(new GameObject()
            {
                Model = Content.Load<Model>("UFO"),
                Position = new Vector3(10f, 0.5f, 10f),
                Scale = new Vector3(0.5f, 0.5f, 0.5f),
            });
        }

        public void UnloadContent()
        {

        }

        public void Update(GameTime gametime)
        {
            //Change to menu state
            if (Keyboard.GetState().IsKeyDown(Keys.Delete))
            {
                game.ChangeState(game.menuState);
            }

            //Camera movement
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                cameraRotation = Matrix.CreateRotationY(MathHelper.ToRadians(1));
                camForward = Vector3.TransformNormal(camForward, cameraRotation);
                camSide = Vector3.TransformNormal(camSide, cameraRotation);
                camTarget = camPosition + camForward;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                cameraRotation = Matrix.CreateRotationY(MathHelper.ToRadians(-1));
                camForward = Vector3.TransformNormal(camForward, cameraRotation);
                camSide = Vector3.TransformNormal(camSide, cameraRotation);
                camTarget = camPosition + camForward;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                camPosition = camPosition + 0.1f * camForward;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                camPosition = camPosition + 0.1f * -camForward;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                camPosition = camPosition + 0.1f * camSide;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                camPosition = camPosition + 0.1f * -camSide;
            }

            camTarget = camPosition + camForward;

            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget,
                Vector3.Up);
        }

        public void Draw()
        {
            graphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };

            graphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Test text",
                new Vector2(10, 10), Color.White);
            spriteBatch.End();

            foreach (GameObject gameObject in gameObjects)
            {
                foreach (ModelMesh mesh in gameObject.Model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.LightingEnabled = true;
                        effect.DirectionalLight0.DiffuseColor =
                            new Vector3(1.0f, 1.0f, 0);
                        effect.DirectionalLight0.Direction =
                            new Vector3(1f, -1f, 0);
                        effect.DirectionalLight0.SpecularColor =
                            new Vector3(0.2f, 0f, 0f);
                        effect.AmbientLightColor =
                            new Vector3(0.7f, 0.7f, 0.7f);
                        effect.PreferPerPixelLighting = true;

                        effect.View = viewMatrix;
                        // No longer using worldMatrix variable
                        //effect.World = worldMatrix;
                        effect.World = gameObject.GetObjectWorldMatrix();
                        effect.Projection = projectionMatrix;
                    }
                    mesh.Draw();
                }
            }
        }
    }
}
