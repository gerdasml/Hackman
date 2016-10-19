using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameHackMan
{
    public partial class Game1
    {
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            if (_state == State.MENU)
                DrawMenu();
            if (_state == State.GAME)
                DrawGame();
            if (_state == State.GAMEOVER)
                DrawDeathScreen();
            if (_state == State.PAUSE)
                DrawPauseScreen();
            if (_state == State.VICTORY)
                DrawVictory();
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawVictory()
        {
            DrawGame();
            Texture2D background = new Texture2D(GraphicsDevice, 1, 1);
            background.SetData(new Color[] { Color.Black });
            //[5 - vardinis] (arba preservedPoints)
            _spriteBatch.Draw(background, destinationRectangle: new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), color: new Color(Color.Black, 0.8f));

            string victoryScreen = "CONGRATULATIONS! YOU WON!";
            DrawString(victoryScreen, new Rectangle(SCREEN_WIDTH / 16, SCREEN_HEIGHT - SCREEN_HEIGHT / 16 * 9, SCREEN_WIDTH / 16 * 14, SCREEN_HEIGHT), Color.White);
        }

        private void DrawNameOfLevel()
        {
            string resultString = "Level " + _currentlevel.ToString();
            if (_currentlevel == _levels.Count - 1)
                resultString = "Level TROLOLO";
            var position = new Vector2(0, 0);
            Vector2 size = Graphics.Font.MeasureString(resultString);
            Vector2 boundaries = new Vector2(SCREEN_WIDTH, TILE_SIZE);
            float xScale = boundaries.X / size.X;
            float yScale = boundaries.Y / size.Y;
            float scale = Math.Min(xScale, yScale);
            _spriteBatch.DrawString(Graphics.Font, resultString, position, Color.DarkOliveGreen, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
        }

        private void DrawHighScore()
        {
            string resultString = "High Score: " + _highScore.ToString();
            Vector2 size = Graphics.Font.MeasureString(resultString);
            Vector2 boundaries = new Vector2(SCREEN_WIDTH, TILE_SIZE);
            float xScale = boundaries.X / size.X;
            float yScale = boundaries.Y / size.Y;
            float scale = Math.Min(xScale, yScale);
            var position = new Vector2(SCREEN_WIDTH - size.X * scale, 0);
            _spriteBatch.DrawString(Graphics.Font, resultString, position, Color.DarkOliveGreen, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
        }

        private void DrawScore()
        {
            string resultString = "Score: " + _points.ToString();
            var position = new Vector2(0, SCREEN_HEIGHT - TILE_SIZE);
            Vector2 size = Graphics.Font.MeasureString(resultString);
            Vector2 boundaries = new Vector2(SCREEN_WIDTH, TILE_SIZE);
            float xScale = boundaries.X / size.X;
            float yScale = boundaries.Y / size.Y;
            float scale = Math.Min(xScale, yScale);
            _spriteBatch.DrawString(Graphics.Font, resultString, position, Color.DarkOliveGreen, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
        }

        private void DrawTimeLeft()
        {
            string resultString = "Time left: ";
            if (_watch == null)
                resultString += String.Format("{0}:{1:00}", _secondsAllowed / 60, _secondsAllowed % 60);
            else
                resultString += String.Format("{0}:{1:00}", (_secondsAllowed - _watch.ElapsedMilliseconds / 1000) / 60, (_secondsAllowed - _watch.ElapsedMilliseconds / 1000) % 60);
            Vector2 size = Graphics.Font.MeasureString(resultString);
            Vector2 boundaries = new Vector2(SCREEN_WIDTH, TILE_SIZE);
            float xScale = boundaries.X / size.X;
            float yScale = boundaries.Y / size.Y;
            float scale = Math.Min(xScale, yScale);
            var position = new Vector2(SCREEN_WIDTH - size.X * scale, SCREEN_HEIGHT - TILE_SIZE);
            _spriteBatch.DrawString(Graphics.Font, resultString, position, Color.DarkOliveGreen, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
        }

        private void DrawDeathScreen()
        {
            DrawGame();
            Texture2D background = new Texture2D(GraphicsDevice, 1, 1);
            background.SetData(new Color[] { Color.Black });
            _spriteBatch.Draw(background, destinationRectangle: new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), color: new Color(Color.Black, 0.8f));

            string gameOverString = "GAME OVER";
            Vector2 size = Graphics.Font.MeasureString(gameOverString);
            Rectangle boundaries = new Rectangle(0, 0, SCREEN_WIDTH * 3 / 4, SCREEN_HEIGHT);
            float xScale = boundaries.Width / size.X;
            float yScale = boundaries.Height / size.Y;
            float scale = Math.Min(xScale, yScale);
            var position = new Vector2(SCREEN_WIDTH / 8, SCREEN_HEIGHT / 2 - size.Y * scale / 2);
            _spriteBatch.DrawString(Graphics.Font, gameOverString, position, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
        }

        private void DrawMenu()
        {
            Texture2D background = new Texture2D(GraphicsDevice, 1, 1);
            background.SetData(new Color[] { Color.Black });
            _spriteBatch.Draw(background, destinationRectangle: new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), color: new Color(Color.Black, 1f));

            string nameOfGame = "HACKMAN";
            string menu = "MENU";
            string instructions = "STORYLINE";
            string textOfInstructions1 = "Congratulations!  You've entered the secret developer's ";
            string textOfInstructions2 = "project. The release is tommorow and your mission is to ";
            string textOfInstructions3 = "destroy all the bugs before they destroy your project. ";
            string textOfInstructions4 = "But 'accidentally' you told your supervisor that everything";
            string textOfInstructions5 = "is working so make sure to fix your mistakes.   Glhf!!!   ";
            string controls = "CONTROLS";
            string textOfControls1 = "ARROW KEYS to move";
            string textOfControls2 = "SPACE to pause";
            string textOfControls3 = "ESC to quit to the main menu";
            string otherControls1 = "PRESS ENTER TO START A NEW GAME";
            string otherControls2 = "PRESS ESC TO QUIT";
            DrawString(nameOfGame, new Rectangle(SCREEN_WIDTH / 16 * 5, 7, SCREEN_WIDTH / 2, SCREEN_HEIGHT - SCREEN_HEIGHT / 16 * 15), Color.DarkOliveGreen);
            DrawString(menu, new Rectangle(SCREEN_WIDTH / 16 * 6, SCREEN_HEIGHT - SCREEN_HEIGHT / 32 * 30, SCREEN_WIDTH / 4, SCREEN_HEIGHT - SCREEN_HEIGHT / 16 * 15), Color.White);
            DrawString(instructions, new Rectangle(SCREEN_WIDTH / 32, SCREEN_HEIGHT - SCREEN_HEIGHT / 32 * 26, SCREEN_WIDTH / 5, SCREEN_HEIGHT - SCREEN_HEIGHT / 5), Color.White);
            DrawString(textOfInstructions1, new Rectangle(SCREEN_WIDTH / 32, SCREEN_HEIGHT - SCREEN_HEIGHT / 32 * 24, SCREEN_WIDTH / 16 * 15, SCREEN_HEIGHT - SCREEN_HEIGHT / 16 * 5), Color.DarkOliveGreen);
            DrawString(textOfInstructions2, new Rectangle(SCREEN_WIDTH / 32, SCREEN_HEIGHT - SCREEN_HEIGHT / 32 * 23, SCREEN_WIDTH / 16 * 15, SCREEN_HEIGHT - SCREEN_HEIGHT / 16 * 5), Color.DarkOliveGreen);
            DrawString(textOfInstructions3, new Rectangle(SCREEN_WIDTH / 32, SCREEN_HEIGHT - SCREEN_HEIGHT / 32 * 22, SCREEN_WIDTH / 16 * 15, SCREEN_HEIGHT - SCREEN_HEIGHT / 16 * 5), Color.DarkOliveGreen);
            DrawString(textOfInstructions4, new Rectangle(SCREEN_WIDTH / 32, SCREEN_HEIGHT - SCREEN_HEIGHT / 32 * 21, SCREEN_WIDTH / 16 * 15, SCREEN_HEIGHT - SCREEN_HEIGHT / 16 * 5), Color.DarkOliveGreen);
            DrawString(textOfInstructions5, new Rectangle(SCREEN_WIDTH / 32, SCREEN_HEIGHT - SCREEN_HEIGHT / 32 * 20, SCREEN_WIDTH / 16 * 15, SCREEN_HEIGHT - SCREEN_HEIGHT / 16 * 5), Color.DarkOliveGreen);
            DrawString(controls, new Rectangle(SCREEN_WIDTH / 32, SCREEN_HEIGHT - SCREEN_HEIGHT / 32 * 17, SCREEN_WIDTH / 5, SCREEN_HEIGHT - SCREEN_HEIGHT / 8 * 2), Color.White);
            DrawString(textOfControls1, new Rectangle(SCREEN_WIDTH / 32, SCREEN_HEIGHT - SCREEN_HEIGHT / 32 * 15, SCREEN_WIDTH / 16 * 5, SCREEN_HEIGHT - SCREEN_HEIGHT / 16 * 5), Color.DarkOliveGreen);
            DrawString(textOfControls2, new Rectangle(SCREEN_WIDTH / 32, SCREEN_HEIGHT - SCREEN_HEIGHT / 32 * 14, SCREEN_WIDTH / 16 * 4, SCREEN_HEIGHT - SCREEN_HEIGHT / 16 * 5), Color.DarkOliveGreen);
            DrawString(textOfControls3, new Rectangle(SCREEN_WIDTH / 32, SCREEN_HEIGHT - SCREEN_HEIGHT / 32 * 13, SCREEN_WIDTH / 16 * 7, SCREEN_HEIGHT - SCREEN_HEIGHT / 16 * 5), Color.DarkOliveGreen);
            DrawString(otherControls1, new Rectangle(SCREEN_WIDTH / 16 * 3, SCREEN_HEIGHT - SCREEN_HEIGHT / 32 * 10, SCREEN_WIDTH / 16 * 11, SCREEN_HEIGHT - SCREEN_HEIGHT / 16 * 7), Color.White);
            DrawString(otherControls2, new Rectangle(SCREEN_WIDTH / 16 * 5, SCREEN_HEIGHT - SCREEN_HEIGHT / 32 * 8, SCREEN_WIDTH / 16 * 6, SCREEN_HEIGHT - SCREEN_HEIGHT / 16 * 7), Color.White);

        }

        private void DrawPauseScreen()
        {
            DrawGame();
            Texture2D background = new Texture2D(GraphicsDevice, 1, 1);
            background.SetData(new Color[] { Color.Black });
            _spriteBatch.Draw(background, destinationRectangle: new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), color: new Color(Color.Black, 0.8f));

            string gamePausedString = "Press SPACE to continue";
            DrawString(gamePausedString, new Rectangle(SCREEN_WIDTH / 16, SCREEN_HEIGHT - SCREEN_HEIGHT / 16 * 9, SCREEN_WIDTH / 16 * 14, SCREEN_HEIGHT), Color.White);
        }

        private void DrawGame()
        {
            foreach (var w in _wall)
            {
                w.Draw(_spriteBatch);
            }
            foreach (var f in _food)
            {
                f.Draw(_spriteBatch);
            }
            _hackMan.Draw(_spriteBatch);
            foreach (var g in _ghost)
            {
                g.Draw(_spriteBatch);
            }

            Texture2D background = new Texture2D(GraphicsDevice, 1, 1);
            background.SetData(new Color[] { Color.Black });
            _spriteBatch.Draw(background, destinationRectangle: new Rectangle(0, SCREEN_HEIGHT - TILE_SIZE, SCREEN_WIDTH, TILE_SIZE), color: new Color(Color.Black, 1f));
            DrawNameOfLevel();
            DrawScore();
            DrawTimeLeft();
            DrawHighScore();
        }

        private void DrawString(string text, Rectangle boundaries, Color color)
        {
            Vector2 size = Graphics.Font.MeasureString(text);
            float xScale = boundaries.Width / size.X;
            float yScale = boundaries.Height / size.Y;
            float scale = Math.Min(xScale, yScale);
            var position = new Vector2(boundaries.X, boundaries.Y);
            _spriteBatch.DrawString(Graphics.Font, text, position, color, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
        }
    }
}
