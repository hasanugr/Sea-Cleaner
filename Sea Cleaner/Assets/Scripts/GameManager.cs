using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game Play Props")]
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _ropePuller;
    [SerializeField] private RopePuller _ropePullerScript;
    [SerializeField] private GameObject _ropeLocker;
    [SerializeField] private LevelDesignSO[] _levelDesigns;
    [SerializeField] private int _level = 1; // No DB for test game, it will start level 1 always.

    [SerializeField] private GameObject[] _collectableObjects;
    [SerializeField] private GameObject[] _staticObjects;

    [SerializeField] private PlayerController _playerController;

    private ObjectController[] _collectableObjectControllers;
    private int _collectedObjectCount;
    private float _ropePullerLimitZPos;
    private float _ropePullerStartZPos;
    private bool _isPullingObjects;
    private bool _isGameActive;

    [Header("UI Props")]
    [SerializeField] private GameObject _succeedPanel;
    [SerializeField] private GameObject[] _succeedPanelStars;
    [SerializeField] private GameObject _failPanel;
    [SerializeField] private GameObject _tapToContinueText;


    private void Awake()
    {
        MakeSingleton();

        _ropePullerStartZPos = _ropePuller.transform.localPosition.z;
        _ropePullerLimitZPos = _ropePullerStartZPos - 5;
        _collectableObjectControllers = new ObjectController[_collectableObjects.Length];
        for (int i = 0; i < _collectableObjects.Length; i++)
        {
            _collectableObjectControllers[i] = _collectableObjects[i].GetComponent<ObjectController>();
        }

        CreateLevel(_level);
    }

    private void FixedUpdate()
    {
        float ropePullerZPos = _ropePuller.transform.localPosition.z;
        if (_isGameActive && _isPullingObjects && ropePullerZPos < _ropePullerLimitZPos)
        {
            _isPullingObjects = false;
            _isGameActive = false;
            StartCoroutine(GameOver(1));
        }

        if (_isPullingObjects)
        {
            _player.transform.position = _ropeLocker.transform.position;
        }
    }

    public void CreateLevel(int levelNumber)
    {
        _succeedPanel.SetActive(false);
        _failPanel.SetActive(false);
        _tapToContinueText.SetActive(false);
        _succeedPanelStars[0].SetActive(false);
        _succeedPanelStars[1].SetActive(false);
        _succeedPanelStars[2].SetActive(false);
        _collectedObjectCount = 0;

        _playerController.ResetPlayer();

        _ropePullerScript.IsPullingMode = false;
        _isPullingObjects = false;
        _isGameActive = true;

        int collactableObjectLength = _levelDesigns[levelNumber - 1].CollectableObjects.Length;
        for (int i = 0; i < _collectableObjects.Length; i++)
        {
            if (i < collactableObjectLength)
            {
                _collectableObjects[i].SetActive(true);
                _collectableObjects[i].transform.localPosition = _levelDesigns[levelNumber - 1].CollectableObjects[i].ObjectPosition;
                _collectableObjects[i].transform.localEulerAngles = _levelDesigns[levelNumber - 1].CollectableObjects[i].ObjectRotation;
                _collectableObjectControllers[i].ResetObject();
            }
            else
            {
                _collectableObjects[i].SetActive(false);
            }
        }

        int staticObjectLength = _levelDesigns[levelNumber - 1].StaticObjects.Length;
        for (int i = 0; i < _staticObjects.Length; i++)
        {
            if (i < staticObjectLength)
            {
                _staticObjects[i].SetActive(true);
                _staticObjects[i].transform.localPosition = _levelDesigns[levelNumber - 1].StaticObjects[i].ObjectPosition;
                _staticObjects[i].transform.localEulerAngles = _levelDesigns[levelNumber - 1].StaticObjects[i].ObjectRotation;
            }
            else
            {
                _staticObjects[i].SetActive(false);
            }
        }
    }

    IEnumerator GameOver(float time)
    {
        //yield on a new YieldInstruction that waits for X seconds.
        yield return new WaitForSeconds(time);

        int collactableObjectLength = _levelDesigns[_level - 1].CollectableObjects.Length;
        if (_collectedObjectCount > 0)
        {
            // Win State
            float percentCollectedObject = ((float)_collectedObjectCount / (float)collactableObjectLength) * 100;
            if (percentCollectedObject >= 100)
            {
                // 3 Star
                _succeedPanelStars[2].SetActive(true);
            }
            else if (percentCollectedObject >= 50)
            {
                // 2 Star
                _succeedPanelStars[1].SetActive(true);
            }
            else
            {
                // 1 Star
                _succeedPanelStars[0].SetActive(true);
            }
            _succeedPanel.SetActive(true);

            if (_level < _levelDesigns.Length)
            {
                _tapToContinueText.SetActive(true);
            }
        }
        else
        {
            // Loose State
            _failPanel.SetActive(true);
            _tapToContinueText.SetActive(true);
        }
    }

    public void NextLevel()
    {
        if (_level < _levelDesigns.Length)
        {
            _level++;
            CreateLevel(_level);
        }
    }

    public void RestartLevel()
    {
        CreateLevel(_level);
    }


    public void PullRope()
    {
        _playerController.ConnectedToRopeLocker(_ropeLocker.transform.position);
        for (int i = 0; i < _collectableObjects.Length; i++)
        {
            if (_collectableObjects[i].activeSelf)
            {
                _collectableObjects[i].GetComponent<ObjectController>().MakeHalfDynamic();
            }
        }
        _ropePullerScript.IsPullingMode = true;
        _isPullingObjects = true;
    }

    public void AddCollectedObjectCount()
    {
        _collectedObjectCount++;
    }

    private void MakeSingleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
