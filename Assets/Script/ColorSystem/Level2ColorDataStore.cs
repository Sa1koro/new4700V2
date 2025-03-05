using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Level2cOLORData", menuName = "Custom/Level2cOLORData")]
public class Level2ColorDataStore : ScriptableObject
{
    [System.Serializable]
    public class ourColors
    {
        public string ColorName;
        public Vector4 color; // (R, G, B, A)
        public Transform position1;
        public Transform position2;
        public int indicate;
    }

    public List<ourColors> elements = new List<ourColors>();

}
