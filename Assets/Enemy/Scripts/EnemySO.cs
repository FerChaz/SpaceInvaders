using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/EnemyData", order = 1)]

public class EnemySO : ScriptableObject
{
    public GameObject enemy;
    public Color enemyColor = Color.white;
}
