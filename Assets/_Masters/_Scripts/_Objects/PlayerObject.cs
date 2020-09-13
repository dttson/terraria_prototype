using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using DG.Tweening;

public enum PlayerState
{
    NONE,
    INTRO,
    RUN,
    JUMP,
    FALL,
    HIT,
    DIE
}

public class PlayerObject : MonoBehaviour
{
    [SerializeField] private SpriteRenderer m_SpriteRenderer;
    [SerializeField] private Animator m_Animator;
    [SerializeField] private Rigidbody2D m_RigidBody;
    [SerializeField] private float m_JumpSpeed = 5.0f;
    [SerializeField] private float m_DeAcceleration = 1.0f;

    public delegate void FinishIntroDelegate();
    public FinishIntroDelegate OnFinishIntro { get; set; }

    public PlayerState State { get; private set; } = PlayerState.NONE;
    public Vector3 OriginPos { get; private set; } = Vector3.zero;

    private bool m_IsOnTheAir = false;
    private Vector3 m_JumpPosY = Vector3.zero;
    private Vector3 m_LastPostion = Vector3.zero;

    public void appear()
    {
        gameObject.SetActive(true);
        m_Animator.SetTrigger("Appear");
        m_RigidBody.gravityScale = 0f;
    }

    public void onFinishAppear()
    {
        m_RigidBody.gravityScale = 3f;
    }

    public void intro()
    {
        State = PlayerState.INTRO;
        run();

        IEnumerator runToCenter()
        {
            float t = 0f;
            float srcX = transform.position.x;
            float destX = 0f;
            float duration = 1f;
            while (t < duration)
            {
                var pos = transform.position;
                pos.x = Mathf.Lerp(srcX, destX, t / duration);
                transform.position = pos;

                yield return null;
                t += Time.deltaTime;
            }

            OnFinishIntro?.Invoke();
        }

        StartCoroutine(runToCenter());
    }

    public void run()
    {
        State = PlayerState.RUN;
        m_IsOnTheAir = false;
        m_Animator.SetTrigger("Run");
    }

    public void jump()
    {
        State = PlayerState.JUMP;
        m_IsOnTheAir = true;

        m_RigidBody.velocity = Vector2.up * m_JumpSpeed;
        //m_RigidBody.AddForce(Vector2.up * m_JumpSpeed);
        m_Animator.SetTrigger("Jump");
    }

    public void highJump()
    {
        State = PlayerState.JUMP;
        m_IsOnTheAir = true;

        m_RigidBody.velocity = Vector2.up * m_JumpSpeed * 1.3f;
        //m_RigidBody.AddForce(Vector2.up * m_JumpSpeed);
        m_Animator.SetTrigger("Jump");
    }

    public void fall()
    {
        State = PlayerState.FALL;
        m_Animator.SetTrigger("Fall");
    }

    public void hit()
    {
        Debug.Log("Player hit");
        State = PlayerState.HIT;
        m_Animator.enabled = false;
        Destroy(m_RigidBody);

        transform.DOKill();
        var destPos = transform.position - new Vector3(0f, 10f, 0f);
        transform.DOJump(destPos, 10f, 1, 1f).SetDelay(0.5f);

        GameScene.Instance.gameOver();
    }

    public void die()
    {
        State = PlayerState.DIE;
        
        GameScene.Instance.gameOver();
    }

    public void stopAllActions()
    {
        m_Animator.enabled = false;
    }

    private void OnValidate()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_RigidBody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
    }

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (GameScene.CurrentGameState != GameState.PLAYING)
        {
            return;
        }

        if (hasInput() && !m_IsOnTheAir)
        {
            jump();
        }

        //Debug.Log($"m_LastPostion={m_LastPostion}, y = {transform.position.y}");

        if (m_IsOnTheAir)
        {
            var veloc = m_RigidBody.velocity;
            veloc.y -= m_DeAcceleration * Time.deltaTime;
            m_RigidBody.velocity = veloc;

            if (m_RigidBody.velocity.y < 0f)
            {
                fall();
            }
        }

        m_LastPostion = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            if (State == PlayerState.NONE)
            {
                //run to center => start game
                intro();
                OriginPos = transform.position;
                Debug.Log("Origin pos = " + OriginPos);
            }
            else
            {
                run();
            }
        }
    }

    private bool hasInput()
    {
#if UNITY_EDITOR
        return Input.GetKeyDown(KeyCode.Space);
#else
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
#endif
    }
}
