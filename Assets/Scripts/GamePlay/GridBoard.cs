using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GridBoard : MonoBehaviour
{
    [FormerlySerializedAs("colums")] [SerializeField] private int columns = 8;
    [SerializeField] private int rows = 8; 
    [SerializeField] ShapeSquare shapeSquarePrefab;
    [SerializeField] Transform startPosition;
    [SerializeField] float _offset = 0.0001f;
    
    private ShapeSquare[,] gridSquares;

    private void Awake()
    {
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        gridSquares = new ShapeSquare[rows, columns];
        
        float xSquareSize = shapeSquarePrefab.underlaySprite.GetComponent<Renderer>().bounds.size.x;
        float ySquareSize = shapeSquarePrefab.underlaySprite.GetComponent<Renderer>().bounds.size.y;
        
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                ShapeSquare shapeSquare = Instantiate(shapeSquarePrefab,startPosition.position + new Vector3(row * (xSquareSize + _offset) ,column * (ySquareSize + _offset) , 0f ) , Quaternion.identity,this.transform);
                shapeSquare.name = "Square "+row+","+column;
                shapeSquare.IsOccupied = false;
                gridSquares[row, column] = shapeSquare;
            }
        }
    }
    
    public bool GetGridSquare(Vector3 worldPosition, out ShapeSquare gridSquare)
    {
        gridSquare = null;
        
        Vector3 localPosition = worldPosition - startPosition.position;
        
        float xSquareSize = shapeSquarePrefab.underlaySprite.GetComponent<Renderer>().bounds.size.x;
        float ySquareSize = shapeSquarePrefab.underlaySprite.GetComponent<Renderer>().bounds.size.y;
        
        int row = Mathf.RoundToInt(localPosition.x / xSquareSize + _offset);
        int column = Mathf.RoundToInt(localPosition.y/ySquareSize + _offset);

        if (row < 0 || row >= rows || column < 0 || column >= columns)
            return false;
        
        gridSquare = gridSquares[row, column];
        
        return gridSquare != null;
    }

    
    public Vector3 GetGridSquarePosition(Vector3 worldPosition)
    {
        Vector3 localPosition = worldPosition - startPosition.position;
        
        float xSquareSize = shapeSquarePrefab.underlaySprite.GetComponent<Renderer>().bounds.size.x;
        float ySquareSize = shapeSquarePrefab.underlaySprite.GetComponent<Renderer>().bounds.size.y;
        
        int row = Mathf.RoundToInt(localPosition.x/(xSquareSize + _offset));
        int column = Mathf.RoundToInt((localPosition.y / (ySquareSize + _offset)));

        if (row < 0 || row >= rows || column < 0 || column >= columns)
            return worldPosition;
        
        return gridSquares[row, column].transform.position;
    }

    public List<ShapeSquare> GetCompleteRowOrColumn()
    {
        List<ShapeSquare> completeSquare = new List<ShapeSquare>();
        bool isFull;

        for (int r = 0; r < rows; r++)
        {
            isFull = true;
            for (int c = 0; c < columns; c++)
            {
                if (!gridSquares[r, c].IsOccupied)
                {
                    isFull = false;
                    break;
                }
            }
            if (isFull)
            {
                for (int c = 0; c < columns; c++)
                {
                    completeSquare.Add(gridSquares[r, c]);
                }
            }
        }

        for (int c = 0; c < columns; c++)
        {
            isFull = true;
            for (int r = 0; r < rows; r++)
            {
                if (!gridSquares[r, c].IsOccupied)
                {
                    isFull = false;
                    break;
                }
            }
            if (isFull)
            {
                for (int r = 0; r < rows; r++)
                {
                    if (!completeSquare.Contains(gridSquares[r, c]))
                    {
                        completeSquare.Add(gridSquares[r, c]);
                    }
                }
            }
        }
        
        return completeSquare;
    }

    public bool CanPlaceShapes(ShapeData shapeData)
    {
        int shapeRows = shapeData.rows;
        int shapeColumns = shapeData.columns;
        
        bool[,] shapeGrid = new bool[shapeRows, shapeColumns];
        for (int r = 0; r < shapeRows; r++)
        {
            for (int c = 0; c < shapeColumns; c++)
            {
                shapeGrid[r, c] = shapeData.shapeBoard[r].colums[c];
            }
        }
        
        int minR = shapeRows, maxR = -1, minC = shapeColumns, maxC = -1;

        for (int r = 0; r < shapeRows; r++)
        {
            for (int c = 0; c < shapeColumns; c++)
            {
                if (shapeGrid[r, c])
                {
                    if(r < minR) minR = r;
                    if(r > maxR) maxR = r;
                    if(c < minC) minC = c;
                    if(c > maxC) maxC = c;
                }
            }
        }

        if (maxR == -1) return false; // shape empty

        int shapeHeight = maxR - minR + 1;
        int shapeWidth = maxC - minC + 1;

        for (int startR = 0; startR <= rows - shapeHeight; startR++)
        {
            for (int startC = 0; startC <= columns - shapeWidth; startC++)
            {
                bool canPlace = true;

                for (int r = minR; r <= maxR; r++)
                {
                    for (int c = minC; c <= maxC; c++)
                    {
                        if (shapeGrid[r, c])
                        {
                            int gridR = startR + (r - minR);
                            int gridC = startC + (c - minC);

                            if (gridSquares[gridR, gridC].IsOccupied)
                            {
                                canPlace = false;
                                break;
                            }
                        }
                    }
                    
                    if(!canPlace) break;
                }

                if (canPlace) return true;
            }
        }
        
        return false;
    }

    public IEnumerator ClearCompletedRowOrColumn(List<ShapeSquare> completeSquare)
    {
        if(completeSquare == null || completeSquare.Count == 0)
            yield break;
        
        HashSet<ShapeSquare> uniqueSquares = new HashSet<ShapeSquare>(completeSquare);
        int linesCleared = completeSquare.Count / columns;

        if (linesCleared > 0)
        {
            ScoreManager.instance.AddScore(linesCleared,ComboManager.instance.CurrentCombo);
        }
        
        List<Coroutine> animCoroutine = new List<Coroutine>();

        foreach (ShapeSquare gridSquare in completeSquare)
        {
            animCoroutine.Add(StartCoroutine((gridSquare.ClearAnimation())));
        }

        foreach (Coroutine c in animCoroutine)
        {
            yield return c;
        }

        foreach (ShapeSquare gridSquare in completeSquare)
        {
            if (gridSquare != null)
            {
                gridSquare.ResetToEmpty();
            }
            
        }
    }
}
