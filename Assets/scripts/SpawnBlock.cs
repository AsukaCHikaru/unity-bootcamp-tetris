using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlock : MonoBehaviour
{
    private bool isSpawned = false;

    [SerializeField]
    private List<GameObject> blockList;
    void Start() {

    }

    void Update() {
        if (!isSpawned) {
            Spawn();
        }
    }

    public void resetIsSpawned() {
        Debug.Log("reset is spawned");
        isSpawned = false;
    }

    GameObject getRandomBlock() {
        int rng = Random.Range(0, blockList.Count);
        return blockList[rng];
    }

    void Spawn() {
        isSpawned = true;
        GameObject blockType = getRandomBlock();
        Vector3 spawnLocation = GameObject.Find("DefaultSpawnLocation").transform.Find(blockType.name).transform.position;
        Instantiate(blockType, spawnLocation, Quaternion.Euler(0, 0, 0));
    }
}
