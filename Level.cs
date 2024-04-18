using System;
using System.Collections.Generic;
using System.Drawing;
using TiledMapParser;

namespace GXPEngine
{
    public class Level : GameObject
    {
        private Player player;
        private Sprite door;
        private Enemy enemy1;
        private List<Coin> coins;
        private Map levelData;
        private TiledLoader loader;
        private int cashGoal;
        private int currentCash;
        private int cashRoom1;
        private int cashRoom2;
        private int cashCollected1;
        private int cashCollected2;
        private string currentRoom;
        private bool wasGameStarted;

        public Level(string fileName)
        {
            wasGameStarted = false;
            cashGoal = 400;
            cashRoom1 = 200;
            cashRoom2 = 200;
            cashCollected1 = 0;
            cashCollected2 = 0;
            levelData = MapParser.ReadMap(fileName);
            SetUpLevel("map1_.tmx");
            SetUpDefaultObjects("map1_.tmx");
        }

        void SetUpLevel(String fileName)
        {
            currentRoom = fileName;
            var container = new Pivot();
            AddChild(container);
            loader = new TiledLoader(fileName, container, true, autoInstance: true);
            container.scale = 0.5f;
            loader.addColliders = false;
            loader.LoadTileLayers(0);
            loader.addColliders = true;
            var colliderContainer = new Pivot();
            loader.rootObject = colliderContainer;
            container.AddChild(colliderContainer);
            loader.LoadTileLayers(1);
            loader.rootObject = container;


            foreach (GameObject col in colliderContainer.GetChildren())
            {
                AnimationSprite AS = (AnimationSprite)col;
                AS.name = "wall";
            }
        }

        void SetUpDefaultObjects(String name)
        {
            door = new Door();
            AddChild(door);

            player = new Player();

            enemy1 = new Enemy(player);
            AddChild(enemy1);
            
            int coinsToSpawn = 0;
            
            if (currentRoom == "map1_.tmx")
            {
                player.SetVFX(false);
                coinsToSpawn = cashRoom1 - cashCollected1;
                if (wasGameStarted)
                {
                    player.SetXY(2353, 2191);
                }
            }
            else if (currentRoom == "map2_.tmx")
            {
                player.SetVFX(true);
                wasGameStarted = true;
                door.SetXY(75, 2161);
                player.SetXY(183, 2250);
                coinsToSpawn = cashRoom2 - cashCollected2;
            }

            coins = new List<Coin>();
            for (int i = 0; i < coinsToSpawn; i++)
            {
                Coin coin = new Coin(player);
                coin.OnCoinCollected += Coin_OnCoinCollected;
                coins.Add(coin);
                AddChild(coin);
            }
            
            AddChild(player);
        }
        
        private void Coin_OnCoinCollected(Coin coin)
        {
            if (currentRoom == "map1_.tmx")
            {
                cashCollected1++;
            }
            else if (currentRoom == "map2_.tmx")
            {
                cashCollected2++;
            }

            coin.OnCoinCollected -= Coin_OnCoinCollected;
        }
        
        void Update()
        {
            if (player != null)
            {
                x = game.width / (2 * game.scaleX) - player.x;
                y = game.height / (2 * game.scaleY) - player.y;
            }

            if (player?.GetCash() == cashGoal)
            {
                player.SetCash(0);
                RemoveAll();
                
                Menu menu = new Menu(false, true);
                game.AddChild(menu);
            }
            
            if (player.collidesWithDoor)
            {
                RemoveAll();
                currentCash = player.GetCash();
                player.collidesWithDoor = false;

                if (currentRoom == "map1_.tmx")
                {
                    SetUpLevel("map2_.tmx");
                    SetUpDefaultObjects("map2_.tmx");
                }
                else
                {
                    SetUpLevel("map1_.tmx");
                    SetUpDefaultObjects("map1_.tmx");
                }
                player.SetCash(currentCash);
            }
        }

        void RemoveAll()
        {
            List<GameObject> children = GetChildren();
            foreach (GameObject child in children)
            {
                child.Destroy();
            }
            
            foreach (Coin coin in coins) {
                coin.OnCoinCollected -= Coin_OnCoinCollected;
            }
        }
    }
}