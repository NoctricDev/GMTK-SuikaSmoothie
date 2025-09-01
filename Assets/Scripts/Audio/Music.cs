using JohaToolkit.UnityEngine.Audio;
using JohaToolkit.UnityEngine.DataStructures;
using UnityEngine;

namespace Audio
{
    public class Music : MonoBehaviourSingleton<Music>
    {
        [SerializeField] private SoundDataAsset music;

        protected override void Awake()
        {
            IsPersistent = true;
            base.Awake();
        }

        private void Start()
        {
            SoundManager.Instance.Play(music);
        }
    }
}
