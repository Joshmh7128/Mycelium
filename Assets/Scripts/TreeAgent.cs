using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeAgent : MonoBehaviour
{
    /// 
    /// tree will
    /// 1. grow up in scale for its entire age
    /// 2. drop X seeds around it in Y radius
    /// 3. die at one point
    /// 

    // our tree stats
    [SerializeField] Transform cosmeticTree; // the tree which we display growing via scale, then falling over
    [SerializeField] float age, maxAge, size, maxSize, growthSpeed, dropRadius, dropAmount, dropAge;
    bool hasDropped, hasDied; // have we dropped our seeds?
    [SerializeField] GameObject treePrefab; // our tree prefab
    [SerializeField] GameObject deadTreePrefab; // our dead tree prefab

    void Awake()
    {
        age = 0;
        cosmeticTree.localScale = Vector3.zero;
        size = 0;

        // build our tree values
        RandomizeValues();
    }

    // our quasi update runs at 100 times per Time.timeScale
    void Update()
    {
        GrowStep();
    }

    void RandomizeValues()
    {
        // decide our values
        maxAge = Random.Range(5f, 8f);
        dropAge = maxAge * 0.9f; // drop new seeds when we reach 90% of our age
        maxSize = Random.Range(3f, 8f);
        growthSpeed = Random.Range(0.1f, 0.25f);
        dropRadius = Random.Range(6f, 12f);
        dropAmount = Random.Range(2, 5);
    }

    void GrowStep()
    {
        // increase our age
        age += Time.deltaTime * growthSpeed;
        // check our age and die if we need to
        if (age > maxAge) { Die(); }
        // check our age and if we are old enough to drop seeds, do it once
        if (age > dropAge && !hasDropped) { DropSeeds(); }
        // set the scale of our cosmetic tree to age/maxAge multiplied by our maxSize
        if (size < maxSize)
        {
            size = (age / maxAge) * maxSize;
        }

        // set the scale of the tree to the current size
        cosmeticTree.localScale = new Vector3(size, size, size);
    }

    // what happens when we drop?
    void DropSeeds()
    {
        // set our hasDropped to true
        hasDropped = true;
    }

    // what happens when we die?
    void Die()
    {
        if (!hasDied)
        {
            Debug.Log("dropping");
            // put a dead tree down
            GameObject deadTree = Instantiate(deadTreePrefab, transform.position, deadTreePrefab.transform.rotation);
            deadTree.transform.localScale = transform.localScale;
            // spawn in dropAmount of our trees in a dropRadius
            for (int i = 0; i < dropAmount; i++)
            {
                // choose a nearby position
                Vector3 spawnRadius = new Vector3(Random.Range(-dropRadius, dropRadius), 0f, Random.Range(-dropRadius, dropRadius));
                // instantiate a tree there
                GameObject newTree = Instantiate(treePrefab, transform.position + spawnRadius, Quaternion.identity, null);
                newTree.GetComponent<TreeAgent>().cosmeticTree.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            }

            // then destroy this object
            Destroy(gameObject);
            // has died
            hasDied = true;
        }
    }
}
