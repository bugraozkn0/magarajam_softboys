using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace NoRyte.Utils
{
    public static class NoRyteUtils
    {
        #region Create World Text
        public static TextMesh CreateWorldText(
            string text,
            Transform parent = null,
            Vector3 localPosition = default(Vector3),
            Vector3 localSize = default(Vector3),
            Vector3 localRotation = default(Vector3),
            int fontSize = 40,
            Color? color = null,
            TextAnchor textAnchor = TextAnchor.UpperLeft,
            TextAlignment textAlignment = TextAlignment.Center,
            int sortingOrder = 0
        )
        {
            GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            transform.localEulerAngles = localRotation;
            transform.localScale = localSize;

            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color ?? Color.white;

            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            return textMesh;
        }

        // Ekranda belirli bir süre boyunca debug mesajı gösterme
        public static void ShowDebugMessage(
            string message,
            float duration = 2f,
            Transform parent = null,
            Vector3 localPosition = default(Vector3)
        )
        {
            TextMesh textMesh = CreateWorldText(message, parent, localPosition, (Vector3.one / 10), new Vector3(90, 0, 0), 20, Color.red);
            GameObject.Destroy(textMesh.gameObject, duration);
        }

        // Yukarıya doğru küçülerek yükselen debug yazısı
        public static void ShowFloatingDebugMessage(
            string message,
            Vector3 worldPosition,
            float duration = 2f,
            float moveSpeed = 2f,
            Color? color = null,
            int fontSize = 20
        )
        {
            GameObject gameObject = new GameObject("Floating_Debug_Text", typeof(TextMesh));
            gameObject.transform.position = worldPosition;

            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.alignment = TextAlignment.Center;
            textMesh.text = message;
            textMesh.fontSize = fontSize;
            textMesh.color = color ?? Color.yellow;

            // Floating text animasyon component'i ekle
            FloatingTextAnimator animator = gameObject.AddComponent<FloatingTextAnimator>();
            animator.Initialize(duration, moveSpeed);
        }
        #endregion

        #region Mouse Position
        public static Vector3 GetMouseWorldPosition()
        {
            // Yeni yöntemi kullanarak mouse pozisyonunu al
            return GetMouseWorldPositionOnGrid();
        }

        // Kamera açısından ışın göndererek zemin üzerindeki mouse pozisyonunu bul
        public static Vector3 GetMouseWorldPositionOnGrid()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float rayLength;

            if (groundPlane.Raycast(ray, out rayLength))
            {
                return ray.GetPoint(rayLength);
            }

            // Eğer ışın zeminle kesişmezse, y=0 düzleminde bir nokta döndür
            return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        }

        public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
        {
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }

        public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
        }
        #endregion

        #region Mesh Creation
        // Basit bir kare mesh oluşturma
        public static Mesh CreateQuadMesh(float width = 1f, float height = 1f)
        {
            Mesh mesh = new Mesh();

            Vector3[] vertices = new Vector3[4]
            {
                new Vector3(-width * 0.5f, -height * 0.5f, 0),
                new Vector3(width * 0.5f, -height * 0.5f, 0),
                new Vector3(-width * 0.5f, height * 0.5f, 0),
                new Vector3(width * 0.5f, height * 0.5f, 0)
            };

            int[] triangles = new int[6] { 0, 2, 1, 2, 3, 1 };

            Vector2[] uv = new Vector2[4]
            {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(0, 1),
                new Vector2(1, 1)
            };

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;
            mesh.RecalculateNormals();

            return mesh;
        }
        #endregion

        #region SFX Creation
        // Basit bir ses efekti çalma (AudioSource gerektirir)
        public static void PlaySound(AudioClip clip, Vector3 position, float volume = 1f, float pitch = 1f)
        {
            if (clip == null) return;

            GameObject soundObject = new GameObject("SoundEffect");
            soundObject.transform.position = position;

            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.pitch = pitch;
            audioSource.Play();

            // Ses bittiğinde nesneyi yok et
            GameObject.Destroy(soundObject, clip.length);
        }
        #endregion

        #region Debug Tools
        // Belirli bir süre boyunca debug çizgisi çizme
        public static void DrawDebugLine(Vector3 start, Vector3 end, Color color, float duration = 0f)
        {
            GameObject debugObject = new GameObject("DebugLine");
            DebugLineDrawer drawer = debugObject.AddComponent<DebugLineDrawer>();
            drawer.DrawLine(start, end, color, duration > 0 ? duration : 0.1f);
        }

        // Belirli bir süre boyunca debug küpü çizme
        public static void DrawDebugCube(Vector3 position, Vector3 size, Color color, float duration = 0f)
        {
            Vector3 halfSize = size * 0.5f;

            Vector3[] points = new Vector3[8]
            {
                position + new Vector3(-halfSize.x, -halfSize.y, -halfSize.z),
                position + new Vector3(halfSize.x, -halfSize.y, -halfSize.z),
                position + new Vector3(-halfSize.x, halfSize.y, -halfSize.z),
                position + new Vector3(halfSize.x, halfSize.y, -halfSize.z),
                position + new Vector3(-halfSize.x, -halfSize.y, halfSize.z),
                position + new Vector3(halfSize.x, -halfSize.y, halfSize.z),
                position + new Vector3(-halfSize.x, halfSize.y, halfSize.z),
                position + new Vector3(halfSize.x, halfSize.y, halfSize.z)
            };

            int[,] lines = new int[12, 2]
            {
                {0, 1}, {1, 3}, {3, 2}, {2, 0}, // Alt yüz
                {4, 5}, {5, 7}, {7, 6}, {6, 4}, // Üst yüz
                {0, 4}, {1, 5}, {2, 6}, {3, 7}  // Dikey kenarlar
            };

            for (int i = 0; i < 12; i++)
            {
                DrawDebugLine(points[lines[i, 0]], points[lines[i, 1]], color, duration);
            }
        }
        #endregion

        #region Object Cloning
        // Bir nesneyi klonlama
        public static GameObject CloneObject(GameObject original, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            return GameObject.Instantiate(original, position, rotation, parent);
        }

        // Bir nesneyi klonlama (basit sürüm)
        public static GameObject CloneObject(GameObject original)
        {
            return GameObject.Instantiate(original);
        }
        #endregion

        #region Random Utilities
        // Min ve max değerleri arasında rastgele float değeri
        public static float RandomFloat(float min, float max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        // Min ve max değerleri arasında rastgele int değeri
        public static int RandomInt(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        // 0-1 arasında rastgele float değeri
        public static float RandomFloat01()
        {
            return UnityEngine.Random.value;
        }

        // Rastgele bool değeri
        public static bool RandomBool()
        {
            return UnityEngine.Random.value > 0.5f;
        }

        // Bir diziden rastgele eleman seçme
        public static T RandomChoice<T>(T[] array)
        {
            if (array == null || array.Length == 0) return default(T);
            return array[RandomInt(0, array.Length)];
        }

        // Bir listeyi karıştırma
        public static List<T> ShuffleList<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = RandomInt(0, i + 1);
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
            return list;
        }

        // Bir diziyi karıştırma (yeni bir dizi döndürür)
        public static T[] ShuffleArray<T>(T[] array)
        {
            T[] shuffledArray = new T[array.Length];
            System.Array.Copy(array, shuffledArray, array.Length);

            for (int i = shuffledArray.Length - 1; i > 0; i--)
            {
                int j = RandomInt(0, i + 1);
                T temp = shuffledArray[i];
                shuffledArray[i] = shuffledArray[j];
                shuffledArray[j] = temp;
            }
            return shuffledArray;
        }
        #endregion

        #region General Definitions 
        public static NR_Direction GetNextDir(NR_Direction dir)
        {
            switch (dir)
            {
                default:
                case NR_Direction.Down: return NR_Direction.Left;
                case NR_Direction.Left: return NR_Direction.Up;
                case NR_Direction.Up: return NR_Direction.Right;
                case NR_Direction.Right: return NR_Direction.Down;
            }
        }
        public static int GetRotationAngle(NR_Direction dir)
        {
            switch (dir)
            {
                default:
                case NR_Direction.Down: return 0;
                case NR_Direction.Left: return 90;
                case NR_Direction.Up: return 180;
                case NR_Direction.Right: return 270;
            }
        }
        #endregion
    }
    public enum NR_Direction
    {
        Down = 0,
        Up = 1,
        Left = 2,
        Right = 3
    }
    public enum NR_Axis
    {
        X,
        Y,
        Z
    }
    // Debug çizgilerini çizmek için yardımcı sınıf
    public class DebugLineDrawer : MonoBehaviour
    {
        private LineRenderer lineRenderer;

        void Start()
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.widthMultiplier = 0.02f;
            lineRenderer.positionCount = 2;
        }

        public void DrawLine(Vector3 start, Vector3 end, Color color, float duration)
        {
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);

            Destroy(gameObject, duration);
        }
    }

    // Floating text animasyonu için sınıf
    public class FloatingTextAnimator : MonoBehaviour
    {
        private float duration;
        private float moveSpeed;
        private float timer;
        private Vector3 startScale;
        private Vector3 startPosition;
        private TextMesh textMesh;

        public void Initialize(float duration, float moveSpeed)
        {
            this.duration = duration;
            this.moveSpeed = moveSpeed;
            this.timer = 0f;
            this.startScale = transform.localScale;
            this.startPosition = transform.position;
            this.textMesh = GetComponent<TextMesh>();
        }

        void Update()
        {
            timer += Time.deltaTime;
            float progress = timer / duration;

            if (progress >= 1f)
            {
                Destroy(gameObject);
                return;
            }

            // Yukarıya doğru hareket
            Vector3 newPosition = startPosition + Vector3.up * (moveSpeed * timer);
            transform.position = newPosition;

            // Küçülme animasyonu (easing out)
            float scale = Mathf.Lerp(1f, 0f, progress * progress);
            transform.localScale = startScale * scale;

            // Alpha fade animasyonu
            if (textMesh != null)
            {
                Color color = textMesh.color;
                color.a = Mathf.Lerp(1f, 0f, progress);
                textMesh.color = color;
            }
        }
    }
}

// Tick System için ayrı namespace
namespace NoRyte.TickSystem
{
    using System.Collections.Generic;
    using UnityEngine;

    // Zamanlı tick sistemi
    public class TimedTickManager : MonoBehaviour
    {
        private static TimedTickManager instance;
        public static TimedTickManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject("TimedTickManager");
                    instance = go.AddComponent<TimedTickManager>();
                }
                return instance;
            }
        }

        private List<TimedTickable> timedTickables = new List<TimedTickable>();
        private List<TimedTickable> toAdd = new List<TimedTickable>();
        private List<TimedTickable> toRemove = new List<TimedTickable>();

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
            }
        }

        private void Update()
        {
            // Eklenecek öğeleri işle
            foreach (var tickable in toAdd)
            {
                if (!timedTickables.Contains(tickable))
                {
                    timedTickables.Add(tickable);
                }
            }
            toAdd.Clear();

            // Kaldırılacak öğeleri işle
            foreach (var tickable in toRemove)
            {
                timedTickables.Remove(tickable);
            }
            toRemove.Clear();

            // Tüm timed tickable öğeleri güncelle
            for (int i = timedTickables.Count - 1; i >= 0; i--)
            {
                if (timedTickables[i] != null && timedTickables[i].tickable != null)
                {
                    timedTickables[i].timer += Time.deltaTime;

                    if (timedTickables[i].timer >= timedTickables[i].tickInterval)
                    {
                        timedTickables[i].tickable.OnTick();
                        timedTickables[i].timer = 0f;
                    }
                }
                else
                {
                    timedTickables.RemoveAt(i);
                }
            }
        }

        public void RegisterTimedTickable(ITickable tickable, float tickInterval)
        {
            TimedTickable timedTickable = new TimedTickable
            {
                tickable = tickable,
                tickInterval = tickInterval,
                timer = 0f
            };

            if (!toAdd.Contains(timedTickable))
            {
                toAdd.Add(timedTickable);
            }
        }

        public void UnregisterTimedTickable(ITickable tickable)
        {
            TimedTickable toRemoveItem = null;

            foreach (var timedTickable in timedTickables)
            {
                if (timedTickable.tickable == tickable)
                {
                    toRemoveItem = timedTickable;
                    break;
                }
            }

            if (toRemoveItem != null && !toRemove.Contains(toRemoveItem))
            {
                toRemove.Add(toRemoveItem);
            }
        }

        // Tick aralığını değiştirme
        public void ChangeTickInterval(ITickable tickable, float newTickInterval)
        {
            foreach (var timedTickable in timedTickables)
            {
                if (timedTickable.tickable == tickable)
                {
                    timedTickable.tickInterval = newTickInterval;
                    break;
                }
            }
        }
    }

    // Zamanlı tick bilgilerini tutan sınıf
    public class TimedTickable
    {
        public ITickable tickable;
        public float tickInterval;
        public float timer;
    }

    // Basit bir tick sistemi (orijinal)
    public class TickManager : MonoBehaviour
    {
        private static TickManager instance;
        public static TickManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject("TickManager");
                    instance = go.AddComponent<TickManager>();
                }
                return instance;
            }
        }

        private List<ITickable> tickables = new List<ITickable>();
        private List<ITickable> toAdd = new List<ITickable>();
        private List<ITickable> toRemove = new List<ITickable>();

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
            }
        }

        private void Update()
        {
            // Eklenecek öğeleri işle
            foreach (var tickable in toAdd)
            {
                if (!tickables.Contains(tickable))
                {
                    tickables.Add(tickable);
                }
            }
            toAdd.Clear();

            // Kaldırılacak öğeleri işle
            foreach (var tickable in toRemove)
            {
                tickables.Remove(tickable);
            }
            toRemove.Clear();

            // Tüm tickable öğeleri güncelle
            for (int i = tickables.Count - 1; i >= 0; i--)
            {
                if (tickables[i] != null)
                {
                    tickables[i].OnTick();
                }
                else
                {
                    tickables.RemoveAt(i);
                }
            }
        }

        public void RegisterTickable(ITickable tickable)
        {
            if (!toAdd.Contains(tickable))
            {
                toAdd.Add(tickable);
            }
        }

        public void UnregisterTickable(ITickable tickable)
        {
            if (!toRemove.Contains(tickable))
            {
                toRemove.Add(tickable);
            }
        }
    }

    // Tick alabilen nesneler için arayüz
    public interface ITickable
    {
        void OnTick();
    }
}