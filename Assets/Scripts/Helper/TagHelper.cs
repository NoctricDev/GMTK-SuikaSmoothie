using System.Linq;
using UnityEngine;

namespace Helper
{
    public static class TagHelper
    {
        public static string[] GetTags()
        {
#if UNITY_EDITOR
            return UnityEditorInternal.InternalEditorUtility.tags;
#else
            Debug.LogError("[TagHelper] This method is only available in the Unity Editor. It will not work in a built application.");
            return new string[0];
#endif
        }

        public static bool EqualsOneOreMoreTags(this GameObject obj, string[] tags)
        {
            return tags.Any(obj.CompareTag);
        }
    }
}
