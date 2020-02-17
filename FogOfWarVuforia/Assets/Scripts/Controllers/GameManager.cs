using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    GAME_WAITING,
    GAME_MENU,
    GAME_NEW_TURN,
    GAME_RED_TURN,
    GAME_BLUE_TURN,
    GAME_END
}

public enum DIRECTIONS
{
    NORTH,
    WEST,
    SOUTH,
    EAST,
    NONE
}

public class GameManager : MonoBehaviour
{


    public PlayerClass red;
    public PlayerClass blue;
    public UIController uiController;

    public BoardClass board;

    public int turnNum = 1;

    public GameState gs;
    // Start is called before the first frame update
    void Start()
    {
        
        gs = GameState.GAME_WAITING;
        uiController.red = red;
        uiController.blue = blue;
        turnNum = 1;
        

    }

    // Update is called once per frame
    void Update()
    {
        if (gs == GameState.GAME_WAITING)
        {
            int whoFirst = Random.Range(1, 3);
            gs = GameState.GAME_NEW_TURN;
            if (whoFirst == 1)
            {
                
                uiController.activePlayer = red;
                uiController.changeOkButtonText();
            }
            else
            {
                uiController.activePlayer = blue;
                uiController.changeOkButtonText();
            }            
        }
        else if (gs == GameState.GAME_NEW_TURN)
        {
            if(uiController.uis == UIState.MAIN_MENU)
            {
                if(uiController.activePlayer == blue)
                {
                    gs = GameState.GAME_BLUE_TURN;
                }
                else
                {
                    gs = GameState.GAME_RED_TURN;
                }
            }
        }
        else if (gs == GameState.GAME_BLUE_TURN)
        {
            if (uiController.uis == UIState.WAIT_MENU)
            {
                ChangeTurn();
            }
        }
        else if (gs == GameState.GAME_RED_TURN)
        {
            if (uiController.uis == UIState.WAIT_MENU)
            {
                ChangeTurn();
            }
        }
        if(uiController.uis == UIState.GAME_OVER)
        {
            //Start();
        }
    }

    void ChangeTurn()
    {
        turnNum++;
        uiController.turns = turnNum;

        if (gs == GameState.GAME_RED_TURN)
        {
            
            uiController.activePlayer = blue;
            uiController.changeOkButtonText();
            blue.speed = blue.maxSpeed;
        }
        else
        {
            uiController.activePlayer = red;
            uiController.changeOkButtonText();
            red.speed = red.maxSpeed;
        }

        gs = GameState.GAME_NEW_TURN;
    }
}
