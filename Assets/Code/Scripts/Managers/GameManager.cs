using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Singleton pattern so there is only one game manager influencing the game flow, the instance is publicly exposed
    //to be accessed on runtime
    public static GameManager Instance;

    public Map mapRef;

    public GameObject enemy;

    public int startGold = 200;
    public int startWood = 50;
    public int startStone = 10;


    private readonly List<Building> _buildings = new();
    private Bank _bank;

    private List<EnemyController> _enemies = new();

    private Map _map;
    private Spawner _spawner;

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
        _bank = Bank.Instance;
        _bank.DepositResource(startGold, startWood, startStone);
    }

    private void Init()
    {
        Destroy(_spawner);
        foreach (var enemy in _enemies) Destroy(enemy.gameObject);

        _enemies = new List<EnemyController>();
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
        _spawner = spawner;
    }

    public void RegenerateMap()
    {
        Init();
        _map.ResetMap();
    }
}