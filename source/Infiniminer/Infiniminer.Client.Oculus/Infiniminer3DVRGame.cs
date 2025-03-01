using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Input.XR;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.XR;

namespace Infiniminer
{
    public class Infiniminer3DVRGame : InfiniminerGame
    {
        SpriteBatch spriteBatch;
        BasicEffect spriteBatchEffect;

        XRDevice _xrDevice;
        HandsState _handsState;

        RasterizerState _wireFrameRasterizerState;
        Model _pickaxe3d;


        //fix model origin, rotation and scale
        Matrix _pickaxeWorldTransform = Matrix.Identity
                                      * Matrix.CreateRotationX(MathHelper.ToRadians(-45f))
                                      * Matrix.CreateTranslation(0f, -0.010f, -0.004f)
                                      ;

        public Infiniminer3DVRGame(string[] args)
            : base(args)
        {

            graphicsDeviceManager.IsFullScreen = true;
            graphicsDeviceManager.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;

            // OXR requires at least feature level 9.3.
            graphicsDeviceManager.GraphicsProfile = GraphicsProfile.HiDef;

            // Syncronize to the headset refresh rate.
            graphicsDeviceManager.SynchronizeWithVerticalRetrace = true;
            IsFixedTimeStep = false;
            // 72Hz Frame rate for oculus.
            TargetElapsedTime = TimeSpan.FromTicks(138888);

            // We don't care is the main window is Focuses or not
            // because we render on the XR rendertargets.
            InactiveSleepTime = TimeSpan.FromSeconds(0);

            // create oculus device
            _xrDevice = new XRDevice("Infiniminer XR", this.Services);
        }

        protected override void Initialize()
        {
            base.Initialize();

            _xrDevice.BeginSessionAsync(XRSessionMode.VR);
            _xrDevice.TrackFloorLevelAsync(false);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteBatchEffect = new BasicEffect(GraphicsDevice);
            spriteBatchEffect.TextureEnabled = true;
            spriteBatchEffect.VertexColorEnabled = true;

            _wireFrameRasterizerState = new RasterizerState();
            _wireFrameRasterizerState.FillMode = FillMode.WireFrame;
            _wireFrameRasterizerState.CullMode = CullMode.None;
            _pickaxe3d = Content.Load<Model>("tools/pickaxe3d");

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {

            if (_xrDevice.DeviceState == XRDeviceState.Enabled)
            {
                _handsState = _xrDevice.GetHandsState();
            }

            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Vector3 cameraPosition = new Vector3(0, 0, 2.5f);
            float aspect = GraphicsDevice.Viewport.AspectRatio;
            Matrix view = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
            Matrix projection = Matrix.CreatePerspectiveFieldOfView(1, aspect, 1, 10);

            if (_xrDevice.DeviceState == XRDeviceState.Enabled)
            {
                // draw on VR headset
                int ovrResult = _xrDevice.BeginFrame();
                if (ovrResult >= 0)
                {
                    HeadsetState headsetState = _xrDevice.GetHeadsetState();
                    Matrix headTransform = Matrix.CreateFromPose(headsetState.HeadPose);

                    this.propertyBag.playerCamera.ApplyHeadTransform(headTransform);

                    // draw each eye on a rendertarget
                    foreach (XREye eye in _xrDevice.GetEyes())
                    {
                        RenderTarget2D rt = _xrDevice.GetEyeRenderTarget(eye);
                        GraphicsDevice.SetRenderTarget(rt);
                        if (this.propertyBag.blockEngine.bloomPosteffect != null)
                            this.propertyBag.blockEngine.bloomPosteffect.DefaultBackBuffer = rt;

                        cameraPosition = this.propertyBag.playerCamera.Position;
                        Matrix globalWorld = Matrix.CreateRotationY(this.propertyBag.playerCamera.Yaw)
                                           * Matrix.CreateTranslation(cameraPosition)
                                           * Matrix.CreateTranslation(0.0f, -0.3f, 0.0f)
                                           ;

                        // VR eye view and projection
                        Matrix eyeTransform = headsetState.GetEyeTransform(eye);
                        view = headsetState.GetEyeView(eye);
                        view = Matrix.Invert(eyeTransform);

                        projection = _xrDevice.CreateProjection(eye, 0.01f, 1000.0f);

                        this.propertyBag.playerCamera.UseVrCamera = true;
                        this.propertyBag.playerCamera.ViewMatrix = Matrix.Invert(globalWorld) * view;
                        this.propertyBag.playerCamera.ProjectionMatrix = projection;

                        DrawScene(gameTime, view, projection);

                        // draw Pickaxe on left hand
                        int handIndex = 1;
                        Model handModel = _pickaxe3d;
                        Matrix handWorldTransform = _pickaxeWorldTransform;
                        DrawHand(gameTime, view, projection, handIndex, handModel, handWorldTransform);

                        // Resolve eye rendertarget
                        GraphicsDevice.SetRenderTarget(null);
                        if (this.propertyBag.blockEngine.bloomPosteffect != null)
                            this.propertyBag.blockEngine.bloomPosteffect.DefaultBackBuffer = null;
                        // submit eye rendertarget
                        _xrDevice.CommitRenderTarget(eye, rt);
                    }

                    // submit frame
                    int result = _xrDevice.EndFrame();

                    // draw on PC screen
                    GraphicsDevice.SetRenderTarget(null);
                    if (this.propertyBag.blockEngine.bloomPosteffect != null)
                        this.propertyBag.blockEngine.bloomPosteffect.DefaultBackBuffer = null;

                    this.propertyBag.playerCamera.ApplyHeadTransform(Matrix.Identity);
                    this.propertyBag.playerCamera.UseVrCamera = false;

                    view = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
                    projection = Matrix.CreatePerspectiveFieldOfView(1, aspect, 1, 10);
                    DrawScene(gameTime, view, projection);

                    this.propertyBag.playerCamera.UseVrCamera = true;
                    this.propertyBag.playerCamera.ApplyHeadTransform(headTransform);

                    // preview VR rendertargets
                    //GraphicsDevice.Clear(Color.Black);
                    //var pp = GraphicsDevice.PresentationParameters;
                    //int height = pp.BackBufferHeight;
                    //float aspectRatio = (float)ovrDevice.GetEyeRenderTarget(0).Width / ovrDevice.GetEyeRenderTarget(0).Height;

                    //int width = Math.Min(pp.BackBufferWidth, (int)(height * aspectRatio));
                    //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
                    //spriteBatch.Draw(ovrDevice.GetEyeRenderTarget(0), new Rectangle(0, 0, width, height), Color.White);
                    //spriteBatch.Draw(ovrDevice.GetEyeRenderTarget(1), new Rectangle(width, 0, width, height), Color.White);
                    //spriteBatch.End();

                    return;
                }
            }

        }

        private void DrawScene(GameTime gameTime, Matrix view, Matrix projection)
        {
            Matrix cameraMtx = Matrix.Invert(view);
            spriteBatchEffect.World = Matrix.CreateConstrainedBillboard(
                    Vector3.Zero, cameraMtx.Translation, Vector3.UnitY, null, Vector3.Forward);
            spriteBatchEffect.View = view;
            spriteBatchEffect.Projection = projection;

            // draw any drawable components
            base.Draw(gameTime);
        }

        private void DrawHand(GameTime gameTime, Matrix view, Matrix proj, int handIndex, Model handModel, Matrix handWorldTransform)
        {
            TouchControllerType touchControllerType = (handIndex == 0)
                                                    ? TouchControllerType.LTouch
                                                    : TouchControllerType.RTouch;

            if (TouchController.GetCapabilities(touchControllerType).IsConnected)
            {
                Matrix world = _handsState.GetHandTransform(handIndex);
                world = handWorldTransform * world;
                                
                GraphicsDevice.BlendState = BlendState.AlphaBlend;
                GraphicsDevice.DepthStencilState = DepthStencilState.None;
                GraphicsDevice.RasterizerState = (GraphicsDevice.Adapter.Backend == GraphicsBackend.GLES || GraphicsDevice.Adapter.Backend == GraphicsBackend.WebGL)
                                               ? RasterizerState.CullCounterClockwise
                                               : _wireFrameRasterizerState;
                foreach (var m in handModel.Meshes)
                {
                    foreach (var e in m.Effects)
                    {
                        ((BasicEffect)e).Alpha = 0.1f;
                        ((BasicEffect)e).VertexColorEnabled = false;

                    }
                }
                handModel.Draw(world, view, proj);

                GraphicsDevice.BlendState = BlendState.Opaque;
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
                foreach (var m in handModel.Meshes)
                {
                    foreach (var e in m.Effects)
                    {
                        ((BasicEffect)e).Alpha = 1.0f;
                        ((BasicEffect)e).VertexColorEnabled = true;
                    }
                }
                handModel.Draw(world, view, proj);
            }
        }
    }


}
