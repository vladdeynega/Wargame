using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int row;
    public int column;

    public SpriteRenderer cellSpriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        cellSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
}
