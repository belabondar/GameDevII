using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Map mapRef;

    public GameObject enemy;
    private readonly List<Building> _buildings = new();

    private readonly List<EnemyController> _enemies = new();
    private readonly List<Spawner> _spawners = new();

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
    }

    private void Start()
    {
        _map = Instantiate(mapRef, new Vector3(0, 0, 0), transform.rotation);
    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
            if (_spawners[0] != null)
                _spawners[0].Spawn(enemy, 1, 1f);
    }

    public void AddEnemy(EnemyController enemy)
    {
        _enemies.Add(enemy);
    }

    public void RemoveEnemy(EnemyController enemy)
    {
        _enemies.Remove(enemy);
    }

    public List<EnemyController> GetEnemies()
    {
        return _enemies;
    }

    public int GetEnemyCount()
    {
        return _enemies.Count;
    }


    public void RegisterTower(Building building)
    {
        _buildings.Add(building);
    }

    public void RemoveTower(Building building)
    {
        _buildings.Remove(building);
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