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
    private float randomTime;

    private float v;

    // Start is called before the first frame update
    void Start()
    {
        targetRot = 0;
        offset = transform.rotation.eulerAngles.y;
        randomIdle = Random.Range(0f, 10f);
        randomTime = Random.Range(0.01f, 3f);
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
            randomIdle = Random.Range(0f, maxRandIdle);
        } 
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            targetRot = 90f;
            idleTime = Time.time;
            randomTime = Random.Range(0.1f, 3f);
        } 
        else if (Time.time > idleTime + idleTarget + randomIdle)
        {
            targetRot = Random.Range(-20f, 20f);
            if (Random.value < 0.8f) targetRot = 0;
            
            currentSmoothTime = smoothTime / randomTime;
        }
        transform.rotation = Quaternion.Euler(modelRot.x, Mathf.SmoothDampAngle(modelRot.y, targetRot + offset, ref v, currentSmoothTime), modelRot.z);

    }
}
