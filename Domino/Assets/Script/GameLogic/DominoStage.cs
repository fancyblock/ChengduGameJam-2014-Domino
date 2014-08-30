using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DominoStage : MonoBehaviour 
{
    public const int INIT_DEPTH = 1000;

    public GameObject m_dominoTemplete;
    public Transform m_stage;
    public Camera m_camera;

    protected List<Domino> m_dominos = new List<Domino>();
    protected int m_curAvailableDepth = INIT_DEPTH;

	// Use this for initialization
	void Start () 
    {
	}
	
	// Update is called once per frame
	void Update () 
    {
        //[TEMP]
        // calculate the mouse position and create a domino 
        if( Input.GetKeyUp( KeyCode.Alpha1 ))
        {
            Domino d = CreateDomino();
            d.transform.localPosition = m_camera.ScreenToWorldPoint(Input.mousePosition) * 384;
        }
        //[TEMP]
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
    /// mouse double click on stage for trigger a force 
    /// </summary>
    public void onMouseClick()
    {
        //[TEMP]
        // calculate the mouse position and trigger
        Vector3 pos = m_camera.ScreenToWorldPoint(Input.mousePosition) * 384;
        ForceToSpot(new Vector2(pos.x, pos.y), new Vector2( 1.0f, 0.0f ), 70);
        ForceToSpot(new Vector2(pos.x, pos.y), new Vector2( -1.0f, 0.0f ), 70);
        ForceToSpot(new Vector2(pos.x, pos.y), new Vector2( 0.0f, 1.0f ), 70);
        ForceToSpot(new Vector2(pos.x, pos.y), new Vector2( 0.0f, -1.0f ), 70);
        //[TEMP]
    }

    /// <summary>
    /// force to spot 
    /// </summary>
    /// <param name="spot"></param>
    /// <param name="forceDis"></param>
    public int ForceToSpot( Vector2 spot, Vector2 dir, float forceDis )
    {
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
    /// return the available depth
    /// </summary>
    /// <returns></returns>
    public int GetNextAvailableDepth()
    {
        return m_curAvailableDepth--;
    }

}
