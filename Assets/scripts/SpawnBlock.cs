using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlock : MonoBehaviour {
    private bool isSpawned = false;
    public List<GameObject> nextBlockList = new List<GameObject>();
    public GameObject holdBlock;
    private float inputInterval = 0.2f;
    private bool isInInputInterval = false;
    private IEnumerator _inputIntervalCoroutine;

    [SerializeField]
    private List<GameObject> blockList;
    void Awake() {
        for (int i = 0; i < 5; i++) {
            Queue(i + 1);
        }
    }

    public void Queue(int i) {
        Debug.Log("queue");
        Vector3 nextBlockPos = GameObject.Find($"nextListLoc_{i}").transform.position;
        GameObject block = Instantiate(getRandomBlock(), nextBlockPos, Quaternion.identity);
        block.transform.parent = GameObject.Find("NextList").transform.Find("list").transform;
        nextBlockList.Add(block);
        block.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
    }

    void Update() {
        if (!isSpawned) {
            Spawn();
        }

        if (Input.GetKey(KeyCode.F)) {
            if (!isInInputInterval) {
                _inputIntervalCoroutine = InputIntervalCoroutine();
                StartCoroutine(_inputIntervalCoroutine);
                HandleHoldClock();
            }
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
        Debug.Log("spawn new block");
        isSpawned = true;
        GameObject block = nextBlockList[0];
        BlockController blockController = block.GetComponent<BlockController>();
        BlockTypeList blockType = blockController.blockType;
        Vector3 spawnPos = GameObject.Find("DefaultSpawnLocation").transform.Find(blockType.ToString()).transform.position;
        block.transform.position = new Vector3(spawnPos.x, spawnPos.y, 0);
        block.transform.localScale = new Vector3(1, 1, 1);
        block.transform.parent = GameObject.Find("Blocks").transform;
        Debug.Log(blockController);
        blockController.Spawn();

        nextBlockList.RemoveAt(0);

        Queue(5);

        for (int i = 0; i < 5; i++) {
            nextBlockList[i].transform.position = GameObject.Find($"nextListLoc_{i + 1}").transform.position;
        }
    }

    public void ClearNextList() {
        Debug.Log("clear next list");
        nextBlockList.RemoveAll((GameObject) => true);
        Transform listTransform = GameObject.Find("NextList").transform.Find("list").transform;
        foreach (Transform block in listTransform) {
            Destroy(block.gameObject);
        }
    }

    public void ClearHold () {
        holdBlock = null;
    }

    public void HandleHoldClock() {
        if (holdBlock != null) {
            ReturnHoldBlock();
        }
        else {
            HoldBlock();
        }
    }

    public void HoldBlock() {
        GameObject block = nextBlockList[0];
        block.transform.position = GameObject.Find("holdLoc").transform.position;
        holdBlock = block;
        nextBlockList.RemoveAt(0);
        Queue(5);
        for (int i = 0; i < 5; i++) {
            nextBlockList[i].transform.position = GameObject.Find($"nextListLoc_{i + 1}").transform.position;
        }
    }

    public void ReturnHoldBlock() {
        GameObject block = holdBlock;
        holdBlock = null;
        Debug.Log(holdBlock);
        nextBlockList.Insert(0, block);
        Debug.Log(nextBlockList[0]);
        Debug.Log(nextBlockList[5]);
        nextBlockList.RemoveAt(5);
        for (int i = 0; i < 5; i++) {
            nextBlockList[i].transform.position = GameObject.Find($"nextListLoc_{i + 1}").transform.position;
        }
    }

    IEnumerator InputIntervalCoroutine() {
        isInInputInterval = true;
        yield return new WaitForSeconds(inputInterval);
        isInInputInterval = false;
    }
}
