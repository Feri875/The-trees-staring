using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_trees_staring
{
    //
    // Sprite
    //
    // This is a base class that provides visibility, movement and collision.
    // All other game sprites will inherit from this.
    //
    internal class Sprite
{
    protected Texture2D _drawTxr, _collisionTxr;            // References for bitmaps for the visible sprite and for the collision outline.
    public Vector2 _position, _velocity;                    // The current location and the current 2d movement of the sprite.
    protected Vector2 _origin, _drawScale;                  // _origin is the pivot-point of the sprite. _drawScale can be used for resizing the visible sprite in 2d.
    protected Vector2 _collisionScale;                      // Allows the size of the collision rectangle to differ from the visible sprite.
    protected bool _isMobile;                               // Whether this sprite will move. If false, velocity and collision do not effect this sprite.
    protected bool _hasCollision;                           // Whether this sprite will detect collision with other colliding sprites.
    protected bool _isonFloor;                              // Whether this sprite has collision and is currently in contact with the floor.
    protected bool _isColliding;                            // Whether this sprite has collision and is currently in contact with a colliding sprite.
    protected bool _drawCollision;                          // If true, the collision rectangle will be drawn visually, for debugging purposes.
    protected bool _flipped;                                // If true, the sprite is moving towards the left. If false, it is moving towards the right.
    public Rectangle _drawDestRect;                     // The region of the screen where the sprite will be drawn.
    protected Rectangle _drawSourceRect;                    // The region of the bitmap that will be sampled for the sprite. For use in spritesheets and animations.
    public Rectangle _collisionRect;                        // The region of the screen that will be considered as this sprites collision body.
    private SpriteEffects _flipEffect;                      // Used for horizontally flipping the sprite, based on _direction.

        //
        // Constructor
        //
        // Recieves visible texture and debug outline texture as parameters, as well as a starting position
        // and then sets various default values.
        //
        public Sprite(Texture2D drawTxr, Texture2D collisionTxr, Vector2 position)
    {
        _drawTxr = drawTxr;
        _collisionTxr = collisionTxr;
        _position = position;


        _drawSourceRect = new Rectangle(0, 0, _drawTxr.Width, _drawTxr.Height);             // By default the source rectangle is the whole of the visible texture. This will need to be overridden if spritesheets are used.
        _origin = new Vector2(_drawSourceRect.Width / 2, _drawSourceRect.Height / 2);       // By default, the origin is the centre of the visible texture. This will need to be overridden if spritesheets are used.

        _velocity = Vector2.Zero;                                                   // Default starting velocity is zero.			
        _collisionScale = Vector2.One;                                              // Default collision scale is 1x1, meaning that _collisionRect is the same size as _drawDestRect.
        _drawScale = Vector2.One;                                                   // Default draw scale is 1x1, meaning that _drawDestRect is the same size as _drawSourceRect; 
        _hasCollision = true;                                                       // Sprites have collision by default, so other sprites can bump into them.
        _isMobile = false;                                                          // Sprites are not mobile by default, so this needs to be overriden to have movement. A sprite can still collide without moving, e.g. a wall.
        _drawCollision = false;                                                     // Hide the collision outline by default - you can turn it on for debugging.

        // Here we define the default draw rectangle, collision rectangle and flip effect.
        // Moving sprites will update this every tick, but they are defined here so that they are also available for stationary sprites.
        _drawDestRect = new Rectangle((int)_position.X,
            (int)_position.Y,
            (int)(_drawTxr.Width * _drawScale.X),
            (int)(_drawTxr.Height * _drawScale.Y));
        _collisionRect = new Rectangle(_drawDestRect.X - (int)(_origin.X * _collisionScale.X),
            _drawDestRect.Y - (int)(_origin.Y * _collisionScale.Y),
            (int)(_drawDestRect.Width * _collisionScale.X),
            (int)(_drawDestRect.Height * _collisionScale.Y));
        _flipEffect = SpriteEffects.None;
    }

    //
    // Update()
    //
    // The update method can be overridden by child classes, but in it's base form
    // it updates the rectangles, collision and position of the sprite.
    // Overrides to this method should also execute these same methods at the
    // appropriate time, in the same order (or call base.Update()).
    //
    public virtual void Update(GameTime gameTime, List<Sprite> spriteList)
    {
        UpdateRectangles();
        CheckCollision(spriteList);
        UpdatePosition(gameTime);
    }

    //
    // UpdateRectangles()
    //
    // If the sprite is mobile and/or has collision, we will need to update the
    // draw rectangle and/or collision rectangle every tick.
    // We can also update the direction and flip effect at the same time.
    // If the sprite is not mobile, this will do nothing.
    //
    protected void UpdateRectangles()
    {
        if (_isMobile) // Only need to update rectangles if the sprite is mobile.
        {
            // _drawScale scales tje drawn rectangle relative to the source rectangle.
            _drawDestRect = new Rectangle((int)_position.X,
                (int)_position.Y,
                (int)(_drawSourceRect.Width * _drawScale.X),
                (int)(_drawSourceRect.Height * _drawScale.Y));

            _origin = new Vector2(_drawSourceRect.Width / 2, _drawSourceRect.Height / 2);

            _flipped = _velocity.X < 0f;  // If the movement is towards the left, _flipped is true.
            _flipEffect = _flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None; // Set the SpriteEffects enum depending on whether _flipped is true or not.

            if (_hasCollision) // Only need to update the collision rectangle if the sprite is mobile and has collision.
            {
                // We need to take the drawn origin into account, as the visibile sprite will be offset from the origin.
                // _collisionScale scales the collision rectangle relative to the draw rectangle.
                _collisionRect = new Rectangle(_drawDestRect.X - (int)(_origin.X * _collisionScale.X),
                    _drawDestRect.Y - (int)(_origin.Y * _collisionScale.Y),
                    (int)(_drawDestRect.Width * _collisionScale.X),
                    (int)(_drawDestRect.Height * _collisionScale.Y));
            }
        }
    }

    //
    // CheckCollision()
    //
    // Not to be confused with CheckCollisionSingle().
    // This takes the list of sprites as an input parameter and, if this sprite has collision and is mobile,
    // it is checked for collision against every Sprite in the sprite list, by executing
    // CheckCollisionSingle() against each one.
    // It also uses hasCollidedAny to keep track of whether any of the Sprites collided. If the Sprite
    // is newly collided, then _isColliding becomes true and OnCollide() is executed.
    //
    protected void CheckCollision(List<Sprite> spriteList)
    {
        _isonFloor = false;
        if (_hasCollision && _isMobile)
        {
            bool hasCollidedAny = false;
            foreach (Sprite eachSprite in spriteList)
            {
                // Execute CheckCollisionSingle for each sprite, and use it's bool return
                // for an if statement.
                if (CheckCollisionSingle(eachSprite))
                {
                    // If CheckCollisionSingle returned true, set hasCollidedAny to true
                    // but if it returned false it will be left as whatever it already is.
                    hasCollidedAny = true;

                    // Since there was a collision, we'll pass a reference to the OnCollideEvent()
                    // virtual method, so that child classes can respond to different types of collisions.
                    OnCollideEvent(eachSprite);
                }
            }

            // If there has been a new collision, react to it will OnCollideEffect().
            // This only happens when there's a new collision, so that it can be used for things like
            // collision sounds or particle effects.
            if (hasCollidedAny && !_isColliding)
            {
                _isColliding = true;
                OnCollideEffect();
            }
            else if (!hasCollidedAny && _isColliding)
            {
                _isColliding = false;
            }
        }
    }

    //
    // CheckCollisonSingle()
    //
    // Not to be confused with CheckCollision().
    // This takes a single Sprite as an input parameter. If they are colliding they are depenetrated and
    // their rectangles are updated. If there is collision, thi
    //
    private bool CheckCollisionSingle(Sprite otherSprite)
    {
        bool hasCollided = false;                                       // Keeps track of whether a collision happened during this method.

        if (otherSprite != this                                         // First make sure this sprite is not check collision against itself...
            && otherSprite._hasCollision                                // ...then make sure the other sprite has collision...
            && _collisionRect.Intersects(otherSprite._collisionRect))   // ...and then check to see if the two sprites' collision rectangles are overlapping.
        {
            hasCollided = true;

            // Create a rectangle representing the overlap. This will help us to easily know if it is a horizontal or a vertical collision.
            Rectangle intersection = Rectangle.Intersect(_collisionRect, otherSprite._collisionRect);

            // If the overlap rectangle is wider than it is tall, then it must be a vertical collision,
            // because we are overlapping on the top or bottom edge...
            if (intersection.Width > intersection.Height)
            {
                // So for as long as the sprites still overlap...
                while (_collisionRect.Intersects(otherSprite._collisionRect))
                {
                    // ...push them apart up or down,
                    // and also update the rectangles so that we can check them again.
                    if (_position.Y >= otherSprite._position.Y)
                    {
                        _position.Y++;
                        UpdateRectangles();
                    }
                    else
                    {
                        _position.Y--;
                        UpdateRectangles();
                        _isonFloor = true;
                    }
                    _velocity.Y = 0f;
                }
            }
            // ...otherwise it must have been a horizontal collsion.
            else
            {
                // So for as long as the sprites still overlap...
                while (_collisionRect.Intersects(otherSprite._collisionRect))
                {
                    // ...push them apart left or right,
                    // and also update the rectangles so that we can check them again.
                    if (_position.X >= otherSprite._position.X)
                    {
                        _position.X++;
                        UpdateRectangles();
                    }
                    else
                    {
                        _position.X--;
                        UpdateRectangles();
                    }
                }
                _velocity.X = 0f;
            }
        }
        return hasCollided;
    }

    //
    // OnCollideEffect()
    //
    // This is a virtual method to be overridden by child classes.
    // It could be used to play a sound or make a particle effect.
    // It is executed once when there is any new collison.
    //
    protected virtual void OnCollideEffect() { }

    //
    // OnCollideEvent()
    //
    // This is a virtual method to be overridden by child classes.
    // It could be used to respond to specific types of collision, for example touching an enemy or collectible.
    // It is executed every tick for each colliding sprite.
    //
    protected virtual void OnCollideEvent(Sprite othersprite) { }

    //
    // UpdatePosition()
    //
    // If the sprite is mobile, this method applies the velocity to the position.
    // This could be overridden in child classes to make more complex types of movement.
    //
    protected virtual void UpdatePosition(GameTime gameTime)
    {
        if (_isMobile)
        {
            _position += _velocity;
        }
    }

    //
    // Draw()
    //
    // This draws the sprite, using all the pre-calculated member variables.
    //
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_drawTxr, _drawDestRect, _drawSourceRect, Color.White, 0f, _origin, _flipEffect, 0f);

        // If the sprite is colliding and has _drawCollision set to true, we will also draw the collision outline.
        if (_hasCollision && _drawCollision)
        {
            spriteBatch.Draw(_collisionTxr, _collisionRect, Color.White);
        }
    }
}
}