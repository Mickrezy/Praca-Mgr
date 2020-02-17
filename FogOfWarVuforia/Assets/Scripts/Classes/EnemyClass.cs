using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClass : MonoBehaviour
{
    public Vector2Int position;
    public int life;
    public int attack;
    public int defense;
    public int maxSpeed;
    public int positionUpdateTurn;
    public int statsUpdateTurn;

    public EnemyClass(Vector2Int pos, int lf, int atk, int def, int spd)
    {
        position = pos;
        life = lf;
        attack = atk;
        defense = def;
        maxSpeed = spd;
        positionUpdateTurn = 1;
        statsUpdateTurn = 1;
    }
}
