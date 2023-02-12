using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeadUpDisplayActiveElements : MonoBehaviour
{
    [SerializeField] private Transform RessourcesRoot;
    [SerializeField] private Transform ToolsRoot;

    [SerializeField] private GameObject RessourcePrefab;
    [SerializeField] private GameObject ToolPrefab;

    [SerializeField] private Sprite EarthIcon;
    [SerializeField] private Sprite WaterIcon;

    [SerializeField] private InteractionSettings _interactionSettings;
    [SerializeField] private TextMeshProUGUI[] _ressourceTexts;

    [SerializeField] public Image _fadeImage;
    [SerializeField] public TextMeshProUGUI _costEarthText;
    [SerializeField] public TextMeshProUGUI _costWaterText;
    [SerializeField] public TextMeshProUGUI _interactionText;
    [SerializeField] public TextMeshProUGUI _interactionDescriptionText;

    [SerializeField] public static Image FadeImage => _instance._fadeImage;
    [SerializeField] public static TextMeshProUGUI CostEarthText => _instance._costEarthText;
    [SerializeField] public static TextMeshProUGUI CostWaterText => _instance._costWaterText;
    [SerializeField] public static TextMeshProUGUI InteractionText => _instance._interactionText;
    [SerializeField] public static TextMeshProUGUI InteractionDescriptionText => _instance._interactionDescriptionText;

    private GameObject _selectedTool;

    public static HeadUpDisplayActiveElements _instance;
    public static HeadUpDisplayActiveElements Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(transform.root.gameObject);
    }

    private void OnDestroy()
    {
        InteractionController.OnInteractionChange -= UpdateToolSelection;
    }

    private void Start()
    {
        InteractionController.OnInteractionChange += UpdateToolSelection;

        InitializeRessources();
        InitializeTools();
    }

    private void OnEnable()
    {
        _ = StartCoroutine(DelayedEnable());
    }

    private IEnumerator DelayedEnable()
    {
        yield return new WaitForEndOfFrame();

        UpdateToolSelection(0);
    }

    public void UpdateEarth(float count)
    {
        count = (int)count;
        _ressourceTexts[0].text = count.ToString();
    }

    public void UpdateWater(float count)
    {
        count = (int)count;
        _ressourceTexts[1].text = count.ToString();
    }

    public void UpdateToolSelection(int index)
    {
        // unselect current tool
        if (_selectedTool != null)
        {
            _selectedTool.transform.GetChild(0).GetComponentInChildren<Image>().color = Color.white;
        }
        if (index >= 0 || index < ToolsRoot.childCount)
        {
            // select new tool
            _selectedTool = ToolsRoot.GetChild(index).gameObject;
            _selectedTool.transform.GetChild(0).GetComponentInChildren<Image>().color = Color.green;
        }
    }

    private void InitializeRessources()
    {
        GameObject earth = Instantiate(RessourcePrefab, RessourcesRoot);
        earth.transform.GetChild(0).GetComponentInChildren<Image>().sprite = EarthIcon;
        earth.GetComponentInChildren<TextMeshProUGUI>().text = RessourceManager.Earth.ToString();

        GameObject water = Instantiate(RessourcePrefab, RessourcesRoot);
        water.transform.GetChild(0).GetComponentInChildren<Image>().sprite = WaterIcon;
        water.GetComponentInChildren<TextMeshProUGUI>().text = RessourceManager.Water.ToString();
        _ressourceTexts = new TextMeshProUGUI[2];
        _ressourceTexts[0] = earth.GetComponentInChildren<TextMeshProUGUI>();
        _ressourceTexts[1] = water.GetComponentInChildren<TextMeshProUGUI>();

        RessourceManager.OnEarthChange += UpdateEarth;
        RessourceManager.OnWaterChange += UpdateWater;
    }

    private void InitializeTools()
    {
        GameObject tool = Instantiate(ToolPrefab, ToolsRoot);
        tool.transform.GetChild(0).GetComponentInChildren<Image>().sprite = EarthIcon;

        foreach (BuildingPreset buildingPreset in _interactionSettings.BuildingSettings)
        {
            tool = Instantiate(ToolPrefab, ToolsRoot);
            tool.transform.GetChild(0).GetComponentInChildren<Image>().sprite = buildingPreset.InteractionIcon;
            // tool.GetComponentInChildren<Text>().text = buildingPreset.displayName;
        }
        int toolCount = 1 + _interactionSettings.BuildingSettings.Length;
        // set toolsroot rect transform width
        ToolsRoot.GetComponent<RectTransform>().sizeDelta = new Vector2(toolCount * 100, 100);
    }
}