using UnityEngine;

public class LuggageSpawner : MonoBehaviour
{
    public static LuggageSpawner Instance;

    [Header("生成設定")]
    [Tooltip("生成する荷物のプレハブリスト")]
    public GameObject[] luggagePrefabs;
    
    [Tooltip("一度に生成する数")]
    public int spawnCount;

    [Tooltip("生成範囲（このTransformの位置を中心にランダムな範囲）")]
    public Vector2 spawnAreaSize = new Vector2(4f, 2f);

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //初期生成.
        SpawnLuggages(spawnCount);
    }

    public void SpawnLuggages(int amount)
    {
        if (luggagePrefabs == null || luggagePrefabs.Length == 0)
        {
            Debug.LogWarning("荷物プレハブが設定されていません。Inspectorから設定してください。");
            return;
        }

        for (int i = 0; i < amount; i++)
        {
            GameObject prefab = luggagePrefabs[Random.Range(0, luggagePrefabs.Length)];
            
            // ランダムな位置を計算
            float randomX = Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f);
            float randomY = Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f);
            Vector3 spawnPos = transform.position + new Vector3(randomX, randomY, 0f);

            Instantiate(prefab, spawnPos, Quaternion.identity);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawCube(transform.position, new Vector3(spawnAreaSize.x, spawnAreaSize.y, 0f));
    }
}
