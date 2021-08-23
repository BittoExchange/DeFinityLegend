using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<EnemyBase> enemies = new List<EnemyBase>();
    public Transform enemiesStatsHolder;
    public EnemyStats enemyStatsPrefab;
    public Vector3 enemyStatsOffset;

    public static EnemyManager Instance;

    void Awake()
    {
        Instance = this;
        CreateEnemies();
    }

    void CreateEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            EnemyStats stats = Instantiate(enemyStatsPrefab, enemiesStatsHolder);
            enemies[i].enemyStats = stats;
        }
    }

    void Update()
    {
        EnemiesStatsPositionUpdate();
    }

    void EnemiesStatsPositionUpdate()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].enemyStats.transform.position = Camera.main.WorldToScreenPoint(enemies[i].transform.position) + enemyStatsOffset;
        }
    }

    public EnemyBase GetAliveEnemy()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if(enemies[i].health > 0)
            {
                return enemies[i];
            }
        }

        return null;
    }

    public EnemyBase[] GetAliveEnemies()
    {
        List<EnemyBase> enemiesAlive = new List<EnemyBase>();

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].health > 0)
            {
                enemiesAlive.Add(enemies[i]);
            }
        }

        return enemiesAlive.ToArray();
    }
}
