using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParralaxLayer : MonoBehaviour
{
    public static bool IsActive { get; set; } = true;

    [SerializeField] protected float m_Speed = 1.0f;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (IsActive)
        {
            transform.Translate(m_Speed * Time.deltaTime, 0f, 0f);
        }
    }
}
