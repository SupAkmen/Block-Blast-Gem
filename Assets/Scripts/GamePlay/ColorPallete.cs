using UnityEngine;

[CreateAssetMenu(fileName = "ColorPallete", menuName = "Scriptable Objects/ColorPallete")]
public class ColorPallete : ScriptableObject
{
    public Color underlayColor =  Color.white;
    public Color topColor = Color.white;
    public Color bottomColor = Color.white;
    public Color leftColor = Color.white;
    public Color rightColor = Color.white;
    public Color overlayColor = Color.white;
    public Color bonusColor = Color.white;
}
