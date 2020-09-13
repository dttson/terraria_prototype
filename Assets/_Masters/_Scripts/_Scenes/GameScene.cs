using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public enum GameState
{
    INTRO,
    PLAYING,
    END
}

public class GameScene : MonoBehaviour
{
    public static GameScene Instance { get; private set; }
    public static GameState CurrentGameState { get; private set; } = GameState.INTRO;

    [SerializeField] private Camera m_Camera;
    [SerializeField] private TilemapLayer m_TilemapLayer;
    [SerializeField] private PlayerObject m_Player;

    private GameObject m_CurrentTilemap;
    private float m_DeltaXCamPlayer = 0f;
    public Rect ScreenRect { get; private set; }

    #region Game state
    public void gameOver()
    {
        CurrentGameState = GameState.END;
        ParralaxLayer.IsActive = false;

        StartCoroutine(coroutineChangeScene());
    }

    private IEnumerator coroutineChangeScene()
    {
        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("MenuScene");
    }
    #endregion Game state

    private void Awake()
    {
        Instance = this;

        Application.targetFrameRate = 300;

        m_Camera = Camera.main;
        ScreenRect = m_Camera.getScreenRect();

        ParralaxLayer.IsActive = false;
        m_Player.OnFinishIntro += onPlayerFinishIntro;
    }

    private void Start()
    {
        m_Player.gameObject.SetActive(false);

        //load tile map
        m_TilemapLayer.loadTilemapAtLevel(1);

        m_Player.appear();
    }

    private void Update()
    {
        if (CurrentGameState == GameState.PLAYING)
        {
            if (m_Player.transform.position.y < m_Player.OriginPos.y - 0.5f || m_Player.transform.position.x < ScreenRect.min.x)
            {
                Debug.Log("Player fall, game over!");
                m_Player.die();
                return;
            }
        }

        float passDistance = m_Player.transform.position.x - m_TilemapLayer.ActiveLevel.LeftMostPosition;
        float totalDistance = m_TilemapLayer.ActiveLevel.TilemapLength;

        GameUIController.Instance.updateProgress(passDistance / totalDistance);

        //if (CurrentGameState == GameState.PLAYING)
        //{
        //    var pos = m_Camera.transform.position;
        //    pos.x = m_Player.transform.position.x - m_DeltaXCamPlayer;
        //    m_Camera.transform.position = pos;
        //}

        //if (CurrentGameState == GameState.PLAYING)
        //{
        //    if (m_Player.transform.position.x < m_Player.OriginPos.x)
        //    {
        //        ParralaxLayer.IsActive = false;
        //    }
        //    else
        //    {
        //        ParralaxLayer.IsActive = true;
        //    }
        //}
    }

    #region Tilemap
    
    #endregion Tilemap

    #region Player
    private void onPlayerFinishIntro()
    {
        CurrentGameState = GameState.PLAYING;
        ParralaxLayer.IsActive = true;

        m_DeltaXCamPlayer = m_Player.transform.position.x - m_Camera.transform.position.x;
    }
    #endregion Player
}
