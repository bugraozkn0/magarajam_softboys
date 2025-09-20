# NoRyte Utils ve Tick System Dokümantasyonu

Bu dokümantasyon, NoRyte Utils ve Tick System bileşenlerinin nasıl kullanılacağına dair bilgiler içerir.

## NoRyte.Utils Namespace

`NoRyteUtils` sınıfı, Unity geliştirme sürecini hızlandırmak için çeşitli yardımcı metodlar içerir.

### Kullanım

```csharp
using NoRyte.Utils;
```

### Metodlar

#### Create World Text
Ekranda 3D metin oluşturma

```csharp
TextMesh textMesh = NoRyteUtils.CreateWorldText(
    "Merhaba Dünya",
    parent: null,
    localPosition: new Vector3(0, 2, 0),
    fontSize: 40,
    color: Color.red
);
```

Varsayılan parametreler:
- `parent`: null (ebeveyn nesne)
- `localPosition`: Vector3.zero (konum)
- `fontSize`: 40 (yazı boyutu)
- `color`: Color.white (renk)
- `textAnchor`: TextAnchor.UpperLeft (hizalama)
- `textAlignment`: TextAlignment.Center (hizalama)
- `sortingOrder`: 0 (derinlik sırası)

#### Show Debug Message
Ekranda belirli bir süre boyunca debug mesajı gösterme

```csharp
NoRyteUtils.ShowDebugMessage("Hata oluştu!", 3f);
```

#### Mouse Position
Fare pozisyonunu dünya koordinatlarında alma

```csharp
Vector3 mousePos = NoRyteUtils.GetMouseWorldPosition();
```

#### Mesh Creation
Basit mesh oluşturma

```csharp
Mesh quadMesh = NoRyteUtils.CreateQuadMesh(2f, 2f);
```

#### SFX Creation
Ses efekti çalma

```csharp
NoRyteUtils.PlaySound(clip, position, volume: 0.8f);
```

#### Debug Tools
Gelişmiş debug araçları

```csharp
// Debug çizgisi çizme (5 saniye boyunca görünür)
NoRyteUtils.DrawDebugLine(start, end, Color.blue, 5f);

// Debug küpü çizme (5 saniye boyunca görünür)
NoRyteUtils.DrawDebugCube(position, size, Color.green, 5f);

// Anlık debug çizgisi çizme (sadece bir frame)
NoRyteUtils.DrawDebugLine(start, end, Color.red);
```

#### Object Cloning
Nesne klonlama

```csharp
GameObject clone = NoRyteUtils.CloneObject(original, position, rotation);
GameObject clone = NoRyteUtils.CloneObject(original); // Basit klonlama
```

#### Random Utilities
Rastgele değerler ve liste karıştırma

```csharp
// Rastgele değerler
float rFloat = NoRyteUtils.RandomFloat(0f, 10f);
int rInt = NoRyteUtils.RandomInt(1, 5);
bool rBool = NoRyteUtils.RandomBool();
float rFloat01 = NoRyteUtils.RandomFloat01(); // 0-1 arasında

// Liste karıştırma
List<int> numbers = new List<int> {1, 2, 3, 4, 5};
List<int> shuffled = NoRyteUtils.ShuffleList(numbers);

// Dizi karıştırma
int[] array = {1, 2, 3, 4, 5};
int[] shuffledArray = NoRyteUtils.ShuffleArray(array);

// Diziden rastgele seçim
string[] options = {"A", "B", "C", "D"};
string choice = NoRyteUtils.RandomChoice(options);
```

## NoRyte.TickSystem Namespace

Her frame veya belirli aralıklarla çalışan tick sistemleri

### Kullanım

```csharp
using NoRyte.TickSystem;
```

### Basit Tick Sistemi

Her frame çalışan tick sistemi:

```csharp
public class ExampleTickable : MonoBehaviour, ITickable
{
    private void Start()
    {
        TickManager.Instance.RegisterTickable(this);
    }
    
    public void OnTick()
    {
        // Her frame çalışacak kod
        Debug.Log("Tick!");
    }
    
    private void OnDestroy()
    {
        TickManager.Instance.UnregisterTickable(this);
    }
}
```

### Zaman Tabanlı Tick Sistemi

Belirli aralıklarla çalışan tick sistemi:

```csharp
public class ExampleTimedTickable : MonoBehaviour, ITimedTickable
{
    private void Start()
    {
        // Her 2 saniyede bir çalışacak
        TimedTickManager.Instance.RegisterTimedTickable(this, 2f);
    }
    
    public void OnTimedTick()
    {
        // Her 2 saniyede bir çalışacak kod
        Debug.Log("Timed Tick!");
    }
    
    private void OnDestroy()
    {
        TimedTickManager.Instance.UnregisterTimedTickable(this);
    }
}
```

### Arayüzler

#### ITickable
Her frame çağrılması gereken nesneler için:

```csharp
public interface ITickable
{
    void OnTick();
}
```

#### ITimedTickable
Belirli aralıklarla çağrılması gereken nesneler için:

```csharp
public interface ITimedTickable
{
    void OnTimedTick();
}
```

## Kurulum

1. `NoRyteUtils.cs` dosyasını projenize ekleyin
2. Kullanmak istediğiniz sınıflarda uygun `using` ifadelerini ekleyin:
   - `using NoRyte.Utils;` (yardımcı metodlar için)
   - `using NoRyte.TickSystem;` (tick sistemleri için)

## Notlar

- Tick sistemleri otomatik olarak singleton instance oluşturur
- Kayıtlı nesneler yok edildiğinde otomatik olarak listeden çıkarılır
- Zaman tabanlı tick sistemi, nesnelerin frame başına düşen yükünü azaltmak için uygundur
- Debug çizgileri ve küpler için kullanılan LineRenderer nesneleri, belirtilen süre sonunda otomatik olarak yok edilir
- Ses efektleri için oluşturulan nesneler, ses çalma süresi sonunda otomatik olarak yok edilir