using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpawner : MonoBehaviour
{
    // Start is called before the first frame update

    public float radius = 10f;
    public GameObject enemy;
    public int spawnRate = 10;
    public int maxInstances = 100;

    private GameManager _gameManager;
    
    
    void Start()
    {
        _gameManager = GameManager.Instance;
        float time = 1f / spawnRate;
        InvokeRepeating(nameof(SpawnEnemy), 0f, time);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnEnemy()
    {
        var activeInstances = _gameManager.GetEnemyCount();
        if (activeInstances > maxInstances) return;
        
        var position = RandomPointOnCircleEdge();
        var dir = this.transform.position - position;
            
        // Aligns rotation to direction vector
        var rotation = Quaternion.LookRotation(dir);
        GameObject.Instantiate(enemy, position, rotation );
    }
    
    private Vector3 RandomPointOnCircleEdge()
    {
        var vector2 = Random.insideUnitCircle.normalized * radius;
        return new Vector3(vector2.x, 0, vector2.y);
    }
}
