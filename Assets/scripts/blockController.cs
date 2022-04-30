using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockController : MonoBehaviour
{

    private Rigidbody2D rigidbody;
    private BoxCollider2D collider;
    public float moveSpeed = 5.0f;
    private bool isStopped = false;
    
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate() {
        if (Input.GetKey(KeyCode.A)) {
            transform.position += new Vector3(-1, 0) * moveSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D)) {
            transform.position += new Vector3(1, 0) * moveSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.W)) {
            transform.Rotate(0, 0, 90f);
        }

        if (!isStopped) {
            transform.position += new Vector3(0, -1) * moveSpeed * Time.deltaTime;
        }

    }

    void OnCollisionEnter2D (Collision2D collision){
        Debug.Log(collision.transform.name);
        // isStopped = true;
    }
}
