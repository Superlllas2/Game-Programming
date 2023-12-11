using System.Collections.Generic;

namespace GXPEngine
{
    public class GameProgramming : Game
    {
        private Sprite level;
        private Sprite sprite;
        private Sprite player;
        private Sprite door;
        private List<Coin> coins;
        
        public GameProgramming() : base(1500, 1000, false)
        {
            sprite = new Sprite("colors.png");
            sprite.y = 100;
            sprite.x = 100;
            AddChild(sprite);

            player = new Player();
            AddChild(player);
            
            door = new Door();
            AddChild(door);

            coins = new List<Coin>();
            for (int i = 0; i < 10; i++) // Example: create 10 coins
            {
                Coin coin = new Coin();
                coins.Add(coin);
                AddChild(coin); // Add each coin to the game
            }
        }
        
         
        static void Main() {
            // Create a "MyGame" and start it:
            new GameProgramming().Start();
        }
    }
}