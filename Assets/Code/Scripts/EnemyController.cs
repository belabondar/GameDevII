using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float mSpeed = 10f;

    public float health = 10f;

    private GameManager _gameManager;

    // Start is called before the first frame update
    private void Start()
    {
        _gameManager = GameManager.Instance;
        _gameManager.AddEnemy(this);
    }

    // Update is called once per frame
    private void Update()
    {
        transform.position += transform.forward * (Time.deltaTime * mSpeed);
    }

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Hit!");
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