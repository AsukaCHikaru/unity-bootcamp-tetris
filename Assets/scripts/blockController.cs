using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
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

    SpawnBlock spawnBlock;

    void Start() {
        spawnBlock = GameObject.Find("GameController").GetComponent<SpawnBlock>();
    }

    void FixedUpdate() {
        if (Input.GetKey(KeyCode.A)) {
            if (!isInInputInterval && !isContactingWallLeft && !isContactingWallBottom) {
                _inputIntervalCoroutine = InputIntervalCoroutine();
                StartCoroutine(_inputIntervalCoroutine);
                transform.position = new Vector3(Mathf.Round(transform.position.x - 1), transform.position.y, transform.position.z);
            }
        }

        if (Input.GetKey(KeyCode.D)) {
            if (!isInInputInterval && !isContactingWallRight && !isContactingWallBottom) {
                _inputIntervalCoroutine = InputIntervalCoroutine();
                StartCoroutine(_inputIntervalCoroutine);
                transform.position = new Vector3(Mathf.Round(transform.position.x + 1), transform.position.y, transform.position.z);
            }
        }

        if (Input.GetKey(KeyCode.W)) {
            if (!isInInputInterval && !isContactingWallBottom) {
                _inputIntervalCoroutine = InputIntervalCoroutine();
                StartCoroutine(_inputIntervalCoroutine);
                transform.Find("wrapper").Rotate(0, 0, 90f);
            }
        }

        if (!isDescendCalled) {
            DescendBlock();
        }

    }

    void DescendBlock() {
        blockDescendCoroutine = DescendBlockCoroutine();
        StartCoroutine(blockDescendCoroutine);
        isDescendCalled = true;
        Debug.Log("descend block");
    }

    IEnumerator DescendBlockCoroutine() {
        while (!isContactingWallBottom) {
            yield return new WaitForSeconds(fallInterval);
            transform.position = new Vector3(transform.position.x, Mathf.Round(transform.position.y - 1), transform.position.z);
            Debug.Log(transform.position);
        }
    }

    IEnumerator InputIntervalCoroutine() {
        isInInputInterval = true;
        yield return new WaitForSeconds(inputInterval);
        isInInputInterval = false;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log(collision.transform.name);
        if (collision.transform.name == "wall bottom") {
            isContactingWallBottom = true;
            Debug.Log("stop coroutine");
            StopCoroutine(blockDescendCoroutine);
            spawnBlock.resetIsSpawned();
        }

        if (collision.transform.name == "wall right") {
            isContactingWallRight = true;
        }

        if (collision.transform.name == "wall left") {
            isContactingWallLeft = true;
        }
    }
}
