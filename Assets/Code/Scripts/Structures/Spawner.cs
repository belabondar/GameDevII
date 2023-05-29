using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private WaveManager _waveManager;

    public EnemyController enemy;

    private void Start()
    {
        _waveManager = WaveManager.Instance;
        _waveManager.RegisterSpawner(this);
    }

    public void Spawn(EnemyType type)
    {
        var enemy = Instantiate(GetEnemyFromType(type), transform.position, transform.rotation);
    }
    private EnemyController GetEnemyFromType(EnemyType type)
    {
        return type switch
        {
            _ => enemy
        };
    }
}