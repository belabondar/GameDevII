using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float mSpeed = 10f;

    public float health = 10f;

    private GameManager _gameManager;

    private Vector3 _target;

    private int _targetIndex;

    private List<Vector3> _wayPoints = new();

    // Start is called before the first frame update
    private void Start()
    {
        _gameManager = GameManager.Instance;
        _gameManager.AddEnemy(this);
        _wayPoints = _gameManager.GetWayPoints();
        _target = _wayPoints[_targetIndex];
    }

    // Update is called once per frame
    private void Update()
    {
        var dir = _target - transform.position;
        // transform.rotation = Quaternion.LookRotation(dir);
        transform.Translate(dir.normalized * (mSpeed * Time.deltaTime));

        if (Vector3.Distance(transform.position, _target) <= 0.2f)
            if (_targetIndex < _wayPoints.Count)
                _target = _wayPoints[_targetIndex++];
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Structure"))
        {
            Death();
        }
        else if (collider.CompareTag("Projectile"))
        {
            var damage = collider.GetComponentInParent<ProjectileController>().damage;
            //Apply Damage
            Damage(damage);
            //Destroy the projectile
            Destroy(collider.gameObject);
        }
    }

    private void Damage(float dmg)
    {
        health -= dmg;
        if (health <= 0) Death();
    }

    private void Death()
    {
        _gameManager.RemoveEnemy(this);
        Destroy(gameObject);
    }
}