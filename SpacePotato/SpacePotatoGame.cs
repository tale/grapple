﻿
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpacePotato {
    public class SpacePotatoGame : Game {
        private readonly GraphicsDeviceManager _graphics;
        private readonly Options _options;
        private readonly ScreenManager _screenManager;
        private SpriteBatch _spriteBatch;

        public static SpacePotatoGame instance;
        
        public KeyboardState lastKeyboardState;

        public SpacePotatoGame(Options options) {
            _graphics = new GraphicsDeviceManager(this);
            _screenManager = new ScreenManager(this, 1);
            
            _options = options;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            instance = this;
        }

        protected override void Initialize() {
            _graphics.PreferredBackBufferWidth =
                _options.Resolution != null ? int.Parse(_options.Resolution.Split('x')[0]) : 1280;
            _graphics.PreferredBackBufferHeight =
                _options.Resolution != null ? int.Parse(_options.Resolution.Split('x')[1]) : 720;
            _graphics.IsFullScreen = _options.Fullscreen && _options.Fullscreen;

            _graphics.PreferMultiSampling = true;
            _graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            _graphics.GraphicsDevice.PresentationParameters.MultiSampleCount = 1024;

            _graphics.ApplyChanges();

            _screenManager.RegisterScreen(new DebugScreen(this, -1));
            _screenManager.RegisterScreen(new LoadingScreen(this, 0));
            _screenManager.RegisterScreen(new MainScreen(this, 1));

            base.Initialize();
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        public static GraphicsDevice getGraphicsDevice() {
            return instance.GraphicsDevice;
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            float deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;

            // key input
            KeyboardState keyState = Keyboard.GetState();
            KeyInfo keys = new KeyInfo(keyState, lastKeyboardState);
            lastKeyboardState = keyState;

            _screenManager.Update(gameTime, keys);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);
            
            _screenManager.Draw(gameTime, _spriteBatch);
            
            base.Draw(gameTime);
        }
    }
}
