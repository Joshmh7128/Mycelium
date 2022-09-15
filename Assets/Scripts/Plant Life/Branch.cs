using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch : MonoBehaviour 
{

    public Branch originBranch;
    public List<Branch> subBranches;
    public bool roomToGrow;
    public float targetY; // set this to the target Y we want this root to go to

}

