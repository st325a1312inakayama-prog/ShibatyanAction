using UnityEngine;
using System.Collections;
/// <summary>
/// ボスの行動パターンの関数を定義するクラス
/// </summary>
public class Boss1Action : MonoBehaviour
{
    [SerializeField] Animator animator; //ボスのアニメーション

    #region 攻撃
    [Header("弾幕"), SerializeField] GameObject[] bullet;
    [Header("弾幕発射ポイント"), SerializeField] Transform[] attackPoint;
    [Header("弾幕発射カウント"), SerializeField] int[] bulletCount;
    bool[] isAttacking; //Attackを毎フレーム呼び出させないためのフラグ
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
        for (int i = 0; i < bulletCount[0]; i++)
        {
            Instantiate(bullet[0], attackPoint[0].position, attackPoint[0].rotation);
            yield return new WaitForSeconds(bulletInterval);
        }
        isAttacking[0] = false;
        animator.SetBool("isAttack1", false);
        //終わるとStopに戻る
        SetStopState();
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

        float bulletAngle = 24f;

        yield return new WaitForSeconds(delaytime);

        float angle = startAngle;

        //発射するごとに-24度ずつ角度が小さくなる
        for (int i = 0; i < bulletCount[1]; i++)
        {
            Quaternion rot = baseRotation * Quaternion.Euler(0, angle, 0);
            Instantiate(bullet[1], attackPoint[1].position, rot);

            angle -= bulletAngle;
            yield return new WaitForSeconds(bulletInterval);
        }

        isAttacking[1] = false;
        animator.SetBool("isAttack2", false);
        //終わるとStopに戻る
        SetStopState();
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
        BossState[] attackPatterns = { BossState.Attack1, BossState.Attack2 };

        //ランダム抽選
        int randomIndex = Random.Range(0, attackPatterns.Length);
        CurrentState = attackPatterns[randomIndex];
    }
    public void SetDeadState()
    {
        //状態をDeadにする
        CurrentState = BossState.Dead;
    }
    public void SetAttack1State()
    {
        //状態をAttack1にする
        CurrentState = BossState.Attack1;
    }
    public void SetAttack2State()
    {
        //状態をAttack2にする
        CurrentState = BossState.Attack2;
    }
    public void SetStopState()
    {
        //状態をStopにする
        CurrentState = BossState.Stop;
    }

    //Stop状態中Playerに集中する
    public void FocusOnPlayer()
    {
        if (playerTransform == null) return;

        Vector3 dir = playerTransform.position - transform.position;
        dir.y = 0f; // Y軸は回転させない（3D用）

        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            rotateSpeed * Time.deltaTime
        );
    }
}
