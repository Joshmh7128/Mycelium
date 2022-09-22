using System.Collections;
using System.Collections.Generic;
using System.Resources;
using System.Xml.Serialization;
using UnityEngine;

public class NW_ResourceSpawner : MonoBehaviour
{
    // this thing spawns resources on the ground
    [SerializeField] GameObject resourcePrefab; // the prefab we are spawning for collection
    [SerializeField] NW_ResourceManager resourceManager; // the resource manager of the scene
    float spawnTime; // how long between spawns?
    [SerializeField] float spawnTimeMin, spawnTimeMax;

    private void Start()
    {
        SetupSpawner();
    }

    void SetupSpawner()
    {
        // set instance
        resourceManager = NW_ResourceManager.instance;
        // calculate spawn time
        spawnTime = Random.Range(spawnTimeMin, spawnTimeMax);
        // start our coroutine
        StartCoroutine(CreateResource());
    }

    private IEnumerator CreateResource()
    {
        yield return new WaitForSeconds(spawnTime);
        // do the thing!

        // restart!
        StartCoroutine(CreateResource());
    }

    private void SpawnResource()
    {

    }
}
