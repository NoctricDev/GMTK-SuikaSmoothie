using JohaToolkit.UnityEngine.Audio;
using UnityEngine;

namespace Audio
{
    public class Music : MonoBehaviour
    {
        [SerializeField] private SoundDataAsset music;

        private void Start()
        {
            SoundManager.Instance.Play(music);
        }
    }
}
