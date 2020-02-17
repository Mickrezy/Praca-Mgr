using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapClass
{
    public Vector2Int position;
    public string type;
    public PlayerClass owner;


    public TrapClass(Vector2Int pos, string cont, PlayerClass own)
    {
        position = pos;
        type = cont;
        owner = own;
    }




    public void DestroyTrap(PlayerClass plr)
    {
        if (type == "life")
        {
            plr.life--;
        }
        else if (type == "speed")
        {
            if (plr.maxSpeed > 1) plr.maxSpeed--;
        }
        else if (type == "attack")
        {
            if (plr.attack > 1) plr.attack--;
        }
        else if (type == "defense")
        {
            if(plr.defense> 1) plr.defense--;
        }
        
    }
}
