using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitSelect : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private List<Unit> curUnit= new List<Unit>(); //current selected single unit
    public List<Unit> CurUnit { get { return curUnit; } }

    private Camera cam;
    private Faction faction;

    public static UnitSelect instance;
    [SerializeField]
    private Building curBuilding; //current selected single building
    public Building CurBuilding { get { return curBuilding; } }
    [SerializeField]
    private RectTransform selectionBox;
    private Vector2 oldAnchoredPos;//Box old anchored position
    private Vector2 startPos;//point where mouse is down
    [SerializeField]
    private Unit curEnemy;
    [SerializeField]
    private ResourceSource curResource; //current selected resource

    private float timer = 0f;
    private float timeLimit = 0.5f;
    



    // Start is called before the first frame update
    void Awake()
    {
        faction = GetComponent<Faction>();
    }

    void Start()
    {
        cam = Camera.main;
        layerMask = LayerMask.GetMask("Unit", "Building", "Resource", "Ground");
        selectionBox = MainUI.instance.SelectionBox;
        instance = this;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            if (EventSystem.current.IsPointerOverGameObject())
            { return; }
            ClearEverything();
        }
        //mouse held down
        if(Input.GetMouseButton(0))
        {
            UpdateSelectionBox(Input.mousePosition);
        }

        // mouse up
        if (Input.GetMouseButtonUp(0))
        {
            ReleaseSelectionBox(Input.mousePosition);
            if (IsPointerOverUIObject())
                return;
            TrySelect(Input.mousePosition);
        }
        timer += Time.deltaTime;
        if(timer >= Time.deltaTime)
        {
            timer = 0f;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        if (curUnit.Count == 1)
            ShowUnit(curUnit[0]);
        else if (curEnemy != null)
            ShowEnemyUnit(curEnemy);
        else if (curResource != null)
            ShowResource();
        else if (curBuilding != null)
        {
            if (GameManager.instance.MyFaction.IsMyBuilding(curBuilding))
                ShowBuilding(curBuilding);//Show building info
            else
                ShowEnemyBuilding(curBuilding);
        }
    }


    private void SelectUnit(RaycastHit hit)
    {
        Unit unit = hit.collider.GetComponent<Unit>();

        unit.ToggleSelectionVisual(true);

        Debug.Log("Selected Unit");
        if (GameManager.instance.MyFaction.IsMyUnit(unit))
        {
            curUnit.Add(unit);
            unit.ToggleSelectionVisual(true);
            ShowUnit(unit);
        }
        else
        {
            curEnemy = unit;
            curEnemy.ToggleSelectionVisual(true);
            ShowEnemyUnit(unit);
        }
    }
    private void TrySelect(Vector2 screenPos)
    {
        Ray ray = cam.ScreenPointToRay(screenPos);
        RaycastHit hit;

        //if we left-click something
        if (Physics.Raycast(ray, out hit, 1000, layerMask))
        {
            switch (hit.collider.tag)
            {
                case "Unit":
                    SelectUnit(hit);
                    break;
                case "Building":
                    BuildingSelect(hit);
                    break;
                case "Resource":
                    ResourceSelect(hit);
                    break;
            }
        }
    }
    private void ClearAllSelectionVisual()
    {
        foreach (Unit u in curUnit)
            u.ToggleSelectionVisual(false);
        if (curResource != null)
            curResource.ToggleSelectionVisual(false);       //1//
        if (curBuilding != null)
        {
            curBuilding.ToggleSelectionVisual(false);
        }
        if (curEnemy != null)
            curEnemy.ToggleSelectionVisual(false);
    }
    private void ClearEverything()
    {
        ClearAllSelectionVisual();
        curUnit.Clear();
        curBuilding = null;
        curResource = null;
        curEnemy = null;
        
        
     /*   InfoManager.instance.ClearAllInfo();
        ActionManager.instance.ClearAllInfo();*/

    }
    private void ShowUnit(Unit u)
    {
        InfoManager.instance.ShowAllInfo(u);
        if(u.IsBuilder)
        {
            ActionManager.instance.ShowBuilderMode(u);
        }
    }
    private void ShowBuilding(Building b)
    {
        InfoManager.instance.ShowAllInfo(b);
        ActionManager.instance.ShowCreateUnitMode(b);
    }
    private void BuildingSelect(RaycastHit hit)
    {
        curBuilding = hit.collider.GetComponent<Building>();
        curBuilding.ToggleSelectionVisual(true);

        if (GameManager.instance.MyFaction.IsMyBuilding(curBuilding))
        {
            Debug.Log("my building");
            ShowBuilding(curBuilding);//Show building info
        }
        else
        {
            ShowEnemyBuilding(curBuilding);
        }
    }
    private void UpdateSelectionBox(Vector3 mousePos)
    {
        //Debug.Log("Mouse Pos - " + curMousePos);
        if (!selectionBox.gameObject.activeInHierarchy && curBuilding == null)
            selectionBox.gameObject.SetActive(true);

        float width = mousePos.x - startPos.x;
        float height = mousePos.y - startPos.y;

        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        selectionBox.anchoredPosition = startPos + new Vector2(width / 2, height / 2);

        //store old position for real unit selection
        oldAnchoredPos = selectionBox.anchoredPosition;
    }
    private void ReleaseSelectionBox(Vector2 mousePos)
    {
        //Debug.Log("Step 2 - " + _doubleClickMode);
        Vector2 min; //down-left corner
        Vector2 max; //top-right corner

        selectionBox.gameObject.SetActive(false);

        min = oldAnchoredPos - (selectionBox.sizeDelta / 2);
        max = oldAnchoredPos + (selectionBox.sizeDelta / 2);

        //Debug.Log("min = " + min);
        //Debug.Log("max = " + max);

        foreach (Unit unit in GameManager.instance.MyFaction.AliveUnits)
        {
            Vector2 unitPos = cam.WorldToScreenPoint(unit.transform.position);

            if (unitPos.x > min.x && unitPos.x < max.x && unitPos.y > min.y && unitPos.y < max.y)
            {
                curUnit.Add(unit);
                unit.ToggleSelectionVisual(true);
            }
        }
        selectionBox.sizeDelta = new Vector2(0, 0); //clear Selection Box's size;
    }
    private void ShowEnemyUnit(Unit u)
    {
        InfoManager.instance.ShowEnemyAllInfo(u);
    }
    private void ShowResource()
    {
        InfoManager.instance.ShowAllInfo(curResource);//Show resource info in Info Panel

    }
    private void ResourceSelect(RaycastHit hit)
    {
        curResource = hit.collider.GetComponent<ResourceSource>();
        if (curResource == null)
            return;

        curResource.ToggleSelectionVisual(true);
        ShowResource();//Show resource info
    }
    private void ShowEnemyBuilding(Building b)
    {
        InfoManager.instance.ShowEnemyAllInfo(b);
    }
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }


}
