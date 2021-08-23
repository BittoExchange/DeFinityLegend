using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldManager : MonoBehaviour
{
    [Header("World")]
    public Transform[] worlds;
    public GameObject worldButtonPrefab;
    public Transform worldButtonHolder;

    readonly List<GameObject> spawnedButtons = new List<GameObject>();

    [Header("Cloud")]
    public int cloudCount;
    public float minSize;
    public float maxSize;
    public float moveSpeed;
    public SpriteRenderer map;
    public GameObject cloudPrefab;
    public Transform cloudHolder;

    readonly List<GameObject> spawnedClouds = new List<GameObject>();

    void Awake()
    {
        SpawnWorldButtons();
        SpawnClouds();
    }

    void LateUpdate()
    {
        WorldButtonsUpdate();
        CloudsUpdate();
    }

    void SpawnWorldButtons()
    {
        for (int i = 0; i < worlds.Length; i++)
        {
            GameObject button = Instantiate(worldButtonPrefab, worldButtonHolder);
            button.GetComponentInChildren<Text>().text = worlds[i].name;
            spawnedButtons.Add(button);
        }
    }

    void SpawnClouds()
    {
        for (int i = 0; i < cloudCount; i++)
        {
            float randomPosX = Random.Range(-map.bounds.extents.x, map.bounds.extents.x);
            float randomPosY = Random.Range(-map.bounds.extents.y, map.bounds.extents.y);

            float randomSize = Random.Range(minSize, maxSize);

            GameObject cloud = Instantiate(cloudPrefab, cloudHolder);
            cloud.transform.position = new Vector3(randomPosX, randomPosY, 0);
            cloud.transform.localScale = new Vector3(randomSize, randomSize, 1);

            spawnedClouds.Add(cloud);
        }
    }

    void WorldButtonsUpdate()
    {
        for (int i = 0; i < spawnedButtons.Count; i++)
        {
            spawnedButtons[i].transform.position = CameraMovement.Instance.mainCamera.WorldToScreenPoint(worlds[i].transform.position);
        }
    }

    void CloudsUpdate()
    {
        for (int i = 0; i < spawnedClouds.Count; i++)
        {
            spawnedClouds[i].transform.position += moveSpeed * Time.deltaTime * Vector3.right;

            if (spawnedClouds[i].transform.position.x > map.bounds.extents.x + 3)
            {
                spawnedClouds[i].transform.position = new Vector3(-map.bounds.extents.x - 3, spawnedClouds[i].transform.position.y, 0);
            }
        }
    }
}
