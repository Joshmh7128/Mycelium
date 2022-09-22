using System.Collections;
using System.Collections.Generic;
using System.Resources;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class NW_ResourceSpawner : MonoBehaviour
{
    // this thing spawns resources on the ground
    [SerializeField] GameObject resourcePrefab; // the prefab we are spawning for collection
    [HideInInspector] public NW_ResourceManager resourceManager; // the resource manager of the scene
    float spawnTime; // how long between spawns?
    [SerializeField] float spawnTimeMin, spawnTimeMax;
    [SerializeField] float spawnRadius;
    [SerializeField] float resourceCapacity; // how many resources we can have
    

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

    // resource creation coroutine
    private IEnumerator CreateResource()
    {
        yield return new WaitForSeconds(spawnTime);
        // do the thing!
        SpawnResource();
        // restart!
        StartCoroutine(CreateResource());
    }

    // create a new resource
    private void SpawnResource()
    {
        // can we spawn?
        if (transform.childCount < resourceCapacity)
        {
            // set our spawn pos
            Vector3 spawnPos = new Vector3((Random.Range(-spawnRadius, spawnRadius)), 0, (Random.Range(-spawnRadius, spawnRadius)));
            // Instantiate resource within radius
            GameObject resource = Instantiate(resourcePrefab, transform.position + spawnPos, Quaternion.identity, transform);
            // add it to the manager
            resourceManager.resourceObjects.Add(resource);
        }
    }
}
