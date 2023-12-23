using System;
using GXPEngine.Core;
using GXPEngine.Managers;

namespace GXPEngine
{
    public class Coin : Canvas
    {
        private AnimationSprite defaultAnimation;
        private Vector2 direction;
        private Sprite player;
        private float speed = 5f;
        
        public Coin(Sprite player) : base(30, 30)
        {
            Spawn();
            this.player = player;
            defaultAnimation = new AnimationSprite("CoinAnimation.png", 6, 1, -1,
                true, false);
            defaultAnimation.SetFrame(Utils.Random(0, defaultAnimation.frameCount));
            AddChild(defaultAnimation);
        }

        void Update()
        {
            Animate();
            if (DistanceTo(player) < 100)
            {
                UpdateDirection();
                MoveTowardsPlayer();
            }
        }
        
        private void UpdateDirection()
        {
            // Calculate the direction vector from enemy to player
            direction.x = player.x - x;
            direction.y = player.y - y;
            // Manually normalize the direction vector
            float magnitude = (float)Math.Sqrt(direction.x * direction.x + direction.y * direction.y);
            if (magnitude > 0)
            {
                direction.x /= magnitude;
                direction.y /= magnitude;
            }
        }
        
        private void MoveTowardsPlayer()
        {
            // Move the enemy towards the player
            x += direction.x * speed;
            y += direction.y * speed;
        }
        
        void Spawn()
        {
            
            x = Utils.Random(0, game.width - this.width);
            y = Utils.Random(0, game.height - this.height);
        }

        void Animate()
        {
            defaultAnimation.Animate(0.04f);
        }
        
        public void PickUp()
        {
            LateDestroy();
            
        } 
    }
}