using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_trees_staring
{
    internal class SpritePlayer : SpriteAnimated
    {

        private Vector2 _gravity = new Vector2(0f, 10f);
        private Vector2 _jumpForce = new Vector2(0, -5f);
        public Vector2 _startPosition;
        private float _walkSpeed = 150f;
        private float _friction = 0.75f;

        private bool _IsjumpPressed = false;

        public SpritePlayer(Texture2D drawTxr, Texture2D collisionTxr, Vector2 position) : base(drawTxr, collisionTxr, position)
        {
            _startPosition = position;
            _hasCollision = true;
            _isMobile = true;
            _collisionScale = new Vector2(0.5f, 1f);
            _drawCollision = false;
           

            _animations.Clear();

            // Idle + Walk animation
            _animations.Add(new List<Rectangle>());
            _animations[0].Add(new Rectangle(7, 6, 19, 26));
            _animations[0].Add(new Rectangle(39, 6, 19, 26));


            // Jump animation
            _animations.Add(new List<Rectangle>());
            _animations[1].Add(new Rectangle(102, 5, 19, 25));

        }

        public void ResetPlayer()
        {
            _position = _startPosition;
            _velocity = Vector2.Zero;
        }

        public override void Update(GameTime gameTime, List<Sprite> spriteList)
        {
            Debug.WriteLine(_framerate + " " + _currentFrame + " " + _framecounter);
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Space) && !_IsjumpPressed)
            {
                if (_isonFloor)
                {
                    _IsjumpPressed = true;
                    _velocity += _jumpForce;
                }

            }
            else if (!keyboardState.IsKeyDown(Keys.Space) && _IsjumpPressed)
            {
                _IsjumpPressed = false;

            }

            if (keyboardState.IsKeyDown(Keys.A))
            {

                _velocity.X = -_walkSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            }
            else if (keyboardState.IsKeyDown(Keys.D))
            {

                _velocity.X = _walkSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            }

            if (_isColliding)
            {
                _velocity.X *= _friction;
            }

            if (!_isColliding && _velocity.Y > 1f)
            {
                ChangeAnim(1);
            }
            else if (Math.Abs(_velocity.X) < 0.25f)
            {
                ChangeAnim(0);
            }
            else
            {
                ChangeAnim(0);
            }


            base.Update(gameTime, spriteList);


        }

        protected override void UpdatePosition(GameTime gameTime)
        {
            _velocity += _gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.UpdatePosition(gameTime);
        }
    }
}
