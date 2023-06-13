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
    internal class SpriteAnimated : Sprite
{

    protected List<List<Rectangle>> _animations = new List<List<Rectangle>>();
    protected int _currentAnimation = 0;
    protected int _currentFrame = 0;
    protected float _framerate = 2f;
    protected float _framecounter = 0f;

    public SpriteAnimated(Texture2D drawTxr, Texture2D collisionTxr, Vector2 position) : base(drawTxr, collisionTxr, position)
    {
        _animations.Add(new List<Rectangle>());
        _animations[0].Add(new Rectangle(0, 0, _drawTxr.Width, _drawTxr.Height));
    }

    public override void Update(GameTime gameTime, List<Sprite> spritelist)
    {
        _framecounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (_framecounter <= 0)
        {
            _framecounter = 1f / _framerate;
            _currentFrame++;
            if (_currentFrame >= _animations[_currentAnimation].Count)
            {
                _currentFrame = 0;
            }
        }

        base.Update(gameTime, spritelist);

    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _drawSourceRect = _animations[_currentAnimation][_currentFrame];

        base.Draw(spriteBatch);
    }

    public void ChangeAnim(int newAnim)
    {
        if (newAnim != _currentAnimation && newAnim >= 0 && newAnim < _animations.Count)
        {
            _currentAnimation = newAnim;
            _currentFrame = 0;



        }
    }

}
}