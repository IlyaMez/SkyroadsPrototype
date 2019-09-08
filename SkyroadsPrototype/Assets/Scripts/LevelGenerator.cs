using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LevelGenerator {

    public GameObject roadPrefab;

    private float currentSpread = 20;
    private Vector3[] waypoints;
    public Vector3[] path;
    private Queue<Vector3> waypointsQueue;
    private List<Transform> roadGOs;
    private Vector3 lastWaypoint = Vector3.zero;

    private const int WAYPOINTS_SEGMENT_LENGHT = 20;
    private const float DIFFICULTY_TO_SPREAD_MULTIPLIER = 0.1f;
    private const float WAYPOINT_Z_DISTANCE = 20;
    private const float MAX_WAYPOINT_SPREAD = 20;
    private const float CURVE_SMOOTHNESS = 20;
    private int currentZStep = 0;
    private GameObject roadSegmentHolder;


    private Vector3 getNewWaypoint()
    {
        float newWaypointX = UnityEngine.Random.Range(lastWaypoint.x - currentSpread,
            lastWaypoint.x + currentSpread);
        float newWaypointY = UnityEngine.Random.Range(lastWaypoint.x - currentSpread,
            lastWaypoint.x + currentSpread);
        float newWaypointZ = WAYPOINT_Z_DISTANCE + lastWaypoint.z;
        return new Vector3(newWaypointX, newWaypointY, newWaypointZ);
    }

    public void CreateInitialPath()
    {
        waypoints = new Vector3[WAYPOINTS_SEGMENT_LENGHT];


        for (int i = 0; i < waypoints.Length; i++)
        {
            Vector3 waypoint = getNewWaypoint();
            waypoints[i] = waypoint;
            lastWaypoint = waypoint;
            UpdateSpreadAndZStep(currentZStep + 1);
        }
   
        path = Curver.MakeSmoothCurve(waypoints, 20);
        InstantiateRoadGO();
    }

    private void InstantiateRoadGO()
    {
        roadGOs = new List<Transform>();
        roadSegmentHolder = new GameObject();
        for (int i = 0; i < path.Length - 1; i++)
        {
            GameObject roadBlockGO = Object.Instantiate(roadPrefab.gameObject);
            roadGOs.Add(roadBlockGO.transform);
            roadBlockGO.transform.parent = roadSegmentHolder.transform;
            roadBlockGO.transform.position = path[i];
            roadBlockGO.transform.forward = path[i + 1] - path[i];
        }
    }

    public void UpdateWaypoints()
    {
        Debug.Log("ZStep before update is: " + currentZStep);
        waypoints[0] = lastWaypoint;
        //  waypoints[1] = preLastWaypoint;
        for (int i = 1; i < waypoints.Length; i++)
        {
            waypoints[i] = getNewWaypoint();
            lastWaypoint = waypoints[i];

            UpdateSpreadAndZStep(currentZStep + 1);
        }
        //       preLastWaypoint = waypoints[waypoints.Length - 1];
        path = Curver.MakeSmoothCurve(waypoints, 20);
        UpdateRoadGO();
        Debug.Log("Road segment updated. Difficulty: " + currentSpread);
    }

    private void UpdateRoadGO()
    {
        for (int i = 0; i < roadGOs.Count; i++)
        {
            roadGOs[i].transform.position = path[i];
            roadGOs[i].transform.forward = path[i + 1] - path[i];
        }
    }
    private void UpdateSpreadAndZStep(int newZStep)
    {
      //  currentSpread = currentZStep * DIFFICULTY_TO_SPREAD_MULTIPLIER;
        currentZStep = newZStep;

        if (newZStep * DIFFICULTY_TO_SPREAD_MULTIPLIER > MAX_WAYPOINT_SPREAD)
        {
            currentSpread = MAX_WAYPOINT_SPREAD;
            return;
        }
    }

}
