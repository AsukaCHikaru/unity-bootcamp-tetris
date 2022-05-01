using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tetrominoes;

public enum BlockTypeList {
    I_block,
    S_block,
    Z_block,
    L_block,
    J_block,
    square_block,
    T_block,
};

public class BlockController : MonoBehaviour {
    PositionMap positionMap = new PositionMap();
    int[,,] map;

    int rotateIndex = 0;
    private float fallInterval = 0.5f;
    public float moveSpeed = 5.0f;
    private bool isBottomOccupied = false;
    private bool isDescendCalled = false;
    private float inputInterval = 0.2f;
    private bool isInInputInterval = false;
    private IEnumerator blockDescendCoroutine;
    private IEnumerator _inputIntervalCoroutine;

    [SerializeField]
    private Vector3 parentLoc;

    [SerializeField]
    private List<GameObject> childBlockList;

    [SerializeField]
    private BlockTypeList blockType;

    SpawnBlock spawnBlock;

    void Start() {
        spawnBlock = GameObject.Find("GameController").GetComponent<SpawnBlock>();
        map = positionMap.GetMap(blockType);
    }

    void FixedUpdate() {
        GetInput();
        CheckCanDescend();

        if (!isDescendCalled) {
            DescendBlock();
        }

        if (isBottomOccupied) {
            spawnBlock.resetIsSpawned();
            Destroy(this);
        }
    }

    void GetInput() {
        if (Input.GetKey(KeyCode.A)) {
            bool _isLeftOccupied = CheckLeftCanMove();
            if (!isInInputInterval && !_isLeftOccupied && !isBottomOccupied) {
                _inputIntervalCoroutine = InputIntervalCoroutine();
                StartCoroutine(_inputIntervalCoroutine);
                transform.position = new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z);
                Register();
            }
        }

        if (Input.GetKey(KeyCode.D)) {
            bool _isRightOccupied = CheckRightCanMove();
            if (!isInInputInterval && !_isRightOccupied && !isBottomOccupied) {
                _inputIntervalCoroutine = InputIntervalCoroutine();
                StartCoroutine(_inputIntervalCoroutine);
                transform.position = new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z);
                Register();
            }
        }

        if (Input.GetKey(KeyCode.W)) {
            if (!isInInputInterval && !isBottomOccupied) {
                _inputIntervalCoroutine = InputIntervalCoroutine();
                StartCoroutine(_inputIntervalCoroutine);
                Rotate();
                Register();
            }
        }
    }

    void Rotate() {
        if (rotateIndex == (map.Length / 8) - 1) {
            rotateIndex = 0;
        }
        else {
            rotateIndex++;
        }
        for (var i = 0; i < 4; i++) {
            childBlockList[i].transform.localPosition = new Vector3(map[rotateIndex, i, 0], map[rotateIndex, i, 1], 0);
        }
    }

    void DescendBlock() {
        blockDescendCoroutine = DescendBlockCoroutine();
        StartCoroutine(blockDescendCoroutine);
        isDescendCalled = true;
    }

    IEnumerator DescendBlockCoroutine() {
        while (!isBottomOccupied) {
            transform.position = new Vector3(transform.position.x, Mathf.Ceil(transform.position.y - 1) - 0.5f, transform.position.z);
            Register();
            yield return new WaitForSeconds(fallInterval);
        }
    }

    private bool CheckRightCanMove() {
        bool isRightOccupied = false;
        foreach (Transform block in transform) {
            SingleBlock singleBlock = block.GetComponent<SingleBlock>();
            GameObject rightGoalBlock = GameObject.Find($"{singleBlock.x + 1},{singleBlock.y}");
            if (singleBlock.x >= 4 || (rightGoalBlock != null && rightGoalBlock.transform.parent != transform)) {
                isRightOccupied = true;
            }
        }
        return isRightOccupied;
    }

    private bool CheckLeftCanMove() {
        bool isLeftOccupied = false;
        foreach (Transform block in transform) {
            SingleBlock singleBlock = block.GetComponent<SingleBlock>();
            GameObject leftGoalBlock = GameObject.Find($"{singleBlock.x - 1},{singleBlock.y}");
            if (singleBlock.x <= -5 || (leftGoalBlock != null && leftGoalBlock.transform.parent != transform)) {
                isLeftOccupied = true;
            }
        }
        return isLeftOccupied;
    }

    void CheckCanDescend() {
        foreach (Transform block in transform) {
            SingleBlock singleBlock = block.GetComponent<SingleBlock>();
            GameObject bottomGoalBlock = GameObject.Find($"{singleBlock.x},{singleBlock.y - 1}");

            if (singleBlock.y == -9) {
                isBottomOccupied = true;
            }

            if (bottomGoalBlock != null && bottomGoalBlock.transform.parent != transform) {
                isBottomOccupied = true;
            }
        }
    }

    void Register() {
        parentLoc = new Vector3(transform.position.x - 0.5f, transform.position.y - 0.5f, 0);
        foreach (Transform block in transform) {
            SingleBlock singleBlock = block.GetComponent<SingleBlock>();
            Vector3 blockLoc = new Vector3(parentLoc.x + block.localPosition.x, parentLoc.y + block.localPosition.y, 0);
            singleBlock.RegisterBlockPos(blockLoc);
        }
        if (isBottomOccupied) {
            StopCoroutine(blockDescendCoroutine);
            spawnBlock.resetIsSpawned();
        }
    }

    IEnumerator InputIntervalCoroutine() {
        isInInputInterval = true;
        yield return new WaitForSeconds(inputInterval);
        isInInputInterval = false;
    }
}