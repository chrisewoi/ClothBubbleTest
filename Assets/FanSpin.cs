using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FanSpin : MonoBehaviour
{
    public bool active;
    public float speed;
    public float activationSpeed;
    private float currentSpeed;

    public GameObject fan;
    public ParticleSystem ps;

    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (currentSpeed < speed)
            {
                currentSpeed += activationSpeed * Time.deltaTime;
            }
            
            if (ps.isStopped) ps.Play();
        } else
        {
            if (ps.isPlaying && currentSpeed == 0) ps.Stop();
            currentSpeed -= activationSpeed * Time.deltaTime;
        }
        currentSpeed = Mathf.Clamp(currentSpeed, 0, speed);
        fan.transform.Rotate(currentSpeed * Time.deltaTime, 0, 0);

    }
    public float GetSpeed()
    {
        return currentSpeed;
    }
}
