using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData" , menuName = "ScriptableObject / EnemyData")]
public class EnemyData : ScriptableObject
{
    public int Rank;
    public int currencyForEnemyDestroy;
    public int scoreForDeath;
    public int healthInc;
    public int speed;
    public int damageToTower;
    public int health;
    
}
