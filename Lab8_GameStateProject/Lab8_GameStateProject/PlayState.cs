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
        Vector3 camForward;
        Vector3 camSide;
        Vector3 camPosition;
        Vector3 camTarget;
        Matrix cameraRotation;

        //BasicEffect shader
        BasicEffect basicEffect;

        //GameObject List
        List<GameObject> gameObjects;

        //Font
        private SpriteFont font;

        //Skybox
        Skybox skybox;



        public PlayState() { }

        public void Initialize(Game1 game, ContentManager c,
            GraphicsDeviceManager gdm, GraphicsDevice gd)
        {
            graphics = gdm;
            graphicsDevice = gd;
            Content = c;
            this.game = game;

            //Setup camera
            camForward = Vector3.Forward; //(0,0,-1)
            camSide = Vector3.Left;
            camPosition = new Vector3(0f, 0.2f, -5);
            camTarget = camPosition + camForward;

            origin = new Vector3(0f, 0f, 0f);
            worldMatrix = Matrix.CreateWorld(origin,
                Vector3.Forward, Vector3.Up);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(60f),
                graphicsDevice.DisplayMode.AspectRatio, 1f, 500f);
            

            //BasicEffect setup
            basicEffect = new BasicEffect(graphicsDevice)
            {
                VertexColorEnabled = false,
                LightingEnabled = true
            };

            gameObjects = new List<GameObject>();

            


            //Setup collisions
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.InitAABB();
                gameObject.InitBSphere();
            }
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
                model = Content.Load<Model>("sh_1\\SH_BUILDING_01"),
                position = new Vector3(10f, -0.5f, 10f),
                scale = new Vector3(0.5f, 0.5f, 0.5f),
            });

            //gameObjects.Add(new GameObject()
            //{
            //    model = Content.Load<Model>("sh_10\\SH_BUILDING_10"),
            //    position = new Vector3(0.75f, -0.975f, 10.13f),
            //    scale = new Vector3(0.5f, 0.5f, 0.5f),
            //});

            gameObjects.Add(new GameObject()
            {
                model = Content.Load<Model>("monocube\\monoCube"),
                position = new Vector3(0f, -1f, 0f),
                scale = new Vector3(100f, 0.1f, 100f),
            });

            skybox = new Skybox("skybox\\sorbin", Content);
        }

        public void UnloadContent()
        {

        }

        public void Update(GameTime gametime)
        {
            Vector3 oldPosition = camPosition;

            //Collision code
            foreach (GameObject gameObject in gameObjects)
            {
                if (Vector3.Distance(camPosition,
                    gameObjects[0].position) < 35f)
                {
                    camPosition = oldPosition;
                    break;
                }                
            }

            //Recalculate AABB/bSphere for moving gameObject
            /*gameObjects[0].InitAABB(); //gameObjects[0].InitBSphere();

            for (int i=1; i<gameObjects.Count; i++)
            {
                if (gameObjects[0].aabb.Intersects(gameObjects[i].aabb))
                //if  (gameObjects[0].bSphere.Intersects(gameObjects[i].bSphere))
                {
                    gameObjects[0].position = oldPosition;
                    gameObjects[0].scale = oldScale;
                    gameObjects[0].rotationMatrix = oldRotMatrix;
                }
            }*/

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
            //Draw background colour
            graphicsDevice.Clear(Color.Gray);

            //Draw skybox
            RasterizerState skyBoxRasterizerState = new RasterizerState();
            skyBoxRasterizerState.CullMode = CullMode.CullClockwiseFace;
            graphicsDevice.RasterizerState = skyBoxRasterizerState;

            viewMatrix.Decompose(out Vector3 tmpScale, out Quaternion tmpRotation, out Vector3 tmpTranslation);
            skybox.Draw(Matrix.CreateFromQuaternion(tmpRotation),
                projectionMatrix);

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
            graphicsDevice.RasterizerState = rasterizerState;

            //Depth buffer on
            graphicsDevice.DepthStencilState = 
                new DepthStencilState() { DepthBufferEnable = true };

            //Draw gameObjects
            foreach (GameObject gameObject in gameObjects)
            {
                foreach (ModelMesh mesh in gameObject.model.Meshes)
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
                        effect.FogEnabled = true;
                        effect.FogColor = new Vector3(0.55f, 0.55f, 0.55f);
                        effect.FogStart = 1f;
                        effect.FogEnd = 10f;

                        effect.View = viewMatrix;
                        // No longer using worldMatrix variable
                        //effect.World = worldMatrix;
                        effect.World = gameObject.GetObjectWorldMatrix();
                        effect.Projection = projectionMatrix;
                    }
                    mesh.Draw();
                }
            }

            //Draw 2D (text)
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Test text",
                new Vector2(10, 10), Color.White);
            spriteBatch.End();
        }
    }
}
