using UnityEngine;

[CreateAssetMenu(fileName = "ShapeData", menuName = "Scriptable Objects/ShapeData")]
public class ShapeData : ScriptableObject
{
    [System.Serializable]
    public class Row
    {
        public bool[] colums;

        public int size;

        public Row(int size)
        {
            CreateRow(size);
        }
        
        public void CreateRow (int size)
        {
            this.size = size;
            colums = new bool[size];
            ClearRow();
        }

        public void ClearRow()
        {
            for (int i = 0; i < size; i++)
            {
                colums[i] = false;
            }
        }
    }

    public int rows;
    public int columns;
    public Row[] shapeBoard;
    
    public ColorPallete colorPallete;

    public void CreateBoard()
    {
        shapeBoard = new Row[rows];

        for (int i = 0; i < rows; i++)
        {
            shapeBoard[i] = new Row(columns);
        }
    }

    public void ClearRow()
    {
        for(int i = 0; i < rows; i++)
        {
            shapeBoard[i].ClearRow();
        }
    }
}
