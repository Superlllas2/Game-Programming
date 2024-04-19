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
        private int detectionRadius = 300;
        private Player player;
        private Vector2 direction;
        private float speed = 0.8f;
        private float lastSeenX;
        private float lastSeenY;
        private float magnitude;
        private bool playerDetected;
        private bool isRunning;
        
        private AnimationSprite idle;
        private AnimationSprite running;
        
        private int minDistance = 300;

        private Sound damageSound;
        private float knockbackStrength = 10f;
        private int health = 3;
        private Vector2 knockbackVelocity;
        float initialKnockbackSpeed = 2f;
        private bool isKnockedBack;
        private float attackCooldown = 1500.0f;
        private float attackTimer;
        
        public Enemy(Player player) : base(60,80)
        {
            damageSound = new Sound("SFX/damage.mp3");
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

        public void ReceiveDamage()
        {
            health--;
            damageSound.Play(volume: 0.8f);
            
            if (health > 0)
            {
                Vector2 knockbackDirection = new Vector2(x - (player.x), y - (player.y));
                
                float knockbackMagnitude = (float)Math.Sqrt(knockbackDirection.x * knockbackDirection.x + knockbackDirection.y * knockbackDirection.y);
                if (knockbackMagnitude > 0)
                {
                    knockbackDirection.x /= knockbackMagnitude;
                    knockbackDirection.y /= knockbackMagnitude;
                }
                
                knockbackVelocity = new Vector2(knockbackDirection.x * initialKnockbackSpeed, knockbackDirection.y * initialKnockbackSpeed);

                isKnockedBack = true;
            }
        }
        
        void Update()
        {
            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }
            
            if (isKnockedBack)
            {
                x += knockbackVelocity.x;
                y += knockbackVelocity.y;
                
                float inertia = 0.9f;
                knockbackVelocity.x *= inertia;
                knockbackVelocity.y *= inertia;
                
                if (Math.Abs(knockbackVelocity.x) < 0.1f && Math.Abs(knockbackVelocity.y) < 0.1f)
                {
                    isKnockedBack = false;
                    knockbackVelocity = new Vector2(0, 0);
                }
            }
            else
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
            
            if (health == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            LateRemove();
        }
        
        private void UpdateDirection()
        {
            if (playerDetected)
            {
                isRunning = true;
                // Calculating the direction vector from enemy to player if IN detection radius
                direction.x = player.x - x;
                direction.y = player.y - y - 60;
            }
            else
            {
                // Calculating the direction vector from enemy to player if NOT IN detection radius
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
            if (other is Player  && attackTimer <= 0)
            {
                player.ReceiveDamage();
                attackTimer = attackCooldown;
            }
        }

        void RespawnEnemy()
        {
            // If a random spawn point is too close to the player, then spawn in another place
            do
            {
                x = Utils.Random(160, 2400);
                y = Utils.Random(160, 2400);
            } while ((Math.Sqrt((x - player.x) * (x - player.x) + (y - player.y) * (y - player.y)) < minDistance));
        }
    }
}