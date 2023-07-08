using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class RunAway : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agent = null;

    [SerializeField]
    private Transform chaser = null;

    [SerializeField]
    private float displacementDist;

    /*UnityEngine.AI.NavMeshHit hit;
    float distanceToEdge = 3;*/

    //public List< NavMeshAnalytics.EdgeHit > m_edge_hits = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (chaser == null)
            return;*/

        float v = Vector3.Distance(transform.position, chaser.position);//Random.Range(0,2);
        /*int ed;
        Debug.Log(v);
        if (Mathf.Sign(v) == 1) {
            ed = Random.Range(0,179); }
        else {
            ed = Random.Range(180,359); }

        Vector3 normDir = (chaser.position - transform.position).normalized;
        normDir = Quaternion.AngleAxis(ed, Vector3.up) * normDir; // Random.Range(0,179)     chaser.position.z - transform.position.z

        MoveToPos(transform.position - (normDir * displacementDist));  // 1 = normDir */

        //Debug.Log(v);
        if (v < displacementDist) {
            Vector3 dirToPlayer = (chaser.position - transform.position).normalized;
            Vector3 newPos = transform.position - (dirToPlayer * 3f);
            agent.SetDestination(newPos);
        }    

        //NavMeshAnalytics.FindNearestEdges( transform.position, 2.0f, ref m_edge_hits );

        /*if (UnityEngine.AI.NavMesh.FindClosestEdge(transform.position, out hit, UnityEngine.AI.NavMesh.AllAreas))
                    {
                        distanceToEdge = hit.distance;
                    }
 
        if (distanceToEdge < 3f)
        {
            Debug.Log("Corner");
        }*/
    }

    void OnEnable() {
        agent.stoppingDistance = 0;
    }

    /*private void MoveToPos(Vector3 pos) {
        agent.SetDestination(pos);  //* (-1)
        agent.isStopped = false;
    }*/
}
