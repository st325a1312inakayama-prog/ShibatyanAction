using UnityEngine;
using DG.Tweening;
using Pixeye.Unity;
using UnityEngine.UI;
/// <summary>
/// セレクトシーンのUIアニメーションのアニメーションの関数を定義するクラス
/// </summary>
public class SelectSceneUIManager : MonoBehaviour
{
    #region コンテナのアニメーション
    [Foldout("コンテナ")]
    [Header("コンテナの外枠"),SerializeField] Image selectContainer1;
    [Foldout("コンテナ")]
    [Header("外枠のサイズ"),SerializeField] Vector2 containerSize1 = new Vector2(300, 100);
    [Foldout("コンテナ")]
    [Header("次のアニメーションへのインターバル1"),SerializeField] float animationInterval1 = 0.3f;
    [Foldout("コンテナ")]
    [Header("コンテナの内枠"),SerializeField] Image selectContainer2;
    [Foldout("コンテナ")]
    [Header("内枠のサイズ"),SerializeField] Vector2 containerSize2 = new Vector2(290, 90);
    [Foldout("コンテナ")]
    [Header("次のアニメーションへのインターバル2"), SerializeField] float animationInterval2 = 0.3f;
    [Foldout("コンテナ")]
    [Header("アニメーションにかかる時間"),SerializeField] float animationTime = 0.1f;
    [Foldout("コンテナ")]
    [Header("外枠と内枠を合わせたもの"),SerializeField] RectTransform container;
    [Foldout("コンテナ")]
    [Header("移動にかかる時間"), SerializeField] float moveTime = 0.6f;
    [Foldout("コンテナ")]
    [Header("選択肢のUI"), SerializeField] RectTransform gameStartUI;
    [Foldout("コンテナ")]
    [SerializeField] RectTransform gameEndUI;
    [Foldout("コンテナ")]
    [SerializeField] RectTransform tutorialUI;
    [Foldout("コンテナ")]
    [SerializeField] RectTransform creditUI;
    [Foldout("コンテナ")]
    [Header("骨の画像のカーソル"),SerializeField] RectTransform cursolUI;
    [Foldout("コンテナ")]
    [Header("画面上に表示するSelectの画像"),SerializeField] RectTransform selectUI;
    [Foldout("コンテナ")]
    [Header("UI表示にかかる時間"), SerializeField] float uIDuration = 0.4f;
    [Foldout("コンテナ")]
    [Header("UIの大きさ"), SerializeField] float uISize = 1f;
    [Foldout("コンテナ")]
    [Header("選択可能状態")] private bool canSelect = false;
    #endregion

    #region Footprints
    [Foldout("犬の足跡のアニメーション")]
    [Header("UI"), SerializeField] Image footprints;
    [Foldout("犬の足跡のアニメーション")]
    [Header("最終的な画像サイズ"), SerializeField] Vector3 footSize = new Vector3(0.3f, 0.3f, 0.3f);
    [Foldout("犬の足跡のアニメーション")]
    [Header("かかる時間"), SerializeField] float footprintsDuration = 0.3f;
    #endregion

    #region FadeOut
    [Foldout("フェードアウト")]
    [Header("黒色の画像"), SerializeField] Image fadeImage;
    [Foldout("フェードアウト")]
    [Header("フェードアウト開始までの時間"), SerializeField] float fadeOutInterval = 0.5f;
    [Foldout("フェードアウト")]
    [Header("フェードアウトの間隔"), SerializeField] float fadeOutDuration = 1.5f;
    #endregion

    public bool CanSelect { get => canSelect; set => canSelect = value; }
    public Image Footprints { get => footprints; set => footprints = value; }

    //セレクト画面の初期UIアニメーション
    public void SelectContainerAnimation()
    {
        RectTransform rect1 = selectContainer1.rectTransform;
        RectTransform rect2 = selectContainer2.rectTransform;

        Sequence s = DOTween.Sequence();

        s.Append(selectUI.DOScaleY(uISize, uIDuration))
         .Append(rect1.DOSizeDelta(containerSize1, animationTime))
         .AppendInterval(animationInterval1)
         .Append(rect2.DOSizeDelta(containerSize2, animationTime))
         .AppendInterval(animationInterval2)
         .Append(gameStartUI.DOScaleY(uISize, uIDuration))
         .Join(creditUI.DOScaleY(uISize, uIDuration))
         .Join(tutorialUI.DOScaleY(uISize, uIDuration))
         .Join(gameEndUI.DOScaleY(uISize, uIDuration))
         .Join(cursolUI.DOScaleY(uISize, uIDuration))
         .OnComplete(() =>
         {
             CanSelect = true;
         });
    }

    //選択決定時のアニメーション
    public void FootStampAnimation()
    {
        RectTransform rect = footprints.rectTransform;
        Sequence s = DOTween.Sequence();
        s.Append(footprints.DOFade(1, footprintsDuration))
            .Join(rect.DOScale(footSize, footprintsDuration))
            .AppendCallback(() =>
            {
                //ここでSEを鳴らす
                AudioManager.Instance.PlaySE(SEManager.SEType.Click);
            });
    }

    //シーン移行前に流すフェードアウト用のアニメーション
    public void FadeOutAnimation()
    {
        Sequence s = DOTween.Sequence();
        s.AppendInterval(fadeOutInterval)
            .Append(fadeImage.DOFade(1, fadeOutDuration));
    }
}
