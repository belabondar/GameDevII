using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private readonly List<EnemyController> _enemies = new();
    private readonly List<TowerController> _towerControllers = new();

    private void Awake()
    {
        if (Instance == null) // If there is no instance already
        {
            DontDestroyOnLoad(
                gameObject); // Keep the GameObject, this component is attached to, across different scenes
            Instance = this;
        }
        else if (Instance != this) // If there is already an instance and it's not `this` instance
        {
            Destroy(gameObject); // Destroy the GameObject, this component is attached to
        }
    }

    private void Update()
    {
        SetTargets();
    }

    public void AddEnemy(EnemyController enemy)
    {
        _enemies.Add(enemy);
    }

    public void RemoveEnemy(EnemyController enemy)
    {
        _enemies.Remove(enemy);
        foreach (var tower in _towerControllers)
            if (tower.GetTarget() == enemy)
                tower.RemoveTarget();
    }

    public int GetEnemyCount()
    {
        return _enemies.Count;
    }


    public void AddTower(TowerController tower)
    {
        _towerControllers.Add(tower);
    }

    public void RemoveTower(TowerController tower)
    {
        _towerControllers.Remove(tower);
    }

    private void SetTargets()
    {
        foreach (var tower in _towerControllers)
        {
            var range = tower.GetRange();
            foreach (var enemy in _enemies)
                if (GetDistance(tower.transform, enemy.transform) <= range)
                {
                    tower.SetTarget(enemy);
                    break;
                }
        }
    }

    private static float GetDistance(Transform a, Transform b)
    {
        return Vector3.Distance(a.position, b.position);
    }
}