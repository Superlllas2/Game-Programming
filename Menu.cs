using System;
using System.Collections.Generic;

namespace GXPEngine
{
    public class Menu : GameObject
    {
        private Button button;
        private bool hasStarted;
        private Sprite MenuUI;
        private Sprite VictoryMenuUI;

        public Menu(bool isStartingMenu, bool isWin)
        {
            if (isStartingMenu)
            {
                VictoryMenuUI = new Sprite("MainMenuUI.png");
                AddChild(VictoryMenuUI);
                VictoryMenuUI.SetOrigin(VictoryMenuUI.width / 2, VictoryMenuUI.height / 2);
                VictoryMenuUI.x = game.width / 2;
                VictoryMenuUI.y = game.height / 2;
                VictoryMenuUI.scale = 0.8f;
            }
            else
            {
                if (isWin)
                {
                    MenuUI = new Sprite("PlayAgain.png");
                    AddChild(MenuUI);
                    MenuUI.SetOrigin(MenuUI.width / 2, MenuUI.height / 2);
                    MenuUI.x = game.width / 2;
                    MenuUI.y = game.height / 2;
                    MenuUI.scale = 0.8f;
                }
                else
                {
                    MenuUI = new Sprite("PlayAgain.png");
                    AddChild(MenuUI);
                    MenuUI.SetOrigin(MenuUI.width / 2, MenuUI.height / 2);
                    MenuUI.x = game.width / 2;
                    MenuUI.y = game.height / 2;
                    MenuUI.scale = 0.8f;
                }
            }

            hasStarted = false;
            button = new Button();
            AddChild(button);
            button.x = 543;
            button.y = 480;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0) && !hasStarted)
            {
                if (button.HitTestPoint(Input.mouseX, Input.mouseY))
                {
                    HideMenu();
                    StartGame();
                }
            }
        }

        void HideMenu()
        {
            button.visible = false;
        }

        void StartGame()
        {
            if (!hasStarted)
            {
                List<GameObject> children = GetChildren();
                foreach (GameObject child in children)
                {
                    child.Destroy();
                }

                hasStarted = true;
                Level level = new Level("map1_.tmx");
                // game.AddChild(new Level("untitled.tmx"));
                game.AddChild(level);
            }
        }
    }
}