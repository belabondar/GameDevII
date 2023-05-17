using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private List<EnemyController> _enemies = new List<EnemyController>();
    private List<TowerController> _towerControllers = new List<TowerController>();
    void Awake()
    {
        if(Instance == null) // If there is no instance already
        {
            DontDestroyOnLoad(gameObject); // Keep the GameObject, this component is attached to, across different scenes
            Instance = this;
        } else if(Instance != this) // If there is already an instance and it's not `this` instance
        {
            Destroy(gameObject); // Destroy the GameObject, this component is attached to
        }
    }

    private void Update()
    {
    }

    public void AddEnemy(EnemyController enemy)
    {
        _enemies.Add(enemy);
    }

    public void RemoveEnemy(EnemyController enemy)
    {
        _enemies.Remove(enemy);
    }

    public int GetEnemyCount()
    {
        return _enemies.Count;
    }

    [CanBeNull]
    public EnemyController GetTarget(Transform towerLocation, float range)
    {
        Debug.Log("Getting target");
        foreach (var enemy in _enemies)
        {
            if (Vector3.Distance(towerLocation.position, enemy.transform.position) <= range)
            {
                return enemy;
            }
        }

        return null;
    }
}
