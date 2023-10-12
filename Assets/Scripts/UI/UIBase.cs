using UnityEngine;

public class UIBase : GenericSingleton<UIBase>
{
    [SerializeField] GameObject _escUI;
    [SerializeField] GameObject _hpBar;
    [SerializeField] GameObject _bossIntro;
    [SerializeField] GameObject _bossHpBar;
    [SerializeField] GameObject _miniMapUI;

    public void Init()
    {
        GenerateMiniMap();
        InvenReset();
        ShowEscUI(false);
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
}
