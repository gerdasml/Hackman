using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace GameHackMan
{
    public partial class Game1
    {
        protected override void Update(GameTime gameTime)
        {


            // TODO: Add your update logic here
            if (_cooldownWatch == null)
            {
                if (_state == State.MENU)
                    UpdateMenu();
                else if (_state == State.GAME)
                    UpdateGame();
                else if (_state == State.GAMEOVER)
                    UpdateDeathScreen();
                else if (_state == State.PAUSE)
                    UpdatePause();
                else if (_state == State.VICTORY)
                    UpdateVictory();
            }
            else if ((int)_cooldownWatch.ElapsedMilliseconds >= _cooldownInMilliseconds)
                _cooldownWatch = null;

            base.Update(gameTime);
        }

        private void UpdateGame()
        {
            if (_watch == null && (
                Keyboard.GetState().IsKeyDown(Keys.Up) ||
                Keyboard.GetState().IsKeyDown(Keys.Down) ||
                Keyboard.GetState().IsKeyDown(Keys.Left) ||
                Keyboard.GetState().IsKeyDown(Keys.Right)
                ))
                _watch = Stopwatch.StartNew();
            if (_watch != null && _watch.ElapsedMilliseconds >= (long)_secondsAllowed * 1000)
            {
                SwitchStateTo(State.GAMEOVER);
                _watch.Stop();
            }

            if (_state == State.GAME)
            {
                _hackMan.Update();
                _points += CollisionChecker.CheckFoodCollitions();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) // o cia reiktu kazkaip sustabdyt geima. kad tipo jei isejom i meniu tj vsio, nebegtu laikas ir pan.
            {
                SwitchStateTo(State.MENU);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                SwitchStateTo(State.PAUSE);
                _watch.Stop();
            }

            if (_food.Count == 0)
            {
                _currentlevel++; // cia
                if (_currentlevel < _levelFileNames.Length)
                {
                    SwitchStateTo(State.GAME);
                    StartGame(preservePoints: true);
                }
                else
                {
                    _state = State.VICTORY;
                    _watch.Stop();
                }
            }



        }

        private void UpdateVictory()
        {

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                SwitchStateTo(State.GAME);
                _currentlevel = 0;
                StartGame();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                SwitchStateTo(State.MENU);
        }

        private void UpdateMenu()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                SwitchStateTo(State.GAME); // cia reiktu kokio nors ResetGame() metodo. kad tipo kai pradedam nauja geima kad viskas butu is naujo
                StartGame();
            }
        }

        private void UpdateDeathScreen()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                _currentlevel = 0;
                StartGame();
                SwitchStateTo(State.GAME);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                SwitchStateTo(State.MENU);
        }

        private void UpdatePause()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && _state == State.PAUSE)
            {
                SwitchStateTo(State.GAME); // anksciau busena keitem tiesiog taip. db keisim SwitchStateTo(State.GAME);
                _watch.Start();
            }
        }

        private void SwitchStateTo(State newState)
        {
            _state = newState;
            _cooldownWatch = Stopwatch.StartNew();
        }
    }
}
