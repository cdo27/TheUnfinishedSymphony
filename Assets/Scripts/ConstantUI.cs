using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ConstantUI : MonoBehaviour
{
    public static ConstantUI Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
