using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanButton : MonoBehaviour
{
    public GameObject button;
    public Collider col;

    public bool pressed;

    public float speed;

    public float up;
    public float down;


    // Start is called before the first frame update
    void Start()
    {
        col = button.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pressed)
        {
            transform.localPosition = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
        } else
        {
            transform.localPosition = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
        }
        transform.localPosition = new Vector3(Mathf.Clamp(transform.position.x + speed * Time.deltaTime, down, up), transform.position.y, transform.position.z);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Ground"))
        {
            pressed = true;

        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (!collision.collider.CompareTag("Ground"))
        {
            pressed = false;

        }
    }
    void OnCollisionStay(Collision collision)
    {
        if (!collision.collider.CompareTag("Ground"))
        {
            pressed = true;

        }
    }
}
