using System;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsToggle : MonoBehaviour
{
    [SerializeField] private List<GameObject> graphics;
    private bool _toggler;
    
    [Header("Sun/Sky Settings")]
    [SerializeField] private List<GameObject> suns;
    [SerializeField] private List<float> exposures;
    private int sunIndex = 0;
    private GameObject activeSun;

    private bool _cursortoggler;

    private void Start()
    {
        _toggler = false;
        _cursortoggler = false;
    }

    private void Update()
    {
        GraphicsTogglingFunctionality();
        SunSelector();
        
        CursorToggle();
    }

    //toggle high fidelity graphics on and off (to test different performance scenarios)
    private void GraphicsTogglingFunctionality()
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

    
    //iterates through list of suns and only allows the indexed sun to remain active
    private void SunSelector()
    {
        activeSun = suns[sunIndex];

        if (Input.GetKeyDown(KeyCode.Mouse3))
        {
            sunIndex++;
        }

        sunIndex = sunIndex % suns.Count;

        for (int i = 0; i < suns.Count; i++)
        {
            if (i == sunIndex)
            {
                suns[i].SetActive(true && _toggler);
            }
            else
            {
                suns[i].SetActive(false && _toggler);
            }
        }
    }

    private void CursorToggle()
    {
        if (Input.GetKeyDown(KeyCode.Mouse4))
        {
            _cursortoggler = !_cursortoggler;
        }        
        
        if (_cursortoggler)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
