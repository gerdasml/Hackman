using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHackMan
{
    class HackMan
    {
        private Texture2D _texture;
        private Vector2 _position;

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        public void Initialize()
        {
            // TODO: Add your initialization logic here
            _position = Vector2.Zero;        //position = new Vector2(0,0);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public void LoadContent(ContentManager content)       //since Content cannot be seen from class Game, so we have to give a perameter to fix this problem
        {
            // TODO: use this.Content to load your game content here
            _texture = content.Load<Texture2D>("Left");     //we're using specificly the Content from class Game
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update()
        {
            // TODO: Add your update logic here
            if (Keyboard.GetState().IsKeyDown(Keys.Up) == true)
                _position.Y -= 10;
            else if (Keyboard.GetState().IsKeyDown(Keys.Down) == true)
                _position.Y += 10;
            else if (Keyboard.GetState().IsKeyDown(Keys.Right) == true)
                _position.X += 10;
            else if (Keyboard.GetState().IsKeyDown(Keys.Left) == true)
                _position.X -= 10;

            if (_position.X < 0) _position.X = Game1.SCREEN_WIDTH;
            if (_position.Y < 0) _position.Y = Game1.SCREEN_HEIGHT;
            if (_position.X > Game1.SCREEN_WIDTH) _position.X = 0;
            if (_position.Y > Game1.SCREEN_HEIGHT) _position.Y = 0;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // TODO: Add your drawing code here
            spriteBatch.Draw(_texture, destinationRectangle: new Rectangle((int)_position.X, (int)_position.Y, 100, 100));
        }
    }
}
