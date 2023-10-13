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

namespace Infiniminer;

public class BlockInformation
{
    public static uint GetCost(BlockType blockType)
    {
        switch (blockType)
        {
            case BlockType.BankRed:
            case BlockType.BankBlue:
            case BlockType.BeaconRed:
            case BlockType.BeaconBlue:
                return 50;

            case BlockType.SolidRed:
            case BlockType.SolidBlue:
                return 10;

            case BlockType.TransRed:
            case BlockType.TransBlue:
                return 25;

            case BlockType.Road:
                return 10;
            case BlockType.Jump:
                return 25;
            case BlockType.Ladder:
                return 25;
            case BlockType.Shock:
                return 50;
            case BlockType.Explosive:
                return 100;
        }

        return 1000;
    }

    public static BlockTexture GetTexture(BlockType blockType, BlockFaceDirection faceDir)
    {
        switch (blockType)
        {
            case BlockType.Metal:
                return BlockTexture.Metal;
            case BlockType.Dirt:
                return BlockTexture.Dirt;
            case BlockType.Lava:
                return BlockTexture.Lava;
            case BlockType.Rock:
                return BlockTexture.Rock;
            case BlockType.Ore:
                return BlockTexture.Ore;
            case BlockType.Gold:
                return BlockTexture.Gold;
            case BlockType.Diamond:
                return BlockTexture.Diamond;
            case BlockType.DirtSign:
                return BlockTexture.DirtSign;

            case BlockType.BankRed:
                switch (faceDir)
                {
                    case BlockFaceDirection.XIncreasing: return BlockTexture.BankFrontRed;
                    case BlockFaceDirection.XDecreasing: return BlockTexture.BankBackRed;
                    case BlockFaceDirection.ZIncreasing: return BlockTexture.BankLeftRed;
                    case BlockFaceDirection.ZDecreasing: return BlockTexture.BankRightRed;
                    default: return BlockTexture.BankTopRed;
                }

            case BlockType.BankBlue:
                switch (faceDir)
                {
                    case BlockFaceDirection.XIncreasing: return BlockTexture.BankFrontBlue;
                    case BlockFaceDirection.XDecreasing: return BlockTexture.BankBackBlue;
                    case BlockFaceDirection.ZIncreasing: return BlockTexture.BankLeftBlue;
                    case BlockFaceDirection.ZDecreasing: return BlockTexture.BankRightBlue;
                    default: return BlockTexture.BankTopBlue;
                }

            case BlockType.BeaconRed:
            case BlockType.BeaconBlue:
                switch (faceDir)
                {
                    case BlockFaceDirection.YDecreasing:
                        return BlockTexture.LadderTop;
                    case BlockFaceDirection.YIncreasing:
                        return blockType == BlockType.BeaconRed ? BlockTexture.BeaconRed : BlockTexture.BeaconBlue;
                    case BlockFaceDirection.XDecreasing:
                    case BlockFaceDirection.XIncreasing:
                        return BlockTexture.TeleSideA;
                    case BlockFaceDirection.ZDecreasing:
                    case BlockFaceDirection.ZIncreasing:
                        return BlockTexture.TeleSideB;
                }
                break;

            case BlockType.Road:
                return BlockTexture.Road;

            case BlockType.Shock:
                switch (faceDir)
                {
                    case BlockFaceDirection.YDecreasing:
                        return BlockTexture.Spikes;
                    case BlockFaceDirection.YIncreasing:
                        return BlockTexture.TeleBottom;
                    case BlockFaceDirection.XDecreasing:
                    case BlockFaceDirection.XIncreasing:
                        return BlockTexture.TeleSideA;
                    case BlockFaceDirection.ZDecreasing:
                    case BlockFaceDirection.ZIncreasing:
                        return BlockTexture.TeleSideB;
                }
                break;

            case BlockType.Jump:
                switch (faceDir)
                {
                    case BlockFaceDirection.YDecreasing:
                        return BlockTexture.TeleBottom;
                    case BlockFaceDirection.YIncreasing:
                        return BlockTexture.JumpTop;
                    case BlockFaceDirection.XDecreasing:
                    case BlockFaceDirection.XIncreasing:
                        return BlockTexture.Jump;
                    case BlockFaceDirection.ZDecreasing:
                    case BlockFaceDirection.ZIncreasing:
                        return BlockTexture.Jump;
                }
                break;

            case BlockType.SolidRed:
                return BlockTexture.SolidRed;
            case BlockType.SolidBlue:
                return BlockTexture.SolidBlue;
            case BlockType.TransRed:
                return BlockTexture.TransRed;
            case BlockType.TransBlue:
                return BlockTexture.TransBlue;

            case BlockType.Ladder:
                if (faceDir == BlockFaceDirection.YDecreasing || faceDir == BlockFaceDirection.YIncreasing)
                    return BlockTexture.LadderTop;
                else
                    return BlockTexture.Ladder;

            case BlockType.Explosive:
                return BlockTexture.Explosive;
        }

        return BlockTexture.None;
    }
}
