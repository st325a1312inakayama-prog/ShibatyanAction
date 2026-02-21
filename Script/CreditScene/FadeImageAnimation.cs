using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
/// <summary>
/// スタート画面のフェードイン・フェードアウトのアニメーション用クラス
/// </summary>
public class FadeImageAnimation : MonoBehaviour
{
    [Header("スタートのアニメーション")]
    [SerializeField] Image fadeImage; //フェードイン・アウト用の画像
    [SerializeField] float fadeTime = 0.5f; //フェードにかかる時間

    public void FadeIn()
    {
        Sequence s = DOTween.Sequence();
        s.Append(fadeImage.DOFade(0, fadeTime));
    }

    public void FadeOut()
    {
        Sequence s = DOTween.Sequence();
        s.Append(fadeImage.DOFade(1, fadeTime))
            .OnComplete(() =>
            {
                //処理を終えるとシーン移行
                SceneManager.LoadScene("Select");
            });
    }
}
