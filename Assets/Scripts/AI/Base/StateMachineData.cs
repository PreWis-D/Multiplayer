using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StateMachineData : MonoBehaviour
{
    [SerializeField] private List<Transform> _waypionts;

    public PlayerController PlayerController { get; private set; }
    public Vector3 LastPlayerPosition { get; private set; }

    public Transform GetNextWaypoint(Transform current)
    {
        if (_waypionts.Count == 0)
            throw new Exception("There are no waypoints assigned");

        if (_waypionts.Count == 1)
            return _waypionts[0];

        int index = Random.Range(0, _waypionts.Count);

        while (_waypionts[index] == current)
            index = Random.Range(0, _waypionts.Count);
        
        return _waypionts[index];
    }

    public void SetPlayerController(PlayerController controller)
    {
        if (controller)
            PlayerController = controller;
    }

    public void SetLastPlayerPosition(Vector3 position)
    {
        LastPlayerPosition = position;
    }
}
