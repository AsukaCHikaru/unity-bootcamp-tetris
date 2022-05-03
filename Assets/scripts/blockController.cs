using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    TetrominoConstants tetrominoConsts = new TetrominoConstants();
    int[,,] map;

    int rotateIndex = 0;
    private float fallInterval = 0.5f;
    public float moveSpeed = 5.0f;
    private bool isBottomOccupied = false;
    private bool isLocked = false;
    private bool isDescendCalled = false;
    private float inputInterval = 0.2f;
    private bool isInInputInterval = false;
    private IEnumerator blockDescendCoroutine;
    private IEnumerator _inputIntervalCoroutine;
    int startInputInt = 0;
    bool canSpeedDescend = false;

    [SerializeField]
    private Vector3 parentLoc;

    [SerializeField]
    private List<GameObject> childBlockList;

    [SerializeField]
    private BlockTypeList blockType;

    SpawnBlock spawnBlock;
    ScoreController scoreController;

    void Start() {
        GameObject gameController = GameObject.Find("GameController");
        spawnBlock = gameController.GetComponent<SpawnBlock>();
        scoreController = gameController.GetComponent<ScoreController>();

        map = positionMap.GetMap(blockType);
        CalculatePossibleBottomPos();
    }

    private void Update() {
        if (startInputInt < 60) {
            startInputInt++;

        }
        else { canSpeedDescend = true; }
    }

    void FixedUpdate() {
        GetInput();

        if (!isDescendCalled) {
            DescendBlock();
        }
    }

    void GetInput() {
        if (Input.GetKey(KeyCode.A)) {
            bool _isLeftOccupied = CheckLeftCanMove();
            if (!isInInputInterval && !_isLeftOccupied && !isLocked) {
                _inputIntervalCoroutine = InputIntervalCoroutine();
                StartCoroutine(_inputIntervalCoroutine);
                transform.position = new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z);
                Register();
                CalculatePossibleBottomPos();
            }
        }

        if (Input.GetKey(KeyCode.D)) {
            bool _isRightOccupied = CheckRightCanMove();
            if (!isInInputInterval && !_isRightOccupied && !isLocked) {
                _inputIntervalCoroutine = InputIntervalCoroutine();
                StartCoroutine(_inputIntervalCoroutine);
                transform.position = new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z);
                Register();
                CalculatePossibleBottomPos();
            }
        }

        if (Input.GetKey(KeyCode.W)) {
            if (!isInInputInterval && !isBottomOccupied) {
                _inputIntervalCoroutine = InputIntervalCoroutine();
                StartCoroutine(_inputIntervalCoroutine);
                Rotate();
                CalculatePossibleBottomPos();
            }
        }

        if (Input.GetKey(KeyCode.S)) {
            if (!isInInputInterval && !isBottomOccupied && canSpeedDescend) {
                _inputIntervalCoroutine = InputIntervalCoroutine();
                StartCoroutine(_inputIntervalCoroutine);
                SpeedDescendBlock();
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

        List<Vector3> newPosList = new List<Vector3>();
        int newPosOverRightWallLevel = 0;
        int newPosOverLeftWallLevel = 0;
        int newPosOverBottomWallLevel = 0;

        for (var i = 0; i < 4; i++) {
            Vector3 localNewPos = new Vector3(map[rotateIndex, i, 0], map[rotateIndex, i, 1], 0);
            Vector3 newPos = new Vector3(parentLoc.x + localNewPos.x, parentLoc.y + localNewPos.y, 0);
            GameObject blockInRight = GameObject.Find($"{newPos.x + 1},{newPos.y}");
            GameObject blockInLeft = GameObject.Find($"{newPos.x - 1},{newPos.y}");
            GameObject blockInBottom = GameObject.Find($"{newPos.x},{newPos.y - 1}");

            if (newPos.x >= tetrominoConsts.RIGHT_WALL_X || (blockInRight != null && blockInRight.transform.parent != transform)) {
                newPosOverRightWallLevel++;
            }
            if (newPos.x <= tetrominoConsts.LEFT_WALL_X || (blockInLeft != null && blockInLeft.transform.parent != transform)) {
                newPosOverLeftWallLevel++;
            }
            if (newPos.y <= tetrominoConsts.BOTTOM_WALL_Y || (blockInBottom != null && blockInBottom.transform.parent != transform)) {
                newPosOverBottomWallLevel++;
            }

            newPosList.Add(localNewPos);
        }

        Vector3 newParentPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        if (newPosOverRightWallLevel > 0) {
            newParentPos = new Vector3(transform.position.x - newPosOverRightWallLevel, transform.position.y, transform.position.z);
        }
        if (newPosOverLeftWallLevel > 0) {
            newParentPos = new Vector3(transform.position.x + newPosOverLeftWallLevel, transform.position.y, transform.position.z);
        }
        if (newPosOverBottomWallLevel > 0) {
            newParentPos = new Vector3(transform.position.x, transform.position.y + newPosOverBottomWallLevel, transform.position.z);
        }

        bool canRotate = true;
        for (var i = 0; i < 4; i++) {
            Vector3 pos = newPosList[i];
            GameObject blockInPos = GameObject.Find($"{(int)pos.x},{(int)pos.y}");
            if (blockInPos != null && blockInPos.transform.parent != transform) {
                canRotate = false;
            }
        }

        if (canRotate) {
            transform.position = newParentPos;
            childBlockList[0].transform.localPosition = newPosList[0];
            childBlockList[1].transform.localPosition = newPosList[1];
            childBlockList[2].transform.localPosition = newPosList[2];
            childBlockList[3].transform.localPosition = newPosList[3];
            Register();
        }
    }

    void CalculatePossibleBottomPos() {
        int lowestY = (int)parentLoc.y;
        for (int y = (int)parentLoc.y; y > tetrominoConsts.BOTTOM_WALL_Y; y--) {
            bool isValid = true;
            for (int i = 0; i < 4; i++) {
                SingleBlock block = childBlockList[i].GetComponent<SingleBlock>();
                Vector3 tryPos = new Vector3(parentLoc.x + map[rotateIndex, i, 0], y + map[rotateIndex, i, 1], 0);
                GameObject blockInTryPos = GameObject.Find($"{(int)tryPos.x},{(int)tryPos.y}");
                if (tryPos.y <= tetrominoConsts.BOTTOM_WALL_Y || (blockInTryPos != null && blockInTryPos.transform.parent != transform)) {

                    isValid = false;
                }
            }
            if (isValid) {
                lowestY = y;
            } else {
                break;
            }
        }

        for (int i = 0; i < 4; i++) {
            Vector3 lowestPos = new Vector3(parentLoc.x + map[rotateIndex, i, 0], lowestY + map[rotateIndex, i, 1], 0);
            GameObject previewBlock = GameObject.Find($"previewBlock_{i + 1}");
            previewBlock.transform.position = new Vector3(lowestPos.x + 0.5f, lowestPos.y + 0.5f, 0);
            previewBlock.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    void SpeedDescendBlock() {
        transform.position = GameObject.Find($"previewBlock_2").transform.position;
        Register();
        CheckReachBottom();
        CheckLock();
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
            CheckReachBottom();
            CheckLock();
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

    void CheckReachBottom () {
        bool isReachedBottom = false;
        foreach(Transform block in transform) {
            SingleBlock singleBlock = block.GetComponent<SingleBlock>();
            GameObject bottomGoalBlock = GameObject.Find($"{singleBlock.x},{singleBlock.y - 1}");
            
            if (singleBlock.y <= tetrominoConsts.BOTTOM_WALL_Y + 1 || (bottomGoalBlock != null && bottomGoalBlock.transform.parent != transform)) {
                isReachedBottom = true;
            }
        }

        isBottomOccupied = isReachedBottom;
    }

    async void CheckLock() {
        await Task.Delay(300);
        CheckReachBottom();
        if (isBottomOccupied) {
            isLocked = true;
            spawnBlock.resetIsSpawned();
            scoreController.CheckCompleteLine();
            Destroy(this);
        }
    }


    void Register() {
        parentLoc = new Vector3(transform.position.x - 0.5f, transform.position.y - 0.5f, 0);
        foreach (Transform block in transform) {
            SingleBlock singleBlock = block.GetComponent<SingleBlock>();
            Vector3 blockLoc = new Vector3(parentLoc.x + block.localPosition.x, parentLoc.y + block.localPosition.y, 0);
            singleBlock.RegisterBlockPos(blockLoc);
        }
    }

    IEnumerator InputIntervalCoroutine() {
        isInInputInterval = true;
        yield return new WaitForSeconds(inputInterval);
        isInInputInterval = false;
    }
}