using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RailPath))]
public class CarSpawner : MonoBehaviour
{
    int railStartIndex = 0;
    List<Vector3> carPositions;

    [SerializeField] private CarController carPrefab;

    private void Start()
    {
        carPositions = GetComponent<RailPath>().GetRailPath();

        for (int i = 0; i < carPositions.Count; i++)
        {
            CarController car = Instantiate(carPrefab, carPositions[railStartIndex], Quaternion.identity);
            car.Init(railStartIndex++, carPositions);
        }
    }
}
