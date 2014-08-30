using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum eDominoStageState
{
    eStateRunning,
    eStateStop,
    eStatePushFailed,
}

public class DominoStage : MonoBehaviour 
{
    public const int INIT_DEPTH = 1000;

    public GameObject m_dominoTemplete;
    public Transform m_stage;
    public Camera m_camera;
    public EndPoint m_endPoint;

    protected eDominoStageState m_state;
    protected List<Domino> m_dominos = new List<Domino>();
    protected int m_curAvailableDepth = INIT_DEPTH;
	
    //------------------------ TEMP ------------------------
    
	// Update is called once per frame
	void Update () 
    {
        // calculate the mouse position and create a domino 
        if( Input.GetKeyUp( KeyCode.Alpha1 ))
        {
            Domino d = CreateDomino();
            d.transform.localPosition = m_camera.ScreenToWorldPoint(Input.mousePosition) * 384;
        }
	}

    public void onDoubleClk()
    {
        // calculate the mouse position and trigger
        Vector3 pos = m_camera.ScreenToWorldPoint(Input.mousePosition) * 384;
        HitDominos(new Vector2(pos.x, pos.y), 50.0f);
    }

    //------------------------ TEMP ------------------------

    /// <summary>
    /// getter && setter of the domino stage state 
    /// </summary>
    protected eDominoStageState STAGE_STATE
    {
        get
        {
            return m_state;
        }
        set
        {
            m_state = value;
        }
    }

    /// <summary>
    /// remove all the dominos and reset all values 
    /// </summary>
    public void Reset()
    {
        // clean old data 
        foreach( Domino d in m_dominos )
        {
            Destroy(d.gameObject);
        }

        m_curAvailableDepth = INIT_DEPTH;
        m_dominos.Clear();
        m_state = eDominoStageState.eStateRunning;
    }

    /// <summary>
    /// create a domino 
    /// </summary>
    /// <returns></returns>
    public Domino CreateDomino()
    {
        Domino domino = null;

        GameObject go = Instantiate(m_dominoTemplete) as GameObject;
        domino = go.GetComponent<Domino>();

        go.transform.parent = m_stage;
        go.transform.localScale = Vector3.one;

        domino.SetDominoStage(this);
        domino.SetStandState();
        domino.SetAngle(0.0f);

        m_dominos.Add(domino);

        return domino;
    }

    /// <summary>
    /// domino push fail 
    /// </summary>
    public void DominoPushFail()
    {
        //m_state = eDominoStageState.eStatePushFailed;
    }

    /// <summary>
    /// force to spot 
    /// </summary>
    /// <param name="spot"></param>
    /// <param name="forceDis"></param>
    public int ForceToSpot( Vector2 spot, Vector2 dir, float forceDis )
    {
        if( m_state != eDominoStageState.eStateRunning )
        {
            return 0;
        }

        // check if hit the endpoint 
        Vector2 epPos = new Vector2(m_endPoint.transform.localPosition.x, m_endPoint.transform.localPosition.y);
        if( ( epPos - spot ).magnitude < forceDis )
        {
            bool isHitEndPoint = false;

            // More accurate calculations
            //TODO 
            isHitEndPoint = true;       //[TEMP]

            if( isHitEndPoint )
            {
                m_endPoint.HitEndPoint();
            }
        }

        int pushDownCount = 0;

        foreach( Domino d in m_dominos )
        {
            if( d.IsStand() )
            {
                if ( d.Push(spot, dir, forceDis) )
                {
                    pushDownCount++;
                }
            }
        }

        return pushDownCount;
    }

    /// <summary>
    /// hit dominos 
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public int HitDominos( Vector2 pos, float range, Domino domino = null )
    {
        if (m_state != eDominoStageState.eStateRunning)
        {
            return 0;
        }

        int hitCount = 0;

        foreach (Domino d in m_dominos)
        {
            if ( d != domino && d.IsStand())
            {
                if (d.Push( pos, range ))
                {
                    hitCount++;
                }
            }
        }

        return hitCount;
    }

    /// <summary>
    /// return the available depth
    /// </summary>
    /// <returns></returns>
    public int GetNextAvailableDepth()
    {
        return m_curAvailableDepth--;
    }

}
