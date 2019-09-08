using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {


    #region PUBLIC VARIABLES
    public GameObject roadPrefab;
    public class LevelProgress
    {
        public float shipZ;
        public float lastPathPointZ;
        public int waypointsGenerated;
        public int currentScore;
        public Vector3[] path;

    }

    private static LevelProgress levelProgress1;
    public static LevelProgress LevelProgress1 { get => levelProgress1; }
    #endregion

    #region EVENTS & DELEGATES
    public delegate void OnShipCollision();
    public event OnShipCollision onShipCollision;
    #endregion



    #region MEMBER VARIABLES
    private bool gameStarted;
    private LevelGenerator levelGenerator;
    private float currentDifficulty = 0;
    private IPlayerShipInput shipInterface;
    private gameState state = gameState.startScreen;
    private enum gameState { startScreen, play, death}


    #endregion

    #region CONSTANTS
    private const int WAYPOINTS_SEGMENT_LENGHT = 30;
    private const float WAYPOINT_Z_DISTANCE = 20;
    private const float MAX_WAYPOINT_X_SPREAD = 20;
    private const float CURVE_SMOOTHNESS = 20;
    private const float SHIP_SPEED = 30;
    private const float MAX_DIFFICULTY = 40;
    private const float PROGRESS_TO_DIFFICULTY_MULTI = 0.1F;
    #endregion

    #region INITIALIZTION
    void Start () {
        SwitchGameState(gameState.startScreen);
    }

    private void OnDisable()
    {
        shipInterface.onShipCollision -= OnPlayerDeathCB;
    }

    private void SubscribeToEvents()
    {
        shipInterface.onShipCollision += OnPlayerDeathCB;
    }
    #endregion

    #region MEMBER METHODS
    private void Update()
    {
        switch (state)
        {
            case gameState.startScreen:
                if (Input.anyKey)
                {
                    SwitchGameState(gameState.play);
                }
                break;
            case gameState.play:
                CheckShipInput();
                break;
            case gameState.death:
                break;
            default:
                break;
        }
    }

    private void SwitchGameState(gameState newState)
    {
        switch (newState)
        {
            case gameState.startScreen:
                shipInterface = FindObjectOfType<Ship>();
                levelProgress1 = new LevelProgress();
                levelGenerator = new LevelGenerator();
                levelGenerator.roadPrefab = roadPrefab;
                LevelProgress1.path = levelGenerator.CreateInitialPath();
                shipInterface.InitializeShip();
                break;

            case gameState.play:
                shipInterface.StartShipMovement(true);
                break;

            case gameState.death:
                break;

            default:
                break;
        }

        state = newState;
    }

    private void CheckShipInput()
    {

        int inputDirection = 0;
        bool isBoosting = false;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            inputDirection = -1;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            inputDirection = 1;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            isBoosting = true;
        }

        shipInterface.MoveLeftRight(inputDirection);
        shipInterface.BoostShip(isBoosting);
    }


    #endregion

    #region CALLBACKS
    private void OnPlayerDeathCB()
    {
        SwitchGameState(gameState.death);
    }
    #endregion
}
