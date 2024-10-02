﻿/* ----------------------------------------------------------------------------
MIT License

Copyright (c) 2009 Zach Barth
Copyright (c) 2023 Christopher Whitley

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
---------------------------------------------------------------------------- */

using System;
using StateMasher;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Infiniminer.States
{
    public class ClassSelectionState : State
    {
        SpriteBatch spriteBatch;
        BasicEffect uiEffect;
        Texture2D texMenuRed, texMenuBlue;
        Rectangle drawRect;
        string nextState = null;

        ClickRegion[] clkClassMenu = new ClickRegion[4] {
            new ClickRegion(new Rectangle(54,168,142,190), "miner"),
            new ClickRegion(new Rectangle(300,169,142,190), "prospector"),
            new ClickRegion(new Rectangle(580,170,133,187), "engineer"),
            new ClickRegion(new Rectangle(819,172,133,190), "sapper")
        };

        int SelectionsCount { get { return clkClassMenu.Length; } }
        int selectionIndex = -1;
        float selectionAlpha = 0;
        Texture2D texMenuDot;
        ClickRegion hoverRegion;

        public override void OnEnter(string oldState)
        {
            _SM.IsMouseVisible = true;

            spriteBatch = new SpriteBatch(_SM.GraphicsDevice);
            uiEffect = new BasicEffect(_SM.GraphicsDevice);
            uiEffect.TextureEnabled = true;
            uiEffect.VertexColorEnabled = true;
            texMenuRed = _SM.Content.Load<Texture2D>("menus/tex_menu_class_red");
            texMenuBlue = _SM.Content.Load<Texture2D>("menus/tex_menu_class_blue");

            texMenuDot = new Texture2D(_SM.GraphicsDevice, 1, 1);
            texMenuDot.SetData<Color>(new Color[] { Color.White });

            UpdateUIViewport(_SM.GraphicsDevice.Viewport);

            _P.KillPlayer("");
        }

        const int VWidth = 1024;
        const int VHeight = 768;
        const float VAspect = (float)VWidth / (float)VHeight;
        private void UpdateUIViewport(Viewport viewport)
        {
            // calculate virtual resolution
            float aspect = viewport.AspectRatio;
            float vWidth = (aspect > VAspect) ? (VHeight * aspect) : VWidth;
            float vHeight = (aspect < VAspect) ? (VWidth / aspect) : VHeight;

            drawRect = new Rectangle((int)vWidth / 2 - VWidth / 2,
                                     (int)vHeight / 2 - VHeight / 2,
                                     1024,
                                     1024);

            Matrix world = Matrix.CreateScale(1f, -1f, -1f) // Flip Y and Depth
                         * Matrix.CreateTranslation(-vWidth / 2f, vHeight / 2f, 0f) // offset center
                         * Matrix.CreateScale(1f / vWidth, 1f / vWidth, 1f); // normalize scale

            if (_SM.propertyBag.playerCamera.UseVrCamera)
            {
                float uiScale = 1f; // scale UI 1meter across.
                world *= Matrix.CreateScale(uiScale, uiScale, 1f);

                // position UI panel
                world *= Matrix.CreateTranslation(0.0f, 0.1f, -1.0f);

                uiEffect.World = world;
                uiEffect.View = _SM.propertyBag.playerCamera.ViewMatrix;
                uiEffect.Projection = _SM.propertyBag.playerCamera.ProjectionMatrix;
            }
            else
            {
                float fov = MathHelper.ToRadians(70);
                float uiScale = ((float)Math.Tan(fov * 0.5)) * aspect * 2f; // scale to fit nearPlane
                world *= Matrix.CreateScale(uiScale, uiScale, 1f);

                world *= Matrix.CreateTranslation(0.0f, 0.0f, -1.0f); // position to near plane

                uiEffect.World = world;
                uiEffect.View = Matrix.Identity;
                uiEffect.Projection = Matrix.CreatePerspectiveFieldOfView(fov, aspect, 1f, 1000.0f);
            }
        }

        public override void OnLeave(string newState)
        {
            _P.RespawnPlayer();
        }

        public override string OnUpdate(GameTime gameTime, KeyboardState keyState, MouseState mouseState)
        {
            // Do network stuff.
            (_SM as InfiniminerGame).UpdateNetwork(gameTime);

            _P.skyplaneEngine.Update(gameTime);
            _P.playerEngine.Update(gameTime);
            _P.interfaceEngine.Update(gameTime);
            _P.particleEngine.Update(gameTime);

            _P.inputEngine.Update(gameTime);

            if (_P.inputEngine.MenuRight.Released())
            {
                selectionIndex = (selectionIndex + 1) % SelectionsCount;
                selectionAlpha = 0;
                _P.PlaySound(InfiniminerSound.ClickLow);
            }
            if (_P.inputEngine.MenuLeft.Released())
            {
                selectionIndex = (selectionIndex - 1 + SelectionsCount) % SelectionsCount;
                selectionAlpha = 0;
                _P.PlaySound(InfiniminerSound.ClickLow);
            }
            if (_SM.WindowHasFocus() && _P.inputEngine.MenuConfirm.Released())
            {
                if (selectionIndex != -1)
                {
                    ClickRegion selectedRegion = clkClassMenu[selectionIndex];
                    SelectNextState(selectedRegion);
                }
            }

            return nextState;
        }

        public override void OnRenderAtEnter(GraphicsDevice graphicsDevice)
        {

        }

        private void DrawSelection(SpriteBatch spriteBatch, GameTime gameTime)
        {
            ClickRegion menuRegion = null;
            if (hoverRegion != null)
                menuRegion = hoverRegion;
            else if (selectionIndex != -1)
                menuRegion = clkClassMenu[selectionIndex];
            else
                return;

            Rectangle rect = menuRegion.Rectangle;
            rect.X += drawRect.X;
            rect.Y += drawRect.Y;

            selectionAlpha += (1f - selectionAlpha) * 0.1f;
            float timeAlpha = (1f + (float)Math.Cos(Math.Tau * gameTime.TotalGameTime.TotalSeconds)) / 2f;
            float alpha = selectionAlpha * 0.175f + timeAlpha * 0.025f;

            spriteBatch.Draw(texMenuDot, rect, new Rectangle(0, 0, 1, 1), Color.White * alpha);
        }

        public override void OnRenderAtUpdate(GraphicsDevice graphicsDevice, GameTime gameTime)
        {
            UpdateUIViewport(graphicsDevice.Viewport);
            spriteBatch.Begin(sortMode: SpriteSortMode.Deferred, blendState: BlendState.AlphaBlend, effect: uiEffect);
            spriteBatch.Draw((_P.playerTeam == PlayerTeam.Red) ? texMenuRed : texMenuBlue, drawRect, Color.White);
            DrawSelection(spriteBatch, gameTime);
            spriteBatch.End();
        }

        public override void OnKeyDown(Keys key)
        {

        }

        public override void OnKeyUp(Keys key)
        {

        }

        public override void OnMouseDown(MouseButton button, int x, int y)
        {
            ScreenToUI(uiEffect, ref x, ref y);
            x -= drawRect.X;
            y -= drawRect.Y;

            ClickRegion selectedRegion = ClickRegion.HitTest(clkClassMenu, new Point(x, y));
            if (selectedRegion != null)
                SelectNextState(selectedRegion);

            selectionIndex = -1;
            selectionAlpha = 0;
        }

        private void SelectNextState(ClickRegion selectedRegion)
        {
            switch (selectedRegion.Tag)
            {
                case "miner":
                    _P.SetPlayerClass(PlayerClass.Miner);
                    nextState = "Infiniminer.States.MainGameState";
                    _P.PlaySound(InfiniminerSound.ClickHigh);
                    break;
                case "engineer":
                    _P.SetPlayerClass(PlayerClass.Engineer);
                    nextState = "Infiniminer.States.MainGameState";
                    _P.PlaySound(InfiniminerSound.ClickHigh);
                    break;
                case "prospector":
                    _P.SetPlayerClass(PlayerClass.Prospector);
                    nextState = "Infiniminer.States.MainGameState";
                    _P.PlaySound(InfiniminerSound.ClickHigh);
                    break;
                case "sapper":
                    _P.SetPlayerClass(PlayerClass.Sapper);
                    nextState = "Infiniminer.States.MainGameState";
                    _P.PlaySound(InfiniminerSound.ClickHigh);
                    break;
            }
        }

        public override void OnMouseUp(MouseButton button, int x, int y)
        {
            ScreenToUI(uiEffect, ref x, ref y);
            x -= drawRect.X;
            y -= drawRect.Y;

        }

        public override void OnMouseScroll(int scrollDelta)
        {
            if (scrollDelta < 0)
            {
                selectionIndex = (selectionIndex + 1) % SelectionsCount;
                selectionAlpha = 0;
                _P.PlaySound(InfiniminerSound.ClickLow);
            }
            else
            {
                selectionIndex = (selectionIndex - 1 + SelectionsCount) % SelectionsCount;
                selectionAlpha = 0;
                _P.PlaySound(InfiniminerSound.ClickLow);
            }
        }

        public override void OnMouseMove(int x, int y)
        {
            ScreenToUI(uiEffect, ref x, ref y);
            x -= drawRect.X;
            y -= drawRect.Y;

            hoverRegion = ClickRegion.HitTest(clkClassMenu, new Point(x, y));
            if (hoverRegion != null)
            {
                selectionIndex = -1;
                selectionAlpha = 0;
            }
        }

        // convert mouse screen position to UI world position
        private void ScreenToUI(IEffectMatrices matrices, ref int x, ref int y)
        {
            Viewport vp = _SM.GraphicsDevice.Viewport;

            Vector3 position3 = vp.Unproject(
                            new Vector3(x, y, 0),
                            matrices.Projection,
                            matrices.View,
                            matrices.World);

            x = (int)position3.X;
            y = (int)position3.Y;
        }
    }
}
