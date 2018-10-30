using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Lab8_GameStateProject
{
    public interface IGameState 
    {
        void Initialize(Game1 game, ContentManager c,
            GraphicsDeviceManager gdm, GraphicsDevice gd);
        void Enter();
        void Exit();
        void LoadContent();
        void UnloadContent();
        void Update(GameTime gameTime);
        void Draw();
    }
}
