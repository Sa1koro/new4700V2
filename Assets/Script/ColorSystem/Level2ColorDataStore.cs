using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Level2ColorData", menuName = "Custom/Level2ColorData")]
public class Level2ColorDataStore : ScriptableObject
{
    [System.Serializable]
    public class ourColors
    {
        public string ColorName;
        public Vector4 color; // (R, G, B, A)
        public Vector3 position1;
        public Vector3 position2;
        public int indicate;
    }

    public List<ourColors> elements = new List<ourColors>();

}
