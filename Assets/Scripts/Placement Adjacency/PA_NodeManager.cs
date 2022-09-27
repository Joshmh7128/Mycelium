using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_NodeManager : MonoBehaviour
{
    public static PA_NodeManager instance;
    private void Awake() {
        if (instance != null) {
            Debug.Log("Warning! More than one instance of PA_NodeManager found!");
            return;
        } else instance = this;
    }

    public GameObject plantPrefab;

    public List<PA_AdjacencyNode> plantNodes = new List<PA_AdjacencyNode>();
    public bool plantDisplayActive;
   
    public List<PA_AdjacencyNode> fungusNodes = new List<PA_AdjacencyNode>();
    public bool fungusDisplayActive;

    public List<PA_AdjacencyNode> adjacencyNodes = new List<PA_AdjacencyNode>(); // for ALL nodes

    [SerializeField] float spawnRadius;

    public float spawnTime;
    [SerializeField] float spawnInterval;
    [SerializeField] Vector2 spawnIntervalRange;

    private void Start()
    {
        plantDisplayActive = false;
        spawnInterval = Random.Range(spawnIntervalRange.x, spawnIntervalRange.y);
        StartCoroutine(WaitToSpawn());
    }

    IEnumerator WaitToSpawn() {
        if (spawnTime < spawnInterval) {
            yield return new WaitForFixedUpdate();
            spawnTime += Time.fixedDeltaTime;
        } else {
            yield return new WaitForFixedUpdate();
            PA_AdjacencyNode node = SpawnPlantNode();
            plantNodes.Add(node);
            adjacencyNodes.Add(node);
            spawnTime = 0;
        }
        StartCoroutine(WaitToSpawn());
    }

    PA_AdjacencyNode SpawnPlantNode() {
        PA_AdjacencyNode newNode = Instantiate(plantPrefab, 
            new Vector3(Random.Range(-spawnRadius, spawnRadius), 0, Random.Range(-spawnRadius, spawnRadius)), 
            Quaternion.identity, transform).GetComponent<PA_AdjacencyNode>();
        // coroutine needs to be run on a delay as a buffer for instantiation to complete
        StartCoroutine(DelayToggle(newNode));
        return newNode;
    }

    IEnumerator DelayToggle(PA_AdjacencyNode node) {
        yield return new WaitForEndOfFrame();
        if (plantDisplayActive) node.ToggleRadiusDisplay(true);
    }

    #region // button functions
    public void TogglePlantRadiusDisplay() {
        plantDisplayActive = !plantDisplayActive;
        foreach (PA_AdjacencyNode node in plantNodes) {
            if (node.adjRadActive != plantDisplayActive)
                node.ToggleRadiusDisplay(plantDisplayActive);
        }
    } 
    public void ToggleFungusRadiusDisplay() {
        fungusDisplayActive = !fungusDisplayActive;
        foreach (PA_AdjacencyNode node in fungusNodes) {
            if (node.adjRadActive != fungusDisplayActive)
                node.ToggleRadiusDisplay(fungusDisplayActive);
        }
    }
    #endregion
}
