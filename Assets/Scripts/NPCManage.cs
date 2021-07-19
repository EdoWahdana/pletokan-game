using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCManage : MonoBehaviour
{
    GameObject[] npc;
    Transform[] startPos;
    Transform[] dest1;
    [HideInInspector]
    public Transform dest2;
    Transform[] _npc;
    Vector3[] destPoint;

    List<Transform> startPosList = new List<Transform>();
    List<Transform> dest1List = new List<Transform>();
    List<Vector3> destPointList = new List<Vector3>();

    NavMeshAgent[] agents;
    List<NavMeshAgent> agentsList = new List<NavMeshAgent>();
    List<Transform> _npcList = new List<Transform>();

    public GameObject enemyLevel1;
    public GameObject enemyLevel2;
    public GameObject enemyLevel3;
    public GameObject[] spawnArea;
    public float speedEnemy;
    public float m_tempDist;

    private Sugeno sugeno;
    private GameManage gameManage;
    private PlayerHealth playerHealth;
    private EnemyHealth[] enemyHealth;
    private GameObject[] enemies;
    private GameObject enemy;
    private Collider destCollider1;
    private Bounds destBounds1;
    private int level, getLevel, countEnemy, m_tempCountEnemy, m_hitTimer = 0, m_hitDelay = 3;

    [SerializeField]
    float spawnPoint;

    public GameObject padiGroup1;
    public GameObject padiGroup2;
    public GameObject padiGroup3;

    public GameObject destination1;
    public GameObject destination2;

    void Awake()
    {
        level = PlayerPrefs.GetInt("checklevel");
        getLevel = PlayerPrefs.GetInt("level");

        //Ieu jeung nentukeun jenis NPC
        if (level >= 0 && level <= 2){
            enemy = enemyLevel1;
            padiGroup1.SetActive(true);
            destination1.SetActive(true);
        }
        else if (level >= 3 && level <= 5){
            enemy = enemyLevel2;
            padiGroup2.SetActive(true);
            destination1.SetActive(true);
        }
        else if (level >= 6 && level <= 8){
            enemy = enemyLevel3;
            padiGroup3.SetActive(true);
            destination2.SetActive(true);
        }
        else { Debug.Log("NPC Tidak Ditemukan");}

        //Ieu jeung nentukeun jumlah NPC
        switch (level)
        {
            case 0: countEnemy = 5; break;
            case 1: countEnemy = 7; break;
            case 2: countEnemy = 9; break;
            case 3: countEnemy = 5; break;
            case 4: countEnemy = 7; break;
            case 5: countEnemy = 9; break;
            case 6: countEnemy = 5; break;
            case 7: countEnemy = 7; break;
            case 8: countEnemy = 9; break;
            default: countEnemy = 0; break;
        }

        m_tempCountEnemy = countEnemy;
        SpawnEnemy();
    }

    void Start()
    {
        FloydWarshall fw = new FloydWarshall();

        sugeno = GetComponent<Sugeno>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        gameManage = GameObject.FindGameObjectWithTag("GameManage").GetComponent<GameManage>();
        npc = GameObject.FindGameObjectsWithTag("NPC");
        agents = new NavMeshAgent[npc.Length];
        startPos = new Transform[npc.Length];
        dest1 = new Transform[npc.Length];
        dest2 = GameObject.FindGameObjectWithTag("Destination").GetComponent<Transform>();
        _npc = new Transform[npc.Length];
        destPoint = new Vector3[npc.Length];
        enemyHealth = new EnemyHealth[npc.Length];
        destCollider1 = destination1.GetComponent<BoxCollider>();
        destBounds1 = destCollider1.bounds;

        for (int i = 0; i < npc.Length; i++)
        {
            startPos[i] = npc[i].GetComponent<Transform>();
            startPosList.Add(startPos[i]);
            dest1[i] = fw.dest1[i];
            dest1List.Add(dest1[i]);
            destPoint[i] = dest1[i].position;
            destPointList.Add(destPoint[i]);
            enemyHealth[i] = npc[i].GetComponentInChildren<EnemyHealth>();
            agents[i] = npc[i].GetComponent<NavMeshAgent>();
            agentsList.Add(agents[i]);
            _npc[i] = npc[i].GetComponent<Transform>();
            _npcList.Add(_npc[i]);
            agents[i].destination = dest1List[i].position;
            agents[i].speed = speedEnemy;
        }
    }

    void Update()
    {
        m_hitTimer += (int) Time.deltaTime;
        if(agentsList.Count != 0) {
            for (int i = 0; i < npc.Length; i++)
            {
                if (enemyHealth[i])
                {
                    m_tempDist = Vector3.Distance(enemyHealth[i].transform.position, playerHealth.transform.position);
                    if (sugeno.Logic() == "Kabur")
                    {
                        Debug.Log("Kabur");
                        float x = Random.Range(destBounds1.min.x, destBounds1.max.x);
                        float y = enemyHealth[i].transform.position.y;
                        float z = Random.Range(destBounds1.min.z, destBounds1.max.z);
                        Vector3 newDest = new Vector3(x, y, z);
                        agentsList[i].speed = 1;
                        agentsList[i].Warp(enemyHealth[i].transform.position);
                        agentsList[i].SetDestination(newDest);
                        //ShortPathFinding(dest1List[i], newDest, destPointList[i], agentsList[i]);
                    }
                    else if (sugeno.Logic() == "Diam") { 
                        Debug.Log("Diam");
                        ShortPathFinding(dest1List[i], dest2.position, destPointList[i], agentsList[i]);
                    }
                    else if (sugeno.Logic() == "Menyerang") { 
                        Debug.Log("Menyerang");
                        ShortPathFinding(dest1List[i], dest2.position, destPointList[i], agentsList[i]);
                    } 
                }

                if (agentsList[i] && agentsList[i].velocity.magnitude <= 2 && m_hitTimer >= m_hitDelay)
                    HitPlayer();
            }
        }
    }

    public void DecreaseEnemy()
    {
        m_tempCountEnemy--;
        gameManage.AddScore((int) Mathf.Floor(100 / countEnemy));

        if (m_tempCountEnemy == 0) {
            if(level == getLevel){
                if(getLevel != 8){
                    PlayerPrefs.SetInt("level", level+1);
                }
            }
            gameManage.Win();
        }
    }

    private void HitPlayer()
    {
        m_hitTimer = 0;
        playerHealth.TakeDamage(5);
    }

    private void SpawnEnemy()
    {
        enemies = new GameObject[countEnemy];
        
        for(int i=0; i<countEnemy; i++){
            //Random Spawning Area
            int r = Random.Range(0, spawnArea.Length);
            Collider collider = spawnArea[r].GetComponent<MeshCollider>();
            Bounds bounds = collider.bounds;

            //Random Koordinat
            float x = Random.Range(bounds.min.x + spawnPoint, bounds.max.x - spawnPoint);
            float y = bounds.max.y;
            float z = Random.Range(bounds.min.z + spawnPoint, bounds.max.z - spawnPoint);

            Vector3 spawn = new Vector3(x, y, z);
            enemies[i] = Instantiate(enemy, spawn, Quaternion.Euler(new Vector3(0, 0, 0)));
            enemies[i].transform.parent = GameObject.FindGameObjectWithTag("SpawnArea").gameObject.transform.parent;
        }
    }

    private void ShortPathFinding(Transform _dest1, Vector3 _dest2, Vector3 _destPoint, NavMeshAgent _agent)
    {
        if(_agent) {
            //Transform[] dest = { _dest1, _dest2 };
            float dist = Vector3.Distance(_destPoint, _agent.transform.position);
            if (dist < 2f){ 
                _agent.SetDestination(_dest2);
                _agent.speed = speedEnemy;
            }
        }
    }
}
