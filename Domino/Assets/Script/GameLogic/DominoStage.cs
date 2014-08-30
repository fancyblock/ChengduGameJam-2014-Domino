using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DominoStage : MonoBehaviour 
{
    public GameObject m_dominoTemplete;
    public Transform m_stage;
    public Camera m_camera;

    List<Domino> m_dominos;

	// Use this for initialization
	void Start () 
    {
        m_dominos = new List<Domino>();
	}
	
	// Update is called once per frame
	void Update () 
    {
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
        // calculate the mouse position and create a domino 
        Domino d = CreateDomino();
        d.transform.localPosition = m_camera.ScreenToWorldPoint(Input.mousePosition) * 384;
    }

    /// <summary>
    /// force to spot 
    /// </summary>
    /// <param name="spot"></param>
    /// <param name="forceDis"></param>
    public int ForceToSpot( Vector2 spot, float forceDis )
    {
        int pushDownCount = 0;

        foreach( Domino d in m_dominos )
        {
            if( d.IsStand() )
            {
                if (d.Push(spot, forceDis) )
                {
                    pushDownCount++;
                }
            }
        }

        return pushDownCount;
    }

}
