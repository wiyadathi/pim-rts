using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MiniMapBlip : MonoBehaviour
{
    private GameObject blip;
    public GameObject Blip { get { return blip; } }

    Unit unit;
    Building building;

    Faction faction;

    private void Awake()
    {
        unit = GetComponent<Unit>();
        building = GetComponent<Building>();
       // faction = GetComponent<Faction>();

    }
    
    // Start is called before the first frame update
    void Start()
    {
        blip = Instantiate(MiniMap.instance.blipParent);
        
        //debug
        if (blip != null)
            Debug.Log("Instantiate bilp > " + blip.name);
        else
            Debug.Log("blip = null");

        blip.transform.SetParent(MiniMap.instance.blipParent.transform);
        SetColor();
    }

    // Update is called once per frame
    void Update()
    {
        blip.transform.position = MiniMap.instance.worldPosToMinimapPos(transform.position);
        blip.transform.position = MainUI.instance.ScalePosition(blip.transform.position);
    }
    private void OnDestroy()
    {
        Destroy(blip);
    }
    private void SetColor()
    {
        if (unit != null)
            faction = unit.Faction;

        if (building != null)
        {
            faction = building.Faction;
            blip.GetComponent<RectTransform>().sizeDelta = new Vector2(6f, 6f);
        }

        if (faction != null)
        {
            Debug.Log("InMinimapBlip > faction = " + faction);
        }
        
        
        if (faction != null)
            blip.GetComponent<Image>().color = faction.GetNationColor();
        else
        {
            Debug.Log("faction = null and to set color");
            blip.GetComponent<Image>().color = Color.white;
        }
    }
    
    


}
