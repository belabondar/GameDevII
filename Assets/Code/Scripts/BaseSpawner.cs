using UnityEngine;

public class BaseSpawner : MonoBehaviour
{
    // Start is called before the first frame update

    public float radius = 10f;
    public GameObject enemy;
    public int spawnRate = 10;
    public int maxInstances = 100;

    private GameManager _gameManager;


    private void Start()
    {
        _gameManager = GameManager.Instance;
        var time = 1f / spawnRate;
        InvokeRepeating(nameof(SpawnEnemy), 0f, time);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void SpawnEnemy()
    {
        var activeInstances = _gameManager.GetEnemyCount();
        if (activeInstances > maxInstances) return;

        var position = RandomPointOnCircleEdge();
        var dir = transform.position - position;

        // Aligns rotation to direction vector
        var rotation = Quaternion.LookRotation(dir);
        var instance = Instantiate(enemy, position, rotation);
        instance.transform.parent = gameObject.transform;
    }

    private Vector3 RandomPointOnCircleEdge()
    {
        var vector2 = Random.insideUnitCircle.normalized * radius;
        return new Vector3(vector2.x, 0, vector2.y);
    }
}