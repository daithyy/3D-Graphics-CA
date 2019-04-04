using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using GraphicsProject.Assets;
using GraphicsProject.Effects;
using GraphicsProject.Materials;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GraphicsProject
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameRoot : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        InputEngine _input;
        DebugEngine _debug;

        public static FPSCamera MainCamera;

        List<GameObject3D> _gameObjects = new List<GameObject3D>();
        CustomEffectModel _model;

        List<MultiplePointLightMaterial> _lights = new List<MultiplePointLightMaterial>();

        public GameRoot()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 768;
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
            _graphics.ApplyChanges();

            _debug = new DebugEngine();
            _input = new InputEngine(this);

            IsFixedTimeStep = true;
            IsMouseVisible = true;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            GameUtilities.Content = Content;
            GameUtilities.GraphicsDevice = GraphicsDevice;

            _debug.Initialize();

            MainCamera = new FPSCamera(this, new Vector3(0, 100, 200), Vector3.Zero, 40f, 10f);

            base.Initialize();
        }

        protected override void LoadContent()
        {            
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            AddModel(new MultiplePointLightModel("house", new Vector3(-200, 0, 50),
                new[]
                {
                    "Textures/Rock_1",
                    "Textures/Rock_2"
                },
                "Textures/Rock_1_normal_intense",
                "Textures/Pokeball/Sphere_specular"));

            AddModel(new MultiplePointLightModel("ArcticBear", new Vector3(0, 0, -50),
                new[]
                {
                    "Models/Arctic Bear/MountPolarBear_Diffuse01",
                    "Models/Arctic Bear/MountPolarBear_Diffuse02"
                },
                "Models/Arctic Bear/MountPolarBear_Diffuse01_Normal",
                "Models/Arctic Bear/MountPolarBear_Diffuse01_spec2"));

            AddModel(new MultiplePointLightModel("Pokeball", new Vector3(200, 100, 0),
                new[]
                {
                    "Textures/Pokeball/Sphere_albedo",
                    "Textures/Pokeball/Sphere_albedo2"
                },
                "Textures/Pokeball/Sphere_normal",
                "Textures/Pokeball/Sphere_specular"));

            _lights.Add(new MultiplePointLightMaterial()
            {
                AmbientColor = Color.DarkGray
            });
        }

        private void AddModel(SimpleModel model)
        {
            model.Initialize();
            _gameObjects.Add(model);
            model.LoadContent();
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (InputEngine.IsKeyPressed(Keys.Escape))
                Exit();

            foreach (var gameObjects in _gameObjects)
            {
                gameObjects.Update();
            }

            _lights.ForEach(l => l.Update());

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGray);

            DrawGameObjects();

            GameUtilities.SetGraphicsDeviceFor3D();

            base.Draw(gameTime);
        }

        private bool FrustumContains(SimpleModel model)
        {
            if (MainCamera.Frustum.Contains(model.AABB) != ContainmentType.Disjoint)
                return true;
            else
                return false;
        }

        private void DrawGameObjects()
        {
            // Only draw within camera frustum
            // Frustum Culling
            foreach (SimpleModel go in _gameObjects)
            {
                if (FrustumContains(go))
                {
                    go.Draw(MainCamera);
                }
            }

            _debug.Draw(MainCamera);
        }
    }
}
