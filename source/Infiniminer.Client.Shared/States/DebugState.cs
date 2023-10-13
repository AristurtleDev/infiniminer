/* ----------------------------------------------------------------------------
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

using System.Diagnostics;
using StateMasher;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Infiniminer.States
{
    public class DebugState : State
    {
        private double flashCounter = 0;

        public override void OnEnter(string oldState)
        {
        }

        public override void OnLeave(string newState)
        {
        }

        public override string OnUpdate(GameTime gameTime, KeyboardState keyState, MouseState mouseState)
        {
            flashCounter += gameTime.ElapsedGameTime.TotalSeconds;
            if (flashCounter > 0.5)
                flashCounter = 0;
            return null;
        }

        public override void OnRenderAtEnter(GraphicsDevice graphicsDevice)
        {
        }

        public override void OnRenderAtUpdate(GraphicsDevice graphicsDevice, GameTime gameTime)
        {
            if (flashCounter < 0.25)
                graphicsDevice.Clear(Color.Blue);
            else
                graphicsDevice.Clear(Color.Red);
        }

        public override void OnKeyDown(Keys key)
        {
            Debug.Print("OnKeyDown(" + key.ToString() + ")");
        }

        public override void OnKeyUp(Keys key)
        {
            Debug.Print("OnKeyUp(" + key.ToString() + ")");
        }

        public override void OnMouseDown(MouseButton button, int x, int y)
        {
            Debug.Print("OnMouseDown(" + button + ", " + x + ", " + y + ")");
        }

        public override void OnMouseUp(MouseButton button, int x, int y)
        {
            Debug.Print("OnMouseUp(" + button + ", " + x + ", " + y + ")");
        }

        public override void OnMouseScroll(int scrollDelta)
        {
            Debug.Print("OnMouseScroll(" + scrollDelta + ")");
        }

        //public override void OnStatusChange(NetConnectionStatus status)
        //{
        //}

        //public override void OnPacket(NetBuffer buffer, NetMessageType type)
        //{
        //}
    }
}
