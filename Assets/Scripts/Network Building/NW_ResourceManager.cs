using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NW_ResourceManager : MonoBehaviour
{
    /// for storing all of our resource objects
    /// 

    // setup instance
    public static NW_ResourceManager instance;
    private void Awake() => instance = this;
  
    // list of all resource gameobjects
    public List<GameObject> resourceObjects = new List<GameObject>(); // all resources are placed in here


}
