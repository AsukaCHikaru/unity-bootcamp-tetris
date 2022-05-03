using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlock : MonoBehaviour {
    private bool isSpawned = false;
    public List<GameObject> nextBlockList = new List<GameObject>();

    [SerializeField]
    private List<GameObject> blockList;
    void Awake() {
        for (int i = 0; i < 5; i++) {
            Queue(i + 1);
        }
    }

    void Queue(int i) {
        Vector3 nextBlockPos = GameObject.Find($"nextListLoc_{i}").transform.position;
        GameObject block = Instantiate(getRandomBlock(), nextBlockPos, Quaternion.identity);
        nextBlockList.Add(block);
        block.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
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
        GameObject block = nextBlockList[0];
        BlockController blockController = block.GetComponent<BlockController>();
        BlockTypeList blockType = blockController.blockType;
        Vector3 spawnPos = GameObject.Find("DefaultSpawnLocation").transform.Find(blockType.ToString()).transform.position;
        block.transform.position = new Vector3(spawnPos.x, spawnPos.y, 0);
        block.transform.localScale = new Vector3(1, 1, 1);
        block.transform.parent = GameObject.Find("Blocks").transform;
        blockController.Spawn();
        nextBlockList.RemoveAt(0);
        Queue(5);
        for (int i = 0; i < 5; i++) {
            nextBlockList[i].transform.position = GameObject.Find($"nextListLoc_{i + 1}").transform.position;
        }
    }
}
