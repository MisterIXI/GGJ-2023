using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using TMPro;
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
    private GameObject _selectedTool;

    private void Start()
    {
        InitializeRessources();
        InitializeTools();
        InteractionController.OnInteractionChange += UpdateToolSelection;
    }
    private void OnEnable()
    {
        StartCoroutine(DelayedEnable());
    }
    private IEnumerator DelayedEnable()
    {
        yield return new WaitForEndOfFrame();
        // InitializeRessources();
        // InitializeTools();

    }

    public void UpdateEarth(int count)
    {
        _ressourceTexts[0].text = count.ToString();
    }

    public void UpdateWater(int count)
    {
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
        earth.GetComponentInChildren<TextMeshProUGUI>().text = RessourceManager.earth.ToString();

        GameObject water = Instantiate(RessourcePrefab, RessourcesRoot);
        water.transform.GetChild(0).GetComponentInChildren<Image>().sprite = WaterIcon;
        water.GetComponentInChildren<TextMeshProUGUI>().text = RessourceManager.water.ToString();
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
        tool = Instantiate(ToolPrefab, ToolsRoot);
        tool.transform.GetChild(0).GetComponentInChildren<Image>().sprite = WaterIcon;
        foreach (BuildingPreset buildingPreset in _interactionSettings.BuildingSettings)
        {
            tool = Instantiate(ToolPrefab, ToolsRoot);
            tool.transform.GetChild(0).GetComponentInChildren<Image>().sprite = buildingPreset.InteractionIcon;
            // tool.GetComponentInChildren<Text>().text = buildingPreset.displayName;
        }
        int toolCount = 2 + _interactionSettings.BuildingSettings.Length;
        // set toolsroot rect transform width
        ToolsRoot.GetComponent<RectTransform>().sizeDelta = new Vector2(toolCount * 100, 100);
    }


}