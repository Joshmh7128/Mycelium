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
    [SerializeField] float age, longevity, size, maxSize, growthSpeed, dropRadius, dropAmount; 

    // our quasi update runs at 100 times per Time.timeScale
    public override void QuasiUpdate()
    {
        
    }

    void GrowStep()
    {
        // increase our age
        age += QuasiDeltaTime * growthSpeed;
    }

    void GrowCheck()
    {

    }

}
