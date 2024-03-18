using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScene : MonoBehaviour
{
    public static LevelScene instance;

    [SerializeField]
    private Transform levelHolderPrefab;
    [SerializeField]
    private Transform levelsContainer;
    [SerializeField]
    private Transform _incommingHolder;


    public Transform sceneTransition;

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

    void Start()
    {
        PrepareLevels();
    }
    public void PlayChangeScene()
    {
        sceneTransition.GetComponent<Animator>().Play("SceneTransitionReverse");
    }

    private void PrepareLevels()
    {
        for (int i = 0; i < LevelManager.instance.levelData.GetLevels().Count; i++)
        {
            Transform holder = Instantiate(levelHolderPrefab, levelsContainer);
            holder.name = i.ToString();
            Level level = LevelManager.instance.levelData.GetLevelAt(i);
            if (LevelManager.instance.levelData.GetLevelAt(i).isPlayable)
            {
                holder.GetComponent<LevelHolder>().EnableLevelClickAndUI();
            }
            else
            {
                holder.GetComponent<LevelHolder>().DisableLevelClickAndUI();
            }
            holder.GetComponent<CanvasGroup>().alpha = 0;
            holder.GetComponent<CanvasGroup>().DOFade(1, .4f).SetDelay(.4f * i);

            holder.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -100);
            holder.GetChild(0).GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), .5f).SetDelay(.4f * i);
        }

        if (_incommingHolder)
        {
            _incommingHolder.GetComponent<CanvasGroup>().alpha = 0;
            float delay = .4f * LevelManager.instance.levelData.GetLevels().Count;
            _incommingHolder.GetComponent<CanvasGroup>().DOFade(1, .4f).SetDelay(delay);
            _incommingHolder.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -100);
            _incommingHolder.GetChild(0).GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), .5f).SetDelay(delay);
        }
    }

}
