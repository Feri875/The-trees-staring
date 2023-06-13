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
    internal class SpritePlatform : SpriteAnimated
    {

        public SpritePlatform(Texture2D drawTxr, Texture2D collisionTxr, Vector2 position) : base(drawTxr, collisionTxr, position)
        {
            _hasCollision = true;
            _isMobile = true;

            _animations.Clear();

            // Idle animation
            _animations.Add(new List<Rectangle>());
            _animations[0].Add(new Rectangle(0, 176, 120, 25));
        }

    }
}
