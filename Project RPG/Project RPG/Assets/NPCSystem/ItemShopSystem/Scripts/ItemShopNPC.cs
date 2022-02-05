using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.ItemShopSystem
{
    public class ItemShopNPC : MonoBehaviour, IInteractable
    {
        private float distance = 2.0f;
        public float Distance => distance;

        public void Interact(GameObject other)
        {
            float calcDistance = Vector3.Distance(other.transform.position, transform.position);
            if (calcDistance > Distance)
            {
                return;
            }

            GameManager.Instance.itemShopUI.gameObject.SetActive(!GameManager.Instance.itemShopUI.gameObject.activeSelf);
        }

        public void StopInteract(GameObject other)
        {

        }
    }
}
