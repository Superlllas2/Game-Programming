namespace GXPEngine
{
    public class UI : GameObject
    {
        private Sprite playerIcon;
        private Sprite money;
        private int cash;
        private EasyDraw text;
        
        public UI(int cash)
        {
            this.cash = cash;
            
            playerIcon = new Sprite("PlayerIcon.png");
            playerIcon.SetXY(20, 20);
            AddChild(playerIcon);

            money = new Sprite("MoneyUI.png");
            money.SetXY(1220, 50);
            AddChild(money);

            text = new EasyDraw(128, 50);
            text.SetXY(1320, 65);
            text.TextSize(30);
            AddChild(text);
        }

        void Update()
        {
            if (parent is Player)
            {
                int currentCash = ((Player)parent).GetCash();
                if (currentCash != cash)
                {
                    cash = currentCash;
                    text.ClearTransparent();
                    text.TextSize(30);
                    text.Text(cash.ToString());
                }
            }
        }
    }
}