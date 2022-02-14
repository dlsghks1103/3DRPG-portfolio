using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManualCollision : MonoBehaviour
{
    #region Variables
    public Vector3 boxSize = new Vector3(2, 2, 2);
    #endregion Variables

    #region Unity Methods
    void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Vector3.zero, boxSize);
    }
    #endregion Unity Methods

    #region Methods
    public Collider[] CheckOverlapBox(LayerMask layerMask)
    {
        return Physics.OverlapBox(transform.position, boxSize * 0.5f, transform.rotation, layerMask);
    }
    #endregion Methods

}
