using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IPooledObject
{

    void OnSpawn();
    void OnDeSpawn();

}

public interface IPlayerShipInput
{
    void InitializeShip();
    void MoveLeftRight(int dir);
    void StartShipMovement(bool isTrue);
    void BoostShip(bool isTrue);
    int GetTargetPoint();

    event GameController.OnShipCollision onShipCollision;
}
public class Interfaces
{

}
