using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelBase : MonoBehaviour
{
    [SerializeField] private float m_TilemapLength = 0f;
    [SerializeField] private Transform m_LeftPivot;

    public float TilemapLength => m_TilemapLength;
    public float LeftMostPosition => m_LeftPivot.transform.position.x;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    private void OnValidate()
    {
        foreach (Tilemap tilemap in GetComponentsInChildren<Tilemap>())
        {
            var size = tilemap.size.x * tilemap.cellSize.x;
            if (size > m_TilemapLength)
            {
                m_TilemapLength = size;
            }
        }
    }
}
