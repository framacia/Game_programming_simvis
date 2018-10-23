using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Week4_3D
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Week_4 : Game
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

        //Shader
        BasicEffect basicEffect;

        //Triangle and buffer
        VertexPositionColor[] vertices;
        VertexBuffer vertexBuffer;

        //Extra variables
        float rotationXAmount;
        float rotationYAmount;
        float zoomAmount;
        float moveZAmount;

        public Week_4()
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
            worldMatrix = Matrix.CreateWorld(origin, Vector3.Forward, Vector3.Up);

            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(60f),
                GraphicsDevice.DisplayMode.AspectRatio, 1f, 500f);

            cameraPosition = new Vector3(0f, 0f, -50f);
            cameraLookAt = new Vector3(0f, 0f, 0f);
            viewMatrix = Matrix.CreateLookAt(cameraPosition,
                cameraLookAt, Vector3.Up);

            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.VertexColorEnabled = true;
            basicEffect.LightingEnabled = false;

            //Vertices
            vertices = new VertexPositionColor[3];
            vertices[0] = new VertexPositionColor(new Vector3(25, -25, 0),
                Color.Red);
            vertices[1] = new VertexPositionColor(new Vector3(0, 25, 0),
                Color.Green);
            vertices[2] = new VertexPositionColor(new Vector3(-25, -25, 0),
                Color.Blue);

            vertexBuffer = new VertexBuffer(GraphicsDevice,
                typeof(VertexPositionColor), 3,
                BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionColor>(vertices);

            //Extra variables
            zoomAmount = 1.0f;
            moveZAmount = 0.0f;

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
            KeyboardState keyboardState = Keyboard.GetState();

            //Animation
            Matrix rotationY = Matrix.CreateRotationY(
                MathHelper.ToRadians(rotationYAmount));
            Matrix rotationX = Matrix.CreateRotationX(
                MathHelper.ToRadians(rotationXAmount));
            //Matrix zoom = Matrix.CreateScale(zoomAmount);
            Matrix moveZ = Matrix.CreateTranslation(0f, 0f, moveZAmount);
            cameraPosition = Vector3.Transform(
                cameraPosition, rotationX * rotationY * moveZ);
            viewMatrix = Matrix.CreateLookAt(
                cameraPosition, cameraLookAt, Vector3.Up);

            //Horizontal Input
            if (keyboardState.IsKeyDown(Keys.Right))
                rotationYAmount = -1.0f;
            else if (keyboardState.IsKeyDown(Keys.Left))
                rotationYAmount = 1.0f;
            else rotationYAmount = 0f;

            //Vertical Input
            if (keyboardState.IsKeyDown(Keys.Up))
                rotationXAmount = 1.0f;
            else if (keyboardState.IsKeyDown(Keys.Down))
                rotationXAmount = -1.0f;
            else rotationXAmount = 0f;

            //Zoom Input
           /* if (keyboardState.IsKeyDown(Keys.W))
                zoomAmount -= 0.0005f;
            else if (keyboardState.IsKeyDown(Keys.S))
                zoomAmount += 0.0005f;
            else zoomAmount = 1.0f;
            */

            //Translation Input
            if (keyboardState.IsKeyDown(Keys.W))
                moveZAmount += 0.05f;
            else if (keyboardState.IsKeyDown(Keys.S))
                moveZAmount -= 0.05f;
            else moveZAmount = 0.0f;


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
            basicEffect.World = worldMatrix;
            basicEffect.Projection = projectionMatrix;
            basicEffect.View = viewMatrix;

            GraphicsDevice.SetVertexBuffer(vertexBuffer);

            //Turn off culling
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;

            foreach(EffectPass pass in
                basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList,
                    0, 1);
            }

            base.Draw(gameTime);
        }
    }
}
