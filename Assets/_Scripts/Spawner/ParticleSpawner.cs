using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour {

    [SerializeField] private float delay = 0.5f;
    [SerializeField] private GameObject particlePrefab;

    // Spawners will keep track of where the target is - avoids recalculating constantly 
    private Vector3 targetTransform;
    [SerializeField] private Spell magic;
    public Transform originTransform;

    private float timeSinceLastSpawn;

    private void Update()
    {
        targetTransform = magic.currentAimAt;
        /*
        if (Time.time - timeSinceLastSpawn >= delay)
        {
            SpawnParticleFromPool();
        }
        */
    }

    public void SpawnParticleFromPool()
    {
        // Resets the spawn timer.
        timeSinceLastSpawn = Time.time;

        // Save missile instance temporarily, and set its target to the player 
        Vector3 position = originTransform.position;

        GameObject gameObject = ParticlePool.Instance.GetFromPool();
        gameObject.transform.position = position;
        Particle particle = gameObject.GetComponent<Particle>();
        particle.TargetTransform = targetTransform;
    }

    public static Vector3 RandomPointInBox(Vector3 center, Vector3 size)
    {
        return center + new Vector3
            (
        (UnityEngine.Random.value - 0.5f) * size.x,
        (UnityEngine.Random.value - 0.5f) * size.y,
        (UnityEngine.Random.value - 0.5f) * size.z
            );
    }

    public void DeleteMsg(EnemyHealth target)
    {
        StartCoroutine(HideMessage(target));
    }

    public IEnumerator HideMessage(EnemyHealth target)
    {
        yield return new WaitForSeconds(1.5f);
        Debug.Log("back!");
        if (target)
            target.hitContainer.active = false;
    }
}
