using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public static class LoadSceneManagement
    {
        private static bool wasFullscreen;
        
        public static async Awaitable LoadSceneAsync(int buildIndex)
        {
            await SceneManager.LoadSceneAsync(buildIndex);
        }
    }
}