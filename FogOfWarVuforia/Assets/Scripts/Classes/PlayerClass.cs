using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerClass : MonoBehaviour
{
    public Vector2Int position;
    public int life;
    public int attack;
    public int defense;
    public int maxSpeed;
    public int speed;
 
    public EnemyClass enemy;
    public DIRECTIONS direction;
    public BoardClass board;
    public UIController uic;

    public List<GameObject> items;

    private void Start()
    {
        items = new List<GameObject>();
    }

    public PlayerClass(Vector2Int pos, int lf, int atk, int def, int spd, EnemyClass enm)
    {
        position = pos;
        life = lf;
        attack = atk;
        defense = def;
        maxSpeed = spd;
        speed = spd;
        enemy = enm;

    }

    ~PlayerClass()
    {

    }

    public void Move()
    {
        speed--;
        if (direction == DIRECTIONS.NORTH)
        {
            position.y += 1;

        }
        else if (direction == DIRECTIONS.WEST)
        {
            position.x -= 1;
        }
        else if (direction == DIRECTIONS.SOUTH)
        {
            position.y -= 1;
        }
        else if (direction == DIRECTIONS.EAST)
        {
            position.x += 1;
        }

        if (board.boardSpaces[position.x, position.y] == "chest")
        {
            uic.CreateNotification("YOU'VE OPENED A CHEST \n +1 " + board.chests[position.x, position.y].content);
            board.chests[position.x, position.y].OpenChest(this);
            board.TakeChest(position.x, position.y, this);          
        }

        else if (board.boardSpaces[position.x, position.y] == "trap" && board.traps[position.x, position.y].owner != this)
        {
            uic.CreateNotification("YOU'VE ACTIVATED A TRAP \n -1 " + board.traps[position.x, position.y].type);
            board.traps[position.x, position.y].DestroyTrap(this);
            board.TakeTrap(position.x, position.y, this);
            if(life == 0)
            {
                uic.uis = UIState.GAME_OVER;
                uic.CreateNotification("YOU LOSE");
                
            }
        }
        else
        {
            uic.EnterMainMenu();
        }
        direction = DIRECTIONS.NONE;
    }

    public void Explore()
    {
        speed--;
        ChestClass chs = board.chestFactory.GenerateChest(this);
        board.chestFactory.InstantiateChest(chs.position.x, chs.position.y, this);
        
        board.AddChest(chs);
    }
    public void setTrap()
    {
        speed--;
        Vector2Int trapPos = new Vector2Int(position.x, position.y);
        if (direction == DIRECTIONS.NORTH) trapPos.y++;
        else if (direction == DIRECTIONS.SOUTH) trapPos.y--;
        else if (direction == DIRECTIONS.WEST) trapPos.x--;
        else if (direction == DIRECTIONS.EAST) trapPos.x++;
        int randomInt = Random.Range(0, 4);
        string type = "";
        if(randomInt == 0) type = "life";
        else if (randomInt == 1) type = "speed";
        else if (randomInt == 2) type = "attack";
        else if (randomInt == 3) type = "defense";
        TrapClass trp = new TrapClass(trapPos, type, this);
        board.trapFactory.InstantiateTrap(trp.position.x, trp.position.y, this);
        board.AddTrap(trp);
        direction = DIRECTIONS.NONE;

    }

    public void Detect(int turn, string mode, PlayerClass enm)
    {
        speed--;
        if (mode == "position")
        {
            enemy.position = enm.position;
            enemy.positionUpdateTurn = turn;
        }
        else if (mode == "stats")
        {
            enemy.life = enm.life;
            enemy.attack = enm.attack;
            enemy.defense = enm.defense;
            enemy.statsUpdateTurn = turn;
        }
    }

    public void Attack(PlayerClass enm, int turn)
    {
        speed--;
        Vector2Int atkPos = new Vector2Int(position.x, position.y);
        if (direction == DIRECTIONS.NORTH) atkPos.y++;
        else if (direction == DIRECTIONS.SOUTH) atkPos.y--;
        else if (direction == DIRECTIONS.WEST) atkPos.x--;
        else if (direction == DIRECTIONS.EAST) atkPos.x++;
        if(enm.position == atkPos)
        {
            int dmg = attack - enm.defense;
            if (dmg < 1) dmg = 1;
            enm.life -= dmg;
            enemy.life = enm.life;
            enemy.attack = enm.attack;
            enemy.defense = enm.defense;
            enemy.statsUpdateTurn = turn;
            if(enemy.life <= 0)
            {
                uic.uis = UIState.GAME_OVER;
                uic.CreateNotification("YOU WIN");
                
            }
            else
            {
                uic.CreateNotification("YOU'VE HIT ENEMY FOR " + dmg.ToString() + " DAMAGE");
            }
        }
        else
        {
            uic.CreateNotification("YOU MISSED");
        }
        direction = DIRECTIONS.NONE;
    }



}
