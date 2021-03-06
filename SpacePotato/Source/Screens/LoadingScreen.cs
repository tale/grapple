using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpacePotato {
    public class LoadingScreen : GameScreen {
        private readonly Spinner _spinner;
        private readonly SpriteFont _titleFont;
        public float timePassed, maxTimePassed = 1F;

        public LoadingScreen(Game game, int screenId) : base(game, screenId) {
            _titleFont = ContentManager.Load<SpriteFont>("Fonts/Title");
            _spinner = new Spinner(ContentManager);
        }

        public override void Update(GameTime gameTime, KeyInfo keys, MouseInfo mouse) {
            _spinner.Update(gameTime);
            float deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;

            timePassed += deltaTime;
            
            if (timePassed >= maxTimePassed) ScreenManager.SwitchScreen(2);
        }

        public override void BasicDraw(GameTime gameTime, SpriteBatch spriteBatch) {
            var (x, _) = _titleFont.MeasureString("Grapple");
            var viewport = spriteBatch.GraphicsDevice.Viewport;

            _spinner.Render(gameTime, spriteBatch, new Vector2(viewport.Width - 40F, viewport.Height - 40F), 0.1F);
            spriteBatch.DrawString(_titleFont, "Grapple", new Vector2(viewport.Width / 2F, viewport.Height / 4F),
                Color.White, 0, new Vector2(x / 2, 0), Vector2.One, SpriteEffects.None, 0);
        }
    }
}
