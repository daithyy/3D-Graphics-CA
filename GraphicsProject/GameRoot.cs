using Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sample;

namespace GraphicsProject
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameRoot : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        InputEngine input;
        Camera mainCamera;

        CustomEffectModel model;

        public GameRoot()
        {
            graphics = new GraphicsDeviceManager(this);
            input = new InputEngine(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            CustomEffectModel model = new CustomEffectModel("ArcticBear", new Vector3(0, 0, 0));

            mainCamera = new Camera("cam", new Vector3(0, 5, 10), new Vector3(0, 0, -1));
            mainCamera.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (InputEngine.IsKeyPressed(Keys.Escape))
                Exit();

            mainCamera.Update();

            if (model != null)
            {
                model.Update();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (model != null)
            {
                model.Draw(mainCamera);
            }

            base.Draw(gameTime);
        }
    }
}
