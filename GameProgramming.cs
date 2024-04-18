using System;
using System.Collections.Generic;
using TiledMapParser;

namespace GXPEngine
{
    public class GameProgramming : Game
    {
        GameProgramming() : base(1500, 1000, false)
        {
            Menu menu = new Menu(true, false);
            AddChild(menu);
        }


        static void Main()
        {
            // Create a "MyGame" and start it:
            new GameProgramming().Start();
        }
    }
}