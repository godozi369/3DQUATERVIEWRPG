using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDataLoader : MonoBehaviour
{
    public static EnemyDataLoader instance { get; private set; }
    public EnemyDataList data { get; private set; }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        TextAsset json = Resources.Load<TextAsset>("Data/enemy_data");
        data = JsonUtility.FromJson<EnemyDataList>(json.text);
    }
}
