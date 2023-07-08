using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//************************************************************************************
//
//************************************************************************************

public class Creature : MonoBehaviour
{
    //********************************************************************************
    //
    //********************************************************************************

    [ NonSerialized ] private List< NavMeshAnalytics.EdgeHit > m_edge_hits = null;

    //********************************************************************************
    //
    //********************************************************************************

    private void Update()
    {
        NavMeshAnalytics.FindNearestEdges( transform.position, 2.0f, ref m_edge_hits );
    }

    //********************************************************************************
    //
    //********************************************************************************

    private void OnDrawGizmos()
    {
        if( m_edge_hits == null ) return;

        Color restore = Gizmos.color;

            Gizmos.color = Color.magenta;

            Vector3 size = Vector3.one * 0.5f;

            for( int e = 0, count = m_edge_hits.Count; e < count; ++e )
            {
                NavMeshAnalytics.EdgeHit hit = m_edge_hits[ e ];

                Gizmos.DrawLine( transform.position, hit.p );

                Gizmos.DrawCube( hit.p, size );
            }

        Gizmos.color = restore;
    }
}
