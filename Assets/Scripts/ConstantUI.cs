using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ConstantUI : MonoBehaviour
{

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
