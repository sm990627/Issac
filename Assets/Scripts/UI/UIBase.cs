using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class UIBase : GenericSingleton<UIBase>
{
    [SerializeField] GameObject _escUI;
    [SerializeField] GameObject _hpBar;
    [SerializeField] GameObject _bossIntro;
    [SerializeField] GameObject _bossHpBar;
    [SerializeField] GameObject _miniMapUI;
    [SerializeField] Image _blackScreen;

    public void Init()
    {
        ShowMiniMap(true);
        GenerateMiniMap();
        InvenReset();
        ShowEscUI(false);
    }
    public void Title()
    {
        ShowEscUI(false);
        ShowMiniMap(false);
    }
    public void HpBarInit()
    {
        _hpBar.GetComponent<HpBarCon>().Init();
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
    }
    public void UpdateBossHP()
    {
        _bossHpBar.GetComponent<BossHpBarUI>().Init();
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
        StartCoroutine(FadeGameEffect(0.1f, 0.3f));
    }
    private IEnumerator FadeGameEffect(float delaytime, float fadeDuration)
    {
        // Display black screen
        GenericSingleton<GameManager>.Instance.SetGameState(GameManager.GameState.Loading);
        _blackScreen.gameObject.SetActive(true);
        _blackScreen.color = Color.black;

        // Wait for a brief moment
        yield return new WaitForSeconds(delaytime);

        // Fade out the black screen
        float elapsedTime = 0;
        Color startColor = _blackScreen.color;
        Color endColor = new Color(0, 0, 0, 0); // Fully transparent
        while (elapsedTime < fadeDuration)
        {
            _blackScreen.color = Color.Lerp(startColor, endColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _blackScreen.gameObject.SetActive(false); // Deactivate the black screen
        GenericSingleton<GameManager>.Instance.SetGameState(GameManager.GameState.Playing);
    }
}
