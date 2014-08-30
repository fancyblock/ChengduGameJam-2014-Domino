﻿using UnityEngine;
using System.Collections;

public class Domino : MonoBehaviour 
{
    public const int STATE_STAND = 0;
    public const int STATE_LAY = 1;
    public const int STATE_OVERLAY = 2;
    public const int STATE_LAYING = 3;

    public GameObject m_imgStand;
    public GameObject m_imgOverlay;
    public GameObject m_imgLay;
    public float m_blockLength;

    protected DominoStage m_stage;
    protected int m_state;
    protected float m_angle;
    protected Vector2 m_dir;
    protected bool m_isForward;

	// Use this for initialization
	void Start () 
    {
        SetStandState();
        SetAngle(0.0f);
	}
	
	// Update is called once per frame
	void Update () 
    {
	}

    /// <summary>
    /// set domino stage 
    /// </summary>
    /// <param name="ds"></param>
    public void SetDominoStage( DominoStage ds )
    {
        m_stage = ds;
    }

    /// <summary>
    /// is stand or not 
    /// </summary>
    /// <returns></returns>
    public bool IsStand()
    {
        return m_state == STATE_STAND;
    }

    /// <summary>
    /// set stand state 
    /// </summary>
    public void SetStandState()
    {
        m_imgStand.SetActive(true);
        m_imgLay.SetActive(false);
        m_imgOverlay.SetActive(false);

        m_state = STATE_STAND;
    }

    /// <summary>
    /// set angle 
    /// </summary>
    /// <param name="angle"></param>
    public void SetAngle( float angle )
    {
        m_angle = angle;

        while( m_angle >= 360.0f )
        {
            m_angle -= 360.0f;
        }
        while( m_angle < 0.0f )
        {
            m_angle += 360.0f;
        }

        // set vector 
        float ang = m_angle * Mathf.PI / 180.0f;
        m_dir.x = Mathf.Cos(ang);
        m_dir.y = Mathf.Sin(ang);

        gameObject.transform.localRotation = Quaternion.AngleAxis( angle, Vector3.forward );
    }

    /// <summary>
    /// rotation the domino 
    /// </summary>
    public void onEditRotation()
    {
        //[TEMP]
        SetAngle(m_angle + 30.0f);
        //[TEMP]
    }

    /// <summary>
    /// push the domino 
    /// </summary>
    /// <param name="spot"></param>
    /// <param name="forceDis"></param>
    public bool Push(Vector2 spot, Vector2 dir, float forceDis)
    {
        Vector2 selfPos = new Vector2(transform.localPosition.x, transform.localPosition.y);
        
        if( ( selfPos - spot ).magnitude < forceDis )   // pretest 
        {
            // calculate if this domino can be push down or not 
            return pushDomino(spot, dir, forceDis, selfPos);
        }

        return false;
    }


    //---------------------- private functions ----------------------- 
    

    /// <summary>
    /// push the domino 
    /// </summary>
    /// <param name="forceSpot"></param>
    /// <param name="forceDir"></param>
    /// <param name="forceDis"></param>
    /// <param name="selfPos"></param>
    /// <returns></returns>
    protected bool pushDomino( Vector2 forceSpot, Vector2 forceDir, float forceDis, Vector2 selfPos )
    {
        Vector2 forceToDomino = selfPos - forceSpot;

        if( Vector2.Dot(forceToDomino, forceDir) * forceDis >= forceToDomino.magnitude )
        {
            forceToDomino.Normalize();

            if( Vector2.Dot( forceToDomino, m_dir ) > 0.0f )
            {
                downOver(true);
            }
            else
            {
                downOver(false);
            }

            return true;
        }

        return false;
    }

    /// <summary>
    /// down over 
    /// </summary>
    /// <param name="isForward"></param>
    protected void downOver( bool isForward )
    {
        m_state = STATE_LAYING;
        m_isForward = isForward;

        // set domino depth 
        setDominoDepth(m_stage.GetNextAvailableDepth());

        // set the display object to right direction 
        if( isForward )
        {
            //TODO 
        }
        else
        {
            //TODO 
        }

        StartCoroutine("onDowning");
    }

    /// <summary>
    /// downing forward 
    /// </summary>
    /// <returns></returns>
    protected IEnumerator onDowning()
    {
        yield return new WaitForFixedUpdate();

        m_imgStand.SetActive(false);

        // play laying animation 
        //TODO 

        yield return new WaitForSeconds(0.2f);

        // try to push next 
        int pushedCount = m_stage.ForceToSpot(new Vector2(transform.localPosition.x, transform.localPosition.y),
            m_isForward ? m_dir : m_dir * -1, m_blockLength);

        if( pushedCount > 0 )
        {
            m_imgOverlay.SetActive(true);
        }
        else
        {
            m_imgLay.SetActive(true);
        }
    }

    /// <summary>
    /// set domino depth 
    /// </summary>
    /// <param name="depth"></param>
    protected void setDominoDepth( int depth )
    {
        m_imgOverlay.GetComponent<UISprite>().depth = depth;
    }

}