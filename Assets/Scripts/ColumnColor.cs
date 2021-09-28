using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnColor : MonoBehaviour
{
    [SerializeField]
    internal int columnNum;
    [SerializeField]
    internal string columnColor;

    MapController mapController;

    void OnEnable()
    {
        mapController = FindObjectOfType<MapController>();
        mapController.GetComponent<MapController>().columns.Add(gameObject);
    }
}
