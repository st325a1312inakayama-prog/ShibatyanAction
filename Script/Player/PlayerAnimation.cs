using UnityEngine;
/// <summary>
/// プレイヤーのアニメーションメソッド用のクラス
/// </summary>
public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] Animator animator; //プレイヤーのアニメーター

    private void Start()
    {
        //アニメーターの取得
        animator = GetComponent<Animator>();
    }
    #region 止まった時のアニメーション
    public void StopAnimation()
    {
        animator.SetBool("isStop", true);
    }
    public void StopAnimationFinish()
    {
        animator.SetBool("isStop", false);
    }
    #endregion

    #region 歩いた時のアニメーション
    public void WalkAnimation()
    {
        animator.SetBool("isWalk", true);
    }
    public void WalkAnimationFinish()
    {
        animator.SetBool("isWalk", false);
    }
    #endregion

    #region 走った時のアニメーション
    public void DashAnimation()
    {
        animator.SetBool("isDash", true);
    }
    public void DashAnimationFinish()
    {
        animator.SetBool("isDash", false);
    }
    #endregion

    #region 死んだときのアニメーション
    public void DeadAnimation()
    {
        animator.SetBool("isDead", true);
    }
    public void DeadAnimationFinish()
    {
        animator.SetBool("isDead", false);
    }
    #endregion

    #region ダメージを受けた時のアニメーション
    public void DamageAnimation()
    {
        animator.SetBool("isDamage", true);
    }
    public void DamageAnimationFinish()
    {
        animator.SetBool("isDamage", false);
    }
    #endregion
}
