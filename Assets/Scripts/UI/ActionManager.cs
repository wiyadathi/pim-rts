using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionManager : MonoBehaviour
{
    public static ActionManager instance;
    [SerializeField] private Button[] unitBtns;
    [SerializeField] private Button[] buildingBtns;
    private CanvasGroup cg;
    private void Awake()
    {
        instance = this;
        cg = GetComponent<CanvasGroup>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void HideCreateUnitButtons()
    {
        for (int i = 0; i < unitBtns.Length; i++)
            unitBtns[i].gameObject.SetActive(false);
    }
    private void HideCreateBuildingButtons()
    {
        for (int i = 0; i < buildingBtns.Length; i++)
            buildingBtns[i].gameObject.SetActive(false);
    }
    public void ClearAllInfo()
    {
        HideCreateUnitButtons();
        HideCreateBuildingButtons();
    }
    private void ShowCreateUnitButtons(Building b)
    {
        if (b.IsFunctional)
        {
            Debug.Log("Hello");
            for (int i = 0; i < b.UnitPrefabs.Length; i++)
            {
                unitBtns[i].gameObject.SetActive(true);
                Unit unit = b.UnitPrefabs[i].GetComponent<Unit>();
                unitBtns[i].image.sprite = unit.UnitPic;
            }
        }
    }
    private void ShowCreateBuildingButtons(Unit u) //Showing list of buildings when selecting a single unit
    {
        if (u.IsBuilder)
        {
            for (int i = 0; i < u.Builder.BuildingList.Length; i++)
            {
                buildingBtns[i].gameObject.SetActive(true);

                if (u.Builder.BuildingList[i] != null)
                {
                    buildingBtns[i].GetComponent<Button>().interactable = true;
                    buildingBtns[i].image.color = Color.white;
                    Building building = u.Builder.BuildingList[i].GetComponent<Building>();
                    buildingBtns[i].image.sprite = building.StructurePic;
                }
                else
                {
                    buildingBtns[i].GetComponent<Button>().interactable = false;
                    buildingBtns[i].image.color = Color.clear;
                }
            }
        }
    }
    public void ShowCreateUnitMode(Building b)
    {
        ClearAllInfo();
        ShowCreateUnitButtons(b);
    }

    public void ShowBuilderMode(Unit unit)
    {
        ClearAllInfo();
        ShowCreateBuildingButtons(unit);
    }
    public void CreateUnitButton(int n)//Map with Create Unit Btns
    {
        Debug.Log("Create " + n);
        UnitSelect.instance.CurBuilding.ToCreateUnit(n);
    }

    public void CreateBuildingButton(int n)//Map with Create Building Btns
    {
        //Debug.Log("1 - Click Button: " + n);
        Unit unit = UnitSelect.instance.CurUnit[0];

        if(unit.IsBuilder)
        {
            unit.Builder.ToCreateNewBuilding(n);
        }
    }
    






}
