namespace Scenes
{
    public enum GameplayScenes
    {
        Bowl,
        Mixer,
        Customer
    }
    public interface IGameplayScene
    {
        public GameplayScenes Scene { get; }

        public void LoadStart(float durationTime) { }

        public void LoadEnd() { }
        public void Unload() { }
    }
}