using UnityEngine;

public class SpawnerHandler : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] float spawnInterval = 2f;
    float spawnTimer = 0f;

    private void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            Spawn();
            spawnTimer = spawnInterval;
        }
    }


    private void Spawn()
    {
        Instantiate(prefab, transform.position, Quaternion.identity);
    }
}