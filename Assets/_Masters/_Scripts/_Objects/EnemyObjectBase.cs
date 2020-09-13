using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    IDLE,
    ATTACK
}

public class EnemyObjectBase : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer m_SpriteRenderer;
    //[SerializeField] protected Animator m_Animator;
    [SerializeField] protected bool m_CanMove = true;
    [SerializeField] protected float m_MoveSpeed;
    [SerializeField] protected float m_MoveDistance;
    [SerializeField] protected bool m_IsMoveToLeftFist = true;

    [SerializeField] private Vector3 m_LeftMostPosition = Vector3.zero;
    [SerializeField] private Vector3 m_RightMostPosition = Vector3.zero;

    [SerializeField] private Sprite[] m_IdleFrames;

    protected EnemyState State { get; set; } = EnemyState.IDLE;
    private float m_Direction = -1f;

    public virtual void idle()
    {
        State = EnemyState.IDLE;

        if (m_IdleFrames.Length > 0)
        {
            StopAllCoroutines();
            StartCoroutine(coroutineAnimIdle());
        }
    }

    private IEnumerator coroutineAnimIdle()
    {
        int index = 0;
        WaitForSeconds waitForSeconds = new WaitForSeconds(m_MoveSpeed * Time.fixedDeltaTime);
        while (true)
        {
            m_SpriteRenderer.sprite = m_IdleFrames[index];

            index = (index + 1) % m_IdleFrames.Length;
            yield return waitForSeconds;
        }
    }

    public virtual void attack()
    {
        if (State == EnemyState.ATTACK)
        {
            return;
        }

        State = EnemyState.ATTACK;
    }

    protected virtual void onHitPlayer(PlayerObject player)
    {
        Debug.Log($"Enemy {gameObject.name} hit player");

        if (player != null)
        {
            player.hit();
        }
    }

    #region UNITY METHODS
    private void Awake()
    {
        m_RightMostPosition = transform.localPosition;
        m_LeftMostPosition = m_RightMostPosition - new Vector3(m_MoveDistance, 0f, 0f);

        if (m_IsMoveToLeftFist)
        {
            m_SpriteRenderer.flipX = false;
            m_Direction = -1f;
        }
        else
        {
            m_SpriteRenderer.flipX = true;
            m_Direction = 1f;
        }
    }

    private void Start()
    {
        idle();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!m_CanMove)
        {
            return;
        }

        Vector3 pos = transform.localPosition;
        pos.x += Time.deltaTime * m_MoveSpeed * m_Direction;
        transform.localPosition = pos;

        Debug.Log("====== " + m_LeftMostPosition + " - " + m_RightMostPosition);
        if (transform.localPosition.x < m_LeftMostPosition.x)
        {
            m_SpriteRenderer.flipX = true;
            m_Direction = 1f;
        }
        else if (transform.localPosition.x > m_RightMostPosition.x)
        {
            m_SpriteRenderer.flipX = false;
            m_Direction = -1f;
        }

        //if enemy appear on screen (which means near player), then run attack action
        if (isOnScreen())
        {
            attack();
        }
    }

    private void OnValidate()
    {
        m_RightMostPosition = transform.localPosition;
        m_LeftMostPosition = m_RightMostPosition - new Vector3(m_MoveDistance, 0f, 0f);
    }

    private void OnDrawGizmos()
    {
        var tilemap = transform.parent;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(tilemap.TransformPoint(m_RightMostPosition), 0.1f);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(tilemap.TransformPoint(m_LeftMostPosition), 0.1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            onHitPlayer(collision.GetComponent<PlayerObject>());
        }
    }
    #endregion UNITY METHODS

    #region Ultilities
    private bool isOnScreen()
    {
        var screenRect = GameScene.Instance.ScreenRect;
        return screenRect.Contains(new Vector2(transform.position.x, transform.position.y));
    }
    #endregion Ultilities
}
