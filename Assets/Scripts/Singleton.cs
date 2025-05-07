using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    private static Singleton instance;
    
    //things to keep track of
    public int currMode; // 0 easy 1 normal 3 hard

    public static Singleton GetInstance => instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}