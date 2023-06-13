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
    internal class Attack : SpriteAnimated
    {
        public Attack(Texture2D drawTxr, Texture2D collisionTxr, Vector2 position) : base(drawTxr, collisionTxr, position)
        {
            _hasCollision = true;
            _isMobile = false;
           

            _animations.Clear();

            // attack animation
            _animations.Add(new List<Rectangle>());
            _animations[0].Add(new Rectangle(3, 2, 27 - 21, 13 - 7));
            


        }
    }
}
