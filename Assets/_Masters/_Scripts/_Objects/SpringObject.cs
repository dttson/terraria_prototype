using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringObject : MonoBehaviour
{
    [SerializeField] private SpriteRenderer m_SpriteRenderer;
    [SerializeField] private Sprite m_ActiveSprite;
    [SerializeField] private Sprite m_IdleSprite;

    // Start is called before the first frame update
    private void Start()
    {
        m_SpriteRenderer.sprite = m_IdleSprite;
    }

    private void OnValidate()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagDefine.PLAYER))
        {
            var player = collision.GetComponent<PlayerObject>();
            if (player != null)
            {
                m_SpriteRenderer.sprite = m_ActiveSprite;
                player.highJump();
            }
        }
    }
}
