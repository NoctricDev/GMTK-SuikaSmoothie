using UnityEngine;

namespace Fruits
{
    public class DestroyFruits : MonoBehaviour
    {
        [SerializeField] private GameObject squishDecalPrefab;
        private void OnTriggerEnter(Collider other)
        {
            if (!other.transform.TryGetComponent(out Fruit fruit))
                return;

            if (fruit.IsCarried)
                return;
            
            Destroy(fruit.gameObject);
            if(squishDecalPrefab != null)
            {
                
                GameObject decal = Instantiate(squishDecalPrefab, other.transform.position, Quaternion.LookRotation(-transform.up, Vector3.up));
                decal.GetComponent<FruitDecal>().Init(fruit.FruitSO);
            }
        }
    }
}
