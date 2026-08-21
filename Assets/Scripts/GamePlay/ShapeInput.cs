using System.Collections;
using System.Collections.Generic;
using Block_Blast.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShapeInput : MonoBehaviour
{
    [SerializeField] private LayerMask gridMask;
    [SerializeField] private LayerMask shapeMask;
    [SerializeField] private GridBoard gridBoard;
    [SerializeField] private ShapePool shapePool;
    [SerializeField] private float ghostAlpha = 0.4f;
    
    private Shape selectedShape;
    private GameObject ghostShape;
    private Vector3 offset;
    private bool isDragging;
    
    private void Update()
    {
        if (Pointer.current == null)
            return;

        if (Pointer.current.press.wasPressedThisFrame)
        {
            SelectShape();
        }

        if (Pointer.current.press.isPressed && isDragging)
        {
            DragShape();
        }

        if (Pointer.current.press.wasReleasedThisFrame)
        {
            ReleaseShape();
        }
    }

    private Vector3 GetWorldPosition()
    {
        Vector2 screenPosition = Pointer.current.position.ReadValue();
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        worldPosition.z = 0f;

        return worldPosition;
    }

    // private void CheckGrid()
    // {
    //     Vector3 worldPosition = GetWorldPosition();
    //     
    //     Collider2D hit = Physics2D.OverlapPoint(worldPosition, gridMask);
    //
    //     if (hit == null)
    //     {
    //         return;
    //     }
    //     
    //     ShapeSquare square = hit.GetComponent<ShapeSquare>();
    //
    //     if (square != null)
    //     {
    //         Debug.Log(square.name);
    //     }
    // }
    //
    // private void CheckShapeOnGrid()
    // {
    //     if (selectedShape == null) return;
    //
    //     List<ShapeSquare> shapeSquares = selectedShape.GetActiveSquares();
    //
    //     foreach (ShapeSquare square in shapeSquares)
    //     {
    //         Vector3 worldPosition = GetWorldPosition();
    //
    //         Collider2D hit = Physics2D.OverlapPoint(worldPosition, gridMask);
    //
    //         if (hit == null)
    //         {
    //             return;
    //         }
    //
    //         ShapeSquare gridSquare = hit.GetComponent<ShapeSquare>();
    //
    //         if (gridSquare != null)
    //         {
    //                 Debug.Log("Square " + square.name + "->" + gridSquare.name);
    //         }
    //     }
    // }

    bool CanPlaceShape()
    {
        if(selectedShape == null) return false;
        
        List<ShapeSquare> shapeSquares = selectedShape.GetActiveSquares();

        foreach (ShapeSquare square in shapeSquares)
        {
            if (!gridBoard.GetGridSquare(square.transform.position, out ShapeSquare gridSquare))
            {
                return false;
            }

            if (gridSquare.IsOccupied)
            {
                return false;
            }
        }

        return true;
    }

    Vector3 GetSnapPosition(Shape shape)
    {
        List<ShapeSquare> shapeSquares = shape.GetActiveSquares();

        if (shapeSquares.Count == 0)
        {
            return shape.transform.position;
        }
        
        ShapeSquare firstSquare = shapeSquares[0];
        
        Vector3 firstSquarePosition = firstSquare.transform.position;
        
        Vector3 offset = shape.transform.position - firstSquarePosition;
        
        Vector3 gridPosition = gridBoard.GetGridSquarePosition(firstSquarePosition);
        
        return gridPosition +  offset;
    }

    private IEnumerator PlaceShape(Shape shape)
    {
        Vector3 snapPosition = GetSnapPosition(shape);
        
        shape.SnapToPosition(snapPosition);
        
        List<ShapeSquare> shapeSquares = shape.GetActiveSquares();

        foreach (ShapeSquare square in shapeSquares)
        {
            if (gridBoard.GetGridSquare(square.transform.position, out ShapeSquare gridSquare))
            {
                gridSquare.CopyFrom(square);

                StartCoroutine(gridSquare.PopAnimation());
            }
        }
        
        List<ShapeSquare> completeSquares = gridBoard.GetCompleteRowOrColumn();
        bool wasLineCleared = completeSquares.Count > 0;

        if (wasLineCleared)
        {
           yield return StartCoroutine(gridBoard.ClearCompletedRowOrColumn(completeSquares));
        }
        
        Destroy(shape.gameObject);
        shapePool.ShapePlaced();
        
        GameBoardManager.instance.OnShapePlaced(wasLineCleared);
    }
    

    #region Input Selection
    private void SelectShape()
    {
        Vector3 worldPosition = GetWorldPosition();
        Collider2D hit =  Physics2D.OverlapPoint(worldPosition,shapeMask);

        if (hit == null)  return;
        
        Shape shape = hit.GetComponent<Shape>();

        if (shape == null)
        {
            ShapeSquare square = hit.GetComponent<ShapeSquare>();
            if (square != null)
            {
                shape = square.GetComponentInParent<Shape>();
            }
        }
        
        if(shape == null) return;

        if (ghostShape != null)
        {
            Destroy(ghostShape);
            ghostShape = null;
        }

        selectedShape = shape;
        selectedShape.SelectedSquare();
        selectedShape.AddToOrderForAllSquares(100);
        offset = selectedShape.transform.position - worldPosition;
        isDragging = true;
    }

    private void DragShape()
    {
        if(selectedShape == null) return;
        
        Vector3 worldPosition = GetWorldPosition();
        selectedShape.transform.position = worldPosition + offset;
        
        bool canPlace  = CanPlaceShape();

        if (canPlace)
        {
            Vector3 snapPosition = GetSnapPosition(selectedShape);

            if (ghostShape == null)
            {
                ghostShape = Instantiate(selectedShape.gameObject);
                
                Collider2D col = ghostShape.GetComponent<Collider2D>();
                if(col != null) col.enabled = false;
                
                Shape ghostScript = ghostShape.GetComponent<Shape>();
                if (ghostScript != null)
                {
                    ghostScript.startScale = selectedShape.selectedScale;
                    
                    ghostScript.SetAlpha(ghostAlpha);
                    
                    ghostScript.AddToOrderForAllSquares(-100);
                    
                    ghostScript.SnapToPosition(snapPosition);
                }
            }
            else
            {
                Shape ghostScript = ghostShape.GetComponent<Shape>();

                if (ghostScript != null)
                {
                    ghostScript.SnapToPosition(snapPosition);
                }
            }
        }
        else
        {
            if (ghostShape != null)
            {
                Destroy(ghostShape);
                ghostShape = null;
            }
        }
    }

    private void ReleaseShape()
    {
        if (selectedShape == null)  return;

        if (ghostShape != null)
        {
            Destroy(ghostShape);
            ghostShape = null;
        }
        
        bool canPlace = CanPlaceShape();

        if (canPlace)
        {
            Shape shapeToPlace = selectedShape;
            StartCoroutine(PlaceShape(shapeToPlace));
        }
        else
        {
            selectedShape.AddToOrderForAllSquares(-100);
            selectedShape.UnSelectedSquare();
        }

        isDragging = false;
        selectedShape = null;
    }
    
    #endregion
}
