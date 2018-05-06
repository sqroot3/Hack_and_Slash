using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePool : MonoBehaviour {

    [SerializeField] private GameObject particlePrefab;
    private Queue<GameObject> availableObjects = new Queue<GameObject>();

    public static ParticlePool Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        GrowPool();
    }

    private void GrowPool()
    {
        for (int i = 0; i < 10; i++)
        {
            var instancesToAdd = Instantiate(particlePrefab) as GameObject;
            instancesToAdd.transform.SetParent(transform);
            AddToPool(instancesToAdd);
        }
    }

    public GameObject GetFromPool()
    {
        if (availableObjects.Count == 0)
            GrowPool();

        var Instance = availableObjects.Dequeue();
        Instance.SetActive(true);
        return Instance;
    }

    public void AddToPool(GameObject Instance)
    {
        Instance.SetActive(false);
        availableObjects.Enqueue(Instance);
    }
}
