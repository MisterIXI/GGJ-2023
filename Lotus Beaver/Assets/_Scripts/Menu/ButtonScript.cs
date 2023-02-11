using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI StartGameButtonText;
    [SerializeField] private TextMeshProUGUI StartGameButtonTextShadow;
    [SerializeField] private GameObject Hmenu;
    [SerializeField] private GameObject CreditMenu;
    [SerializeField] private GameObject SettingsMenu;
    [SerializeField] private GameObject ControlMenu;
    [SerializeField] private GameObject HUD;
    [SerializeField] private Button QuitButton;
    private readonly GameObject[] menuList = new GameObject[4];

    // EDIT IN BUTTON EVENT BOX CHANGE ENUM MENU
    private enum Menu
    {
        HMenu,
        CreditMenu,
        SettingMenu,
        ControlMenu,
        HUD
    }

    private void Awake()
    {
#if UNITY_WEBGL
        QuitButton.interactable = false;
#endif
    }

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        OnChangeMenu(Menu.HMenu);
    }

    private void OnChangeMenu(Menu changeMenu)
    {
        // MENU SET ENABLE AND FALSE OTHER MENUS
        if (changeMenu == Menu.HMenu)
        {
            SetMenuState(CreditMenu, false);
            SetMenuState(ControlMenu, false);
            SetMenuState(SettingsMenu, false);
            SetMenuState(SettingsMenu, false);

            SetMenuState(Hmenu, true);
        }
        else if (changeMenu == Menu.CreditMenu)
        {
            SetMenuState(Hmenu, false);
            SetMenuState(ControlMenu, false);
            SetMenuState(SettingsMenu, false);
            SetMenuState(SettingsMenu, false);

            SetMenuState(CreditMenu, true);
        }
        else if (changeMenu == Menu.ControlMenu)
        {
            SetMenuState(Hmenu, false);
            SetMenuState(SettingsMenu, false);
            SetMenuState(CreditMenu, false);
            SetMenuState(SettingsMenu, false);

            SetMenuState(ControlMenu, true);
        }
        else if (changeMenu == Menu.SettingMenu)
        {
            SetMenuState(Hmenu, false);
            SetMenuState(CreditMenu, false);
            SetMenuState(ControlMenu, false);
            SetMenuState(SettingsMenu, false);

            SetMenuState(SettingsMenu, true);
        }
        else if (changeMenu == Menu.HUD)
        {
            SetMenuState(Hmenu, false);
            SetMenuState(CreditMenu, false);
            SetMenuState(ControlMenu, false);
            SetMenuState(SettingsMenu, false);

            SetMenuState(HUD, true, false);
        }
    }

    private void SetMenuState(GameObject menu, bool newState, bool selectFirstButton = true)
    {
        if (newState)
        {
            menu.SetActive(true);
            if (selectFirstButton)
            {
                menu.GetComponentInChildren<Button>().Select();
            }
        }
        else
        {
            menu.SetActive(false);
        }
    }

    public void OnButtonStart()
    {
        StartGameButtonText.text = "Resume Game";
        StartGameButtonTextShadow.text = "Resume Game";
        if (GameManager.GameState == GameState.GameOver)
        {
            GameManager.StartNewGame();
        }
        GameManager.SetGameState(GameState.Ingame);
        OnChangeMenu(Menu.HUD);
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

    public void OnButtonMenu()
    {
        OnChangeMenu(Menu.HMenu);
    }
}