namespace GXPEngine
{
    public class GameProgramming : Game
    {
        public GameProgramming() : base(800, 600, false)
        {
            Sprite sprite = new Sprite("colors.png");
            sprite.x = 100;
            sprite.y = 100;
            Player player = new Player();
            AddChild(sprite);
            AddChild(player);
        }
        
         
        static void Main() {
            // Create a "MyGame" and start it:
            new GameProgramming().Start();
        }
    }
}