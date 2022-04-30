using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockController : MonoBehaviour
{

    private float fallInterval = 0.2f;
    public float moveSpeed = 5.0f;
    private bool isStopped = false;
    private bool isDescendCalled = false;
    private IEnumerator blockDescendCoroutine;
    
    void Start()
    {
        
    }

    void FixedUpdate() {
        if (Input.GetKey(KeyCode.A)) {
            transform.position = new Vector3(Mathf.Round(transform.position.x - 1), transform.position.y, transform.position.z);
        }

        if (Input.GetKey(KeyCode.D)) {
            transform.position = new Vector3(Mathf.Round(transform.position.x + 1), transform.position.y, transform.position.z);
        }

        if (Input.GetKey(KeyCode.W)) {
            transform.Rotate(0, 0, 90f);
        }

        if (!isDescendCalled) {
            DescendBlock();
        }

    }

    void DescendBlock () {
        blockDescendCoroutine = DescendBlockCoroutine();
        StartCoroutine(blockDescendCoroutine);
        isDescendCalled = true;
        Debug.Log("descend block");
    }

    IEnumerator DescendBlockCoroutine () { 
        while (!isStopped) {
            yield return new WaitForSeconds(fallInterval);
            transform.position = new Vector3(transform.position.x, Mathf.Round(transform.position.y - 1), transform.position.z);
            Debug.Log(transform.position);
        }
    }

    void OnCollisionEnter2D (Collision2D collision){
        Debug.Log(collision.transform.name);
        if (collision.transform.name != "wall top") { 
            isStopped = true;
            Debug.Log("stop coroutine");
            StopCoroutine(blockDescendCoroutine);
        }
    }
}
