using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapLayer : ParralaxLayer
{
    public LevelBase ActiveLevel { get; private set; }

    [SerializeField] private LevelBase[] m_ListLevels;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    public void loadTilemapAtLevel(int levelId)
    {
        int index = levelId - 1;
        LevelBase lvObject = Instantiate(m_ListLevels[index], Vector3.zero, Quaternion.identity, transform);
        ActiveLevel = lvObject;
    }


}
