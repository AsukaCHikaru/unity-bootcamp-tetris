using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockTypeList 
{
    I_block,
    S_block,
    Z_block,
    L_block,
    J_block,
    square_block,
    T_block,
};

public class BlockController : MonoBehaviour
{
    int[,,] blockPositionMap = new int[,,] { 
        { { 0, 2 }, { 0, 1 }, { 1, 1 }, { 1, 0 } }, 
        { { 2, 1 }, { 1, 1 }, { 1, 0 }, { 0, 0 } }, 
    };
    int rotateIndex = 0;
    private float fallInterval = 0.2f;
    public float moveSpeed = 5.0f;
    private bool isContactingWallBottom = false;
    private bool isContactingWallRight = false;
    private bool isContactingWallLeft = false;
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
    }

    void FixedUpdate() {
        GetInput();

        if (!isDescendCalled) {
            DescendBlock();
        }
    }

    void GetInput () {
        if (Input.GetKey(KeyCode.A)) {
            if (!isInInputInterval && !isContactingWallLeft && !isContactingWallBottom) {
                _inputIntervalCoroutine = InputIntervalCoroutine();
                StartCoroutine(_inputIntervalCoroutine);
                transform.position = new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z);
            }
        }

        if (Input.GetKey(KeyCode.D)) {
            if (!isInInputInterval && !isContactingWallRight && !isContactingWallBottom) {
                _inputIntervalCoroutine = InputIntervalCoroutine();
                StartCoroutine(_inputIntervalCoroutine);
                transform.position = new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z);
            }
        }

        if (Input.GetKey(KeyCode.W)) {
            if (!isInInputInterval && !isContactingWallBottom) {
                _inputIntervalCoroutine = InputIntervalCoroutine();
                StartCoroutine(_inputIntervalCoroutine);
                Rotate();
            }
        }
    }

    void Rotate () {
        if (rotateIndex == 1) {
            rotateIndex = 0;
        } else {
            rotateIndex++;
        }
        for (var i = 0; i < 4; i++) {
            childBlockList[i].transform.localPosition = new Vector3(blockPositionMap[rotateIndex,i,0], blockPositionMap[rotateIndex,i,1], -1);
        }
    }

    void DescendBlock() {
        blockDescendCoroutine = DescendBlockCoroutine();
        StartCoroutine(blockDescendCoroutine);
        isDescendCalled = true;
    }

    IEnumerator DescendBlockCoroutine() {
        while (!isContactingWallBottom) {
            yield return new WaitForSeconds(fallInterval);
            transform.position = new Vector3(transform.position.x, Mathf.Ceil(transform.position.y - 1) - 0.5f, transform.position.z);
            Register();
            CheckCanMove();
        }
    }

    void CheckCanMove () {
        bool isGoalOccupied = parentLoc.y <= -9f;
        foreach(Transform block in transform) {
            SingleBlock singleBlock = block.GetComponent<SingleBlock>();
            GameObject goalBlock = GameObject.Find($"{singleBlock.x},{singleBlock.y - 1}");
            if (goalBlock != null && goalBlock.transform.parent != transform) {
                isGoalOccupied = true;
            }
        }
        if (isGoalOccupied) {
            isContactingWallBottom = true;
            StopCoroutine(blockDescendCoroutine);
            spawnBlock.resetIsSpawned();
        }
    }

    void Register () {
        parentLoc = new Vector3(transform.position.x - 0.5f, transform.position.y - 0.5f, 0);
        foreach(Transform block in transform) {
            SingleBlock singleBlock = block.GetComponent<SingleBlock>();
            Vector3 blockLoc = new Vector3(parentLoc.x + block.localPosition.x, parentLoc.y + block.localPosition.y, 0);
            block.name = $"{(int)blockLoc.x},{(int)blockLoc.y}";
            singleBlock.x = (int)blockLoc.x;
            singleBlock.y = (int)blockLoc.y;
        }
    }

    IEnumerator InputIntervalCoroutine() {
        isInInputInterval = true;
        yield return new WaitForSeconds(inputInterval);
        isInInputInterval = false;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log($"{collision.transform.name} from {transform.name}");
        if (collision.transform.name == "wall bottom") {
            // isContactingWallBottom = true;
            // rigidbody.bodyType = RigidbodyType2D.Static;
            
        }

        if (collision.transform.name == "wall right") {
            // isContactingWallRight = true;
        }

        if (collision.transform.name == "wall left") {
            // isContactingWallLeft = true;
        }
    }
}
