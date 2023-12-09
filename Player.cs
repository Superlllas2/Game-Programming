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

        public Player() : base(60,80)
        {
           // graphics.Clear(Color.Red);
            
            SetOrigin(width/2, height);
            idle = new AnimationSprite("Main character/Player/SpriteSheets/idle.png", 8, 1, -1,
                true, false);
            running = new AnimationSprite("Main character/Player/SpriteSheets/running.png", 10, 1, -1,
                true, false);
            AddChild(running);
            AddChild(idle);
            idle.scale = 0.5f;
            running.scale = 0.5f;
            idle.x = -idle.width / 2;
            idle.y = -100;
            running.x = -running.width / 2;
            running.y = -100;
        }

        void Update()
        {
            Movement();
            idle.Animate(0.03f);
            running.Animate(0.08f);
        }

        void Movement()
        {
            playerSpeed = 1.3f;

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
            
            if (CheckDiagonal())
            {
                playerSpeed *= 0.7071f;
            }
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

        bool CheckDiagonal()
        {
            return ((Input.GetKey(Key.DOWN) && Input.GetKey(Key.LEFT)) || 
                    (Input.GetKey(Key.DOWN) && Input.GetKey(Key.RIGHT)) || 
                    (Input.GetKey(Key.UP) && Input.GetKey(Key.LEFT)) ||
                    (Input.GetKey(Key.UP) && Input.GetKey(Key.RIGHT)));
        }
    }
}