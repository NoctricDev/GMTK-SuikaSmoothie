namespace Scenes
{
    public interface IGameplaySceneObject
    {
        public void LoadStart(float durationTime) { }
        public void LoadEnd() { }
        public void Unload() { }
    }
}