using System;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsToggle : MonoBehaviour
{
    [SerializeField] private List<GameObject> graphics;
    private bool _toggler;

    private void Start()
    {
        _toggler = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            _toggler = !_toggler;
        }
        
        foreach (GameObject graphic in graphics)
        {
            graphic.SetActive(_toggler);
        }
    }
}
