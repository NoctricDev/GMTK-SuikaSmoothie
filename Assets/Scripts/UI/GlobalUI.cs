using Scenes;
using UnityEngine;

namespace UI
{
    public class GlobalUI : MonoBehaviour, IGameplaySceneObject
    {
        public void LoadEnd()
        {
            gameObject.SetActive(true);
        }

        public void Unload()
        {
            gameObject.SetActive(false);
        }
    }
}
