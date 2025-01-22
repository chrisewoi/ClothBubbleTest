using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraMove : MonoBehaviour
{
    public GameObject[] groundObjects;
    public Renderer[] groundRenderers;
    public float[] groundYs;

    public GameObject player;
    public Rigidbody playerRb;
    [Header("L, R, U, D")]
    public Vector4 screenEdgePadding;

    public float yUpper;
    public float yLower;

    public Vector3 v;

    public float cameraLowLimit;
    public float cameraHighLimit;
    public float cameraSmoothTime;
    
    
    // Start is called before the first frame update
    void Start()
    {
        cameraLowLimit = transform.position.y;
        cameraHighLimit = 1000f;

        playerRb = player.GetComponent<Rigidbody>();
        
        groundObjects = GameObject.FindGameObjectsWithTag("Ground");
        groundRenderers = new Renderer[groundObjects.Length];
        groundYs = new float[groundObjects.Length];
        
        for (int i = 0; i < groundObjects.Length; i++)
        {
            groundRenderers[i] = groundObjects[i].GetComponent<Renderer>();
            groundYs[i] = groundObjects[i].transform.position.y;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(player.transform.position);
        float normX = playerScreenPosition.x / Screen.width;
        float normY = playerScreenPosition.y / Screen.height;
        Debug.Log("player screen pos: " + playerScreenPosition);
        Debug.Log("normX: " + normX);
        Debug.Log("normY: " + normY);
        
        SetCameraX(player.transform.position.x);
        
        cameraHighLimit = cameraLowLimit; 
        
        if (normY > yUpper)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height*yUpper));
            //cameraHighLimit = player.transform.position.y + (Screen.height / 2f * 0.9f);

            float distance = Mathf.Abs(player.transform.position.z - transform.position.z);
            Vector3 targetPoint = ray.origin + (ray.direction * distance);

            float difference = player.transform.position.y - targetPoint.y;
            if (difference > 0)
            {
                cameraHighLimit = transform.position.y + difference;
                Debug.Log("cameraHighLimit: " + cameraHighLimit);
                SetCameraY(transform.position.y + distance);

            }

        }
        else if (normY < yLower)
        {
            float distance = Mathf.Abs(player.transform.position.z - transform.position.z);
            distance *= Time.deltaTime;
            SetCameraY(transform.position.y - distance);

        }
        
    }

    void SetCameraX(float x)
    {
        Vector3 p = transform.position;
        transform.position = new Vector3(player.transform.position.x, p.y, p.z);
    }

    void SetCameraY(float y)
    {
        y = Mathf.Clamp(y, cameraLowLimit, cameraHighLimit); // Can add cameraHighLimit here

        Vector3 p = transform.position;
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(p.x, y, p.z), ref v, cameraSmoothTime);
    }
    void SmoothCameraY(float y)
    {
        y = Mathf.Clamp(y, cameraLowLimit, cameraHighLimit); // Can add cameraHighLimit here
    }
}
