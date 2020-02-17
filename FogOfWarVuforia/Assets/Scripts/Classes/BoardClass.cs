using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardClass : MonoBehaviour
{
    public string[,] boardSpaces;
    public ChestClass[,] chests;
    public TrapClass[,] traps;
    public int boardSize;
    public PlayerClass red;
    public PlayerClass blue;
    public TrapFactory trapFactory;
    public ChestFactory chestFactory;

    public void Start()
    {
        boardSize = 8;
        boardSpaces = new string[boardSize, boardSize];
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                boardSpaces[i, j] = "empty";
            }
        }
        traps = new TrapClass[boardSize, boardSize];
        chests = new ChestClass[boardSize, boardSize];
    }

    public void AddChest(ChestClass chest)
    {
        boardSpaces[chest.position.x, chest.position.y] = "chest";
        chests[chest.position.x, chest.position.y] = chest;
    }

    public void AddTrap(TrapClass trap)
    {
        boardSpaces[trap.position.x, trap.position.y] = "trap";
        traps[trap.position.x, trap.position.y] = trap;
    }

    public void TakeChest(int posx, int posy, PlayerClass plr)
    {
        boardSpaces[posx, posy] = "empty";
        chests[posx,posy].owner.items.Remove(chestFactory.chestPrefabs[posx,posy]);
        chests[posx, posy] = null;
        Destroy(chestFactory.chestPrefabs[posx, posy]);
    }

    public void TakeTrap(int posx, int posy, PlayerClass plr)
    {
        boardSpaces[posx, posy] = "empty";
        traps[posx, posy].owner.items.Remove(trapFactory.trapPrefabs[posx, posy]);
        traps[posx, posy] = null;
        Destroy(trapFactory.trapPrefabs[posx, posy]);

    }
}
