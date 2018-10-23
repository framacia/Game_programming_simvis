using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lab5_Model
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Lab5_Model : Game
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
        Vector3 cameraPosition;

        //BasicEffect shader
        BasicEffect basicEffect;

        //3D Model
        Model UFO;

        public Lab5_Model()
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
            cameraPosition = new Vector3(0f, 0f, -75f);
            cameraLookAt = new Vector3(0f, 0f, 0f);
            viewMatrix = Matrix.CreateLookAt(cameraPosition,
                cameraLookAt, Vector3.Up);

            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.VertexColorEnabled = false;
            basicEffect.LightingEnabled = true;

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
            UFO = this.Content.Load<Model>("UFO");
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

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
            foreach (ModelMesh mesh in UFO.Meshes)
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
                    effect.World = worldMatrix;
                    effect.Projection = projectionMatrix;
                }
                mesh.Draw();
            }

            base.Draw(gameTime);
        }
    }
}
