using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.PlyerController;

public class PlayerStartPoint : MonoBehaviour
{
    #region Variables
    public string mapName;
    private PlayerController player;
    #endregion Variables

    #region Unity Methods
    void OnEnable()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerController>();
        }
        if(mapName == player.currentMapName)
        {
            player.transform.position = transform.position;
        }
    }
    #endregion Unity Methods
}
