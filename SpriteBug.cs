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
    internal class SpriteBug : SpriteAnimated
    {

        int _positionStart, _positionEnd; // The bug will use a similar movement system to the bat, though it will move horizontally, rather than vertically
        float _moveSpeed = 0.7f;

        public SpriteBug(Texture2D drawTxr, Texture2D collisionTxr, Vector2 position) : base(drawTxr, collisionTxr, position)
        {

            _positionStart = (int)position.X; 
            _positionEnd = _positionStart + 75; // The bug will move a shorter distance, to make sure it stays on the platform

            _hasCollision = true;
            _isMobile = true;
            

            _animations.Clear();

            // walking animation
            _animations.Add(new List<Rectangle>());
            _animations[0].Add(new Rectangle(2, 10, 10, 6));
            _animations[0].Add(new Rectangle(19, 10, 9, 6));
        }

        public override void Update(GameTime gameTime, List<Sprite> spriteList)
        {

            if (_moveSpeed > 0)
            {
                _velocity.X = _moveSpeed;
                if (_position.X > _positionEnd)
                {
                    _moveSpeed *= -1f;
                }
            }
            else if (_moveSpeed < 0)
            {
                _velocity.X = _moveSpeed;
                if (_position.X < _positionStart)
                {
                    _moveSpeed *= -1f;
                }
            }

            base.Update(gameTime, spriteList);

        }


    }
}
