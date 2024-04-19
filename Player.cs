using System;
using System.Collections.Generic;
using GXPEngine.Core;

namespace GXPEngine
{
    public class Player : Canvas
    {
        // TODO: get rid of hard coded values
        private float playerSpeed;
        private float runningAnimationSpeed;
        
        // Hitbox variables
        private EasyDraw hitbox;
        private bool isHitboxActive;
        private float hitboxTimer;

        private Enemy enemy;
        
        public bool collidesWithDoor;
        public int cash;
        
        private AnimationSprite idle;
        private AnimationSprite running;
        private AnimatedSprite attackAnimation;
        private UI ui;

        private float dX;
        private float dY;

        private Sprite VFX;
        private Sound swordSwing;

        private Sound damageSound;
        private Vector2 knockbackVelocity;
        private float initialKnockbackSpeed = 20f;
        private float knockbackStrength = 100f;
        private bool isKnockedBack;
        private int health;
        
        public void SetEnemy(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public void SetVFX(bool isActive)
        {
            if (isActive)
            {
                VFX.visible = true;
            }
            else
            {
                VFX.visible = false;
            }
        }
        
        public int GetCash()
        {
            return cash;
        }

        public void SetCash(int value)
        {
            cash = value;
        }

        public int GetHealth()
        {
            return health;
        }
        
        public Player() : base(10, 10)
        {
            VFX = new Sprite("circle3.png");
            VFX.visible = false;
            VFX.scale = 1.2f;
            VFX.blendMode = BlendMode.MULTIPLY; 
            VFX.SetXY(x - game.width/2 - 180, y - game.height/2 - 150);
            AddChild(VFX);

            damageSound = new Sound("SFX/damage.mp3");
            swordSwing = new Sound("SFX/swordSwing.mp3");

            health = 3;
            
            ui = new UI(cash);
            AddChild(ui);
            
            ui.SetXY(-750, -500);
            RespawnPlayer();
            
            // TODO: fix animation spriteSheet
            idle = new AnimationSprite("Main character/Player/SpriteSheets/idle2.png", 8, 1, -1,
                true, false);
            running = new AnimationSprite("Main character/Player/SpriteSheets/running2.png", 10, 1, -1,
                true, false);
            idle.scale = 0.5f;
            running.scale = 0.5f;
            idle.x = -idle.width / 2;
            idle.y = -idle.height / 2;
            running.x = -running.width / 2;
            running.y = -running.height / 2;
            attackAnimation = new AnimatedSprite("Main character/Player/SpriteSheets/playerAttack.png", 8, 1);
            attackAnimation.scale = 0.5f;
            attackAnimation.visible = false;
            attackAnimation.SetFrameDelay(15);
            attackAnimation.SetXY(-attackAnimation.width / 2, -attackAnimation.height / 2);
            AddChild(attackAnimation);
            
            // Hitbox
            hitbox = new EasyDraw(100, 275);
            hitbox.Fill(255, 0, 0);
            hitbox.Rect(0, 0, 200, 275);
            hitbox.SetXY(30, -75);
            hitbox.visible = false;
            AddChild(hitbox);
            
            // ChildrenGarden
            AddChild(running);
            AddChild(idle);
        }
        
        void Update()
        {
            idle.Animate(0.02f);
            running.Animate(runningAnimationSpeed);
            
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
                Controls(enemy);
            }
            
            // Updating hitbox for testing
            UpdateHitbox();
        }

        void Controls(GameObject other)
        {
            dX = x;
            dY = y;
            playerSpeed = 2f;
            // WHENEVER MOVING
            if (Input.GetKey(Key.RIGHT) || Input.GetKey(Key.LEFT) || Input.GetKey(Key.UP) || Input.GetKey(Key.DOWN))
            {
                if (Input.GetKey(Key.LEFT_SHIFT))
                {
                    runningAnimationSpeed = 0.1f;
                    playerSpeed = 3f;
                }
                else
                {
                    runningAnimationSpeed = 0.08f;
                }

                if (Input.GetMouseButtonUp(0))
                {
                    Hit();
                }

                if (attackAnimation.visible == false)
                {
                    idle.visible = false;
                    running.visible = true;
                }
            } else if (Input.GetMouseButtonUp(0))
            {
                Hit();
            }
            else
            {
                if (attackAnimation.visible == false)
                {
                    running.visible = false;
                    idle.visible = true;
                }
            }
            
            // If running diagonally, speed slows down
            if (CheckDiagonal())
            {
                playerSpeed *= 0.7071f;
            }
                
            // MOVEMENT
            if (Input.GetKey(Key.RIGHT))
            {
                hitbox.SetXY(30, -65);
                hitbox.Mirror(false, false);
                idle.Mirror(false,false);
                running.Mirror(false, false);
                attackAnimation.Mirror(false, false);
                x += playerSpeed;
            } else if (Input.GetKey(Key.LEFT))
            { 
                hitbox.SetXY(-110, -65);
                idle.Mirror(true,false);
                running.Mirror(true, false);
                attackAnimation.Mirror(true, false);
                x -= playerSpeed;
            }
            if (Input.GetKey(Key.UP))
            {
                y -= playerSpeed;
            } else if (Input.GetKey(Key.DOWN))
            {
                y += playerSpeed;
            }
            
            GameObject[] collisions = GetCollisions();
            foreach (var obj in collisions)
            {
                if (obj is AnimationSprite && obj.name == "wall")
                {
                    x = dX;
                    y = dY;
                    break;
                }
            }
        }

        
        public void ReceiveDamage() {
            health--;
            damageSound.Play(volume: 0.8f);
    
            if (health > 0) {
                Vector2 knockbackDirection = new Vector2(x - enemy.x, y - enemy.y);
                
                float knockbackMagnitude = (float)Math.Sqrt(knockbackDirection.x * knockbackDirection.x + knockbackDirection.y * knockbackDirection.y);
                if (knockbackMagnitude > 0) {
                    knockbackDirection.x /= knockbackMagnitude;
                    knockbackDirection.y /= knockbackMagnitude;
                }
        
                knockbackVelocity = new Vector2(knockbackDirection.x * initialKnockbackSpeed, knockbackDirection.y * initialKnockbackSpeed);
                isKnockedBack = true;
                
                // speed = 0;
            } else {
                Die();
            }
        }
        
        void Hit()
        {
            // MOUSE LKM
            swordSwing.Play();
            idle.visible = false;
            running.visible = false;
            attackAnimation.visible = true;
            attackAnimation.PlayOnce();
            ActivateHitbox();
            
            if (hitbox.HitTest(enemy))
            {
                Console.WriteLine(enemy.name);
                if (attackAnimation.ReadyForAction())
                {
                    enemy.ReceiveDamage();
                }
            }
        }
        
        void ActivateHitbox()
        {
            isHitboxActive = true;
            hitbox.visible = false;
            hitboxTimer = 0;
        }
        
        void UpdateHitbox()
        {
            // if (isHitboxActive)
            // {
            //     hitboxTimer += Time.deltaTime;
            //     if (hitboxTimer >= 1000) // 2 seconds
            //     {
            //         hitbox.visible = false;
            //         isHitboxActive = false;
            //     }
            // }
        }
        
        void OnCollision(GameObject other)
        {
            if (other is Door)
            {
                collidesWithDoor = true;
            }

            if (other is Coin)
            {
                Coin coin = other as Coin;
                coin.PickUp();
                cash++;
            }
        }

        void Die()
        {
            List<GameObject> children = parent.GetChildren();
            foreach (GameObject child in children)
            {
                child.LateDestroy();
            }
            
            Menu menu = new Menu(false, false);
            game.LateAddChild(menu);
        }
        
        void RespawnPlayer()
        {
            // Set Up initial position
            x = 200;
            y = 250;
        }
        
        bool CheckDiagonal()
        {
            return ((Input.GetKey(Key.DOWN) && Input.GetKey(Key.LEFT)) ||
                    (Input.GetKey(Key.DOWN) && Input.GetKey(Key.RIGHT)) ||
                    (Input.GetKey(Key.UP) && Input.GetKey(Key.LEFT)) ||
                    (Input.GetKey(Key.UP) && Input.GetKey(Key.RIGHT)));
        }
    }
}