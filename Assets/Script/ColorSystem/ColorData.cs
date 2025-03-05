using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ColorData", menuName = "Custom/Color Data")]
public class ColorData : ScriptableObject
{
    [System.Serializable]
    public class ourColors
    {
        public string ColorName;
        public Vector4 color; // (R, G, B, A)
        public float SourceAmount; //how much left
        public bool selected;
    }

    public List<ourColors> elements = new List<ourColors>();
    public ourColors DetectedColor = new ourColors();

}
