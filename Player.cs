using System;
using System.Drawing;
using System.Security.AccessControl;
using GXPEngine;


namespace GXPEngine
{
    public class Player : Canvas
    {
        private float playerSpeed;
        
        private AnimationSprite idle;
        private AnimationSprite running;
        private int cash;

        public Player() : base(60,80)
        {
           // graphics.Clear(Color.Red);
            RespawnPlayer();
           
            SetOrigin(width/2, height);
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

            // Some UI
            EasyDraw UI = new EasyDraw(128, 24);
            UI.Text("Cash: " + cash);
            UI.SetXY(game.width - UI.width, 0);
            game.AddChild(UI);
            
            // ChildrenGarden
            AddChild(running);
            AddChild(idle);
        }

        void Update()
        {
            Controls();
            idle.Animate(0.03f);
            running.Animate(0.08f);
        }

        void Controls()
        {
            playerSpeed = 1.3f;

            // If moving, making running animation active and other way around
            if (Input.GetKey(Key.RIGHT) || Input.GetKey(Key.LEFT) || Input.GetKey(Key.UP) || Input.GetKey(Key.DOWN))
            {
                idle.visible = false;
                running.visible = true;
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
        }


        void OnCollision(GameObject other)
        {
            if (other is Door)
            {
                RespawnPlayer();
                Console.WriteLine("Door!");
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
        
        bool CheckDiagonal()
        {
            return ((Input.GetKey(Key.DOWN) && Input.GetKey(Key.LEFT)) ||
                    (Input.GetKey(Key.DOWN) && Input.GetKey(Key.RIGHT)) ||
                    (Input.GetKey(Key.UP) && Input.GetKey(Key.LEFT)) ||
                    (Input.GetKey(Key.UP) && Input.GetKey(Key.RIGHT)));
        }
    }
}