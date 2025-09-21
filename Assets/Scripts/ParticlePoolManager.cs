using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePoolManager : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] private GameObject particlePrefab;
    [SerializeField] private int poolSize = 10;
    [SerializeField] private float simulationTime = 3f;

    private Camera targetCamera;

    private Queue<GameObject> particlePool = new Queue<GameObject>();
    private List<GameObject> activeParticles = new List<GameObject>();

    void Start()
    {
        // Eğer kamera atanmamışsa main camera'yı kullan
        targetCamera = GetComponent<Camera>();

        InitializePool();
    }

    void Update()
    {
        HandleMouseInput();
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject particle = Instantiate(particlePrefab);
            particle.SetActive(false);
            particlePool.Enqueue(particle);
        }
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 worldPos = targetCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, targetCamera.nearClipPlane + 5f));

            SpawnParticle(worldPos);
        }
    }

    private void SpawnParticle(Vector3 position)
    {
        if (particlePool.Count > 0)
        {
            GameObject particle = particlePool.Dequeue();
            particle.transform.position = position;
            particle.SetActive(true);

            // Particle System'i başlat
            ParticleSystem ps = particle.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
            }

            activeParticles.Add(particle);

            // Belirtilen süre sonra pool'a geri yolla
            StartCoroutine(ReturnToPoolAfterTime(particle, simulationTime));
        }
        else
        {
            Debug.LogWarning("Particle pool is empty! Consider increasing pool size.");
        }
    }

    private IEnumerator ReturnToPoolAfterTime(GameObject particle, float time)
    {
        yield return new WaitForSeconds(time);
        ReturnToPool(particle);
    }

    private void ReturnToPool(GameObject particle)
    {
        if (activeParticles.Contains(particle))
        {
            // Particle System'i durdur
            ParticleSystem ps = particle.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Stop();
                ps.Clear(); // Mevcut particle'ları temizle
            }

            particle.SetActive(false);
            activeParticles.Remove(particle);
            particlePool.Enqueue(particle);
        }
    }
}