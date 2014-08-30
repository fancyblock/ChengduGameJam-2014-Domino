using UnityEngine;
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
    public GameObject m_hintFrame;
    public SpriteAnim m_aniDowning;
    public Transform m_aniContainer;
    public UIDragObject m_dragObject;
    public float m_blockLength;

    protected DominoStage m_stage;
    protected int m_state;
    protected float m_angle;
    protected Vector2 m_dir;
    protected bool m_isForward;

    protected float m_timer = 0.0f;    // for trigger in edit mode 

	// Use this for initialization
	void Start () 
    {
        SetStandState();
        SetAngle(0.0f);
        m_hintFrame.SetActive(false);
        m_aniDowning.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
    {
        if( m_state == STATE_STAND )
        {
            if (m_dragObject.IN_DRAGGING)
            {
                m_hintFrame.SetActive(true);

                // trigger ( in edit mode, you still can put down other dominos )
                m_timer += Time.fixedDeltaTime;
                if( m_timer > 0.3f )
                {
                    m_timer = 0.0f;
                    m_stage.HitDominos(new Vector2(transform.localPosition.x, transform.localPosition.y), 20.0f, this);
                }
            }
            else
            {
                m_hintFrame.SetActive(false);
            }
        }
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
        SetAngle(m_angle + 30.0f);
    }

    /// <summary>
    /// push the domino by a vector 
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

    /// <summary>
    /// push by a point 
    /// </summary>
    /// <param name="spot"></param>
    /// <param name="forceDis"></param>
    /// <returns></returns>
    public bool Push( Vector2 spot, float forceDis )
    {
        Vector2 selfPos = new Vector2(transform.localPosition.x, transform.localPosition.y);

        if ((selfPos - spot).magnitude < forceDis)   // pretest 
        {
            // calculate if this domino can be push down or not 
            pushDomino(spot, forceDis, selfPos);

            return true;
        }

        return false;
    }


    //---------------------- private functions ----------------------- 
    
    /// <summary>
    /// push domino 
    /// </summary>
    /// <param name="forcePoint"></param>
    /// <param name="forceRange"></param>
    /// <param name="selfPos"></param>
    /// <returns></returns>
    protected void pushDomino( Vector2 forcePoint, float forceRange, Vector2 selfPos )
    {
        Vector2 forceToDomino = selfPos - forcePoint;

        forceToDomino.Normalize();

        if (Vector2.Dot(forceToDomino, m_dir) > 0.0f)
        {
            // down forward 
            downOver(true);
        }
        else
        {
            // down back 
            downOver(false);
        }
    }

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
                // down forward 
                downOver(true);
            }
            else
            {
                // down back 
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
            m_aniContainer.localRotation = Quaternion.AngleAxis(0.0f, Vector3.forward);
        }
        else
        {
            m_aniContainer.localRotation = Quaternion.AngleAxis(180.0f, Vector3.forward);
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

        // play laying animation 
        m_aniDowning.Play();

        yield return new WaitForFixedUpdate();
        m_imgStand.SetActive(false);

        yield return new WaitForSeconds(0.14f);

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

            // fail ( Game Over ) 
            m_stage.DominoPushFail();
        }
    }

    /// <summary>
    /// set domino depth 
    /// </summary>
    /// <param name="depth"></param>
    protected void setDominoDepth( int depth )
    {
        m_imgOverlay.GetComponent<UISprite>().depth = depth;
        m_imgLay.GetComponent<UISprite>().depth = depth;
        m_aniDowning.GetComponent<UISprite>().depth = depth;
    }

}
