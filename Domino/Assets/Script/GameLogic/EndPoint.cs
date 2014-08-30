using UnityEngine;
using System;
using System.Collections;

public class EndPoint : MonoBehaviour 
{
    public Action HIT_CALLBACK { get; set; }

    /// <summary>
    /// hit endpoint 
    /// </summary>
    public void HitEndPoint()
    {
        HIT_CALLBACK();
    }
}
