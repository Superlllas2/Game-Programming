using System;
using System.Drawing;
using System.IO.Pipes;
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
        private float magnitude;
        private bool playerDetected;
        private Time elapsedTime;
            
        public Enemy(Sprite player) : base(sizeX, sizeY)
        {
            this.player = player;
            elapsedTime = new Time();
            RespawnEnemy();
            Fill(255);
            Rect(0, 0, sizeX, sizeY);
        }

        void Update()
        {
            // Console.WriteLine(elapsedTime.);
            UpdateDirection();
            MoveTowardsPlayer();
            if (DistanceTo(player) < detectionRadius)
            {
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
                // Calculate the direction vector from enemy to player if IN detection radius
                direction.x = player.x - x;
                direction.y = player.y - y;
            }
            else
            {
                // Calculate the direction vector from enemy to player if NOT IN detection radius
                if (lastSeenX != 0 && lastSeenY != 0)
                {
                    direction.x = lastSeenX - x;
                    direction.y = lastSeenY - y; 
                }
            }
            
            // Manually normalize the direction vector
            magnitude = (float)Math.Sqrt(direction.x * direction.x + direction.y * direction.y);
            if (magnitude > 1)
            {
                direction.x /= magnitude;
                direction.y /= magnitude;
            }
        }

        private void IdleGuard()
        {
            
        }
        
        // TODO: Finish normalization method extraction
        // private Vector2 Normalize()
        // {
        //     return magnitude;
        // }

        private void MoveTowardsPlayer()
        {
            // Move the enemy towards the player
            x += direction.x * speed;
            y += direction.y * speed;
        }

        // Logic for collision with enemy
        void OnCollision(GameObject other)
        {
            if (other is Player)
            {
                
            }
        }

        void RespawnEnemy()
        {
            x = Utils.Random(0, game.width - width);
            y = Utils.Random(0, game.height - height);
        }
    }
}