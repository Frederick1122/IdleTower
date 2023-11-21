using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SphereCollider))]
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;

    private SphereCollider _sphereCollider;
    
    private void Start()
    {
        _sphereCollider = GetComponent<SphereCollider>();
    }

    [ContextMenu("Spawn")]
    public void SpawnEnemy()
    {
        var randomAngle = transform.rotation;
        randomAngle.eulerAngles = new Vector3(0, Random.Range(1, 359), 0);
        transform.rotation = randomAngle;
        var enemyPosition = transform.position + transform.forward * _sphereCollider.radius;
        var newEnemy = Instantiate(_enemyPrefab, enemyPosition,quaternion.identity);
    }
}
