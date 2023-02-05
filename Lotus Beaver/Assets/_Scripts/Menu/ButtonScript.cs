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
    
    public enum Menu
    {
        HMenu,
        CreditMenu,
        SettingMenu,
        ControlMenu
    }
    // Start is called before the first frame update
    public void OnChangeMenu(Menu changeMenu)
    {
        // MENU SET ENABLE AND FALSE IF MENU
        if(changeMenu== Menu.HMenu)
        {
            
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
}
