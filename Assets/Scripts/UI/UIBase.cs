using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIBase : GenericSingleton<UIBase>
{
    [SerializeField] GameObject _escUI;
    [SerializeField] GameObject _optionUI;
    public GameObject OptionUI { get { return _optionUI; } }
    [SerializeField] GameObject _hpBar;
    [SerializeField] GameObject _bossIntro;
    [SerializeField] GameObject _bossHpBar;
    [SerializeField] GameObject _miniMapUI;
    [SerializeField] GameObject _gameOverUI;
    [SerializeField] Image _blackScreen;
    [SerializeField] AudioClip[] _uiSound;

    public delegate void Sound(float value);
    AudioSource _audioSource;
    public Sound MusicVolume;
    public Sound EffectVolume;

 


    
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        EffectVolume += Volume;
    }
    private void Update()
    {
        if(_escUI.activeSelf && !_optionUI.activeSelf)   //escUI 컨트롤
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                EscArrowUp();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                EscArrowDown();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                EscUISelectedMenu();
            }
        }
        else if ( _optionUI.activeSelf)  //optionUI컨트롤
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                OptionArrowUp();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                OptionArrowDown();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                OptionArrowLeft();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                OptionArrowRight();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ShowOptionUI(false);
            }

        }
        if(GenericSingleton<GameManager>.Instance.CurrentState == GameState.GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space))
            {
                GoTitle();
            }
        }
        
    }
    public void GoTitle()
    {  
        GenericSingleton<GameManager>.Instance.Title();
        Title();
        SceneManager.LoadSceneAsync("TitleScene");
    }

    public void Init()
    { 
        ShowMiniMap(true);
        GenerateMiniMap();
        OptionInit();
        InvenReset();
        ShowHpBar(true);
        ShowBossHpBar(false);
        ShowEscUI(false);
        ShowGameOverUI(false);
        HpBarInit();
    }
    public void Title()
    {
        StartCoroutine(TitleFade(0.3f, 1f));
        ShowGameOverUI(false);
        ShowEscUI(false);
        ShowHpBar(false);
        ShowBossHpBar(false);
        OptionInit();
        ShowMiniMap(false);
    }
    public void HpBarInit()
    {
        _hpBar.GetComponent<HpBarCon>().Init();
    }
    public void ShowHpBar(bool isShow)
    {
        _hpBar.SetActive(isShow);
    }
    public void HpUpdate()
    {
        _hpBar.GetComponent<HpBarCon>().HpUpdate();
    }
    public void GenerateMiniMap()
    {
        _miniMapUI.GetComponent<MiniMapUI>().GenerateMiniMap();
    }
    public void UpdateMiniMap()
    {
        _miniMapUI.GetComponent<MiniMapUI>().UpdateMiniMap();
    }
    public void ShowBossIntro(bool isShow)
    {
        _bossIntro.SetActive(isShow);
    }
    public void BossIntroSound()
    {
        _audioSource.PlayOneShot(_uiSound[2]);
    }
    public void ShowOptionUI(bool isShow)
    {
        _optionUI.SetActive(isShow);
    }
    public void ShowMiniMap(bool isShow)
    {
        _miniMapUI.SetActive(isShow);
    }
    public void ShowBossHpBar(bool isShow)
    {
        _bossHpBar.SetActive(isShow);
    }
    public void ShowEscUI(bool isShow)
    {
        _escUI.SetActive(isShow);
        _escUI.GetComponent<EscUI>().EscUIinit();
    }
    public void ShowGameOverUI(bool isShow)
    {
        _gameOverUI.SetActive(isShow);
    }
    public void SetGameOverEnemy(string enemy)
    {
        _gameOverUI.GetComponent<GameOverUI>().SetEnemyImage(enemy);
    }
    public void UpdateBossHP(float hp, float maxhp)
    {
        _bossHpBar.GetComponent<BossHpBarUI>().Init(hp,maxhp);
    }
    public void InvenDraw(int idx)
    {
        _escUI.GetComponent<EscUI>().DrawItem(idx);
    }
    public void InvenReset()
    {
        _escUI.GetComponent<EscUI>().Init();
    }
    public void FadeEffect()
    {
        StartCoroutine(FadeGameEffect(0.3f, 0.5f));
    }

    public void EscArrowUp()
    {
        _escUI.GetComponent<EscUI>().MoveArrowUp();
        _audioSource.PlayOneShot(_uiSound[0]);
    }
    public void EscArrowDown()
    {
        _escUI.GetComponent<EscUI>().MoveArrowDown();
        _audioSource.PlayOneShot(_uiSound[1]);
    }
    public void EscUISelectedMenu()
    {
        _escUI.GetComponent<EscUI>().CurrentUI();
    }
    public void OptionInit()
    {
        _optionUI.GetComponent<OptionUI>().Init();
    }
    public void OptionArrowUp()
    {
        _optionUI.GetComponent<OptionUI>().MoveArrowUp();
        _audioSource.PlayOneShot(_uiSound[0]);
    }
    public void OptionArrowDown()
    {
        _optionUI.GetComponent<OptionUI>().MoveArrowDown();
        _audioSource.PlayOneShot(_uiSound[1]);
    }
    public void OptionArrowLeft()
    {
        _optionUI.GetComponent<OptionUI>().VolumeDown();
        _audioSource.PlayOneShot(_uiSound[1]);
    }
    public void OptionArrowRight()
    {
        _optionUI.GetComponent<OptionUI>().VolumeUp();
        _audioSource.PlayOneShot(_uiSound[1]);
    }

    public void EffectSound(float value)
    {
        if (EffectVolume != null)
        {
            EffectVolume(value);
        }

    }
    public void MusicSound(float value)
    {
        if (MusicVolume != null)
        {
            MusicVolume(value);
        }
    }

    void Volume(float value)
    {
        _audioSource.volume = value;
    }

    public void SoundInit()
    {
        _optionUI.GetComponent<OptionUI>().SoundInit();
    }



    private IEnumerator FadeGameEffect(float delaytime, float fadeDuration)
    {
        GenericSingleton<GameManager>.Instance.SetGameState(GameState.Loading);
        _blackScreen.gameObject.SetActive(true);
        _blackScreen.color = Color.black;

        yield return new WaitForSeconds(delaytime);
        GenericSingleton<GameManager>.Instance.CheckState();
        GenericSingleton<GameManager>.Instance.SetGameState(GameState.Loading);
        float elapsedTime = 0;
        Color startColor = _blackScreen.color;
        Color endColor = new Color(0, 0, 0, 0); 
        while (elapsedTime < fadeDuration)
        {
            _blackScreen.color = Color.Lerp(startColor, endColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _blackScreen.gameObject.SetActive(false); 
        GenericSingleton<GameManager>.Instance.SetGameState(GameState.Playing);
    }
    private IEnumerator TitleFade(float delaytime, float fadeDuration)
    { 
        _blackScreen.gameObject.SetActive(true);
        _blackScreen.color = Color.black;

        yield return new WaitForSeconds(delaytime);
        float elapsedTime = 0;
        Color startColor = _blackScreen.color;
        Color endColor = new Color(0, 0, 0, 0);
        while (elapsedTime < fadeDuration)
        {
            _blackScreen.color = Color.Lerp(startColor, endColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _blackScreen.gameObject.SetActive(false);
    }
}
