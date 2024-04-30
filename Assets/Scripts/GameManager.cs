using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Faction myFaction;
    public Faction MyFaction { get { return myFaction; } }

    [SerializeField] private Faction enemyFaction;
    public Faction EnemyFaction { get { return enemyFaction; } }

    //All factions in this game (2 factions for now)
    [SerializeField] private Faction[] factions;

    public static GameManager instance;
    void Awake()
    {
        instance = this;
        SetupPlayers(Settings.mySide, Settings.EnemySide);
    }

    // Start is called before the first frame update
    void Start()
    {
        MainUI.instance.UpdateAllResource(myFaction);
        Debug.Log("my faction = " + myFaction);
        CameraController.instance.FocusOnPosition(myFaction.StartPosition.position);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetupPlayers(Nation myNation, Nation enemyNation)
    {
        foreach (Faction f in factions)
        {
            //Debug.Log("Now is :" + f);
            if (f.Nation == myNation)
            {
                //Debug.Log("My Side is :" + f);
                myFaction = f;

                f.gameObject.AddComponent<UnitSelect>();
                f.gameObject.AddComponent<UnitCommand>();
            }
            else if (f.Nation == enemyNation)//Enemy
            {
                //Debug.Log("Enemy Side is :" + f);
                enemyFaction = f;

                f.gameObject.AddComponent<FactionAI>(); //Routine AI

                f.gameObject.AddComponent<AIController>(); //controller to choose among AI specific commands
                f.gameObject.AddComponent<AISupport>();
                f.gameObject.AddComponent<AIDoNothing>();
                f.gameObject.AddComponent<AIStrike>();
                f.gameObject.AddComponent<AICreateHQ>();
                f.gameObject.AddComponent<AICreateHouse>();
                f.gameObject.AddComponent<AICreateBarrack>();
            }
        }
    }

}
