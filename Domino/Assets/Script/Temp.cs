using UnityEngine;
using System.Collections;

public class Temp : MonoBehaviour 
{
    public DominoStage m_dominoStage;
    public Storage m_storage;
    public UILabel m_txtResult;
    public EndPoint m_endPoint;

    public void Start()
    {
        m_endPoint.HIT_CALLBACK = onHit;
    }

    /// <summary>
    /// reset the game 
    /// </summary>
    public void onReset()
    {
        m_dominoStage.Reset();
        m_storage.Reset( Random.Range( 10, 30 ) );
        m_txtResult.gameObject.SetActive(false);
    }

    protected void onHit()
    {
        m_txtResult.gameObject.SetActive(true);
        m_txtResult.text = "You Win !";
    }
}
