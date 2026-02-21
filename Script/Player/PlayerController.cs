using UnityEngine;
using Pixeye.Unity;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// プレイヤーキャラクターの入力に応じた移動・攻撃などの処理を行うクラス
/// </summary>
public class PlayerController : MonoBehaviour
{
    #region コンポーネント参照
    Rigidbody rb; // プレイヤーのRigidbody
    PlayerGroundHitBox groundBox; // 地面判定
    PlayerHPManager hpManager; // プレイヤーHPの管理クラス
    AttackUIAnimation attackUI; //攻撃時間UI用のクラス
    PlayerAnimation pAnime; //プレイヤーのアニメーション呼び出しクラス
    #endregion

    #region 移動
    [Foldout("移動")][Header("移動スピード"), SerializeField] float speed = 5.0f;
    [Foldout("移動")][Header("落下スピード上限"), SerializeField] float maxFallSpeed = -20f;
    [Foldout("移動")][Header("ダッシュ状態"), SerializeField] bool canDash = false;
    [Foldout("移動")][Header("ダッシュスピード倍率"), SerializeField] float dashScale = 1.7f;
    #endregion

    #region ジャンプ
    [Foldout("ジャンプ")][Header("ジャンプ力"), SerializeField] float jumpPower = 8.0f;
    [Foldout("ジャンプ")][Header("ジャンプフラグ")] bool jumpFlag = false;
    #endregion

    #region 回転
    [Foldout("回転")][Header("回転スピード"), SerializeField] float rotationSpeed = 540f;
    #endregion

    #region 攻撃A（噛みつき）
    [Foldout("攻撃A")][Header("パワー"), SerializeField] int fireA_Power = 30;
    [Foldout("攻撃A")][Header("牙プレハブ"), SerializeField] GameObject fang;
    [Foldout("攻撃A")][Header("出現ポイント"), SerializeField] Transform fireA_Point;
    [Foldout("攻撃A")][Header("全体硬直"), SerializeField] float fireA_Time = 0.8f;
    [Foldout("攻撃A")][Header("発動フラグ"),SerializeField]bool fireA_Push = false;
    #endregion

    #region 攻撃B（骨投げ）
    [Foldout("攻撃B")][Header("パワー"), SerializeField] int fireB_Power = 20;
    [Foldout("攻撃B")][Header("骨プレハブ1"), SerializeField] GameObject bone1;
    [Foldout("攻撃B")][Header("骨プレハブ2"), SerializeField] GameObject bone2;
    [Foldout("攻撃B")][Header("骨プレハブ3"), SerializeField] GameObject bone3;
    [Foldout("攻撃B")][Header("骨プレハブ4"), SerializeField] GameObject bone4;
    [Foldout("攻撃B")][Header("骨プレハブ5"), SerializeField] GameObject bone5;

    [Foldout("攻撃B")][Header("出現ポイント1"), SerializeField] List<Transform> fireB_Point = new List<Transform>();

    [Foldout("攻撃B")][Header("チャージ時間1"), SerializeField] float boneTime1 = 1f;
    [Foldout("攻撃B")][Header("チャージ時間2"), SerializeField] float boneTime2 = 2f;
    [Foldout("攻撃B")][Header("再チャージ時間"), SerializeField] float reChargeTime = 1.5f;
    [Foldout("攻撃B")][Header("発動フラグ"), SerializeField] bool fireB_Push = false;
    [Foldout("攻撃B")][Header("再チャージ状態フラグ"), SerializeField] bool reCharging = false;
    Coroutine chargeCoroutine;//コルーチン停止のための参照
    #endregion

    [SerializeField] bool inputBlocked = false; //Timeline再生中に攻撃をできなくする

    Vector2 input;//移動の際のキー入力の状態
    Vector3 moveDir;//カメラの向きに対する3Dベクトル
    float gravity = -30f;//重力

    #region プロパティ
    public bool FireA_Push { get => fireA_Push; }
    public int FireA_Power { get => fireA_Power; }
    public Transform FireA_Point { get => fireA_Point; }
    public bool InputBlocked { get => inputBlocked; set => inputBlocked = value; }
    public bool CanDash { get => canDash; set => canDash = value; }
    public float FireA_Time { get => fireA_Time; set => fireA_Time = value; }
    public float ReChargeTime { get => reChargeTime; set => reChargeTime = value; }
    public int FireB_Power { get => fireB_Power; set => fireB_Power = value; }
    #endregion

    void Start()
    {
        //コンポーネントの取得
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        groundBox = FindAnyObjectByType<PlayerGroundHitBox>();
        hpManager = FindAnyObjectByType<PlayerHPManager>();
        attackUI = FindAnyObjectByType<AttackUIAnimation>();
        pAnime = GetComponent<PlayerAnimation>();
        //ゲームのフレームレートを60にする
        Application.targetFrameRate = 60;
    }

    void FixedUpdate()
    {
        // --- カメラ方向から移動入力を変換 ---
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;
        camForward.y = 0;
        camRight.y = 0;
        moveDir = (camForward.normalized * input.y + camRight.normalized * input.x).normalized;

        // --- 回転 ---
        if (moveDir.magnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime);
        }

        // --- ジャンプ ---
        if (jumpFlag && groundBox.IsGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpPower, rb.linearVelocity.z);
            jumpFlag = false;
        }

        // --- 重力 ---
        if (!groundBox.IsGrounded && rb.linearVelocity.y > maxFallSpeed)
            rb.AddForce(Vector3.up * gravity);

        // --- 移動 ---
        if(!CanDash)
        {
            Vector3 move = moveDir * speed;
            rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);
        }
        if (CanDash)
        {
            Vector3 move = moveDir * speed * dashScale;
            rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);
        }

        bool isMoving = input.sqrMagnitude > 0.01f; // 入力があるか
        if (!isMoving && !CanDash)
        {
            pAnime.StopAnimation();
        }
        else
        {
            pAnime.StopAnimationFinish();
        }
    }

    //シネマシーン中の動きの制御
    public void ResetInputState()
    {
        input = Vector2.zero;
        fireA_Push = false;
        fireB_Push = false;
        reCharging = false;

        if (chargeCoroutine != null)
        {
            StopCoroutine(chargeCoroutine);
            chargeCoroutine = null;
        }

        CanDash = false;
    }


    // -------------------------------
    // 攻撃Aコルーチン
    // -------------------------------
    IEnumerator FireACoroutine()
    {
        fireA_Push = true;
        rotationSpeed = 0;

        //UI開始（fireA硬直）
        attackUI.StartFireA(FireA_Time);

        Instantiate(fang, fireA_Point.position, fireA_Point.rotation);

        yield return new WaitForSeconds(FireA_Time);

        rotationSpeed = 540;
        fireA_Push = false;
    }


    // -------------------------------
    // 攻撃Bコルーチン（チャージ→発射→再チャージ）
    // -------------------------------
    IEnumerator FireBCoroutine()
    {
        fireB_Push = true;
        float chargeTimer = 0f;

        // チャージ中
        while (fireB_Push)
        {
            chargeTimer += Time.deltaTime;
            yield return null;
        }

        // 発射
        FireBone(chargeTimer);

        // 再チャージ開始
        reCharging = true;

        //UI開始（再チャージ時間）
        attackUI.StartFireB(ReChargeTime);

        yield return new WaitForSeconds(ReChargeTime);

        reCharging = false;
        chargeCoroutine = null;
    }


    public IEnumerator TakeDamageAction()
    {
        //ダメージ開始
        hpManager.TakeDamage = true;
        this.tag = "PlayerInvincible";
        pAnime.DamageAnimation();

        //プレイヤーのレンダラーを取得
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        float damageInterval = 0.5f;
        // --- 0.5秒待機 ---
        yield return new WaitForSeconds(damageInterval);

        float blinkTime = 0f;
        float totalTime = 2f;        // 点滅時間
        float blinkInterval = 0.2f;  // 点滅間隔

        // --- 2秒間点滅 ---
        while (blinkTime < totalTime)
        {
            //レンダラー ON/OFF
            foreach (var r in renderers)
            {
                r.enabled = !r.enabled;
            }

            blinkTime += blinkInterval;
            yield return new WaitForSeconds(blinkInterval);
        }
        pAnime.DamageAnimationFinish();
        // --- 点滅終了（必ず表示状態に戻す） ---
        foreach (var r in renderers)
        {
            r.enabled = true;
        }

        //無敵終了
        this.tag = "Player";
        hpManager.TakeDamage = false;
    }

    // -------------------------------
    // 骨発射処理
    // -------------------------------
    void FireBone(float chargeTime)
    {
        //タイムライン再生中はできない
        if (InputBlocked) return;

        //チャージ時間が1秒以下↓
        if (chargeTime <= boneTime1)
        {
            //骨を1個生成
            Instantiate(bone1, fireB_Point[0].position, fireB_Point[0].rotation);
        }

        //チャージ時間が1秒以上2秒以下↓
        else if (chargeTime <= boneTime2)
        {
            //骨を3個生成
            Instantiate(bone1, fireB_Point[0].position, fireB_Point[0].rotation);
            Instantiate(bone2, fireB_Point[1].position, fireB_Point[1].rotation);
            Instantiate(bone3, fireB_Point[2].position, fireB_Point[2].rotation);
        }

        //チャージ時間が2秒以上↓
        else
        {
            //骨を5個生成
            Instantiate(bone1, fireB_Point[0].position, fireB_Point[0].rotation);
            Instantiate(bone2, fireB_Point[1].position, fireB_Point[1].rotation);
            Instantiate(bone3, fireB_Point[2].position, fireB_Point[2].rotation);
            Instantiate(bone4, fireB_Point[3].position, fireB_Point[3].rotation);
            Instantiate(bone5, fireB_Point[4].position, fireB_Point[4].rotation);
        }
    }

    // -------------------------------
    // 入力処理
    // -------------------------------
    public void OnMove(InputAction.CallbackContext context)
    {
        //タイムライン再生中はできない
        if (InputBlocked) return;

        //移動に割り当てた入力を数値化する
        input = context.ReadValue<Vector2>();
        if (input.sqrMagnitude > 0.01f && !canDash)
        {
            //歩きのアニメーションを再生する
            pAnime.WalkAnimation();
        }
        else if(input.sqrMagnitude <= 0.01f)
        {
            //歩きのアニメーションの再生を終了する
            pAnime.WalkAnimationFinish();
        }
        if (input.sqrMagnitude > 0.01 && canDash)
        {
            //canDashがtrueの状態で入力するとダッシュアニメーションが再生される
            pAnime.DashAnimation();
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //タイムライン再生中はできない
        if (InputBlocked) return;
        if (context.started && groundBox.IsGrounded)
        {
            //ジャンプフラグがtrueになる
            jumpFlag = true;

            //効果音を再生する
            AudioManager.Instance.PlaySE(SEManager.SEType.Jump);
        }
    }

    public void OnFireA(InputAction.CallbackContext context)
    {
        //タイムライン再生中はできない
        if (InputBlocked) return;
        if (context.started && !fireA_Push　&& !fireB_Push)
            //攻撃Aを実行する
            StartCoroutine(FireACoroutine());
    }

    public void OnFireB(InputAction.CallbackContext context)
    {
        //タイムライン再生中か攻撃再チャージ中はできない
        if (InputBlocked || reCharging) return;

        if (context.started && chargeCoroutine == null && !fireB_Push && !fireA_Push)
        {
            //攻撃Bを実行する
            chargeCoroutine = StartCoroutine(FireBCoroutine());
        }

        if (context.canceled)
        {
            //離すとfalseになる(falseになるとチャージ時間が増えない)
            fireB_Push = false;
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        //タイムライン再生中はできない
        if(inputBlocked) return;

        if(context.started)
        {
            //移動しながら押すと走ることができる
            CanDash = true;
            //ダッシュアニメーションを再生する
            pAnime.WalkAnimationFinish();
        }
        if(context.canceled)
        {
            //離すと走らない
            CanDash = false;
            //ダッシュアニメーションが終了し、歩きアニメーションが再生れる
            pAnime.DashAnimationFinish();
            pAnime.WalkAnimation();
        }
    }
}
