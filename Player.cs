using System;
using System.Drawing;
using System.Security.AccessControl;
using GXPEngine;


namespace GXPEngine
{
    public class Player : Canvas
    {
        private float playerSpeed;
        private float runningAnimationSpeed;
        
        private AnimationSprite idle;
        private AnimationSprite running;
        private int cash;
        private EasyDraw hitArea;
        private EasyDraw UI;

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
            
            // TODO: get rid of hard coded values
            // hitArea = new EasyDraw(100, 100, true);

            // Some UI
            UI = new EasyDraw(128, 24);
            UI.SetXY(game.width - UI.width, 0);
            
            // ChildrenGarden
            game.AddChild(UI);
            // AddChild(hitArea);
            AddChild(running);
            AddChild(idle);
        }

        void Update()
        {
            Controls();
            // TODO: Fix Rect next to the player. Realise why x and y params interfere with width and height
            // hitArea.Fill(255);
            // hitArea.Rect(50, 10, 100, 100);
            ShowUI();
            idle.Animate(0.02f);
            running.Animate(runningAnimationSpeed);
        }

        void Controls()
        {
            playerSpeed = 1.2f;

            // WHENEVER MOVING
            if (Input.GetKey(Key.RIGHT) || Input.GetKey(Key.LEFT) || Input.GetKey(Key.UP) || Input.GetKey(Key.DOWN))
            {
                // If moving, making running animation active and other way around
                idle.visible = false;
                running.visible = true;
                if (Input.GetKey(Key.LEFT_SHIFT))
                {
                    runningAnimationSpeed = 0.1f;
                    playerSpeed = 2f;
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