using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Pixeye.Unity;
using UnityEngine.SceneManagement;
/// <summary>
/// スタートシーンのUIアニメーションの関数やシーンの移行を定義するクラス
/// </summary>
public class StartSceneUIManager : MonoBehaviour
{
    #region アニメーション

    #region ClickStartUI
    [Foldout("「クリックしてスタート」のアニメーション")]
    [Header("UI"),SerializeField] Image clickStart_UI;
    [Foldout("「クリックしてスタート」のアニメーション")]
    [Header("フラッシュの時間"), SerializeField] float flashTime = 1.5f;
    #endregion

    #region BackGround1(スタート直後の背景)
    [Foldout("背景のサイズの変化アニメーション")]
    [Header("背景1"),SerializeField] RectTransform backGround1;
    [Foldout("背景のサイズの変化アニメーション")]
    [Header("背景のサイズ変化"), SerializeField] Vector3 backGroundSize = new Vector3(1.2f, 1.2f, 1.2f);
    [Foldout("背景のサイズの変化アニメーション")]
    [Header("元の背景のサイズ"), SerializeField] Vector3 backGroundNormalSize = new Vector3(1f, 1f, 1f);
    [Foldout("背景のサイズの変化アニメーション")]
    [Header("サイズの変化にかかる時間"), SerializeField] float backGroundDuration = 0.3f;
    #endregion

    #region Footprints
    [Foldout("犬の足跡のアニメーション")]
    [Header("UI"),SerializeField] Image footprints;
    [Foldout("犬の足跡のアニメーション")]
    [Header("最終的な画像サイズ"),SerializeField] Vector3 footSize = new Vector3(0.5f, 0.5f, 0.5f);
    [Foldout("犬の足跡のアニメーション")]
    [Header("かかる時間"),SerializeField] float footprintsDuration = 0.3f;
    #endregion

    #region ImageMovement
    [Foldout("背景の移動")]
    [Header("赤色の画像1"), SerializeField] Image redImage1;
    [Foldout("背景の移動")]
    [Header("赤色の画像2"), SerializeField] Image redImage2;
    [Foldout("背景の移動")]
    [Header("黒色の画像"), SerializeField] Image blackImage;
    [Foldout("背景の移動")]
    [Header("移動場所"), SerializeField] Vector2 destination = new Vector2(1400, 0);
    [Foldout("背景の移動")]
    [Header("移動開始までのインターバル"), SerializeField] float intervalTime = 0.8f;
    [Foldout("背景の移動")]
    [Header("移動のインターバル"), SerializeField] float intervalTime2 = 0.4f;
    [Foldout("背景の移動")]
    [Header("画像の移動間隔"), SerializeField] float intervalTime3 = 0.1f;
    #endregion
    #endregion

    //「クリックしてスタート」のUIの点滅アニメーション
    public void StartFlashing()
    {
        clickStart_UI.DOKill();
        clickStart_UI.DOFade(1f, flashTime)
            .SetLoops(-1, LoopType.Yoyo);
    }

    //点滅ストップ
    public void StopFlashing()
    {
        clickStart_UI.DOKill();
        clickStart_UI.DOFade(1f, 0);
    }

    //スタート開始時のアニメーション
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
    
    //ゲームスタートした際の背景ズーム演出(一瞬ズームしてからまた大きさが戻る)
    public void BackGroundAnimation()
    {
        Sequence s = DOTween.Sequence();
        s.AppendInterval(footprintsDuration) //footStampのアニメーションが終わるまでの時間
            .Append(backGround1.DOScale(backGroundSize, backGroundDuration))
            .Append(backGround1.DOScale(backGroundNormalSize, backGroundDuration));
    }

    //シーン移行前のフェードアウト演出
    public void ImageMovementAnimation()
    {
        Sequence s = DOTween.Sequence();
        RectTransform rect1 = redImage1.rectTransform;
        RectTransform rect2 = redImage2.rectTransform;
        RectTransform rect3 = blackImage.rectTransform;

        s.AppendInterval(intervalTime) //footStampのアニメーションが終わるまでの時間
            .AppendCallback(() =>
            {
                //ここでSEを鳴らす
                AudioManager.Instance.PlaySE(SEManager.SEType.SceneChange);
            })
            .Append(rect1.DOAnchorPos(destination, intervalTime2))
            .Join(rect2.DOAnchorPos(destination, intervalTime2).SetDelay(intervalTime3))
            .Join(rect3.DOAnchorPos(destination, intervalTime2).SetDelay(intervalTime3))
            .OnComplete(() =>
            {
                SceneManager.LoadScene("Select");
            });
    }
}
