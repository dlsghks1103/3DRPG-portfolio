using UnityEngine;

namespace RPG.Core
{
    public interface IInteractable
    {
        float Distance
        {
            get;
        }

        void Interact(GameObject other);
        void StopInteract(GameObject other);
    }
}
