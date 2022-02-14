using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UIs
{
    public class EnemyItemDrop : MonoBehaviour
    {
        #region Variables
        [SerializeField]
        private GameObject itemPrefab;
        #endregion Variables

        #region Methods
        private void ItemDrop(Transform transform)
        {
            if (itemPrefab != null)
            {
                GameObject itemGO = Instantiate(itemPrefab);
                itemGO.transform.parent = transform;
            }
        }
        #endregion Methods
    }
}
