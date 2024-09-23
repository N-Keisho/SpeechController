using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // --- components ---
    private SpeechController sc;
    private Rigidbody rb;

    // --- parameters ---
    public float speed = 100.0f;
    public float jumpForce = 300.0f;
    private bool isGrounded = true;
    public bool isTestMode = false;

    // --- keywords ---
    private string goStraight;
    private string goBack;
    private string stop;
    private string goLeft;
    private string goRight;
    private string jump;
    void Start()
    {
        // --- get components ---
        sc = GameObject.Find("__SpeechController__").GetComponent<SpeechController>();
        rb = GetComponent<Rigidbody>();

        // --- get keywords ---
        goStraight = sc.words[0];
        goBack = sc.words[1];
        goLeft = sc.words[2];
        goRight = sc.words[3];
        jump = sc.words[4];
        stop = sc.words[5];
    }

    void Update()
    {
        // --- move ---
        if (sc.flags[goStraight] || Input.GetKey(KeyCode.W) && isTestMode)
        {
            sc.flags[goBack] = false;
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        if (sc.flags[goBack] || Input.GetKey(KeyCode.S) && isTestMode)
        {
            sc.flags[goStraight] = false;
            transform.position -= transform.forward * speed * Time.deltaTime;
        }
        if (sc.flags[goLeft] || Input.GetKey(KeyCode.A) && isTestMode)
        {
            sc.flags[goRight] = false;
            transform.position -= transform.right * speed * Time.deltaTime;
        }
        if (sc.flags[goRight] || Input.GetKey(KeyCode.D) && isTestMode)
        {
            sc.flags[goLeft] = false;
            transform.position += transform.right * speed * Time.deltaTime;
        }
        if (isGrounded && (sc.flags[jump] || Input.GetKey(KeyCode.Space) && isTestMode))
        {
            rb.AddForce(Vector3.up * jumpForce * Time.deltaTime, ForceMode.Impulse);
        }
        if (sc.flags[stop])
        {
            sc.flags[goStraight] = false;
            sc.flags[goBack] = false;
            sc.flags[goLeft] = false;
            sc.flags[goRight] = false;
            sc.flags[stop] = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
            sc.flags[jump] = false;
        }
    }
}
