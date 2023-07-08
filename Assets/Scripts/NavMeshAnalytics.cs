using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//************************************************************************************
//
//************************************************************************************

public class NavMeshAnalytics : MonoBehaviour
{
    //********************************************************************************
    //
    //********************************************************************************

    public struct Edge { public int v0, v1; public Edge( int param_v0, int param_v1 ) { v0 = param_v0; v1 = param_v1; } }

    public struct Area { public List< int > edges; }

    //********************************************************************************
    //
    //********************************************************************************

    static private NavMeshTriangulation NAVMESH_DATAS      = default;

    static private List< Edge >         NAVMESH_BOUNDARIES = new List< Edge     >( 8  );

    static private List< Area >         NAVMESH_AREAS      = new List< Area     >( 8  );

    static private List< Vector3  >     NAVMESH_CORNERS    = new List< Vector3  >( 64 );

    //********************************************************************************
    //
    //********************************************************************************

    static public void Clear()
    {
        NAVMESH_BOUNDARIES.Clear();

        NAVMESH_AREAS.Clear     ();

        NAVMESH_CORNERS.Clear   ();
    }

    //********************************************************************************
    //
    //********************************************************************************

    static public void Bake()
    {
        Clear();

        NAVMESH_DATAS = NavMesh.CalculateTriangulation();

        BakeBoundaries();

        BakeAreas     ();
                      
        BakeCorners   ();
    }

    //********************************************************************************
    //
    //********************************************************************************

    static private bool TriContains( Vector3[] verts, int[] tris, int t, int v )
    {
        if( verts == null )
        {
            if( v == tris[ t     ] ) return true;

            if( v == tris[ t + 1 ] ) return true;

            if( v == tris[ t + 2 ] ) return true;
        }
        else
        {
            Vector3 pos = verts[ v ];

            if( pos == verts[ tris[ t     ] ] ) return true;

            if( pos == verts[ tris[ t + 1 ] ] ) return true;

            if( pos == verts[ tris[ t + 2 ] ] ) return true;
        }


        return false;
    }

    //********************************************************************************
    //
    //********************************************************************************

    static private List< int > GetAdjacentTris( int[] tris, int v )
    {
        List< int > result = new List< int >( 8 );

        for( int t = 0, nb_tris = tris.Length; t < nb_tris; t += 3 )
        {
            if( TriContains( null, tris, t, v ) ) result.Add( t );
        }

        return result;
    }

    //********************************************************************************
    //
    //********************************************************************************

    static private bool EdgesAreConnected( Vector3[] verts, Edge e0, Edge e1 )
    {
        if( verts == null )
        {
            if( ( e0.v0 == e1.v0 ) || ( e0.v0 == e1.v1 ) ) return true;

            if( ( e0.v1 == e1.v0 ) || ( e0.v1 == e1.v1 ) ) return true;
        }
        else
        {
            Vector3 e0_v0 = verts[ e0.v0 ];

            Vector3 e1_v0 = verts[ e1.v0 ];

            Vector3 e1_v1 = verts[ e1.v1 ];

            if( ( e0_v0 == e1_v0 ) || ( e0_v0 == e1_v1 ) ) return true;


            Vector3 e0_v1 = verts[ e0.v1 ];

            if( ( e0_v1 == e1_v0 ) || ( e0_v1 == e1_v1 ) ) return true;
        }

        return false;
    }

    //********************************************************************************
    //
    //********************************************************************************

    static private bool IsBoundaryEdge( Vector3[] verts, int[] tris, int t, int v0, int v1 )
    {
        for( int other = 0, nb_tris = tris.Length; other < nb_tris; other += 3 )
        {
            if( other != t )
            {
                if( TriContains( verts, tris, other, v0 ) && TriContains( verts, tris, other, v1 ) )
                {
                    return false;
                }
            }
        }

        return true;
    }

    //********************************************************************************
    //
    //********************************************************************************

    static private List< Edge > GetBoundaryEdges( Vector3[] verts, int[] tris )
    {
        List< Edge > edges = new List< Edge >( 16 );

        for( int t = 0, nb_tris = tris.Length; t < nb_tris; t += 3 )
        {
            Edge e0 = new Edge( tris[ t     ], tris[ t + 1 ] );

            Edge e1 = new Edge( tris[ t + 1 ], tris[ t + 2 ] );

            Edge e2 = new Edge( tris[ t + 2 ], tris[ t     ] );

            if( IsBoundaryEdge( verts, tris, t, e0.v0, e0.v1 ) ) edges.Add( e0 );

            if( IsBoundaryEdge( verts, tris, t, e1.v0, e1.v1 ) ) edges.Add( e1 );

            if( IsBoundaryEdge( verts, tris, t, e2.v0, e2.v1 ) ) edges.Add( e2 );
        }

        return edges;
    }

    //********************************************************************************
    //
    //********************************************************************************

    static private bool GetNextConnectedEdge( Vector3[] verts, List< Edge > edges, int e_bgn, ref int e_cur, BitArray consumed )
    {
        for( int other = 0, count = edges.Count; other < count; ++other )
        {
            if( consumed.Get( other ) == false )
            {
                if( EdgesAreConnected( verts, edges[ e_cur ], edges[ other ] ) ) 
                {
                    e_cur = other;

                    return   true;
                }
            }
        }

        return false;
    }

    //********************************************************************************
    //
    //********************************************************************************

    static private void BakeBoundaries()
    {
        NAVMESH_BOUNDARIES.Clear();

        NAVMESH_BOUNDARIES = GetBoundaryEdges( NAVMESH_DATAS.vertices, NAVMESH_DATAS.indices );

        Debug.Log( $"NavmeshAnalytics: baked {NAVMESH_BOUNDARIES.Count} boundary edges" );
    }

    //********************************************************************************
    //
    //********************************************************************************

    static private void BakeAreas()
    {
        NAVMESH_AREAS.Clear();

        Vector3[]   verts = NAVMESH_DATAS.vertices;

        BitArray consumed = new BitArray( NAVMESH_BOUNDARIES.Count );

        for( int e = 0, count = NAVMESH_BOUNDARIES.Count; e < count; ++e )
        {
            if( consumed.Get( e ) == false )
            {
                Area  area = new Area(); 

                area.edges = new List< int >( 16 );

                NAVMESH_AREAS.Add( area );


                int e_bgn = e;

                int e_cur = e;

                area.edges.Add( e_bgn );

                consumed.Set( e_bgn, true );

                while( GetNextConnectedEdge( verts, NAVMESH_BOUNDARIES, e_bgn, ref e_cur, consumed ) )
                {
                    area.edges.Add( e_cur );

                    consumed.Set( e_cur, true );
                }

                for( int E = 0; E < area.edges.Count; ++E )
                {
                    int  i0 = area.edges[ E ];

                    int  i1 = area.edges[ ( E + 1 ) % area.edges.Count ];

                    Edge e0 = NAVMESH_BOUNDARIES[ i0 ];

                    Edge e1 = NAVMESH_BOUNDARIES[ i1 ];

                    if( verts[ e0.v1 ] != verts[ e1.v0 ] )
                    {
                        int tmp = e1.v1;

                        e1.v1 = e1.v0;

                        e1.v0 = tmp;

                        NAVMESH_BOUNDARIES[ i1 ] = e1;
                    }
                }
            }
        }

        Debug.Log( $"NavmeshAnalytics: baked {NAVMESH_AREAS.Count} areas" );
    }

    //********************************************************************************
    //
    //********************************************************************************

    static private void BakeCorners()
    {
        NAVMESH_CORNERS.Clear();

        Vector3[] verts = NAVMESH_DATAS.vertices;

        for( int a = 0; a < NAVMESH_AREAS.Count; ++a )
        {
            Area area = NAVMESH_AREAS[ a ];

            for( int e = 0; e < area.edges.Count; ++e )
            {
                Edge    e0 = NAVMESH_BOUNDARIES[ area.edges[ e ] ];
                        
                Edge    e1 = NAVMESH_BOUNDARIES[ area.edges[ ( e + 1 ) % area.edges.Count ] ];

                Vector3 s0 = ( verts[ e0.v1 ] - verts[ e0.v0 ] );

                Vector3 s1 = ( verts[ e1.v1 ] - verts[ e1.v0 ] );

                if( ( 180f - Vector3.Angle( s0, s1 ) ) <= 90.0f )
                {
                    NAVMESH_CORNERS.Add( verts[ e0.v1 ] );
                }
            }
        }

        Debug.Log( $"NavmeshAnalytics: baked {NAVMESH_CORNERS.Count} corners" );
    }

    //********************************************************************************
    //
    //********************************************************************************
    
    static public bool IsNearCorner( Vector3 pos, float dist )
    {
        float sqr_dist = dist * dist;

        for( int c = 0; c < NAVMESH_CORNERS.Count; ++c )
        {
            Vector3 corner = NAVMESH_CORNERS[ c ];

            Vector3 sep    = corner - pos;

            if( sep.sqrMagnitude < sqr_dist ) return true;
        }

        return false;
    }
    
    //********************************************************************************
    //
    //********************************************************************************

    static private bool GetNearestPointOnEdge( Vector3 pos, Edge edge, float sqr_dist, ref Vector3 p )
    {
        Vector3 v0 = NAVMESH_DATAS.vertices[ edge.v0 ];

        Vector3 v1 = NAVMESH_DATAS.vertices[ edge.v1 ];

        Vector3 V  = v1  - v0;

        Vector3 v  = pos - v0;

        if     ( Vector3.Dot( v,        V       ) <= 0.0f ) p = v0;

        else if( Vector3.Dot( pos - v1, v0 - v1 ) <= 0.0f ) p = v1;

        else   { Vector3 unit_V = V.normalized; p = v0 + ( Vector3.Dot( v, unit_V ) * unit_V ); }

        return ( p - pos ).sqrMagnitude <= sqr_dist;
    }

    //********************************************************************************
    //
    //********************************************************************************
    
    public struct EdgeHit 
    { 
        public Vector3 p; 
        
        public EdgeHit( Vector3 param_p ) { p = param_p; } 
    }

    static public bool FindNearestEdges( Vector3 pos, float dist, ref List< EdgeHit > edges )
    {
        if( edges == null ) edges = new List< EdgeHit >( 8 );

        else                edges.Clear();


        float   sqr_dist = dist * dist;

        Vector3 p = Vector3.zero;

        for( int e = 0, nb_edges = NAVMESH_BOUNDARIES.Count; e < nb_edges; ++e )
        {
            if( GetNearestPointOnEdge( pos, NAVMESH_BOUNDARIES[ e ], sqr_dist, ref p ) )
            {
                edges.Add( new EdgeHit( p ) ); 
            }
        }

        return edges.Count > 0;
    }
    
    //********************************************************************************
    //
    //********************************************************************************

    private void Awake() { Bake(); }

    //********************************************************************************
    //
    //********************************************************************************

    private void OnDrawGizmos() 
    { 
        Color restore = Gizmos.color;

            Vector3[] verts = NAVMESH_DATAS.vertices;

                int[]  tris  = NAVMESH_DATAS.indices;

            int    nb_verts = ( verts != null ) ? verts.Length : 0;

            int    nb_tris  = ( tris  != null ) ? tris.Length  : 0;

            if( nb_verts > 0 )
            {
                Gizmos.color  = Color.red;

                Vector3  size = Vector3.one * 0.2f;

                for( int v = 0; v < nb_verts; ++v ) 
                {
                    Gizmos.DrawCube( verts[ v ], size );
                }


                Gizmos.color = Color.blue;

                for( int a = 0; a < NAVMESH_AREAS.Count; ++a )
                {
                    Area area = NAVMESH_AREAS[ a ];

                    for( int e = 0; e < area.edges.Count; ++e )
                    {
                        Edge edge = NAVMESH_BOUNDARIES[ area.edges[ e ] ];

                        Gizmos.DrawLine( verts[ edge.v0 ], verts[ edge.v1 ] );
                    }
                }


                Gizmos.color = Color.yellow;

                for( int c = 0; c < NAVMESH_CORNERS.Count; ++c )
                {
                    Gizmos.DrawSphere( NAVMESH_CORNERS[ c ], 0.5f );
                }
            }

        Gizmos.color = restore;
    }
}
