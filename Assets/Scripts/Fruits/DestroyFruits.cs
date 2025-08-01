using UnityEngine;

namespace Fruits
{
    public class DestroyFruits : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.transform.TryGetComponent(out Fruit fruit))
                return;
            
            Destroy(fruit.gameObject);
        }
    }
}
