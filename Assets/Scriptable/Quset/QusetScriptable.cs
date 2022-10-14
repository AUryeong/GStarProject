using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Ingredients Data", menuName = "ScriptableObject/Ingredients", order = int.MaxValue)]
[System.Serializable]
public class Quset
{
    public bool isClear;
}
public class QusetScriptable : MonoBehaviour
{
    public class Ingredients : ScriptableObject
    {
        public List<stats> Stats = new List<stats>();
    }

}
