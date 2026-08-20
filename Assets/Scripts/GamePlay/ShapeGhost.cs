using System.Collections.Generic;
using UnityEngine;

public class ShapeGhost : MonoBehaviour
{
     [SerializeField] private ShapeSquare shapeSquarePrefab;
     [SerializeField] private Color ghostColor = new Color(1f, 1f, 1f, 0.35f);
     
     private List<ShapeSquare> ghostSquares = new List<ShapeSquare>();

     public void CreatGhostShape(Shape shape)
     {
          
     }

     public void Show()
     {
          gameObject.SetActive(true);
     }

     public void Hide()
     {
          gameObject.SetActive(false);
     }

     public void ClearGhostShape()
     {
          foreach (ShapeSquare shapeSquare in ghostSquares)
          {
               if (shapeSquare != null)
               {
                    Destroy(shapeSquare.gameObject);
               }
          }
          
          ghostSquares.Clear();
     }

}
