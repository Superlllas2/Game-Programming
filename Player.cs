using System;

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

        public Canvas enemy;
        public bool collidesWithDoor;
        public int cash;
        
        private AnimationSprite idle;
        private AnimationSprite running;
        private UI ui;

        private float dX;
        private float dY;

        private Sprite VFX;

        public void SetEnemy(Canvas enemy)
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
        
        public Player() : base(10, 10)
        {
            VFX = new Sprite("circle3.png");
            VFX.visible = false;
            VFX.scale=1.2f;
            VFX.blendMode = BlendMode.MULTIPLY; 
            VFX.SetXY(x - game.width/2 - 180, y - game.height/2 - 150);
            AddChild(VFX);
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
            idle.y = -100;
            running.x = -running.width / 2;
            running.y = -100;
            
            // Hitbox
            hitbox = new EasyDraw(100, 100);
            hitbox.Fill(255, 0, 0);
            hitbox.Rect(0, 40, 50, 100);
            hitbox.visible = false;
            AddChild(hitbox);
            
            // ChildrenGarden
            AddChild(running);
            AddChild(idle);
        }
        
        public int GetCash()
        {
            return cash;
        }

        public void SetCash(int value)
        {
            cash = value;
        }
        
        void Update()
        {
            Controls(enemy);
            idle.Animate(0.02f);
            running.Animate(runningAnimationSpeed);
            
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
                idle.visible = false;
                running.visible = true;
                if (Input.GetKey(Key.LEFT_SHIFT))
                {
                    runningAnimationSpeed = 0.1f;
                    playerSpeed = 3f;
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
                if (hitbox.HitTest(enemy))
                {
                    Console.WriteLine("hit");
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
        
        void ActivateHitbox()
        {
            isHitboxActive = true;
            hitbox.visible = true;
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