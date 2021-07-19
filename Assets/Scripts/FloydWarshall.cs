using System.Collections.Generic;
using UnityEngine;

public class FloydWarshall
{
    public GameObject[] npc;
    public Transform[,] dest;
    public Transform[] node;
    public Transform[] _dest;
    public Transform[] dest1;
    public Transform destination;

    public List<Transform> nodeList = new List<Transform>();
    public List<Transform> destList = new List<Transform>(); 

    public float[,] minDist;
    
    public FloydWarshall()
    {
        destination = GameObject.FindGameObjectWithTag("Destination").GetComponent<Transform>();
        npc = GameObject.FindGameObjectsWithTag("NPC");
        
        foreach (GameObject n in npc){
            nodeList.Add(n.transform);
        }
        
        node = nodeList.ToArray();
        Debug.Log("Node : " + node);
        minDist = new float[node.Length,npc.Length];
        dest = new Transform[node.Length, npc.Length];

        for(int k=0; k<node.Length; k++){
            for(int i=0; i<npc.Length; i++){
                minDist[i,k] = Vector3.Distance(node[i].position, destination.position);
                float dist1 = minDist[i,k];
                float dist2 = Vector3.Distance(npc[i].transform.position, node[k].position) + Vector3.Distance(node[k].position, destination.position);
                minDist[i,k] = Mathf.Min(dist1, dist2);
                dest[i, k] = (minDist[i, k] == dist1) ? destination : node[k].transform;
                destination = dest[i, k];
                destList.Add(dest[i,k]);
                _dest = destList.ToArray();
            }
        }
        dest1 = _dest;
    }
}
