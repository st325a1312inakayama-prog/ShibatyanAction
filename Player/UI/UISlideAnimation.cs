using UnityEngine;
using DG.Tweening;
/// <summary>
/// タイムライン再生時にプレイヤーのUI(HP・攻撃のゲージ)を画面外へ動かすためのクラス
/// </summary>
public class UISlideAnimation : MonoBehaviour
{
    PlayerController pCon;
    [SerializeField] RectTransform playerUI;
    [SerializeField] float slideTime = 1.0f;
    [SerializeField] float slidePosition = 30.0f;
    void Start()
    {
        //コンポーネントの取得
        pCon = FindAnyObjectByType<PlayerController>();
    }
    void Update()
    {
        UISlide();
    }

    //タイムライン再生時などの行動できない状態でのみUIを動かす関数
    private void UISlide()
    {
        if (pCon.InputBlocked)
            return;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(playerUI.DOAnchorPosY(slidePosition, slideTime));
    }
}
