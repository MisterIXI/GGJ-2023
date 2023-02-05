using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] private GameObject Hmenu;
    [SerializeField] private GameObject CreditMenu;
    [SerializeField] private GameObject SettingsMenu;
    [SerializeField] private GameObject ControlMenu;
    private GameObject[] menuList = new GameObject[4];
    // EDIT IN BUTTON EVENT BOX CHANGE ENUM MENU
    private enum Menu
    {
        HMenu,
        CreditMenu,
        SettingMenu,
        ControlMenu
    }
    // Start is called before the first frame update
    private void OnChangeMenu(Menu changeMenu)
    {
        // MENU SET ENABLE AND FALSE OTHER MENUS
        if(changeMenu == Menu.HMenu)
        {
            SetMenuState(CreditMenu,false);
            SetMenuState(ControlMenu, false);
            SetMenuState(SettingsMenu,false);

            SetMenuState(Hmenu, true);

        }else if (changeMenu == Menu.CreditMenu)
        {
            SetMenuState(Hmenu,false);
            SetMenuState(ControlMenu, false);
            SetMenuState(SettingsMenu,false);
            
            SetMenuState(CreditMenu,true);

        }else if(changeMenu == Menu.ControlMenu)
        {
            SetMenuState(Hmenu,false);
            SetMenuState(SettingsMenu,false);
            SetMenuState(CreditMenu,false);
            
            SetMenuState(ControlMenu, true);
        }else if(changeMenu == Menu.SettingMenu)
        {
            SetMenuState(Hmenu,false);
            SetMenuState(CreditMenu,false);
            SetMenuState(ControlMenu, false);
            
            SetMenuState(SettingsMenu,true);
        }

    }
    private void SetMenuState(GameObject menu, bool newState)
    {
        if(newState)
        {
            menu.SetActive(true);
        }
        else{
            menu.SetActive(false);
        }
    }
    public void OnButtonStart()
    {
         // START HERE GAMELOOP
    }
    public void OnButtonSettings()
    {
        OnChangeMenu(Menu.SettingMenu);
    }
    public void OnButtonCredits()
    {
        OnChangeMenu(Menu.CreditMenu); 
    }
    public void OnButtonControls()
    {
        OnChangeMenu(Menu.ControlMenu);
    }
    public void OnButtonBack()
    {
        OnChangeMenu(Menu.HMenu);
    }
}
