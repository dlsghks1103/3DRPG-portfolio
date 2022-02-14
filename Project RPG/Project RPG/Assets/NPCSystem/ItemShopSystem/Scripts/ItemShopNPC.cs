using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.ItemShopSystem
{
    public class ItemShopNPC : MonoBehaviour, IInteractable
    {
        #region Variables
        private float distance = 2.0f;
        #endregion Variables

        #region Properties
        public float Distance => distance;
        #endregion Properties

        #region IInteractable Interfaces
        public void Interact(GameObject other)
        {
            float calcDistance = Vector3.Distance(other.transform.position, transform.position);
            if (calcDistance > Distance)
            {
                return;
            }

            GameManager.Instance.itemShopUI.gameObject.SetActive(!GameManager.Instance.itemShopUI.gameObject.activeSelf);
            GameManager.Instance.dynamicInventory.gameObject.SetActive(!GameManager.Instance.dynamicInventory.gameObject.activeSelf);
        }

        public void StopInteract(GameObject other)
        {

        }
        #endregion IInteractable Interfaces
    }
}
