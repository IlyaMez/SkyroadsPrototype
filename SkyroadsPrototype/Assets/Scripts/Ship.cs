using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour, IPlayerShipInput
{
    #region PUBLIC VARIABLES
    #endregion

    #region EVENTS
    public event GameController.OnShipCollision onShipCollision;
    #endregion

    #region MEMBER VARIABLES
    private Vector3 shipOffset = Vector2.zero;
    private bool isMovingFoward = false;
    private bool isBoosted = false;
    private int targetPathPointIndex = 1;
    #endregion

    #region CONSTANTS
    private const float SPEED = 20;
    private const float SHIP_OFFSET_BOUNDS = 5.5f;
    private const float BOOST_SPEED_MULTIPLIER = 2;
    private const float X_AXIS_MOVEMENT_SPEED = 4f;
    private const float SHIP_TILT_ANGLE = 20f;


    #endregion

    #region INITIALIZATION

    #endregion

    #region MEMBER METHODS
    void Update()
    {
        if (isMovingFoward)
        {
            MoveTowardsNextPoint();
        }
    }

    private void MoveTowardsNextPoint()
    {
        if (transform.position.z >= GameController.LevelProgress1.path[targetPathPointIndex].z
           && targetPathPointIndex + 2 < GameController.LevelProgress1.path.Length)
        {
            targetPathPointIndex++;
        }

        float finalSpeed = SPEED;

        if (isBoosted)
        {
            finalSpeed = SPEED * BOOST_SPEED_MULTIPLIER;
        }

        Vector3 newShipPos = Vector3.MoveTowards(transform.position,
                                                 GameController.LevelProgress1.path[targetPathPointIndex] + shipOffset,
                                                 finalSpeed * Time.deltaTime);


        Vector3 newShipFoward = Vector3.RotateTowards(transform.forward,
                                                 (GameController.LevelProgress1.path[targetPathPointIndex + 1] + shipOffset) - transform.position,
                                                 finalSpeed * Time.deltaTime,
                                                 finalSpeed  * Time.deltaTime);

        transform.position = newShipPos;
        transform.forward = newShipFoward;
    }
    #endregion

    #region CALLBACKS
    private void OnTriggerEnter(Collider collision)
    {
        isMovingFoward = false;
        onShipCollision.Invoke();
    }
    #endregion

    #region API
    public void InitializeShip()
    {
        onShipCollision = new GameController.OnShipCollision(() => { });
        transform.position = GameController.LevelProgress1.path[0];
        transform.forward = Vector3.RotateTowards(transform.forward,
                                                GameController.LevelProgress1.path[1] - transform.position,
                                                100, 100);
    }

    public void StartShipMovement(bool isTrue)
    {
        isMovingFoward = isTrue;

        if (isTrue)
        {
            MoveTowardsNextPoint();
        }
    }

    public void BoostShip(bool isTrue)
    {
        isBoosted = isTrue;
    }

    public void MoveLeftRight(int dir)
    {
        float newOffset = shipOffset.x + (X_AXIS_MOVEMENT_SPEED * dir) * Time.deltaTime;
        if (newOffset < SHIP_OFFSET_BOUNDS * -1 && dir == -1)
        {
            newOffset = SHIP_OFFSET_BOUNDS * -1;
        }

        if (newOffset > SHIP_OFFSET_BOUNDS && dir == 1)
        {
            newOffset = SHIP_OFFSET_BOUNDS;
        }

        shipOffset.x = newOffset;
    }

    public int GetTargetPoint()
    {
        return targetPathPointIndex;
    }
    #endregion
}
