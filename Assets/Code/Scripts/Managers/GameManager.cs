using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Map mapRef;

    public GameObject enemy;

    private readonly List<EnemyController> _enemies = new();
    private readonly List<Spawner> _spawners = new();
    private readonly List<TowerController> _towerControllers = new();

    private Map _map;

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

        _map = Instantiate(mapRef, new Vector3(0, 0, 0), transform.rotation);
    }

    private void Update()
    {
        SetTargets();
        if (Input.GetKeyDown("space"))
            if (_spawners[0] != null)
                _spawners[0].Spawn(enemy, 10, 1f);
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
            var hasTarget = false;
            var range = tower.GetRange();
            foreach (var enemy in _enemies)
                if (GetDistance(tower.transform, enemy.transform) <= range)
                {
                    tower.SetTarget(enemy);
                    hasTarget = true;
                    break;
                }

            if (!hasTarget) tower.RemoveTarget();
        }
    }

    private static float GetDistance(Transform a, Transform b)
    {
        return Vector3.Distance(a.position, b.position);
    }

    public List<Vector3> GetWayPoints()
    {
        return _map.GetWaypoints();
    }

    public void AddSpawner(Spawner spawner)
    {
        _spawners.Add(spawner);
    }

    public void RegenerateMap()
    {
        _map.ResetMap();
    }
}