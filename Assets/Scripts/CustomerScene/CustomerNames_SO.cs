using UnityEngine;

namespace CustomerScene
{
    [CreateAssetMenu(fileName = "CustomerNames_SO", menuName = "Scriptable Objects/CustomerNames_SO")]
    public class CustomerNames_SO : ScriptableObject
    {
        public string[] Names;
    }
}