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

        private Color color;

        public MenuState() { }

        public void Initialize(Game1 game, ContentManager c,
            GraphicsDeviceManager gdm, GraphicsDevice gd)
        {
            graphics = gdm;
            graphicsDevice = gd;
            Content = c;
            this.game = game;

            color = Color.Black;
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

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                color = Color.Yellow;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                color = Color.Blue;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                color = Color.Red;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F))
            {
                color = Color.Green;
            }


        }

        public void Draw()
        {
            graphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };

            graphicsDevice.Clear(color);

            spriteBatch.Begin();
            spriteBatch.DrawString(font, "welcome to the menu",
                new Vector2(10, 10), Color.White);
            spriteBatch.End();
        }
    }
}