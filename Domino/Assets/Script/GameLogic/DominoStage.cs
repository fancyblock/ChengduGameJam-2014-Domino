using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DominoStage : MonoBehaviour 
{
    protected List<Domino> m_dominos;

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
    /// force to spot 
    /// </summary>
    /// <param name="spot"></param>
    /// <param name="forceDis"></param>
    public void ForceToSpot( Vector2 spot, float forceDis )
    {
        foreach( Domino d in m_dominos )
        {
            if( d.IsStand() )
            {
                d.Push(spot, forceDis);
            }
        }
    }

}
