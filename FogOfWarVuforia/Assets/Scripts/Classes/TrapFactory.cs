using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapFactory : MonoBehaviour
{

    public GameObject trapPrefab;
    public BoardClass board;
    public GameObject[,] trapPrefabs;
    public GameObject BoardObject;
    

    private void Start()
    {
        trapPrefabs = new GameObject[8, 8];
    }

    public void InstantiateTrap(int posx, int posy, PlayerClass plr)
    {
        trapPrefabs[posx, posy] = Instantiate(trapPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        trapPrefabs[posx, posy].transform.parent = GameObject.Find("ImageTarget").transform;
        trapPrefabs[posx, posy].transform.localPosition = new Vector3(-0.75f + 0.2f * posx, 0.03f, -0.75f + 0.2f * posy);
        trapPrefabs[posx, posy].transform.localRotation = new Quaternion(0, 0, 0, 0);
        plr.items.Add(trapPrefabs[posx, posy]);
        trapPrefabs[posx, posy].SetActive(false);
    }
}
