using System;
using System.Drawing;
using System.IO.Pipes;
using GXPEngine.Core;
using Rectangle = GXPEngine.Core.Rectangle;

namespace GXPEngine
{
    public class Enemy : Canvas
    {
        private static int sizeX = 100;
        private static int sizeY = 100;
        private int detectionRadius = 400;
        private Player player;
        private Vector2 direction;
        private float speed = 1.4f;
        private float lastSeenX;
        private float lastSeenY;
        private float magnitude;
        private bool playerDetected;
        private bool isRunning;
        
        private AnimationSprite idle;
        private AnimationSprite running;
        
        public Enemy(Player player) : base(60,80)
        {
            isRunning = false;
            this.player = player;
            idle = new AnimationSprite("EnemyIdleAnimation.png", 8, 1, -1,
                true, false);
            AddChild(idle);
            running = new AnimationSprite("EnemyRunAnimation.png", 10, 1, -1,
                true, false);
            AddChild(running);
            running.visible = false;
            idle.scale = 0.5f;
            running.scale = 0.5f;
            player.SetEnemy(this);
            RespawnEnemy();
        }

        void Update()
        {
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
                isRunning = false;
                IdleGuard();
                playerDetected = false;
            }

            if (isRunning)
            {
                idle.visible = false;
                running.visible = true;
                running.Animate(0.08f);
            }
        }

        private void UpdateDirection()
        {
            if (playerDetected)
            {
                isRunning = true;
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
            running.visible = false;
            idle.visible = true;
            idle.Animate(0.08f);
        }
        
        public void RemoveFromGame()
        {
            Console.WriteLine("Removing enemy from game");
            if (parent != null)
            {
                parent.RemoveChild(this);
            }
        }

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
                Game.main.Destroy();
            }
        }

        void RespawnEnemy()
        {
            x = Utils.Random(0, 2560 - width);
            y = Utils.Random(0, 2560 - height);
        }
    }
}