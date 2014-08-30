using UnityEngine;
using System.Collections;

public class SpriteAnim : MonoBehaviour 
{
    public UISprite m_sprite;
    public string[] m_frames;
    public float m_interval;

    protected bool m_isPlaying = false;
    protected float m_timer = 0.0f;

	
	// Update is called once per frame
	void Update () 
    {
        if( m_isPlaying )
        {
            int frameIndex = Mathf.FloorToInt(m_timer / m_interval);
            Debug.Log("-- " + frameIndex + "    " + m_timer);
            if( frameIndex >= m_frames.Length )
            {
                gameObject.SetActive(false);
                return;
            }

            if( m_sprite.spriteName != m_frames[frameIndex] )
            {
                //Debug.Log("change sprite " + m_frames[frameIndex]);
                m_sprite.spriteName = m_frames[frameIndex];
            }

            m_timer += Time.fixedDeltaTime;
        }
	}

    /// <summary>
    /// play the animation 
    /// </summary>
    public void Play()
    {
        gameObject.SetActive(true);

        m_timer = 0.0f;
        m_isPlaying = true;
    }

}
