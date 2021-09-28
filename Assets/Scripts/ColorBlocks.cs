using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBlocks : MonoBehaviour
{
    [SerializeField]
    private int row; //строка блока

    [SerializeField]
    private int column; //столбец блока

    [SerializeField]
    internal string color; // цвет блока

    private int nextRow; // строка куда хотим передвинут фишку
    private int nextColumn; // столбец куда хотим передвинут фишку

    private bool cellIsEmpty; // проверка свободна ли клетка куда хотим передвинуть фишку

    private BoxCollider2D boxCollider;
    private Rigidbody2D body;
    private MapController mapController;

    private Vector2 curPosition; // текущие координаты фишки
    private Vector2 mousePosition; // координаты мыши
    private Vector2 cellPosition; // координаты клетки на которую хотим передвинуть фишку
    private Vector3 offset; // смещение для управления мышью

    private Cell curCell; // текущая клетка 

    bool isNear(int row1, int column1, int row2, int column2) // Проверка является ли клетка соседней по вертикали/горизонтали
    {
        int nearRow = Mathf.Abs(row2 - row1);
        int nearColumn = Mathf.Abs(column2 - column1);
        return (nearRow + nearColumn == 0) || (nearRow == 1 && nearColumn == 0) || (nearRow == 0 && nearColumn == 1) ? true : false;
    }
    void OnMouseDown()
    {
        boxCollider.isTrigger = true;
        body.bodyType = RigidbodyType2D.Dynamic;
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
    }

    void OnMouseDrag()
    {       
        Vector2 curScreenPoint = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        mousePosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = mousePosition;
    }

    void OnMouseUp()
    {     
        boxCollider.isTrigger = false;
        if (cellIsEmpty) // при отжатии мыши, если клетка свободна mapController меняет ее позицию
        {           
            mapController.ChangePosition(row, column, nextRow, nextColumn, gameObject);  
            row = nextRow;
            column = nextColumn;
            transform.position = cellPosition; // чтобы фишка становилась в центр клетки
            curPosition = transform.position;

        }
        else transform.position = curPosition; // если клетка занята фишка возвращается на текущую позицию
        body.bodyType = RigidbodyType2D.Kinematic;
    }

   
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Cell" )
        {
            curCell = other.gameObject.GetComponent<Cell>();
            nextRow = curCell.row;
            nextColumn = curCell.column;

            if (mapController.IsBoard(nextRow, nextColumn) && isNear(row, column, nextRow, nextColumn) && mapController.IsNull(nextRow, nextColumn))
            {
                cellPosition = other.transform.position;
                curCell.cellSpriteRenderer.color = Color.green;
                cellIsEmpty = true;
            }
            else if (row == nextRow && column == nextColumn)
                curCell.cellSpriteRenderer.color = Color.green;
            else
            {
                curCell.cellSpriteRenderer.color = Color.red;
                cellIsEmpty = false;
            }

            }
        else
        {
            cellIsEmpty = false;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Cell")
        {
            curCell.cellSpriteRenderer.color = Color.white;
            cellPosition = curPosition;
        }
    }
    void OnEnable()
    {
        mapController = FindObjectOfType<MapController>();

        mapController.GetComponent<MapController>().field[row, column] = gameObject;

        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        body = gameObject.GetComponent<Rigidbody2D>();

        curPosition = gameObject.transform.position;
    }

    
    
}
