using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelSpin : MonoBehaviour
{
    public float spinRate;

    void Update()
    {
        this.transform.Rotate(new Vector3(0,0,-1) * spinRate * Time.deltaTime);
    }
}
