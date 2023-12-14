using System.Collections.Generic;

namespace GXPEngine
{
    public class GameProgramming : Game
    {
        private Sprite level;
        private Sprite sprite;
        private Sprite player;
        private Sprite door;
        private Enemy enemy;
        private List<Coin> coins;
        // private Level level;
        
        public GameProgramming() : base(1500, 1000, false)
        {
            // level = new Level();
            player = new Player();
            AddChild(player);
            
            enemy = new Enemy(player);
            AddChild(enemy);

            door = new Door();
            AddChild(door);
            
            coins = new List<Coin>();
            for (int i = 0; i < 10; i++) // Example: create 10 coins
            {
                Coin coin = new Coin(player);
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