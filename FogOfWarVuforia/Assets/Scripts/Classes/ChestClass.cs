using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestClass
{

    public Vector2Int position;
    public string content;

    public PlayerClass owner;


    public ChestClass(Vector2Int pos, string cont, PlayerClass own)
    {
        position = pos;
        content = cont;
        owner = own;
    }


    public void OpenChest(PlayerClass plr)
    {
        if (content == "life")
        {
            plr.life++;
        }
        else if (content == "speed")
        {
            plr.maxSpeed++;
        }
        else if (content == "attack")
        {
            plr.attack++;
        }
        else if (content == "defense")
        {
            plr.defense++;
        }
        
    }
}

