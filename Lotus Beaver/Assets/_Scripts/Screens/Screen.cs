using UnityEngine;
using UnityEngine.UI;

public class Screen : MonoBehaviour
{
    [SerializeField] private Selectable _firstSelectable;

    private static Screen _currentScreen;

    public static void SetCurrentScreen(Screen screen)
    {
        _currentScreen?.SetActive(false);

        _currentScreen = screen;

        screen.SetActive(true);
    }

    private void OnEnable()
    {
        SelectFirstSelectable();
    }

    private void SelectFirstSelectable()
    {
        if (_firstSelectable == null)
        {
            return;
        }

        _firstSelectable.Select();
    }

    public virtual void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}
