using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UISTATE {  HIDDEN, VISIBLE }
public enum UIMENU {  HIDDEN, VISIBLE }


/// <summary>
/// This class handles changes in menus. What menu we are on and if it is hidden or not. This includes the HUD
/// </summary>
public class UIStatemanager : MonoBehaviour
{
    public GameObject _startMenu;

    private Dictionary<string, GameObject> _menus = new Dictionary<string, GameObject>();
    private GameObject _lastMenu = null, _currentMenu = null;

    void Awake()
    {
        DontDestroyOnLoad(this);
        foreach (Transform child in transform)
        {
            if (!_menus.ContainsKey(child.name))
            {
                _menus[child.name] = child.gameObject;
            }
        }
        HideAllMenus();
        _startMenu.SetActive(true);
        _currentMenu = _startMenu;
    }
    private void Start()
    {
        Core.Instance.Runs.OnStateChange.AddListener(OnRunStateChange);
    }
    private void OnRunStateChange(RUNSTATE state)
    {
        switch (state)
        {
            case RUNSTATE.PRERUN:
                GotToMenu("MainMenu");
                break;
            case RUNSTATE.INRUN:
                GotToMenu("HUD");
                break;
            case RUNSTATE.POSTRUN:
                GotToMenu("PostRun");
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _currentMenu.SetActive(!_currentMenu.activeSelf);
        }
    }

    public void GotToMenu(string menuName)
    {
        if (!_menus.ContainsKey(menuName))
        {
            return;
        }
        HideAllMenus();
        _menus[menuName].SetActive(true);
        _lastMenu = _currentMenu;
        _currentMenu = _menus[menuName];
    }

    public void GoToPreviousMenu()
    {
        HideAllMenus();
        _currentMenu = _lastMenu;
        _currentMenu.SetActive(true);
    }

    public void HideMenu()
    {
        HideAllMenus();
    }

    public void ShowMenu()
    {
        HideAllMenus();
        _currentMenu.SetActive(true);
    }

    private void HideAllMenus()
    {
        foreach (string key in _menus.Keys)
        {
            _menus[key].SetActive(false);
        }
    }


}
