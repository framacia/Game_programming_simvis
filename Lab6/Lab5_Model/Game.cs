using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace Lab5_Model
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

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

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            origin = new Vector3(0f, 0f, 0f);
            worldMatrix = Matrix.CreateWorld(origin,
                Vector3.Forward, Vector3.Up);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(60f),
                GraphicsDevice.DisplayMode.AspectRatio, 1f, 500f);
            cameraLookAt = new Vector3(0f, 0f, 0f);
            viewMatrix = Matrix.CreateLookAt(camPosition,
                camTarget, Vector3.Up);

            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.VertexColorEnabled = false;
            basicEffect.LightingEnabled = true;

            gameObjects = new List<GameObject>();

            //Setup camera
            camForward = Vector3.Forward; //(0,0,-1)
            camSide = Vector3.Left;
            camPosition = new Vector3(0f, 0f, -5);
            camTarget = camPosition + camForward;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

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

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back ==
                ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();

            // TODO: Add your update logic here

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

            /* UFO MOVEMENT
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                gameObjects[0].Position += new Vector3(0f, 0.1f, 0f);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                gameObjects[0].Position += new Vector3(0f, -0.1f, 0f);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                gameObjects[0].Position += new Vector3(0.1f, 0f, 0f);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                gameObjects[0].Position += new Vector3(-0.1f, 0f, 0f);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                gameObjects[0].RotationMatrix *=
                    Matrix.CreateRotationY(MathHelper.ToRadians(-5));
            }

            if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                gameObjects[0].RotationMatrix *=
                    Matrix.CreateRotationY(MathHelper.ToRadians(5));
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Z))
            {
                // reduce scale
                gameObjects[0].Scale *= 0.9f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.X))
            {
                // increase scale
                gameObjects[0].Scale *= 1.1f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {
                Console.WriteLine(gameObjects[0].ToString());
            }
            */

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
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
            base.Draw(gameTime);
        }
    }
}
