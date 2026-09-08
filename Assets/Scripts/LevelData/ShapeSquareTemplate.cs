using UnityEngine;

namespace LevelData
{
    [CreateAssetMenu(fileName = "ShapeSquareTemplate",menuName = "LevelData/ShapeSquareTemplate")]
    public class ShapeSquareTemplate : ScriptableObject
    {
        public Color backgroundColor;
        public Color underlayColor;
        public Color bottomColor;
        public Color topColor;
        public Color leftColor;
        public Color rightColor;
        public Color overlayColor;

        public Sprite backgroundSprite;
        public Sprite underlaySprite;
        public Sprite bottomSprite;
        public Sprite topSprite;
        public Sprite leftSprite;
        public Sprite rightSprite;
        public Sprite overlaySprite;
        
        public bool[] colorEnable = new bool[7]{true,true,true,true,true,true,true};

        public ShapeSquare customShapeSquarePrefab;
        
        public bool HasCustomPrefab() => customShapeSquarePrefab != null;
    }
}