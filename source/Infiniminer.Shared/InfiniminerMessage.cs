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

public enum InfiniminerMessage : byte
{
    BlockBulkTransfer,      // x-value, y-value, followed by 64 bytes of blocktype ; 
    BlockSet,               // x, y, z, type
    UseTool,                // position, heading, tool, blocktype 
    SelectClass,            // class
    ResourceUpdate,         // ore, cash, weight, max ore, max weight, team ore, red cash, blue cash: ReliableInOrder1
    DepositOre,
    DepositCash,
    WithdrawOre,
    TriggerExplosion,       // position

    PlayerUpdate,           // (uint id for server), position, heading, current tool, animate using (bool): UnreliableInOrder1
    PlayerJoined,           // uint id, player name :ReliableInOrder2
    PlayerLeft,             // uint id              :ReliableInOrder2
    PlayerSetTeam,          // (uint id for server), byte team   :ReliableInOrder2
    PlayerDead,             // (uint id for server) :ReliableInOrder2
    PlayerAlive,            // (uint id for server) :ReliableInOrder2
    PlayerPing,             // uint id

    ChatMessage,            // byte type, string message : ReliableInOrder3
    GameOver,               // byte team
    PlaySound,              // byte sound, bool isPositional, ?Vector3 location : ReliableUnordered
    TriggerConstructionGunAnimation,
    SetBeacon,              // vector3 position, string text ("" means remove)
}
