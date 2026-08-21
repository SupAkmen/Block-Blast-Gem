using System;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
     [SerializeField] ShapeData currentShapeData;
     [SerializeField] ShapeSquare shapeSquarePrefab;
     [SerializeField] private Transform startPosition;
     [SerializeField] private float _offset = 0.01f;
     
     [SerializeField] public float startScale = 0.7f;
     [SerializeField] public float selectedScale = 1;
     [SerializeField] private BoxCollider2D shapeCollider2D;
     
     private Vector3 initialPosition;
     
     
     private void Start()
     {
          transform.localScale = new Vector3(startScale, startScale, startScale);
          initialPosition = transform.position;
     }

     public void CreateGridShape()
     {
          int row = currentShapeData.rows;
          int column = currentShapeData.columns;

          float xSquareSize = shapeSquarePrefab.underlaySprite.GetComponent<Renderer>().bounds.size.x;
          float ySquareSize = shapeSquarePrefab.underlaySprite.GetComponent<Renderer>().bounds.size.y;
          
          float totalHeight = (row - 1) * (xSquareSize + _offset);
          float totalWidth = (column - 1) * (xSquareSize + _offset);
          Vector3 centerOffset  = new Vector3(-totalWidth/2f,-totalHeight/2f,0);
        
          for (int r = 0; r < row; r++)
          {
               for (int c = 0; c < column; c++)
               {
                    if (currentShapeData.shapeBoard[r].colums[c] == true)
                    {
                         Vector3 pos = startPosition.position + centerOffset + new Vector3(r * (xSquareSize + _offset), c * (ySquareSize + _offset), 0f);
                         ShapeSquare shapeSquare = Instantiate(shapeSquarePrefab, pos , Quaternion.identity,this.transform);
                         shapeSquare.IsActive = true;
                         shapeSquare.name = "Square "+r+","+c;
                    }
               }
          }
     }
     
     public void SetShapeData(ShapeData shapeData)
     {
          currentShapeData = shapeData;
          CreateGridShape();
          
          ApplyPalleteToAllSquares((currentShapeData.colorPallete));
          
          transform.localScale = new Vector3(startScale, startScale, startScale);
          initialPosition = transform.position;
     }
     
     public ShapeData GetShapeData() => currentShapeData;

     public void ApplyPalleteToAllSquares(ColorPallete pallete)
     {
          foreach (Transform child in transform)
          {
               ShapeSquare shape = child.GetComponent<ShapeSquare>();

               if (shape != null)
               {
                    shape.ApplyPallete(pallete);
               }
               
          }
     }

     public List<ShapeSquare> GetActiveSquares()
     {
          List<ShapeSquare> squares = new List<ShapeSquare>();

          foreach ( Transform child in transform )
          {
               ShapeSquare shapeSquare = child.GetComponent<ShapeSquare>();

               if (shapeSquare != null && shapeSquare.IsActive)
               {
                    squares.Add(shapeSquare);
               }
          }
          return squares;
     }

     public void SetAlpha(float alpha)
     {
          foreach (Transform child in transform)
          {
               ShapeSquare shapeSquare = child.GetComponent<ShapeSquare>();
               if (shapeSquare != null)
               {
                    shapeSquare.SetAlpha(alpha);
               }
              
          }
     }

     public void AddToOrderForAllSquares(int offset)
     {
          foreach (Transform child in transform)
          {
               ShapeSquare shapeSquare = child.GetComponent<ShapeSquare>();
               
               if(shapeSquare != null)
                    shapeSquare.AddToOrderInLayer(offset);
          }
     }

     public void SelectedSquare()
     {
          transform.localScale = new Vector3(selectedScale, selectedScale, selectedScale);
     }

     public void UnSelectedSquare()
     {
          transform.localScale = new Vector3(startScale, startScale, startScale);
          transform.position = initialPosition;
     }

     public void SnapToPosition(Vector3 position)
     {
          transform.position = position;
     }
}
