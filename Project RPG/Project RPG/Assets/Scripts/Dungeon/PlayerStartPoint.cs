using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.PlyerController;

public class PlayerStartPoint : MonoBehaviour
{
    public string mapName;
    private PlayerController player;
    private bl_Joystick bl_Joystick;

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

    
}
