using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace GameHackMan
{
    public partial class Game1
    {
        protected override void Update(GameTime gameTime)
        {


            // TODO: Add your update logic here
            if (_cooldownWatch == null)         //if game ended or we reached some kind of state
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
            else if ((int)_cooldownWatch.ElapsedMilliseconds >= _cooldownInMilliseconds)            //if time ended for the game 
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
                return;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) 
            {
                SwitchStateTo(State.MENU);
            }
            //===============================================

            if(Keyboard.GetState().IsKeyDown(Keys.S))
            {
                UpdateSecret();
            }

            if(Keyboard.GetState().IsKeyDown(Keys.T))
            {
                UpdateTime();
            }

            

            //===============================================

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                if (_watch == null) SwitchStateTo(State.PAUSE);
                else
                {
                    SwitchStateTo(State.PAUSE);
                    _watch.Stop();
                }
            }

            if (_watch == null) return;

            _hackMan.Update();
            foreach (var g in _ghost)
            {
                g.Update();
            }
            if (!Keyboard.GetState().IsKeyDown(Keys.I))
            {
                if (CollisionChecker.IsHackmanDead())
                {
                    _state = State.GAMEOVER;
                    _watch.Stop();
                    return;
                }
            }
            _points += CollisionChecker.CheckFoodCollitions();
            _highScore = Math.Max(_points, _highScore);


            if (_food.Count == 0)
            {
                _points += (int)(_levels[_currentlevel].TimeInSeconds - _watch.ElapsedMilliseconds / 1000) * BONUS_POINTS_MULTIPLIER;
                _currentlevel++; // cia
                if (_currentlevel < _levelFileNames.Length)
                {
                    SwitchStateTo(State.GAME);
                    StartGame(preservePoints: true);        //if game isn't over then calculate points continiously
                }
                else
                {
                    _state = State.VICTORY;
                    _watch.Stop();
                }
            }
        }
        //===================================================

        private void UpdateSecret()
        {
            SwitchStateTo(State.GAME);
            _currentlevel = 7;
            StartGame();
        }

        private void UpdateTime()
        {
            _watch.Stop();
        }

        //====================================================

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
            {
                WriteHighScore();
                Exit();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                SwitchStateTo(State.GAME); // cia reiktu kokio nors ResetGame() metodo. kad tipo kai pradedam nauja geima kad viskas butu is naujo
                _currentlevel = 0;
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
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                if (_watch == null) SwitchStateTo(State.GAME);
                else
                {
                    SwitchStateTo(State.GAME);
                    _watch.Stop();
                }
            }
        }

        private void SwitchStateTo(State newState)
        {
            _state = newState;
            _cooldownWatch = Stopwatch.StartNew();
        }
    }
}
