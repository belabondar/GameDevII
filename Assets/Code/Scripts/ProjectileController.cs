using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float speed = 30f;

    public float damage = 1f;

    public float distanceCull = 10f;

    public bool followTarget;

    private Vector3 _start;
    public EnemyController Target { get; set; }

    // Update is called once per frame
    private void Start()
    {
        _start = gameObject.transform.position;
    }

    private void Update()
    {
        if (followTarget) AimAtTarget();
        transform.position += transform.forward * (Time.deltaTime * speed);
        if (Vector3.Distance(_start, transform.position) > distanceCull) Destroy(gameObject);
    }

    private void AimAtTarget()
    {
        if (Target != null)
            transform.LookAt(Target.transform);
        else
            followTarget = false;
    }

    public void SetTarget(EnemyController target)
    {
        Target = target;
    }
}