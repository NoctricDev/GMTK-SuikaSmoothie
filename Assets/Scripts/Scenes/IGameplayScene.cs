using Input;

namespace Scenes
{
    public enum GameplayScenes
    {
        Bowl
    }
    public interface IGameplayScene
    {
        public GameplayScenes Scene { get; }
        public void LoadStart(float durationTime);
        public void LoadEnd();
        public void Unload();
    }
}