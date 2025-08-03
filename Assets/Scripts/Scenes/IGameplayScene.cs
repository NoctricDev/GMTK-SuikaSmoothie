namespace Scenes
{
    public enum GameplayScenes
    {
        Bowl,
        Mixer,
        Customer,
        MainMenu,
        TimerStopped,
    }
    public interface IGameplayScene
    {
        public GameplayScenes Scene { get; }

        public void LoadStart(float durationTime) { }

        public void LoadEnd() { }
        public void Unload() { }
    }
}