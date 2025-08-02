using Scenes;
using UnityEngine;

namespace Helper
{
    public class SetActiveOnSceneLoad : MonoBehaviour, IGameplaySceneObject
    {
        public void LoadStart(float durationTime)
        {
            gameObject.SetActive(true);
        }

        public void Unload()
        {
            gameObject.SetActive(false);
        }
    }
}
