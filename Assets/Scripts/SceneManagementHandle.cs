using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagementHandle : MonoBehaviour
{
    public static SceneManagementHandle instance;

    [SerializeField] GameObject loadingScreen;
    [SerializeField] float minimumLoadTime = 1f; // Sahte bekleme süresi

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Başlangıçta level1'i yükle
        StartCoroutine(LoadSceneAsync(SceneType.level1, minimumLoadTime));
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ChangeScene((SceneType)SceneManager.GetActiveScene().buildIndex);
        }
    }
    public void NextScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int maxScenes = System.Enum.GetValues(typeof(SceneType)).Length;
        int nextIndex = (currentIndex + 1) % maxScenes;

        if (nextIndex == 0)
            nextIndex = 1;

        ChangeScene((SceneType)nextIndex);
    }
    public void ChangeScene(SceneType type, float fakeWaitTime = 0f)
    {
        StartCoroutine(LoadSceneAsync(type, fakeWaitTime));
    }

    private IEnumerator LoadSceneAsync(SceneType sceneType, float fakeWaitTime)
    {
        // Loading screen'i aktif et
        if (loadingScreen != null)
            loadingScreen.SetActive(true);

        // Sahte bekleme süresi varsa bekle
        if (fakeWaitTime > 0f)
        {
            yield return new WaitForSeconds(fakeWaitTime);
        }

        // Asenkron sahne yüklemeyi başlat
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync((int)sceneType);

        // Sahne yüklenene kadar bekle
        while (!asyncLoad.isDone)
        {
            // İsteğe bağlı: progress bar için asyncLoad.progress kullanabilirsin (0-0.9 arası)
            yield return null;
        }

        // Minimum yükleme süresi kontrolü
        yield return new WaitForSeconds(minimumLoadTime);

        // Loading screen'i kapat
        if (loadingScreen != null)
            loadingScreen.SetActive(false);
    }
}

public enum SceneType
{
    setupScene,
    level1,
    level2
}