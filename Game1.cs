using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_trees_staring
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D _backgroundTxr, _sprites1Txr, _sprites2Txr, _sprites3Txr, _sprites4Txr, _sprites5Txr, _sprites6Txr, _debugCollisionTxr;

        List<Sprite> _spriteList = new List<Sprite>();
        Point _screenSize = new Point(800, 480);

        List<List<Sprite>> _Levels = new List<List<Sprite>>();
        int _CurrentLevel = 0;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = _screenSize.X;
            _graphics.PreferredBackBufferHeight = _screenSize.Y;
            _graphics.ApplyChanges();
            base.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _backgroundTxr = Content.Load<Texture2D>("Background");
            _sprites1Txr = Content.Load<Texture2D>("Knight");
            _sprites2Txr = Content.Load<Texture2D>("Bat");
            _sprites3Txr = Content.Load<Texture2D>("Platforms");
            _sprites4Txr = Content.Load<Texture2D>("Torch");
            _sprites5Txr = Content.Load<Texture2D>("Bug");
            _sprites6Txr = Content.Load<Texture2D>("SwordSlashFX");
            _debugCollisionTxr = Content.Load<Texture2D>("DebugCollision");




            BuildLevels();
            ChangeLevel(_CurrentLevel);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach (Sprite eachSprite in _spriteList)
            {
                eachSprite.Update(gameTime, _spriteList);
            }

            SpritePlayer player = _spriteList.OfType<SpritePlayer>().ToList()[0];
            SpriteTorch Torch = _spriteList.OfType<SpriteTorch>().ToList()[0];

            foreach(SpriteBat bat in _spriteList.OfType<SpriteBat>().ToList())
            {
                if (player._collisionRect.Intersects(bat._drawDestRect))
                {
                    player.ResetPlayer();
                }
            }

            foreach (SpriteBug bug in _spriteList.OfType<SpriteBug>().ToList())
            {
                if (player._collisionRect.Intersects(bug._drawDestRect))
                {
                    player.ResetPlayer();
                }
            }

            if (player._collisionRect.Intersects(Torch._drawDestRect))
            {
                ChangeLevel(_CurrentLevel + 1);

            }

            if (player._position.Y > _screenSize.Y + player._drawDestRect.Height)
            {

                player.ResetPlayer();
            }

            

           // if (player._collisionRect.Intersects(Bug._drawDestRect))
            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _spriteBatch.Draw(_backgroundTxr, new Rectangle(0, 0, _screenSize.X, _screenSize.Y), Color.White);

            foreach (Sprite eachSprite in _spriteList)
            {
                eachSprite.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        void ChangeLevel(int newLevel)
        {
            _CurrentLevel = newLevel;
            if (_CurrentLevel >= _Levels.Count)
            {
                _CurrentLevel = 0;
            }

            _spriteList = new List<Sprite>();

            foreach (Sprite levelSprite in _Levels[_CurrentLevel])
            {
                _spriteList.Add(levelSprite);
            }

            SpritePlayer player = _spriteList.OfType<SpritePlayer>().ToList()[0];
            player.ResetPlayer();
        }

        void BuildLevels()
        {
            _Levels.Clear();

            _Levels.Add(new List<Sprite>());
            _Levels[0].Add(new SpritePlayer(_sprites1Txr, _sprites1Txr, new Vector2(300f, 150f)));
            _Levels[0].Add(new SpriteTorch(_sprites4Txr, _sprites4Txr, new Vector2(500f, 220f)));
            _Levels[0].Add(new SpritePlatform(_sprites3Txr, _sprites3Txr, new Vector2(300, 200f)));
            _Levels[0].Add(new SpritePlatform(_sprites3Txr, _sprites3Txr, new Vector2(500, 250f)));
            _Levels[0].Add(new SpriteBat(_sprites2Txr, _sprites2Txr, new Vector2(400, 100)));

            _Levels.Add(new List<Sprite>());
            _Levels[1].Add(new SpritePlayer(_sprites1Txr, _sprites1Txr, new Vector2(215f, 100f)));
            _Levels[1].Add(new SpriteTorch(_sprites4Txr, _sprites4Txr, new Vector2(560f, 250f)));
            _Levels[1].Add(new SpritePlatform(_sprites3Txr, _sprites3Txr, new Vector2(300, 300f)));
            _Levels[1].Add(new SpritePlatform(_sprites3Txr, _sprites3Txr, new Vector2(400, 300f)));
            _Levels[1].Add(new SpritePlatform(_sprites3Txr, _sprites3Txr, new Vector2(500, 300f)));

            _Levels.Add(new List<Sprite>());
            _Levels[2].Add(new SpritePlayer(_sprites1Txr, _sprites1Txr, new Vector2(200f, 100f)));
            _Levels[2].Add(new SpritePlatform(_sprites3Txr, _sprites3Txr, new Vector2(400, 300f)));
            _Levels[2].Add(new SpriteTorch(_sprites4Txr, _sprites4Txr, new Vector2(560f, 250f)));

            _Levels.Add(new List<Sprite>());
            _Levels[3].Add(new SpritePlayer(_sprites1Txr, _sprites1Txr, new Vector2(215f, 100f)));
            _Levels[3].Add(new SpriteTorch(_sprites4Txr, _sprites4Txr, new Vector2(560f, 250f)));

            _Levels.Add(new List<Sprite>());
            _Levels[4].Add(new SpritePlayer(_sprites1Txr, _sprites1Txr, new Vector2(215f, 100f)));
            _Levels[4].Add(new SpriteTorch(_sprites4Txr, _sprites4Txr, new Vector2(560f, 250f)));

            _Levels.Add(new List<Sprite>());
            _Levels[5].Add(new SpritePlayer(_sprites1Txr, _sprites1Txr, new Vector2(215f, 100f)));
            _Levels[5].Add(new SpriteTorch(_sprites4Txr, _sprites4Txr, new Vector2(560f, 250f)));

            _Levels.Add(new List<Sprite>());
            _Levels[6].Add(new SpritePlayer(_sprites1Txr, _sprites1Txr, new Vector2(215f, 100f)));
            _Levels[6].Add(new SpriteTorch(_sprites4Txr, _sprites4Txr, new Vector2(560f, 250f)));



        }

    }
}