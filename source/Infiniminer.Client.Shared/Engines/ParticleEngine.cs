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

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Infiniminer
{
    public class Particle
    {
        public Vector3 Position;
        public Vector3 Velocity;
        public float Size;
        public Color Color;
        public bool FlaggedForDeletion = false;
    }

    public class ParticleEngine
    {
        InfiniminerGame gameInstance;
        PropertyBag _P;
        List<Particle> particleList;
        Effect particleEffect;
        Random randGen;
        VertexDeclaration vertexDeclaration;
        VertexBuffer vertexBuffer;

        public ParticleEngine(InfiniminerGame gameInstance)
        {
            this.gameInstance = gameInstance;
            particleEffect = gameInstance.Content.Load<Effect>("effect_particle");
            randGen = new Random();
            particleList = new List<Particle>();

            vertexDeclaration = new VertexDeclaration(VertexPositionTextureShade.VertexElements);
            VertexPositionTextureShade[] vertices = GenerateVertices();
            vertexBuffer = new VertexBuffer(gameInstance.GraphicsDevice, typeof(VertexPositionTextureShade), vertices.Length * VertexPositionTextureShade.SizeInBytes, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertices);
        }

        private VertexPositionTextureShade[] GenerateVertices()
        {
            VertexPositionTextureShade[] cubeVerts = new VertexPositionTextureShade[36];

            // BOTTOM
            cubeVerts[0] = new VertexPositionTextureShade(new Vector3(-1, -1, -1), new Vector2(0, 0), 0.3);
            cubeVerts[1] = new VertexPositionTextureShade(new Vector3(1, -1, -1), new Vector2(0, 0), 0.3);
            cubeVerts[2] = new VertexPositionTextureShade(new Vector3(-1, 1, -1), new Vector2(0, 0), 0.3);
            cubeVerts[3] = new VertexPositionTextureShade(new Vector3(1, -1, -1), new Vector2(0, 0), 0.3);
            cubeVerts[4] = new VertexPositionTextureShade(new Vector3(1, 1, -1), new Vector2(0, 0), 0.3);
            cubeVerts[5] = new VertexPositionTextureShade(new Vector3(-1, 1, -1), new Vector2(0, 0), 0.3);

            // TOP
            cubeVerts[30] = new VertexPositionTextureShade(new Vector3(-1, -1, 1), new Vector2(0, 0), 1.0);
            cubeVerts[31] = new VertexPositionTextureShade(new Vector3(1, -1, 1), new Vector2(0, 0), 1.0);
            cubeVerts[32] = new VertexPositionTextureShade(new Vector3(-1, 1, 1), new Vector2(0, 0), 1.0);
            cubeVerts[33] = new VertexPositionTextureShade(new Vector3(1, -1, 1), new Vector2(0, 0), 1.0);
            cubeVerts[34] = new VertexPositionTextureShade(new Vector3(1, 1, 1), new Vector2(0, 0), 1.0);
            cubeVerts[35] = new VertexPositionTextureShade(new Vector3(-1, 1, 1), new Vector2(0, 0), 1.0);

            // LEFT
            cubeVerts[6] = new VertexPositionTextureShade(new Vector3(-1, -1, -1), new Vector2(0, 0), 0.7);
            cubeVerts[7] = new VertexPositionTextureShade(new Vector3(-1, -1, 1), new Vector2(0, 0), 0.7);
            cubeVerts[8] = new VertexPositionTextureShade(new Vector3(-1, 1, 1), new Vector2(0, 0), 0.7);
            cubeVerts[9] = new VertexPositionTextureShade(new Vector3(-1, -1, -1), new Vector2(0, 0), 0.7);
            cubeVerts[10] = new VertexPositionTextureShade(new Vector3(-1, 1, 1), new Vector2(0, 0), 0.7);
            cubeVerts[11] = new VertexPositionTextureShade(new Vector3(-1, 1, -1), new Vector2(0, 0), 0.7);

            // RIGHT
            cubeVerts[12] = new VertexPositionTextureShade(new Vector3(1, -1, -1), new Vector2(0, 0), 0.7);
            cubeVerts[13] = new VertexPositionTextureShade(new Vector3(1, -1, 1), new Vector2(0, 0), 0.7);
            cubeVerts[14] = new VertexPositionTextureShade(new Vector3(1, 1, 1), new Vector2(0, 0), 0.7);
            cubeVerts[15] = new VertexPositionTextureShade(new Vector3(1, -1, -1), new Vector2(0, 0), 0.7);
            cubeVerts[16] = new VertexPositionTextureShade(new Vector3(1, 1, 1), new Vector2(0, 0), 0.7);
            cubeVerts[17] = new VertexPositionTextureShade(new Vector3(1, 1, -1), new Vector2(0, 0), 0.7);

            // FRONT
            cubeVerts[18] = new VertexPositionTextureShade(new Vector3(-1, 1, -1), new Vector2(0, 0), 0.5);
            cubeVerts[19] = new VertexPositionTextureShade(new Vector3(-1, 1, 1), new Vector2(0, 0), 0.5);
            cubeVerts[20] = new VertexPositionTextureShade(new Vector3(1, 1, 1), new Vector2(0, 0), 0.5);
            cubeVerts[21] = new VertexPositionTextureShade(new Vector3(-1, 1, -1), new Vector2(0, 0), 0.5);
            cubeVerts[22] = new VertexPositionTextureShade(new Vector3(1, 1, 1), new Vector2(0, 0), 0.5);
            cubeVerts[23] = new VertexPositionTextureShade(new Vector3(1, 1, -1), new Vector2(0, 0), 0.5);

            // BACK
            cubeVerts[24] = new VertexPositionTextureShade(new Vector3(-1, -1, -1), new Vector2(0, 0), 0.5);
            cubeVerts[25] = new VertexPositionTextureShade(new Vector3(-1, -1, 1), new Vector2(0, 0), 0.5);
            cubeVerts[26] = new VertexPositionTextureShade(new Vector3(1, -1, 1), new Vector2(0, 0), 0.5);
            cubeVerts[27] = new VertexPositionTextureShade(new Vector3(-1, -1, -1), new Vector2(0, 0), 0.5);
            cubeVerts[28] = new VertexPositionTextureShade(new Vector3(1, -1, 1), new Vector2(0, 0), 0.5);
            cubeVerts[29] = new VertexPositionTextureShade(new Vector3(1, -1, -1), new Vector2(0, 0), 0.5);

            return cubeVerts;
        }

        private static bool ParticleExpired(Particle particle)
        {
            return particle.FlaggedForDeletion;
        }

        public void Update(GameTime gameTime)
        {
            if (_P == null)
                return;

            foreach (Particle p in particleList)
            {
                p.Position += (float)gameTime.ElapsedGameTime.TotalSeconds * p.Velocity;
                p.Velocity.Y -= 8 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_P.blockEngine.SolidAtPoint(p.Position))
                    p.FlaggedForDeletion = true;
            }
            particleList.RemoveAll(ParticleExpired);
        }

        public void CreateExplosionDebris(Vector3 explosionPosition)
        {
            for (int i = 0; i < 50; i++)
            {
                Particle p = new Particle();
                p.Color = new Color(90, 60, 40);
                p.Size = (float)(randGen.NextDouble() * 0.4 + 0.05);
                p.Position = explosionPosition;
                p.Position.Y += (float)randGen.NextDouble() - 0.5f;
                p.Velocity = new Vector3((float)randGen.NextDouble() * 8 - 4, (float)randGen.NextDouble() * 8, (float)randGen.NextDouble() * 8 - 4);
                particleList.Add(p);
            }
        }

        public void CreateBloodSplatter(Vector3 playerPosition, Color color)
        {
            for (int i = 0; i < 30; i++)
            {
                Particle p = new Particle();
                p.Color = color;
                p.Size = (float)(randGen.NextDouble() * 0.2 + 0.05);
                p.Position = playerPosition;
                p.Position.Y -= (float)randGen.NextDouble();
                p.Velocity = new Vector3((float)randGen.NextDouble() * 5 - 2.5f, (float)randGen.NextDouble() * 4f, (float)randGen.NextDouble() * 5 - 2.5f);
                particleList.Add(p);
            }
        }

        public void Render(GraphicsDevice graphicsDevice)
        {
            // If we don't have _P, grab it from the current gameInstance.
            // We can't do this in the constructor because we are created in the property bag's constructor!
            if (_P == null)
                _P = gameInstance.propertyBag;

            foreach (Particle p in particleList)
            {
                Matrix worldMatrix = Matrix.CreateScale(p.Size / 2) * Matrix.CreateTranslation(p.Position);
                particleEffect.Parameters["xWorld"].SetValue(worldMatrix);
                particleEffect.Parameters["xView"].SetValue(_P.playerCamera.ViewMatrix);
                particleEffect.Parameters["xProjection"].SetValue(_P.playerCamera.ProjectionMatrix);
                particleEffect.Parameters["xColor"].SetValue(p.Color.ToVector4());
                particleEffect.Techniques[0].Passes[0].Apply();

                graphicsDevice.RasterizerState = RasterizerState.CullNone;
                graphicsDevice.SetVertexBuffer(vertexBuffer);
                graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, vertexBuffer.VertexCount / 3);
            }
        }
    }
}
