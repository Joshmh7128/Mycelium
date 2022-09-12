using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeAgent : QuasiBehaviour
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

    void QuasiStart()
    {
        // build our tree values
        RandomizeValues();
    }

    // our quasi update runs at 100 times per Time.timeScale
    void QuasiUpdate()
    {
        GrowStep();
    }

    void RandomizeValues()
    {
        // decide our values
        maxAge = Random.Range(30f, 80f);
        dropAge = maxAge * 0.9f; // drop new seeds when we reach 90% of our age
        maxSize = Random.Range(3f, 8f);
        growthSpeed = Random.Range(1, 5);
        dropRadius = Random.Range(0.1f, 3f);
        dropAmount = Random.Range(0, 4);
    }

    void GrowStep()
    {
        Debug.Log("growing");
        // increase our age
        age += QuasiDeltaTime * growthSpeed;
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
            // spawn in dropAmount of our trees in a dropRadius
            for (int i = 0; i < dropAmount; i++)
            {
                // choose a nearby position
                Vector3 spawnRadius = new Vector3(Random.Range(-dropRadius, dropRadius), 0f, Random.Range(-dropRadius, dropRadius));
                // instantiate a tree there
                Instantiate(treePrefab, transform.position + spawnRadius, Quaternion.identity, null);
            }

            // then destroy this object
            Destroy(gameObject);
            // has died
            hasDied = true;
        }
    }
}
