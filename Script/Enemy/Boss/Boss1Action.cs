using UnityEngine;
using System.Collections;
/// <summary>
/// ボスの行動パターンの関数を定義するクラス
/// </summary>
public class Boss1Action : MonoBehaviour
{
    [SerializeField] Animator animator; //ボスのアニメーション

    #region 攻撃
    //[Header("弾幕1"), SerializeField] GameObject bullet;
    //[Header("弾幕2"), SerializeField] GameObject bullet2;

    [Header("弾幕"), SerializeField]
    GameObject[] bullets; //弾幕のプレハブをまとめた配列（Inspector上で弾幕を増やせるようにするため）

    //[Header("弾幕発射ポイント1"), SerializeField] Transform attackPoint1;
    //[Header("弾幕発射ポイント2"), SerializeField] Transform attackPoint2;

    [Header("発射ポイント"), SerializeField]
    Transform[] attackPoints; //弾幕発射ポイントをまとめた配列（Inspector上で増やせるようにするため）

    [Header("攻撃中フラグ"), SerializeField]
    bool[] isAttacking; //攻撃中かどうかのフラグをまとめた配列（Inspector上で増やせるようにするため）
    
    //bool isAttack1Running = false; //Attack1を毎フレーム呼び出させないためのフラグ
    //bool isAttack2Running = false; //Attack2を毎フレーム呼び出させないためのフラグ
    bool isDead = false; //Deadを毎フレーム呼び出さないためのフラグ

    #endregion

    #region プレイヤーの位置の情報と回転
    [SerializeField] Transform playerTransform; //プレイヤーのTransform
    [SerializeField] float rotateSpeed = 5f; //回転のスピード
    #endregion

    //ボスの行動の状態
    public enum BossState
    {
        Stop,
        Attack1,
        Attack2,
        Dead
    }

    [SerializeField]
    private BossState currentState = BossState.Stop; //初期状態

    public BossState CurrentState
    {
        get => currentState;
        private set => currentState = value;
    }

    // Stop が多重実行されないためのフラグ
    bool isStop = false;

    private void Start()
    {
        //コンポーネントの取得
        animator = GetComponent<Animator>();
    }

    //ストップ状態での行動
    public void Stop()
    {
        // Stop中なら何もしない
        if (isStop) return;

        isStop = true;

        //ストップ状態のアニメーションを再生
        animator.SetBool("isStop", true);

        StartCoroutine(StopCoroutine());
    }

    IEnumerator StopCoroutine()
    {
        //停止時間
        float stopTime = 2.5f;

        yield return new WaitForSeconds(stopTime);

        animator.SetBool("isStop", false);

        isStop = false;

        //Stop終了時に1回だけ抽選
        SetRandomAttackState();
    }

    //Attack1状態での行動
    public void Attack1()
    {
        if (CurrentState != BossState.Attack1) return;

        //Attack1中なら何もしない
        if (isAttacking[0]) return;

        isAttacking[0] = true;
        //Attack1状態のアニメーションを再生
        animator.SetBool("isAttack1", true);

        StartCoroutine(Attack1Coroutine());
    }

    IEnumerator Attack1Coroutine()
    {
        //発射前ディレイ
        float delaytime = 1.2f;

        //弾の発射間隔
        float bulletInterval = 0.15f;

        yield return new WaitForSeconds(delaytime);

        // 弾幕8発
        for (int i = 0; i < 8; i++) //８マジックナンバー発見！変数化してください
        {
            Instantiate(bullet, attackPoint1.position, attackPoint1.rotation);
            yield return new WaitForSeconds(bulletInterval);
        }
        isAttacking[0] = false;
        animator.SetBool("isAttack1", false);
        //終わるとStopに戻る
        CurrentState = BossState.Stop;
    }

    //Attack2状態での行動
    public void Attack2()
    {
        if (CurrentState != BossState.Attack2) return;

        //Attack2中なら何もしない
        if (isAttacking[1]) return;

        isAttacking[1] = true;
        //Attack2のアニメーションを再生
        animator.SetBool("isAttack2", true);
        StartCoroutine(Attack2Coroutine());
    }
    IEnumerator Attack2Coroutine()
    {
        //発射前ディレイ
        float delaytime = 1.2f;

        //弾幕の発射間隔
        float bulletInterval = 0.1f;

        // ボスの向きを基準にする
        Quaternion baseRotation = transform.rotation;

        // 一発目を必ず「横」から出す
        float startAngle = 90f;

        yield return new WaitForSeconds(delaytime);

        float angle = startAngle;

        //発射するごとに-24度ずつ角度が小さくなる
        for (int i = 0; i < 15; i++) //１５マジックナンバー発見！変数化してください
        {
            Quaternion rot = baseRotation * Quaternion.Euler(0, angle, 0);
            Instantiate(bullet2, attackPoint2.position, rot);

            angle -= 24f; //24マジックナンバー発見！変数化してください
            yield return new WaitForSeconds(bulletInterval);
        }

        isAttacking[1] = false;
        animator.SetBool("isAttack2", false);
        //終わるとStopに戻る
        CurrentState = BossState.Stop;
    }

    //Dead状態の行動
    public void StartDeadProcess()
    {
        if (CurrentState != BossState.Dead) return;

        //Dead中なら何もしない
        if (isDead) return;

        isDead = true;
        StartCoroutine(DeadSequence());
    }
    IEnumerator DeadSequence()
    {
        // すべての攻撃・停止コルーチンを止める
        StopAllCoroutines();

        // Damage 再生
        animator.SetBool("isDamage", true);
        animator.SetBool("IsAttack1", false);
        animator.SetBool("IsAttack2", false);
        animator.SetBool("IsStop", false);
        yield return null; // Animator に反映させるため1フレーム待つ

        // isDamage の長さを取得
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float damageTime = stateInfo.length;

        yield return new WaitForSeconds(damageTime);

        // Damage 終了
        animator.SetBool("isDamage", false);

        // Dead 再生
        animator.SetBool("isDead", true);
    }

    //Attack1とAttack2をランダムに抽選する
    void SetRandomAttackState()
    {
        //半々の確立で抽選される
        CurrentState = (Random.Range(0, 2) == 0) //配列を使う場合の書き方に改善！
            ? BossState.Attack1
            : BossState.Attack2;

        Debug.Log("Next State : " + CurrentState);
    }
    public void SetDeadState() //他のステート変更にも対応できるように、引数でステートを渡す形にした方がいいと思います
    {
        //状態をDeadにする
        CurrentState = BossState.Dead;
    }

    //Stop状態中Playerに集中する
    public void FocusOnPlayer()
    {
        if (player == null) return;

        Vector3 dir = player.position - transform.position;
        dir.y = 0f; // Y軸は回転させない（3D用）

        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            rotateSpeed * Time.deltaTime
        );
    }
}
