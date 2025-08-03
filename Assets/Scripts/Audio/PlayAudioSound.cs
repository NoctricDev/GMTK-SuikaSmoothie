using JohaToolkit.UnityEngine.Audio;
using UnityEngine;

namespace Audio
{
    public class PlayAudioSound : MonoBehaviour
    {
        [SerializeField] private SoundDataAsset _asset;

        public void Play()
        {
            SoundManager.Instance.Play(_asset);
        }
    }
}
