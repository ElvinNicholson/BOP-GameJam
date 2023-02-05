using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject tutorial;
    [SerializeField] private RootManager rootManager;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (rootManager.has_drawn)
        {
            tutorial.SetActive(false);
        }
    }
}
