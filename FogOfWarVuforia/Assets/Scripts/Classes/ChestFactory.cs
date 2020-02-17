using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestFactory : MonoBehaviour
{
    public GameObject chestPrefab;
    public BoardClass board;
    public GameObject[,] chestPrefabs;


    private void Start()
    {
        chestPrefabs = new GameObject[8, 8];
    }

    public void InstantiateChest(int posx, int posy, PlayerClass plr)
    {
        chestPrefabs[posx, posy] = Instantiate(chestPrefab, new Vector3(0,0,0), Quaternion.identity);
        chestPrefabs[posx, posy].transform.parent = GameObject.Find("ImageTarget").transform;
        chestPrefabs[posx, posy].transform.localPosition = new Vector3(-0.75f + 0.2f * posx, 0.05f, -0.75f + 0.2f * posy);
        chestPrefabs[posx, posy].transform.localRotation = new Quaternion(0, 0, 0, 0);
        plr.items.Add(chestPrefabs[posx, posy]);
        chestPrefabs[posx, posy].SetActive(false);
    }

    public ChestClass GenerateChest(PlayerClass plr)
    {
        int randomInt = Random.Range(0, 4);
        string type = "";
        if (randomInt == 0) type = "life";
        else if (randomInt == 1) type = "speed";
        else if (randomInt == 2) type = "attack";
        else if (randomInt == 3) type = "defense";
        Vector2Int pos = GeneratePosition(plr.position.x, plr.position.y);
        ChestClass chs = new ChestClass(pos, type, plr);
        return chs;
    }

    public Vector2Int GeneratePosition(int posx, int posy)
    {
        bool generated = false;
        int minposx = 0;
        int minposy = 0;
        int maxposx = 7;
        int maxposy = 7;
        if (posx <= 3) minposx = 0;
        else minposx = posx - 2;
        if (posy <= 3) minposy = 0;
        else minposy = posy - 2;
        if (posx >= 5) maxposx = 8;
        else maxposx = posx + 3;
        if (posy >= 5) maxposy = 8;
        else maxposy = posx + 3;
        Vector2Int pos = new Vector2Int(0, 0);
        int rand1, rand2;
        while (!generated)
        {
            rand1 = Random.Range(minposx, maxposx);
            rand2 = Random.Range(minposy, maxposy);
            pos.x = rand1;
            pos.y = rand2;
            if (board.red.position != pos && board.blue.position != pos && board.boardSpaces[rand1, rand2] == "empty") generated = true;
        }
        return pos;
    }

}
