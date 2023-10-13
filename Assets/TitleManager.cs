using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField] Transform _startPos;
    [SerializeField] Transform _targetPos;
    [SerializeField] RectTransform[] _menus;
    [SerializeField] RectTransform _arrow;
    [SerializeField] AudioClip[] _clip;
    [SerializeField] GameObject _continueOn;

    int _currentIdx = 0;
    AudioSource _audioSource;
    bool _isMove = false;
    bool _isTitle = true;
    [SerializeField] float _delay = 0.5f; // 전환에 걸리는 시간
    public bool _isdata;
    AsyncOperation _async;
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if(!GenericSingleton<UIBase>.Instance.OptionUI.activeSelf)
        {
            if (!_isTitle && !_isMove)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    MoveArrowUp();
                }

                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    MoveArrowDown();
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    CurrentUI();
                }
            }
            if (Input.GetKeyDown(KeyCode.Space) && !_isMove && _isTitle)
            {
                StartCoroutine(GameStartMenu());
            }
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                if (!_isMove && !_isTitle)
                {
                    StartCoroutine(TitleBack());
                }
                if (!_isMove && _isTitle)
                {
                    Debug.Log("게임종료");
                }
            }
        }
       
        
        
    }

    private IEnumerator GameStartMenu()
    {
        _continueOn.SetActive(_isdata);
        _audioSource.PlayOneShot(_clip[0]);
        _isMove = true;
        _isTitle = false;
        float t = 0.0f;

        while (t < 1.0f)
        {
            t += Time.deltaTime / _delay;
            Camera.main.transform.position = Vector2.Lerp(_startPos.position, _targetPos.position, t);

            yield return null;
        }

        _isMove = false;
    }
    private IEnumerator TitleBack()
    {
        _audioSource.PlayOneShot(_clip[0]);
        _isMove = true;
        _isTitle = true;
        float t = 0.0f;

        while (t < 1.0f)
        {
            t += Time.deltaTime / _delay;
            Camera.main.transform.position = Vector2.Lerp(_targetPos.position, _startPos.position, t);

            yield return null;
        }

        _isMove = false;
    }
   


    void MoveArrowDown()
    {
        if (_currentIdx < _menus.Length - 1)
        {
            _currentIdx++;

        }
        if(!_isdata && (_currentIdx == 1))
        {
            MoveArrowDown();
        }

        _audioSource.PlayOneShot(_clip[1]);
        UpdateArrowPosition();
    }

    void MoveArrowUp()
    {
        if (_currentIdx > 0)
        {
            _currentIdx--;
        }
        if (!_isdata && (_currentIdx == 1))
        {
            MoveArrowUp();
        }

        _audioSource.PlayOneShot(_clip[2]);
        UpdateArrowPosition();
    }

    void UpdateArrowPosition()
    {
        _arrow.anchoredPosition = _menus[_currentIdx].anchoredPosition + new Vector2(-140f, -20f); 
    }

    void CurrentUI()
    {
        switch (_currentIdx)
        {
            case 0:
                {
                    LoadSceneAsync();
                }
                break;
            case 1:
                {
                    //로드
                }
                break;
            case 2:
                {
                    GenericSingleton<UIBase>.Instance.ShowOptionUI(true);
                }
                break;
        }
    }
    public void LoadSceneAsync()
    {
        _async = SceneManager.LoadSceneAsync("Basement0");

        _async.completed += OnLoadComplete;
        _async.allowSceneActivation = true;
        //_async.allowSceneActivation = false; 
    }

    

    private void OnLoadComplete(AsyncOperation obj)
    {
        Debug.Log("로딩이 완료되었습니다.");
        GenericSingleton<GameManager>.Instance.SetGameState(GameManager.GameState.GameStart);
    }
}
