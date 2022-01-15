using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UIs
{
    public class EnemyItemDrop : MonoBehaviour
    {
        [SerializeField]
        private GameObject itemPrefab;

        private void ItemDrop(Transform transform)
        {
            if (itemPrefab != null)
            {
                GameObject itemGO = Instantiate(itemPrefab);
                itemGO.transform.parent = transform;
            }
        }
    }
}
