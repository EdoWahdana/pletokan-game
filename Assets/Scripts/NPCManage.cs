using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCManage : MonoBehaviour
{
    [HideInInspector]
    public Transform dest2;

    Animator[] animator;
    GameObject[] npc;
    Transform[] startPos;
    Transform[] dest1;
    Transform[] _npc;
    Vector3[] destPoint;

    List<EnemyHealth> enemyHealthList = new List<EnemyHealth>();
    List<Transform> startPosList = new List<Transform>();
    List<Transform> dest1List = new List<Transform>();
    List<Vector3> destPointList = new List<Vector3>();

    NavMeshAgent[] agents;
    List<NavMeshAgent> agentsList = new List<NavMeshAgent>();
    List<Transform> _npcList = new List<Transform>();
    
    public GameObject padiGroup1;
    public GameObject padiGroup2;
    public GameObject padiGroup3;
    public GameObject destination1;
    public GameObject destination2;
    public GameObject enemyLevel1;
    public GameObject enemyLevel2;
    public GameObject enemyLevel3;
    public GameObject[] spawnArea;
    public float m_tempDist;

    private Sugeno[] sugeno;
    private GameManage gameManage;
    private PlayerHealth playerHealth;
    private EnemyHealth[] enemyHealth;
    private GameObject[] enemies;
    private GameObject enemy;
    private Collider destCollider2;
    private float speedEnemy;
    private bool isAttack = true;
    private float spawnPoint = 2f;
    private float m_hitDelay = speedAttack;
    private int level, getLevel, countEnemy, m_tempCountEnemy;
    private static float speedAttack = 3f;
    
    [HideInInspector]
    public bool isOver = false;


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
            case 0: case 3: case 6: countEnemy = 5; break;
            case 1: case 4: case 7: countEnemy = 7; break;
            case 2: case 5: case 8: countEnemy = 9; break;
            default: countEnemy = 0; break;
        }

        //Ieu jeung nentukeun speed NPC
        switch (level){
            case 0: speedEnemy = 1.5f; break;
            case 1: speedEnemy = 2f; break;
            case 2: speedEnemy = 2.5f; break;
            case 3: speedEnemy = 2.5f; break;
            case 4: speedEnemy = 3f; break;
            case 5: speedEnemy = 3.5f; break;
            case 6: speedEnemy = 3.5f; break;
            case 7: speedEnemy = 4f; break;
            case 8: speedEnemy = 5f; break;
            default: speedEnemy = 0f; break;
        }

        m_tempCountEnemy = countEnemy;
        SpawnEnemy();
    }

    void Start()
    {
        FloydWarshall fw = new FloydWarshall();

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
        destCollider2 = GameObject.FindGameObjectWithTag("Destination").GetComponent<BoxCollider>();
        animator = new Animator[npc.Length];
        sugeno = new Sugeno[npc.Length];

        for (int i = 0; i < npc.Length; i++)
        {
            sugeno[i] = npc[i].GetComponent<Sugeno>();
            startPos[i] = npc[i].GetComponent<Transform>();
            startPosList.Add(startPos[i]);
            dest1[i] = fw.dest1[i+6];
            dest1List.Add(dest1[i]);
            destPoint[i] = dest1[i].position;
            destPointList.Add(destPoint[i]);
            enemyHealth[i] = npc[i].GetComponentInChildren<EnemyHealth>();
            enemyHealthList.Add(enemyHealth[i]);
            agents[i] = npc[i].GetComponent<NavMeshAgent>();
            agentsList.Add(agents[i]);
            _npc[i] = npc[i].GetComponent<Transform>();
            _npcList.Add(_npc[i]);
            //agents[i].destination = dest1List[i].position;
            agents[i].speed = speedEnemy;
            animator[i] = agents[i].GetComponent<Animator>();
        }
    }

    void Update()
    {
        if(agentsList.Count != 0) {
            for (int i = 0; i < npc.Length; i++)
            {
                if (enemyHealthList[i].currentHealth > 0)
                {
                    float dist = Vector3.Distance(playerHealth.transform.position, enemyHealthList[i].transform.position);
                    Vector3 newDest;
                    if (sugeno[i].Logic() == "Kabur")
                    {
                        if (enemyHealthList[i].transform.position.x < playerHealth.transform.position.x) {
                            newDest = new Vector3(Random.Range(0, (dist * 2) - enemyHealthList[i].transform.position.x), enemyHealthList[i].transform.position.y, Random.Range(0, (dist * 2) - enemyHealthList[i].transform.position.z));
                        } else { 
                            newDest = new Vector3((dist * 2) + enemyHealthList[i].transform.position.x, enemyHealthList[i].transform.position.y, (dist * 2) + enemyHealthList[i].transform.position.z);
                        }
                        isAttack = false;
                        agentsList[i].speed = speedEnemy;
                        agentsList[i].SetDestination(newDest);
                    }
                    else if (sugeno[i].Logic() == "Diam") {
                        isAttack = false;
                        agentsList[i].destination = agentsList[i].transform.position;
                    }
                    else if (sugeno[i].Logic() == "Menyerang") { 
                        isAttack = true;
                        ShortPathFinding(dest1List[i], dest2.position, destPoint[i], agentsList[i]);
                    }


                    if ((Vector3.Distance(dest2.position, agentsList[i].transform.position)) <= 20f)
                    {
                        if(!isOver)
                            AttackNPC();
                        
                        animator[i].SetBool("isAttack", true);
                    }
                    else {
                        animator[i].SetBool("isAttack", false);
                    }

                } // End of enemyHealth check
            } // End of for loop
        } // End of agentList.Count
    }

    private void ShortPathFinding(Transform _dest1, Vector3 _dest2, Vector3 _destPoint, NavMeshAgent _agent)
    {
        if (_agent)
        {
            _agent.destination = _dest1.position;
            //Transform[] dest = { _dest1, _dest2 };
            float dist = Vector3.Distance(_destPoint, _agent.transform.position);
            if (dist < 2f)
            {
                _agent.SetDestination(_dest2);
                _agent.speed = speedEnemy;
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

    private void AttackNPC()
    {
        if (isAttack) { 
            m_hitDelay -= Time.deltaTime;
            if(m_hitDelay <= 0f){
                playerHealth.TakeDamage(5);
                m_hitDelay = speedAttack;
            }
        }
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

}
