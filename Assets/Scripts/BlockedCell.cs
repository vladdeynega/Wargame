using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockedCell : MonoBehaviour
{
    public int row;
    public int column;

    MapController mapController;

    void OnEnable()
    {
        mapController = FindObjectOfType<MapController>();
        mapController.GetComponent<MapController>().field[row, column] = gameObject;
    }
}
