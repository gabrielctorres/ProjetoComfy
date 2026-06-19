using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GameData")]

public class GameData : ScriptableObject
{
    public int point;
    public int currenteDay = 1;
}
