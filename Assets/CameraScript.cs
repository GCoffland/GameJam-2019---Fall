using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public static CameraScript instance;
    private int shake_remaining = 0;

    Vector3 position;
    Vector3 shookPosition;
    private float intensity = 0;
    private int frame = 0;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        frame++;
        if(shake_remaining > 0)
        {
            shake_remaining--;
            if (frame % 10 == 0)
            {
                shookPosition = position;
                shookPosition.x += intensity * (Random.value - 0.5f);
                shookPosition.y += intensity * (Random.value - 0.5f);
                intensity *= 0.8f;
                transform.position = shookPosition;
            }
        }
        else if(shake_remaining == 0)
        {
            transform.position = position;
            shake_remaining--;
        }

    }

    public void Shake(int duration, float intensity)
    {
        shake_remaining = duration;
        this.intensity = intensity;
        shookPosition = position;
    }
}
