using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossIntro : MonoBehaviour
{
    [SerializeField] GameObject _bossIntro;
    [SerializeField] GameObject _blackScreen;
    [SerializeField] RectTransform _player;
    [SerializeField] RectTransform _boss;
    [SerializeField] RectTransform _vsText;

    void OnEnable()
    {
        GenericSingleton<GameManager>.Instance.SetGameState(GameManager.GameState.Loading);
        StartCoroutine(IntroAnimation());
        
    }

    IEnumerator IntroAnimation()
    {
        // 초기 위치 설정
        _bossIntro.SetActive(true);
        GenericSingleton<SoundManager>.Instance.Stop();
        _player.localPosition = new Vector3(-820, 0, 0);
        _boss.localPosition = new Vector3(1050, 0, 0);
        _vsText.localScale = Vector3.zero;

        float duration = 0.5f;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        

        // 양쪽에서 가운데로 이동
        duration = 0.8f;
        elapsedTime = 0;
        GenericSingleton<UIBase>.Instance.BossIntroSound();
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            _player.localPosition = Vector3.Lerp(new Vector3(-820, 0, 0), new Vector3(-50, 0, 0), t);
            _boss.localPosition = Vector3.Lerp(new Vector3(1050, 0, 0), new Vector3(50, 0, 0), t);
            _vsText.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, 360, t));
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        
        // 가운데에서 VS 텍스트 등장 (회전)
        _vsText.localScale = Vector3.one * 2;
        _vsText.localRotation = Quaternion.Euler(0, 0, 0);

        duration = 0.6f;
        elapsedTime = 0;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            _player.localPosition = Vector3.Lerp(new Vector3(-50, 0, 0), Vector3.zero, t);
            _boss.localPosition = Vector3.Lerp(new Vector3(50, 0, 0), Vector3.zero, t);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        //1초 대기
        duration = 1.0f;
        elapsedTime = 0;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        // 다시 양쪽으로 빠져나가기
        duration = 1.0f;
        elapsedTime = 0;

        Color textColor = _vsText.GetComponent<Image>().color;
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            _player.localPosition = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(-820, 0, 0), t);
            _boss.localPosition = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(1050, 0, 0), t);
            float alpha = Mathf.Lerp(1.0f, 0.0f, t);
            textColor.a = alpha;
            _vsText.GetComponent<Image>().color = textColor;
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        GenericSingleton<GameManager>.Instance.SetGameState(GameManager.GameState.EnemiesOn);
        _bossIntro.SetActive(false);
        GenericSingleton<UIBase>.Instance.ShowBossHpBar(true);
        GenericSingleton<SoundManager>.Instance.SetBoss();
    }
}
