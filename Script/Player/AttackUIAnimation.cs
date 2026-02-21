using System.Collections;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 攻撃ゲージUIのアニメーション作成クラス
/// </summary>
public class AttackUIAnimation : MonoBehaviour
{
    [Header("攻撃Aゲージ")]
    [SerializeField] RectMask2D fireAMask;

    [Header("攻撃Bゲージ")]
    [SerializeField] RectMask2D fireBMask;

    float maxPadding = 100f; //ゲージの最大値

    Coroutine fireACoroutine;　//攻撃Aのコルーチン
    Coroutine fireBCoroutine;　//攻撃Bのコルーチン

    //攻撃Aのゲージ回復関数
    public void StartFireA(float duration)
    {
        if (fireACoroutine != null)
            StopCoroutine(fireACoroutine);

        fireACoroutine = StartCoroutine(GaugeAnimation(fireAMask, duration));
    }

    //攻撃Bのゲージ回復関数
    public void StartFireB(float duration)
    {
        if (fireBCoroutine != null)
            StopCoroutine(fireBCoroutine);

        fireBCoroutine = StartCoroutine(GaugeAnimation(fireBMask, duration));
    }

    //ゲージのチャージ処理
    IEnumerator GaugeAnimation(RectMask2D mask, float duration)
    {
        float timer = 0f;

        SetBottom(mask, maxPadding);

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float rate = timer / duration;
            float bottom = Mathf.Lerp(maxPadding, 0f, rate);
            SetBottom(mask, bottom);
            yield return null;
        }

        SetBottom(mask, 0f);
        AudioManager.Instance.PlaySE(SEManager.SEType.Gauge);
    }

    //攻撃時に下から徐々にゲージが増えていくための処理
    void SetBottom(RectMask2D mask, float value)
    {
        Vector4 padding = mask.padding;
        padding.w = value;
        mask.padding = padding;
    }
}
