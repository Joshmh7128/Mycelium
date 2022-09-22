using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NW_Generator : MonoBehaviour
{
    [SerializeField] GameObject resourceSpawner; // the resource spawner prefab
    [SerializeField] float MapRadius, MapComplexity; // the radius of the map

    private void Start() => Generate();

    void Generate()
    {
        // spawn our resource spawners in a radius
        for (int i = 0; i < MapComplexity; i++)
        {
            Vector3 rad = new Vector3(Random.Range(-MapRadius, MapRadius), 0f, Random.Range(-MapRadius, MapRadius));

            Instantiate(resourceSpawner, transform.position + rad, Quaternion.identity);
        }
    }
}
