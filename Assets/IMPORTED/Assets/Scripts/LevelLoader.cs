using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    // Sonraki sahnenin (seviyenin) adını buraya yazın
    public string nextSceneName; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Eğer çarpışan nesne "Player" tag'ine sahipse
        if (other.CompareTag("Player"))
        {
            // "nextSceneName" adındaki sahneyi yükle
            SceneManager.LoadScene(nextSceneName);
            Debug.Log("Bir sonraki seviyeye geçiliyor: " + nextSceneName);
        }
    }
}