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
    }

    public List<ourColors> elements = new List<ourColors>();
}
