using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public static LevelProgress levelProgress;
    #endregion

    #region EVENTS
    public delegate void OnShipCollision();
    public event OnShipCollision shipCollisionEvent;
    #endregion

    #region MEMBER VARIABLES
    private bool gameStarted;
    private LevelGenerator levelGenerator;
    private float currentDifficulty = 0;
    private IPlayerShipInput ship;
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
        ship = FindObjectOfType<Ship>();
        levelProgress = new LevelProgress();
        levelGenerator = new LevelGenerator();
        levelGenerator.roadPrefab = roadPrefab;
        levelGenerator.CreateInitialPath();
        levelProgress.path = levelGenerator.path;
        

    }

    private void OnDisable()
    {
        ship.onShipCollision -= OnPlayerDeathCB;
    }

    private void SubscribeToEvents()
    {
        ship.onShipCollision += OnPlayerDeathCB;
    }
    #endregion

    #region MEMBER METHODS
    private void Update()
    {
        switch (state)
        {
            case gameState.startScreen:
                break;
            case gameState.play:
                CheckInput();
                break;
            case gameState.death:
                break;
            default:
                break;
        }
        //if (ship.GetTargetPoint() == levelProgress.path.Length - 2)
        //{
        //    updatedifficulty();
        //    debug_lock = true;
        //    levelgenerator.updatewaypoints();
        //    targetpathpoint = 0;
        //    debug_lock = false;
        //}
        //moveship();
      

    }

    private void SwitchGameState(gameState newState)
    {
        switch (newState)
        {
            case gameState.startScreen:
                break;
            case gameState.play:
                break;
            case gameState.death:
                break;
            default:
                break;
        }
    }
    private void CheckInput()
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

        ship.MoveLeftRight(inputDirection);
        ship.BoostShip(isBoosting);
    }
    #endregion

    #region CALLBACKS
    private void OnPlayerDeathCB()
    {

    }
    #endregion
}
