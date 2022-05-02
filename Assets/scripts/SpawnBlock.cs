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
        GameObject newTetromino = Instantiate(blockType, new Vector3(spawnLocation.x, spawnLocation.y, 0), Quaternion.identity);
        newTetromino.transform.parent = GameObject.Find("Blocks").transform;
    }
}
