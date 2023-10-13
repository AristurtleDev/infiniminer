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

namespace Infiniminer;

public class Defines
{
    public const string INFINIMINER_VERSION = "v1.5";
    public const int GROUND_LEVEL = 8;

    public const string DEATH_BY_LAVA = "WAS INCINERATED BY LAVA!";
    public const string DEATH_BY_ELEC = "WAS ELECTROCUTED!";
    public const string DEATH_BY_EXPL = "WAS KILLED IN AN EXPLOSION!";
    public const string DEATH_BY_FALL = "WAS KILLED BY GRAVITY!";
    public const string DEATH_BY_MISS = "WAS KILLED BY MISADVENTURE!";
    public const string DEATH_BY_SUIC = "HAS COMMITED PIXELCIDE!";

    public static Color IM_BLUE = new Color(80, 150, 255);
    public static Color IM_RED = new Color(222, 24, 24);
}