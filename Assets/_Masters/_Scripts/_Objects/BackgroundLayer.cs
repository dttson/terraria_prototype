using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BackgroundLayer : ParralaxLayer
{
    [SerializeField] private Camera m_Camera;
    [SerializeField] private List<SpriteRenderer> m_ListItems;

    private float m_BgSize = 0f;
    private Rect m_ScreenRect;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        m_Camera = Camera.main;
        m_BgSize = m_ListItems[0].bounds.size.x;
        m_ScreenRect = m_Camera.getScreenRect();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        var bgItem = m_ListItems[0];

        if (m_Speed > 0)
        {    
            if (bgItem.bounds.min.x > m_ScreenRect.max.x)
            {
                var lastBgItem = m_ListItems[m_ListItems.Count - 1];
                bgItem.transform.updateX(lastBgItem.transform.position.x - m_BgSize);
                m_ListItems.RemoveAt(0);
                m_ListItems.Add(bgItem);
            }
        }
        else
        {
            if (bgItem.bounds.max.x < m_ScreenRect.min.x)
            {
                var lastBgItem = m_ListItems[m_ListItems.Count - 1];
                bgItem.transform.updateX(lastBgItem.transform.position.x + m_BgSize);
                m_ListItems.RemoveAt(0);
                m_ListItems.Add(bgItem);
            }
        }
    }

}
