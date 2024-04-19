namespace GXPEngine
{
    public class Button : Sprite
    {
        private Sound buttonSound;
        
        public Button() : base("Empty.png")
        {
            buttonSound = new Sound("SFX/button.mp3");
        }
        
        public void PlaySound()
        {
            buttonSound.Play();
        }
    }
}