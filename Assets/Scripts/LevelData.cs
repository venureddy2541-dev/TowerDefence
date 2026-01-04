using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelData" , menuName = "ScriptableObject / LevelData")]
public class LevelData : ScriptableObject
{
    public List<WaveData> waveData = new List<WaveData>();
}
