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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StateMasher
{
    public enum MouseButton
    {
        LeftButton,
        MiddleButton,
        RightButton
    }

    public class State
    {
        public StateMachine _SM = null;
        public Infiniminer.PropertyBag _P = null;

        public virtual void OnEnter(string oldState)
        {
        }

        public virtual void OnLeave(string newState)
        {
        }

        public virtual string OnUpdate(GameTime gameTime, KeyboardState keyState, MouseState mouseState)
        {
            return null;
        }

        public virtual void OnRenderAtEnter(GraphicsDevice graphicsDevice)
        {
        }

        public virtual void OnRenderAtUpdate(GraphicsDevice graphicsDevice, GameTime gameTime)
        {
        }

        public virtual void OnKeyDown(Keys key)
        {
        }

        public virtual void OnKeyUp(Keys key)
        {
        }

        public virtual void OnMouseDown(MouseButton button, int x, int y)
        {
        }

        public virtual void OnMouseUp(MouseButton button, int x, int y)
        {
        }

        public virtual void OnMouseScroll(int scrollWheelValue)
        {
        }

        //public virtual void OnStatusChange(NetConnectionStatus status)
        //{
        //}

        //public virtual void OnPacket(NetBuffer buffer, NetMessageType type)
        //{
        //}
    }
}
