using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public SceneChanger sceneChanger;
    public GameScene gameScene;


    Level currentLevelData;
    #region Game status
    private bool isGameWin = false;
    private bool isGameLose = false;
    #endregion

    private void Start()
    {
        currentLevelData = LevelManager.instance.levelData.GetLevelAt(LevelManager.instance.currentLevelIndex);

        GameObject map = Instantiate(currentLevelData.map);
        GetComponent<CameraFollowing>().Follow(map.transform.GetChild(0));

        Time.timeScale = 1;
    }


    public void Win()
    {
        if (LevelManager.instance.levelData.GetLevels().Count > LevelManager.instance.currentLevelIndex + 1)
        {
            if (LevelManager.instance.levelData.GetLevelAt(LevelManager.instance.currentLevelIndex + 1).isPlayable == false)
            {
                LevelManager.instance.levelData.SetLevelData(LevelManager.instance.currentLevelIndex + 1, true, false);
            }
        }
        LevelManager.instance.levelData.SaveDataJSON();

        isGameWin = true;

        gameScene.ShowWinPanel();
    }

    public void Lose()
    {
        isGameLose = true;
        StartCoroutine(WaitToLose());
    }

    private IEnumerator WaitToLose()
    {
        yield return new WaitForSecondsRealtime(1f);
        gameScene.ShowLosePanel();
    }

    public bool IsGameWin()
    {
        return isGameWin;
    }

    public bool IsGameLose()
    {
        return isGameLose;
    }
}

