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
    [SerializeField] float _delay = 0.5f; // ��ȯ�� �ɸ��� �ð�
    public bool _isdata;
    AsyncOperation _async;
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    void Update()
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
                ExecuteCurrentUI();
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && !_isMove &&_isTitle)
        {
            StartCoroutine(GameStartMenu());
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if(!_isMove && !_isTitle)
            {
                StartCoroutine(TitleBack());
            }
            if(!_isMove && _isTitle)
            {
                Debug.Log("��������");
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
        _arrow.anchoredPosition = _menus[_currentIdx].anchoredPosition + new Vector2(-240f, -40f); 
    }

    void ExecuteCurrentUI()
    {
        switch (_currentIdx)
        {
            case 0:
                LoadSceneAsync();
                break;
        }
    }
    public void LoadSceneAsync()
    {
        _async = SceneManager.LoadSceneAsync("Basement0");
        _async.completed += OnLoadComplete;
        _async.allowSceneActivation = false; 
    }

    private void OnLoadComplete(AsyncOperation obj)
    {
        Debug.Log("�ε��� �Ϸ�Ǿ����ϴ�.");
        _async.allowSceneActivation = true;
        GenericSingleton<GameManager>.Instance.SetGameState(GameManager.GameState.GameStart);
    }
}
