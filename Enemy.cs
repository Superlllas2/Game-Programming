using System;
using System.Drawing;
using GXPEngine.Core;

namespace GXPEngine
{
    public class Enemy : EasyDraw
    {
        private static int sizeX = 100;
        private static int sizeY = 100;
        private int detectionRadius = 200;
        private Sprite player;
        private Vector2 direction;
        private float speed = 1.2f;
        private float lastSeenX;
        private float lastSeenY;
        private bool playerDetected = false;
            
        public Enemy(Sprite player) : base(sizeX, sizeY)
        {
            this.player = player;
            RespawnEnemy();
            Fill(255);
            Rect(0, 0, sizeX, sizeY);
        }

        void Update()
        {
            if (DistanceTo(player) < detectionRadius)
            {
                UpdateDirection();
                MoveTowardsPlayer();
                lastSeenX = player.x;
                lastSeenY = player.y;
                playerDetected = true;
            }
            else
            {
                playerDetected = false;
            }
        }

        private void UpdateDirection()
        {
            if (playerDetected)
            {
                
            }
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

        private void MoveTowardsLastSeen()
        {
            // Move the enemy towards the last position of the player seen
        }
        
        void OnCollision(GameObject other)
        {
            if (other is Player)
            {
            }
        }

        void RespawnEnemy()
        {
            x = Utils.Random(0, game.width - this.width);
            y = Utils.Random(0, game.height - this.height);
        }
    }
}