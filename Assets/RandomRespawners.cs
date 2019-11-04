using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRespawners : MonoBehaviour
{

    GameObject respawner;

    // Start is called before the first frame update
    void Start()
    {
        respawner = Resources.Load<GameObject>("Prefabs/ResSprite");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
