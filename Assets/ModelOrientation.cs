using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelOrientation : MonoBehaviour
{
    public float smoothTime;
    public bool moving => Input.GetAxisRaw("Horizontal") == 0; // for animation
    public float targetRot;
    public Vector3 modelRot;
    public float offset;
    private float idleTime;
    public float idleTarget;
    public float maxRandIdle;
    private float randomIdle;

    private float v;

    // Start is called before the first frame update
    void Start()
    {
        targetRot = 0;
        offset = transform.rotation.eulerAngles.y;
        randomIdle = Random.Range(0f, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        float currentSmoothTime = smoothTime;
        modelRot = gameObject.transform.rotation.eulerAngles;

        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            targetRot = -90f;
            idleTime = Time.time;   
        } 
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            targetRot = 90f;
            idleTime = Time.time;
        } 
        else if (Time.time > idleTime + idleTarget + randomIdle)
        {
            targetRot = 0;
            randomIdle = Random.Range(0f, maxRandIdle);
            currentSmoothTime = smoothTime / Random.Range(0.1f, 3f);
        }
        transform.rotation = Quaternion.Euler(modelRot.x, Mathf.SmoothDampAngle(modelRot.y, targetRot + offset, ref v, currentSmoothTime), modelRot.z);

    }
}
