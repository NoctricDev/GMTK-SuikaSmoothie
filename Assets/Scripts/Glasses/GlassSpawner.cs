using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Glasses
{
    public class GlassSpawner : MonoBehaviour
    {
        [Title("References")]
        [SerializeField] private Glass glassPrefab;

        [SerializeField] private Slot spawnSlot;

        private void Start()
        {
            Glass spawned = Instantiate(glassPrefab, spawnSlot.transform.position, Quaternion.identity);
            spawnSlot.SetSlot(spawned);
        }
    }
}
