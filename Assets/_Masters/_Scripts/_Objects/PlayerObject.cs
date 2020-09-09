using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerObject : MonoBehaviour
{
    [SerializeField] private SpriteRenderer m_SpriteRenderer;
    [SerializeField] private Animator m_Animator;
    [SerializeField] private Rigidbody2D m_RigidBody;
    [SerializeField] private float m_JumpSpeed = 5.0f;

    private bool m_IsJumping = false;
    private Vector3 m_OriginPosY = Vector3.zero;
    private Vector3 m_JumpPosY = Vector3.zero;

    private Vector3 m_LastPostion = Vector3.zero;


    public void run()
    {
        m_IsJumping = false;
        m_Animator.SetTrigger("Run");
    }

    public void jump()
    {
        m_IsJumping = true;

        m_RigidBody.AddForce(Vector2.up * m_JumpSpeed);
        m_Animator.SetTrigger("Jump");
    }

    public void fall()
    {
        m_Animator.SetTrigger("Fall");
    }

    public void die()
    {
        m_Animator.SetTrigger("Hit");
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
        m_OriginPosY = transform.position;
        m_JumpPosY = transform.position + Vector3.up*0.5f;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !m_IsJumping)
        {
            jump();
        }

        Debug.Log($"m_LastPostion={m_LastPostion}, y = {transform.position.y}");

        if (m_IsJumping)
        {
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
            run();
        }
    }
}
