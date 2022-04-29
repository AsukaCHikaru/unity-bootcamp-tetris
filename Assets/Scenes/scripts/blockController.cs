using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockController : MonoBehaviour
{

    private Rigidbody2D rigidbody;
    public float moveSpeed = 5.0f;
    private bool isStopped = false;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (Input.GetKey(KeyCode.A)) {
            transform.position += new Vector3(-1, 0) * moveSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D)) {
            transform.position += new Vector3(1, 0) * moveSpeed * Time.deltaTime;
        }

        if (!isStopped) {
            transform.position += new Vector3(0, -1) * moveSpeed * Time.deltaTime;
        }
    }
}
