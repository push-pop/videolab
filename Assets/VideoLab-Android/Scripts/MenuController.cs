using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MenuState
{
    Closed,
    Open,
}

public class MenuController : MonoBehaviour {

    [SerializeField]
    MenuState State = MenuState.Open;

    [SerializeField]
    GameObject _menu;

    [SerializeField]
    GameObject _scenes;

    [SerializeField]
    GameObject _devices;


    public void ToggleMenu()
    {
        if (State == MenuState.Closed)
            GoToState(MenuState.Open);
        else if (State == MenuState.Open)
            GoToState(MenuState.Closed);
    }

    public void GoToState(MenuState newState)
    {
        State = newState;

        switch (newState)
        {
            case MenuState.Open:
                _menu.SetActive(true);
                _devices.SetActive(true);
                _scenes.SetActive(false);
                break;
            case MenuState.Closed:
                _menu.SetActive(false);
                _devices.SetActive(false);
                _scenes.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void ShowDevices()
    {
        _devices.SetActive(true);
        _scenes.SetActive(false);
        //_menu.SetActive(false);
    }

    public void ShowScenes()
    {
        _devices.SetActive(false);
        _scenes.SetActive(true);
        //_menu.SetActive(false);
    }

    // Use this for initialization
    void Start () {
        GoToState(State);
	}
	
	// Update is called once per frame
	void Update () {
		
	}


}
