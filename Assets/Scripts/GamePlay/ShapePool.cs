using System;
using UnityEngine;
using System.Collections.Generic;


namespace Block_Blast.Scripts
{
    public class ShapePool : MonoBehaviour
    {
        [SerializeField] private Shape shapePrerfab;
        [SerializeField] private List<ShapeData> shapeData =  new List<ShapeData>();
        [SerializeField] private Transform[] spawnPositions;

        [SerializeField] private List<ColorPallete> colorPalletes;
        [SerializeField] private GridBoard gridBoard;
        
        private int placedShapeCount;
        
        private void Start()
        {
            GenerateShape();
        }
        
        void GenerateShape()
        {
            placedShapeCount = 0;
            int maxAttemps = 100;

            for (int i = 0; i < spawnPositions.Length; i++)
            {
                ShapeData selectedData = null;
                int attempts = 0;

                while (attempts < maxAttemps && selectedData == null)
                {
                    ShapeData randomData = shapeData[UnityEngine.Random.Range(0, shapeData.Count)];

                    if (gridBoard.CanPlaceShapes(randomData))
                    {
                        selectedData = randomData;
                        break;
                    }
                    
                    attempts++;
                }
                
                if (selectedData == null)
                {
                    continue;
                }
                
                Shape shape = Instantiate(shapePrerfab, spawnPositions[i].position, Quaternion.identity,spawnPositions[i]);
                shape.SetShapeData(selectedData);

                ColorPallete randomPallete = null;

                if (colorPalletes != null && colorPalletes.Count > 0)
                {
                    randomPallete = colorPalletes[UnityEngine.Random.Range(0, colorPalletes.Count)];
                }

                if (randomPallete != null)
                {
                    shape.ApplyPalleteToAllSquares(randomPallete);
                }
            }
        }

        public bool HasAvailableShapes()
        {
            Shape[] shapes = GetComponentsInChildren<Shape>();

            if (shapes.Length == 0) return false;

            foreach (Shape shape in shapes)
            {
                if(gridBoard.CanPlaceShapes((shape.GetShapeData())))
                    return true;
            }

            return false;
        }

        public void ShapePlaced()
        {
            placedShapeCount++;
            
            if (placedShapeCount >= spawnPositions.Length)
            {
                GenerateShape();
            }
        }
    }
}