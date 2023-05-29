using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    //Movement Speed
    public float mSpeed = 10f;

    //Health
    public float maxHealth = 10f;

    public int goldDrop = 10;

    //Get UI Element to always face camera
    public GameObject healthBarUi;

    //Get Slider to adjust health display
    public Slider healthBar;
    private Bank _bank;
    private Camera _camera;

    private Vector3 _direction;

    private GameManager _gameManager;
    private float _health;
    private Quaternion _lookRotation;

    private Rigidbody _rb;

    private int _targetIndex;
    private Vector3 _targetPoint;
    private List<Vector3> _wayPoints = new();

    // Start is called before the first frame update
    private void Start()
    {
        _gameManager = GameManager.Instance;
        _bank = Bank.Instance;
        _gameManager.AddEnemy(this);
        _wayPoints = _gameManager.GetWayPoints();
        //target the first waypoint
        _targetPoint = _wayPoints[_targetIndex];

        transform.rotation = LookAt(_targetPoint);
        //Spawn enemy with max health
        _health = maxHealth;
        _camera = Camera.main;
        healthBar.value = _health / maxHealth;
    }


    // Update is called once per frame
    private void Update()
    {
        TurnTowards(_targetPoint, mSpeed * 2);
        FaceCamera(healthBarUi);

        transform.Translate(Vector3.forward * (Time.deltaTime * mSpeed));


        if (Vector3.Distance(transform.position, _targetPoint) <= 0.5f)
            if (_targetIndex < _wayPoints.Count - 1)
            {
                _targetIndex++;
                _targetPoint = _wayPoints[_targetIndex];
            }
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

    private void FaceCamera(GameObject targetObject)
    {
        var camRotation = _camera.transform.rotation;
        targetObject.transform.LookAt(transform.position + camRotation * Vector3.back,
            camRotation * Vector3.up);
    }

    private void TurnTowards(Vector3 targetPoint, float speed)
    {
        _lookRotation = LookAt(targetPoint);
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * speed);
    }

    private Quaternion LookAt(Vector3 targetPoint)
    {
        _direction = targetPoint - transform.position;
        _direction.y = 0;
        return Quaternion.LookRotation(_direction);
    }

    private void Damage(float dmg)
    {
        if (_health.Equals(maxHealth))
            healthBarUi.SetActive(true);
        _health -= dmg;
        if (_health <= 0) Death();
        healthBar.value = _health / maxHealth;
    }

    private void Death()
    {
        if (_health <= 0) _bank.DepositResource(goldDrop, 0, 0);
        _gameManager.RemoveEnemy(this);
        Destroy(gameObject);
    }
}