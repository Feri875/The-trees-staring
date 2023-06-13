using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace The_trees_staring
{
    internal class SpriteBat : SpriteAnimated
    {
        int _positionStart, _positionEnd;
        float _moveSpeed = 0.7f;

        public SpriteBat(Texture2D drawTxr, Texture2D collisionTxr, Vector2 position) : base(drawTxr, collisionTxr, position)
        {
            _hasCollision = true;
            _isMobile = true;
            

            _positionStart = (int)position.Y; // The bat will spawn towards the top of the screen
            _positionEnd = _positionStart + 100; // Once the bat has flown 100 pixels downwards, it will flip, and begin to fly upwards instead,

            _animations.Clear();

            // Flying animation
            _animations.Add(new List<Rectangle>());
            _animations[0].Add(new Rectangle(5, 5, 24, 10));
            _animations[0].Add(new Rectangle(39, 7, 22, 11));
        }

        public override void Update(GameTime gameTime, List<Sprite> spriteList)
        {
            if (_moveSpeed > 0) // When the bat spawns in, it will immediately gain momentum and start moving
            {
                _velocity.Y = _moveSpeed;
                if (_position.Y > _positionEnd)
                {
                    _moveSpeed *= -1f;
                }
            }
            else if (_moveSpeed < 0)
            {
                _velocity.Y = _moveSpeed;
                if (_position.Y < _positionStart)
                {
                    _moveSpeed *= -1f;
                }
            }

            base.Update(gameTime, spriteList);
        }

    }
}

