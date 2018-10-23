using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D playerShip;
        Vector2 playerPosition, enemyPosition, mousePosition;
        Vector2 moveRight, moveLeft, moveUp, moveDown, rotationDirection;
        float rotationSpeed, rotationAngle, movementSpeed;

        KeyboardState prevKeyboardState;
       

        public Game1()
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
            prevKeyboardState = Keyboard.GetState();

            movementSpeed = 4f;
            rotationAngle = 0f;
            rotationSpeed = 0.03f;
            playerPosition = new Vector2(0, 0);
            enemyPosition = new Vector2(475, 75);
            moveRight = new Vector2(movementSpeed, 0);
            moveLeft = new Vector2(-movementSpeed, 0);
            moveUp = new Vector2(0, -movementSpeed);
            moveDown = new Vector2(0, movementSpeed);



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
            playerShip = Content.Load<Texture2D>("player");
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
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState gpState = GamePad.GetState(PlayerIndex.One);

            rotationDirection = new Vector2((float)Math.Cos(rotationAngle), (float)Math.Sin(rotationAngle));

            IsMouseVisible = true;
            System.Diagnostics.Debug.WriteLine("Mouse  at:  " + mouseState.X + "," + mouseState.Y);
            mousePosition = new Vector2(mouseState.X, mouseState.Y);

            //Keyboard input
            if (keyboardState.IsKeyDown(Keys.Right))
                rotationAngle += rotationSpeed;
            if (keyboardState.IsKeyDown(Keys.Left))
                rotationAngle -= rotationSpeed;
            if (keyboardState.IsKeyDown(Keys.Up))
                playerPosition += rotationDirection * movementSpeed;
            if (keyboardState.IsKeyDown(Keys.Down))
                playerPosition += -rotationDirection * movementSpeed;

            //Analog stick input
            if (gpState.ThumbSticks.Left.X > 0.5f)
                playerPosition += moveRight;
            if (gpState.ThumbSticks.Left.X < -0.5f)
                playerPosition += moveLeft;
            if (gpState.ThumbSticks.Left.Y > 0.5f)
                playerPosition += rotationDirection * movementSpeed;
            if (gpState.ThumbSticks.Left.Y < -0.5f)
                playerPosition += -rotationDirection * movementSpeed;

            //D-pad input
            if (gpState.IsButtonDown(Buttons.DPadRight))
                playerPosition += moveRight;
            if (gpState.IsButtonDown(Buttons.DPadLeft))
                playerPosition += moveLeft;
            if (gpState.IsButtonDown(Buttons.DPadUp))
                playerPosition += rotationDirection * movementSpeed;
            if (gpState.IsButtonDown(Buttons.DPadDown))
                playerPosition += -rotationDirection * movementSpeed;

            //Makes the ship stay on-screen at all times
            if (playerPosition.X >= 800 - playerShip.Width / 2)
                playerPosition += moveLeft;
            if (playerPosition.X <= 0 + playerShip.Width / 2)
                playerPosition += moveRight;
            if (playerPosition.Y >= 480 - playerShip.Height / 2)
                playerPosition += moveUp;
            if (playerPosition.Y <= 0 + playerShip.Height / 2)
                playerPosition += moveDown;


            //Transports ship to left-click position
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                playerPosition.X = mouseState.X;
                playerPosition.Y = mouseState.Y;
            }
                
                base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.TransparentBlack);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(playerShip, 
                playerPosition, 
                rotation: (float)Math.PI / 2.0f + rotationAngle, 
                origin: new Vector2(playerShip.Width / 2, playerShip.Height / 2),
                color: Color.White);                 // Depth
            spriteBatch.Draw(playerShip, enemyPosition, Color.Red);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
