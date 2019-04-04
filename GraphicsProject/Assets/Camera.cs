using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GraphicsProject.Assets
{
    public class FPSCamera : GameComponent
    {
        public Vector3 Position
        {
            get => _cameraPosition;
            set
            {
                _cameraPosition = value;
                UpdateLookAt();
            }
        }

        public Vector3 Rotation
        {
            get => _cameraRotation;
            set
            {
                _cameraRotation = value;
                UpdateLookAt();
            }
        }

        private Vector3 _cameraPosition;
        private Vector3 _cameraRotation;
        private Vector3 _mouseRotationBuffer;
        private MouseState _currentMouseState;
        private MouseState _previousMouseState;

        private float _cameraSpeed;
        private float _speed;
        private float _sprintSpeed;
        private float _mouseSpeed;

        private Vector3 _cameraLookAt;

        public Matrix View => Matrix.CreateLookAt(_cameraPosition, _cameraLookAt, Vector3.Up);
        public Matrix Projection { get; protected set; }       

        protected float NearPlane = 1.0f;
        protected float FarPlane = 1000.0f;

        public BoundingFrustum Frustum => new BoundingFrustum(View * Projection);

        public FPSCamera(Game game, Vector3 position, Vector3 rotation, float cameraSpeed, float mouseSpeed)
            : base(game)
        {
            game.Components.Add(this);

            MoveTo(position, rotation);
            _cameraSpeed = cameraSpeed;
            _speed = cameraSpeed;
            _sprintSpeed = cameraSpeed * 2;
            _mouseSpeed = mouseSpeed;

            // Setup Projection Matrix
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.AspectRatio, NearPlane, FarPlane);

            // Keep track of mouse movement changes
            _previousMouseState = Mouse.GetState();
        }

        private void MoveTo(Vector3 position, Vector3 rotation)
        {
            Position = position;
            Rotation = rotation;
        }

        public override void Initialize()
        {
            // Set mouse position and do initial get state
            Mouse.SetPosition(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);

            base.Initialize();
        }

        private void UpdateLookAt()
        {
            // Build a rotation matrix
            Matrix rotationMatrix = Matrix.CreateRotationX(_cameraRotation.X) * Matrix.CreateRotationY(_cameraRotation.Y);

            // Build a look at offset vector
            Vector3 lookAtOffset = Vector3.Transform(Vector3.UnitZ, rotationMatrix);

            // Update camera look at vector
            _cameraLookAt = _cameraPosition + lookAtOffset;
        }

        private Vector3 PreviewMove(Vector3 amount)
        {
            // Create a rotate matrix
            Matrix rotate = Matrix.CreateRotationY(_cameraRotation.Y);

            // Create movement vector
            Vector3 movement = new Vector3(amount.X, amount.Y, amount.Z);
            movement = Vector3.Transform(movement, rotate);

            // Return the value of camera position + movement vector
            return _cameraPosition + movement;
        }

        private void Move(Vector3 scale)
        {
            MoveTo(PreviewMove(scale), Rotation);
        }

        public override void Update(GameTime gameTime)
        {
            float dt = (float) gameTime.ElapsedGameTime.TotalSeconds;

            _currentMouseState = Mouse.GetState();

            // Handle basic key movement
            Vector3 moveVector = Vector3.Zero;

            if (InputEngine.IsKeyHeld(Keys.W))
                moveVector.Z = 1;
            if (InputEngine.IsKeyHeld(Keys.S))
                moveVector.Z = -1;
            if (InputEngine.IsKeyHeld(Keys.A))
                moveVector.X = 1;
            if (InputEngine.IsKeyHeld(Keys.D))
                moveVector.X = -1;
            if (InputEngine.IsKeyHeld(Keys.Q))
                moveVector.Y = 1;
            if (InputEngine.IsKeyHeld(Keys.E))
                moveVector.Y = -1;

            if (InputEngine.IsKeyHeld(Keys.LeftShift))
                _cameraSpeed = _sprintSpeed;
            else
                _cameraSpeed = _speed;

            if (moveVector != Vector3.Zero)
            {
                // Normalize vector
                // So there isn't a faster diagonal movement
                moveVector.Normalize();
                // Add in smoothness and speed
                moveVector *= dt * _cameraSpeed;

                Move(moveVector);
            }

            // Handle mouse movement
            RotateWithMouse(dt);            

            Mouse.SetPosition(Game.GraphicsDevice.Viewport.Width / 2, Game.GraphicsDevice.Viewport.Height / 2);

            _previousMouseState = _currentMouseState;

            base.Update(gameTime);
        }

        private void RotateWithMouse(float dt)
        {
            float deltaX;
            float deltaY;

            if (_currentMouseState != _previousMouseState)
            {
                // Cache mouse location
                deltaX = _currentMouseState.X - (Game.GraphicsDevice.Viewport.Width / 2);
                deltaY = _currentMouseState.Y - (Game.GraphicsDevice.Viewport.Height / 2);

                _mouseRotationBuffer.X -= 0.01f * (deltaX * _mouseSpeed) * dt;
                _mouseRotationBuffer.Y -= 0.01f * (deltaY * _mouseSpeed) * dt;

                var angle = 75.0f;

                if (_mouseRotationBuffer.Y < MathHelper.ToRadians(-angle))
                    _mouseRotationBuffer.Y =
                        _mouseRotationBuffer.Y - (_mouseRotationBuffer.Y - MathHelper.ToRadians(-angle));

                if (_mouseRotationBuffer.Y > MathHelper.ToRadians(angle))
                    _mouseRotationBuffer.Y =
                        _mouseRotationBuffer.Y - (_mouseRotationBuffer.Y - MathHelper.ToRadians(angle));

                Rotation = new Vector3(
                    -MathHelper.Clamp(
                        _mouseRotationBuffer.Y, 
                        MathHelper.ToRadians(-angle), 
                        MathHelper.ToRadians(angle)), 
                    MathHelper.WrapAngle(_mouseRotationBuffer.X), 0); // Wrap angle to prevent spinning out of control

                deltaX = 0;
                deltaY = 0;
            }
        }
    }
}
