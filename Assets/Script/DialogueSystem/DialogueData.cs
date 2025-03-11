using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue System/Dialogue Data")]
public class DialoguesData : ScriptableObject
{
    [TextArea(2, 5)]
    public List<string> dialogueEntries = new List<string>(); // List of dialogue texts
}
