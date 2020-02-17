using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UIState
{
    START_MENU,
    MAIN_MENU,
    ACTION_MENU,
    CONTROL_MENU,
    SPY_MENU,
    WAIT_MENU,
    GAME_OVER
}
public class UIController : MonoBehaviour
{
    public GameObject ActionButtons;
    public Button BackButton;
    public Button Move;
    public Button Attack;
    public Button Explore;
    public Button Spy;
    public Button SetTrap;

    public GameObject MainButtons;
    public Text turnCounter;
    public Button SeeStats;
    public Text SeeStatsText;
    public Button Action;
    public Button EndTurn;

    public GameObject Stats;
    public GameObject PlayerStats;
    public Text playerLife;
    public Text playerMaxSpeed;
    public Text playerSpeed;
    public Text playerAttack;
    public Text playerDefense;
    public GameObject EnemyStats;
    public Text enemyLife;
    public Text enemyMaxSpeed;
    public Text enemyAttack;
    public Text enemyDefense;
    public Text enemyStatsUpdate;
    public Text enemyPositionUpdate;

    public GameObject ControlButtons;
    public Button Up;
    public Button Down;
    public Button Left;
    public Button Right;
    public Button BackControl;
    public Button OkControl;

    public GameObject NextTurn;
    public Button OkButton;
    public Text OkButtonText;

    public GameObject SpyButtons;
    public Button SpyStats;
    public Button SpyPosition;
    public Button BackSpy;

    public GameObject StartMenu;
    public Button PlayButton;
    public Button InstructionsButton;

    public GameObject Notifications;
    public Text NotificationText;
    public Button NotificationButton;

    public PlayerClass red;
    public PlayerClass blue;
    public PlayerClass activePlayer;

    public Button VirtualButton;

    public VuforiaController vuforia;

    public UIState uis;

    public int turns;

    private int myStats;
    private bool isSettingTrap;
    private bool isMoving;
    private bool isAttacking;

    // Start is called before the first frame update
    void Start()
    {
        ActionButtons.SetActive(false);
        MainButtons.SetActive(false);
        ControlButtons.SetActive(false);
        PlayerStats.SetActive(false);
        EnemyStats.SetActive(false);
        Stats.SetActive(false);
        NextTurn.SetActive(false);
        SpyButtons.SetActive(false);
        Notifications.SetActive(false);
        StartMenu.SetActive(true);

        myStats = 0;
        isSettingTrap = false;
        isMoving = false; ;
        isAttacking = false;
       
        
        turns = 1;
        uis = UIState.START_MENU;

        //OkButton.onClick.AddListener(onOkButtonClicked);
        NotificationButton.onClick.AddListener(onOkButtonClicked);
        SeeStats.onClick.AddListener(onSeeStatsButtonClicked);
        BackButton.onClick.AddListener(onBackButtonClicked);
        Action.onClick.AddListener(onActionButtonClicked);
        EndTurn.onClick.AddListener(onEndTurnButtonClicked);
        Move.onClick.AddListener(onMoveButtonClicked);
        Attack.onClick.AddListener(onAttackButtonClicked);
        Explore.onClick.AddListener(onExploreButtonClicked);
        Spy.onClick.AddListener(onSpyButtonClicked);
        SetTrap.onClick.AddListener(onSetTrapButtonClicked);
        SpyStats.onClick.AddListener(onSpyStatsButtonClicked);
        SpyPosition.onClick.AddListener(onSpyPositionButtonClicked);
        BackSpy.onClick.AddListener(onBackSpyButtonClicked);
        OkControl.onClick.AddListener(onOkControlButtonClicked);
        BackControl.onClick.AddListener(onBackControlButtonClicked);
        Up.onClick.AddListener(onUpButtonClicked);
        Down.onClick.AddListener(onDownButtonClicked);
        Left.onClick.AddListener(onLeftButtonClicked);
        Right.onClick.AddListener(onRightButtonClicked);
        PlayButton.onClick.AddListener(onGameStartButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeOkButtonText()
    {
        /*if (activePlayer == red) OkButtonText.text = "TURA GRACZA CZERWONEGO";
        else OkButtonText.text = "TURA GRACZA NIEBIESKIEGO";*/
        if (activePlayer == red)
        {
            NotificationText.text = "RED PLAYERS TURN";
        }
        else
        {
            NotificationText.text = "BLUE PLAYERS TURN";
        }
        uis = UIState.WAIT_MENU;
    }

    public void onOkButtonClicked()
    {
        if (uis == UIState.WAIT_MENU)
        {          
            vuforia.HideAll();
            vuforia.RefreshPositions();          
            EnterMainMenu();
        }
        else if (uis == UIState.GAME_OVER)
        {
            Start();
        }
        else
        {
            EnterMainMenu();
            vuforia.HideAll();
            vuforia.RefreshPositions();
        }
    }

    public void onSeeStatsButtonClicked()
    {
        if (myStats == 0)
        {
            myStats = 1;
            PlayerStats.SetActive(false);
            EnemyStats.SetActive(true);
            loadStats();
            SeeStatsText.text = "Hide stats";
            updateTurns();
        }
        else if (myStats == 2)
        {
            myStats = 0;
            EnemyStats.SetActive(false);
            PlayerStats.SetActive(true);           
            loadStats();
            SeeStatsText.text = "Show enemy stats";
            updateTurns();
        }
        else if (myStats == 1)
        {
            myStats = 2;
            EnemyStats.SetActive(false);
            PlayerStats.SetActive(false);
            turnCounter.text = " ";
            SeeStatsText.text = "Show my stats";
        }
    }

    public void onActionButtonClicked()
    {

        EnterActionMenu();
    }

    public void onBackButtonClicked()
    {
        EnterMainMenu();
    }

    public void onMoveButtonClicked()
    {
        EnterControlMenu();
        isMoving = true;
    }
    public void onAttackButtonClicked()
    {
        EnterControlMenu();
        isAttacking = true;

    }
    public void onExploreButtonClicked()
    {
        activePlayer.Explore();
        vuforia.HideAll();
        vuforia.RefreshPositions();
        EnterMainMenu();
    }

    public void onSpyButtonClicked()
    {

        EnterSpyMenu();
    }

    public void onSetTrapButtonClicked()
    {
        EnterControlMenu();
        isSettingTrap = true;
    }

    public void onUpButtonClicked()
    {
        activePlayer.direction = DIRECTIONS.NORTH;
        OkControl.interactable = true;
        vuforia.RefreshArrows();
    }

    public void onDownButtonClicked()
    {
        activePlayer.direction = DIRECTIONS.SOUTH;
        OkControl.interactable = true;
        vuforia.RefreshArrows();
    }

    public void onLeftButtonClicked()
    {
        activePlayer.direction = DIRECTIONS.WEST;
        OkControl.interactable = true;
        vuforia.RefreshArrows();
    }

    public void onRightButtonClicked()
    {
        activePlayer.direction = DIRECTIONS.EAST;
        OkControl.interactable = true;
        vuforia.RefreshArrows();
    }


    public void onEndTurnButtonClicked()
    {
        ActionButtons.SetActive(false);
        MainButtons.SetActive(false);
        ControlButtons.SetActive(false);
        PlayerStats.SetActive(false);
        EnemyStats.SetActive(false);
        Stats.SetActive(false);
        //NextTurn.SetActive(true);
        Notifications.SetActive(true);
        SpyButtons.SetActive(false);
        vuforia.HideAll();
        uis = UIState.WAIT_MENU;
    }

    public void onSpyStatsButtonClicked()
    {

        if (activePlayer == red)
        {
            activePlayer.Detect(turns, "stats", blue);
        }
        else
        {
            activePlayer.Detect(turns, "stats", red);
        }
        EnterMainMenu();
    }

    public void onSpyPositionButtonClicked()
    {
        if (activePlayer == red)
        {
            activePlayer.Detect(turns, "position", blue);

        }
        else
        {
            activePlayer.Detect(turns, "position", red);
        }
        vuforia.HideAll();
        vuforia.RefreshPositions();
        EnterMainMenu();
    }

    public void onOkControlButtonClicked()
    {
        //wykonanie akcji
        if (activePlayer.direction != DIRECTIONS.NONE)
        {
            if (isMoving == true)
            {
                activePlayer.Move();
                vuforia.HideAll();
                vuforia.RefreshPositions();
                isMoving = false;
                

            }
            else if (isAttacking == true)
            {
                if(activePlayer == red)
                {
                    activePlayer.Attack(blue, turns);
                }
                else
                {
                    activePlayer.Attack(red, turns);
                }
                isAttacking = false;
                
            }
            else if (isSettingTrap == true)
            {                
                isSettingTrap = false;
                activePlayer.setTrap();
                vuforia.HideAll();
                vuforia.RefreshPositions();
                EnterMainMenu();
            }
            
        }
    }

    public void onBackControlButtonClicked()
    {
        EnterActionMenu();
        isSettingTrap = false;
        isAttacking = false;
        isMoving = false;
        activePlayer.direction = DIRECTIONS.NONE;
    }

    public void onBackSpyButtonClicked()
    {
        EnterActionMenu();
    }

    public void loadStats()
    {
        if(myStats == 0)
        {
            playerLife.text = "Life: " + activePlayer.life.ToString();
            playerMaxSpeed.text = "Max Speed: " + activePlayer.maxSpeed.ToString(); 
            playerSpeed.text = "Speed: " + activePlayer.speed.ToString();
            playerAttack.text = "Attack: " + activePlayer.attack.ToString();
            playerDefense.text = "Defense: " + activePlayer.defense.ToString();
        }
        else
        {
            enemyLife.text = "Life: " + activePlayer.enemy.life.ToString();
            enemyMaxSpeed.text = "Max Speed: " + activePlayer.enemy.maxSpeed.ToString();
            enemyAttack.text = "Attack: " + activePlayer.enemy.attack.ToString();
            enemyDefense.text = "Defense: " + activePlayer.enemy.defense.ToString();
            enemyStatsUpdate.text = "Position update turn: " + activePlayer.enemy.positionUpdateTurn.ToString();
            enemyPositionUpdate.text = "Stats update turn: " + activePlayer.enemy.statsUpdateTurn.ToString();
        }
        
    }

    public void updateTurns()
    {
        turnCounter.text = "Turn " + turns.ToString();
    }

    public void CheckBorders()
    {
        Up.interactable = true;
        Down.interactable = true;
        Left.interactable = true;
        Right.interactable = true;

        if (activePlayer.position.x == 0)
        {
            Left.interactable = false;
        }
        if (activePlayer.position.x == 7)
        {
            Right.interactable = false;
        }
        if (activePlayer.position.y == 0)
        {
            Down.interactable = false;
        }
        if (activePlayer.position.y == 7)
        {
            Up.interactable = false;
        }
    }

    public void EnterMainMenu()
    {
        //NextTurn.SetActive(false);
        Notifications.SetActive(false);
        ControlButtons.SetActive(false);
        SpyButtons.SetActive(false);
        Stats.SetActive(true);
        ActionButtons.SetActive(false);
        if (myStats == 0)
        {
            PlayerStats.SetActive(true);
            loadStats();
            updateTurns();
        }
        else if (myStats == 1)
        {
            EnemyStats.SetActive(true);
            loadStats();
            updateTurns();
        }
        else
        {
            PlayerStats.SetActive(false);
            EnemyStats.SetActive(false);
            turnCounter.text = " ";
        }
        MainButtons.SetActive(true);
        SpyButtons.SetActive(false);
        if(activePlayer.speed <= 0)
        {
            Action.interactable = false;
        }
        else Action.interactable = true;
        uis = UIState.MAIN_MENU;
    }

    public void EnterControlMenu()
    {
        activePlayer.direction = DIRECTIONS.NONE;
        ActionButtons.SetActive(false);
        MainButtons.SetActive(false);
        ControlButtons.SetActive(true);
        PlayerStats.SetActive(false);
        EnemyStats.SetActive(false);
        Stats.SetActive(false);
        //NextTurn.SetActive(false);
        Notifications.SetActive(false);
        SpyButtons.SetActive(false);
        OkControl.interactable = false;
        CheckBorders();
        uis = UIState.CONTROL_MENU;
    }

    public void EnterSpyMenu()
    {
        ActionButtons.SetActive(false);
        MainButtons.SetActive(false);
        ControlButtons.SetActive(false);
        PlayerStats.SetActive(false);
        EnemyStats.SetActive(false);
        Stats.SetActive(false);
        //NextTurn.SetActive(false);
        Notifications.SetActive(false);
        SpyButtons.SetActive(true);

        uis = UIState.SPY_MENU;
    }

    public void EnterActionMenu()
    {
        ActionButtons.SetActive(true);
        MainButtons.SetActive(false);
        ControlButtons.SetActive(false);
        PlayerStats.SetActive(false);
        EnemyStats.SetActive(false);
        Stats.SetActive(false);
        //NextTurn.SetActive(false);
        Notifications.SetActive(false);
        SpyButtons.SetActive(false);
        uis = UIState.ACTION_MENU;
    }

    public void onGameStartButtonClicked()
    {
        //uis = UIState.WAIT_MENU;
        StartMenu.SetActive(false);
        Notifications.SetActive(true);
        changeOkButtonText();
        //NextTurn.SetActive(true);
    }

    public void CreateNotification(string msg)
    {
        //vuforia.HideAll();
        Stats.SetActive(false);
        MainButtons.SetActive(false);
        Notifications.SetActive(true);
        NotificationText.text = msg;
        if(uis == UIState.GAME_OVER)
        {
            NotificationButton.enabled = false;
        }
    }
}
