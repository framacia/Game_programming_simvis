using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Lab8_GameStateProject
{
    public class MenuState : IGameState
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ContentManager Content;
        GraphicsDevice graphicsDevice;
        Game1 game;
        private SpriteFont font;

        public MenuState() { }

        public void Initialize(Game1 game, ContentManager c,
            GraphicsDeviceManager gdm, GraphicsDevice gd)
        {
            graphics = gdm;
            graphicsDevice = gd;
            Content = c;
            this.game = game;
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
            font = Content.Load<SpriteFont>("montserrat");
        }

        public void UnloadContent()
        {

        }

        public void Update(GameTime gametime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                game.Exit();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                game.ChangeState(game.playState);
            }
        }

        public void Draw()
        {
            graphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            spriteBatch.DrawString(font, "welcome to the menu",
                new Vector2(10, 10), Color.White);
            spriteBatch.End();
        }
    }
}