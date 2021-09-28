using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapController : MonoBehaviour
{   
    static int MAPSIZE = 5;

    [SerializeField]
    private Text victoryText;
    [SerializeField]
    private Button restart;
    [SerializeField]
    private Button exitButton;

    internal GameObject[,] field = new GameObject[MAPSIZE, MAPSIZE]; // массив хранящий информацию о положении фишек

    internal List<GameObject> columns = new List<GameObject>(); // список цветных блоков находящихся над полем

    private IEnumerable<string> sortColumns; // перечисление соответствий вертикаьных рядов цвету над полем

    void Start()
    {
        SortColumns();
    }
    private void SortColumns() 
    {
        sortColumns = from c in columns
                      orderby c.GetComponent<ColumnColor>().columnNum
                      select c.GetComponent<ColumnColor>().columnColor;
    }
    private bool IsGameFinished() // проверка собранности всех столбцов
    {
        int columnsFinished = 0;
        int k = 0; // для приведения формата перечисления к формату поля (0,1,2) => (0,2,4)
        HashSet<string> set = new HashSet<string>();
        for (int i = 0; i < 5; i = i + 2)
        {
            for (int j = 0; j < 5; j++)
            {
                string color = field[j, i]?.GetComponent<ColorBlocks>().color ?? "0";
                set.Add(color);
            }
           
            foreach (string e in set)
            {
                if (e == sortColumns.ElementAt(i + k) && set.Count == 1) // если в хэш-сете только одно значение и оно равно цвету над своим рядом то ряд собран верно 
                {
                    columnsFinished++;
                }              
            }
            k--;
            set.Clear();
        }
        return columnsFinished == 3 ? true : false;
    }

    private void Victory()
    {
        victoryText.text = "Победа! Конец игры";
        restart.gameObject.SetActive(true);
        Debug.Log("Победа");
        Time.timeScale = 0;
    }
    private void Restart()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
        restart.gameObject.SetActive(false);
    }
    internal void ChangePosition(int row, int column, int curRow, int curColumn, GameObject colorBlock)// обновление информации о положении 
    {
        field[row, column] = null;
        field[curRow, curColumn] = colorBlock;
        if (IsGameFinished())
            Victory();
    }
    internal bool IsBoard(int row, int column) 
    {
        return (row >= 0 || row <= 4) && (column >= 0 || column <= 4) & field[row, column] == null ? true : false;
    }
    internal bool IsNull(int row, int column) 
    {
        return field[row, column] == null ?  true : false;
    }
    public void Exit()
    {
        Application.Quit();
    }
}
