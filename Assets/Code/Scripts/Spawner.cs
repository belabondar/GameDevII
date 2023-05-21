using UnityEngine;

public class Spawner : MonoBehaviour
{
    private int _count;
    private GameObject _enemy;
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _gameManager.AddSpawner(this);
    }

    public void Spawn(GameObject enemy, int count, float time)
    {
        _count = count;
        _enemy = enemy;
        InvokeRepeating(nameof(SpawnEnemy), 0f, time);
    }

    private void SpawnEnemy()
    {
        Instantiate(_enemy, transform.position, transform.rotation);

        if (--_count == 0) CancelInvoke(nameof(SpawnEnemy));
    }
}