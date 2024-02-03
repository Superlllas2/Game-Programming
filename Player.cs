using System;
using System.Drawing;
using System.Security.AccessControl;
using GXPEngine;


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

        public EasyDraw enemy;
        
        private AnimationSprite idle;
        private AnimationSprite running;
        private int cash;
        private EasyDraw hitArea;
        private EasyDraw UI;

        public void SetEnemy(EasyDraw enemy)
        {
            this.enemy = enemy;
        }
        
        public Player() : base(60,80)
        {
           // graphics.Clear(Color.Red);
            RespawnPlayer();
            SetOrigin(width/2, height);
            // TODO: fix animation spriteSheet
            idle = new AnimationSprite("Main character/Player/SpriteSheets/idle.png", 8, 1, -1,
                true, false);
            running = new AnimationSprite("Main character/Player/SpriteSheets/running.png", 10, 1, -1,
                true, false);
            idle.scale = 0.5f;
            running.scale = 0.5f;
            idle.x = -idle.width / 2;
            idle.y = -100;
            running.x = -running.width / 2;
            running.y = -100;
            
            // Hitbox
            hitbox = new EasyDraw(50, 100); // Adjust size as needed
            hitbox.Fill(255, 0, 0); // Red color
            hitbox.Rect(0, 40, 50, 100); // Draw a square
            hitbox.visible = false; // Initially invisible
            game.AddChild(hitbox);

            // Some UI
            UI = new EasyDraw(128, 24);
            UI.SetXY(game.width - UI.width, 0);
            
            // ChildrenGarden
            game.AddChild(UI);
            AddChild(running);
            AddChild(idle);
        }
        
        void Update()
        {
            Console.WriteLine("Enemy Position: " + enemy.x + ", " + enemy.y);
            Controls(enemy);
            // TODO: Fix Rect next to the player. Realise why x and y params interfere with width and height
            ShowUI();
            idle.Animate(0.02f);
            running.Animate(runningAnimationSpeed);
            
            // Updating hitbox for testing
            UpdateHitboxPosition();
            UpdateHitbox();
        }

        void Controls(GameObject other)
        {
            playerSpeed = 3f;

            // WHENEVER MOVING
            if (Input.GetKey(Key.RIGHT) || Input.GetKey(Key.LEFT) || Input.GetKey(Key.UP) || Input.GetKey(Key.DOWN))
            {
                // If moving, making running animation active and other way around
                idle.visible = false;
                running.visible = true;
                if (Input.GetKey(Key.LEFT_SHIFT))
                {
                    runningAnimationSpeed = 0.1f;
                    playerSpeed = 4f;
                }
                else
                {
                    runningAnimationSpeed = 0.08f;
                }
            }
            else
            {
                running.visible = false;
                idle.visible = true;
            }
            
            // MOUSE LKM
            if (Input.GetMouseButtonUp(0)) 
            {
                ActivateHitbox();
                if (other is Enemy)
                {
                    Console.WriteLine("Enemy Position: " + enemy.x + ", " + enemy.y);
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
                idle.Mirror(false,false);
                running.Mirror(false, false);
                x += playerSpeed;
            } else if (Input.GetKey(Key.LEFT))
            { 
                idle.Mirror(true,false);
                running.Mirror(true, false);
                x -= playerSpeed;
            }
            if (Input.GetKey(Key.UP))
            {
                y -= playerSpeed;
            } else if (Input.GetKey(Key.DOWN))
            {
                y += playerSpeed;
            }
            
            // FIGHTING
            if (Input.GetKey(Key.LEFT_ALT))
            {
                
            }
        }
        
        void ActivateHitbox()
        {
            isHitboxActive = true;
            hitbox.visible = true;
            hitbox.x = x + 20; // Adjust position as needed
            hitbox.y = y;
            hitboxTimer = 0;
        }
        
        void UpdateHitbox()
        {
            if (isHitboxActive)
            {
                hitboxTimer += Time.deltaTime;
                if (hitboxTimer >= 1000) // 2 seconds
                {
                    hitbox.visible = false;
                    isHitboxActive = false;
                }
            }
        }
        
        void UpdateHitboxPosition()
        {
            if (isHitboxActive)
            {
                // Update hitbox position relative to player
                hitbox.x = x + 20; // Adjust the offset as needed
                hitbox.y = y;
            }
        }
        
        void OnCollision(GameObject other)
        {
            if (other is Door)
            {
                RespawnPlayer();
            }

            if (other is Coin)
            {
                Coin coin = other as Coin;
                coin.PickUp();
                cash++;
            }
        }
        
        void RespawnPlayer()
        {
            // Set Up initial position
            x = 600;
            y = 500;
        }

        void ShowUI()
        {
            UI.ClearTransparent();
            UI.Text("Cash: " + cash);
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