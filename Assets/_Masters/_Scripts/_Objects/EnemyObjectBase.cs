using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObjectBase : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer m_SpriteRenderer;
    [SerializeField] protected Animator m_Animator;
    [SerializeField] protected float m_MoveSpeed;
    [SerializeField] protected float m_MoveDistance;

    protected bool m_IsAttacking = false;
    private Vector3 m_LeftMostPosition = Vector3.zero;
    private Vector3 m_RightMostPosition = Vector3.zero;


    private float m_Direction = -1f;

    public virtual void idle()
    {
        m_Animator.SetTrigger("Idle");
    }

    public virtual void attack()
    {
        m_IsAttacking = true;
    }

    protected virtual void onHitPlayer(PlayerObject player)
    {
        if (player != null)
        {
            player.die();
        }
    }

    private void OnValidate()
    {
        m_Animator = GetComponent<Animator>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Awake()
    {
        m_RightMostPosition = transform.position;
        m_LeftMostPosition = m_RightMostPosition - new Vector3(m_MoveDistance, 0f, 0f);
    }

    private void Start()
    {
        idle();
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 pos = transform.position;
        pos.x += Time.deltaTime * m_MoveSpeed * m_Direction;
        transform.position = pos;

        if (m_SpriteRenderer.bounds.min.x < m_LeftMostPosition.x)
        {
            m_Direction = 1f;
        }
        else if (m_SpriteRenderer.bounds.max.x > m_RightMostPosition.x)
        {
            m_Direction = -1f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            onHitPlayer(collision.GetComponent<PlayerObject>());
        }
    }
}
