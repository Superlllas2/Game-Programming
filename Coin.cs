using System;

namespace GXPEngine
{
    public class Coin : Sprite
    {
        public Coin() : base("checkers.png")
        {
            Spawn();
        }

        void Spawn()
        {
            x = Utils.Random(0, game.width - this.width);
            y = Utils.Random(0, game.height - this.height);
        }

        public void PickUp()
        {
            Spawn();
            
        } 
    }
}