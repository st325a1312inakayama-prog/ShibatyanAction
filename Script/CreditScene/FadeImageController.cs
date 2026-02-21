using UnityEngine;
/// <summary>
/// スタート画面のフェードイン演出を行うためのクラス
/// </summary>
public class FadeImageController : MonoBehaviour
{
    FadeImageAnimation fadeImageAnimation;
    [Header("クリックorボタンの押下")]
    [SerializeField] private bool clicked = false;
    void Start()
    {
        //コンポーネントの取得
        fadeImageAnimation = GetComponent<FadeImageAnimation>();
        //アニメーションの呼び出し
        fadeImageAnimation.FadeIn();
    }

    public void OnClick()
    {
        if(!clicked)
        {
            //効果音
            AudioManager.Instance.PlaySE(SEManager.SEType.Click);
            //アニメーションの呼び出し
            fadeImageAnimation.FadeOut();
            //クリック済み
            clicked = true;
        }
    }
}
