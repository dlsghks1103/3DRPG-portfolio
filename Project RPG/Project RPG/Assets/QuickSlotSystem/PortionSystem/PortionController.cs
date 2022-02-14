using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RPG.InventorySystem.Inventory;
using RPG.InventorySystem.Items;

namespace RPG.QuickSlot.Protion
{
    public class PortionController : MonoBehaviour
    {
        #region Variables
        public TextMeshProUGUI textMeshPro;
        public QuickSlotObject quickSlotObject;
        #endregion Variables

        #region Unity Methods
        private void Start()
        {
            textMeshPro.text = quickSlotObject.amount.ToString();
        }
        #endregion Unity Methods

        #region Methods
        public void SwapPortionSlot(InventorySlot slot)
        {
            if (slot == null)
            {
                return;
            }
            else
            {
                quickSlotObject.amount +=slot.amount;
            }

            textMeshPro.text = quickSlotObject.amount.ToString();
        }

        public void OnClickPortionSlot()
        {
            if (quickSlotObject.amount <= 0)
            {
                return;
            }
            {
                GameManager.Instance.playerController.OnUseItem(quickSlotObject);
                textMeshPro.text = quickSlotObject.amount.ToString();
            }
        }
        #endregion Methods
    }
}
