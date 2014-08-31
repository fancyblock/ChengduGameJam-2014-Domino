using UnityEngine;
using System.Collections;

public class Storage : MonoBehaviour 
{
    public DominoStage m_dominoStage;
    public UILabel m_txtRestCount;
    public int m_count;

	// Use this for initialization
	void Start () 
    {
        m_txtRestCount.text = "Rest count: " + m_count;
	}

    /// <summary>
    /// reset the storage 
    /// </summary>
    /// <param name="count"></param>
    public void Reset( int count )
    {
        m_count = count;
        m_txtRestCount.text = "Rest count: " + m_count;
    }
	
    /// <summary>
    /// create a domino 
    /// </summary>
    public void onCreateDonimo()
    {
        if( m_count > 0 )
        {
            Domino domino = m_dominoStage.CreateDomino();
            domino.transform.localPosition = transform.localPosition;

            m_count--;
            m_txtRestCount.text = "Rest count: " + m_count;
        }
    }
}
