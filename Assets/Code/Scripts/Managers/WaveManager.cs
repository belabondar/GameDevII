using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    public TMP_Text spawnTimer;
    public int timeBetweenWaves = 5;
    public float timeBetweenSpawns = 0.2f;

    public EnemyController enemy;

    private Spawner _spawner;

    private int _waves;

    private void Awake()
    {
        if (Instance != null) // If there is already an instance and it's not `this` instance
        {
            Debug.LogError(
                "More than one BuildManager in scene!"); // Destroy the GameObject, this component is attached to
            return;
        }

        Instance = this;
    }

    public void RegisterSpawner(Spawner spawner)
    {
        _spawner = spawner;
        StartCoroutine(Countdown());
    }

    public void RemoveSpawner()
    {
        _spawner = null;
        StopCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        //Each while loop is one wave
        while (true)
        {
            _waves += 1;
            var time = GetWaveTimer();
            spawnTimer.enabled = true;
            while (time > 0)
            {
                spawnTimer.text = time.ToString();
                yield return new WaitForSeconds(1);
                time -= 1;
            }

            spawnTimer.enabled = false;
            StartCoroutine(SpawnEnemies());

            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    private IEnumerator SpawnEnemies()
    {
        var wave = ComposeWave();
        while (wave.Count > 0)
        {
            _spawner.Spawn(wave.Dequeue());
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    private int GetWaveTimer()
    {
        return _waves + 2;
    }

    private Queue<EnemyType> ComposeWave()
    {
        var wave = new Queue<EnemyType>();
        for (var i = 0; i < _waves; i++) wave.Enqueue(GetBiasedEnemyType());

        return wave;
    }

    private EnemyType GetBiasedEnemyType()
    {
        return EnemyType.Base;
    }
}