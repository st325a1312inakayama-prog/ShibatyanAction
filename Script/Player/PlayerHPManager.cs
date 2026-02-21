using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// プレイヤーのHPを管理するクラス
/// </summary>
public class PlayerHPManager : MonoBehaviour
{
    [Header("プレイヤーの最大体力"), SerializeField] int maxHP = 200;
    [Header("現在のHP"), SerializeField] int hp = 200;
    [Header("ダメージを受けたフラグ"), SerializeField] bool takeDamage = false;
    PlayerController playerController; //プレイヤーの行動を制御するクラス
    [SerializeField] RectMask2D hpGaugeMask; //HPのゲージを減らしたように見せるための透明な画像
    PlayerRespawn pRespawn; //プレイヤーのリスポーンアクションを制御するクラス
    [Header("ゲージの最大幅"),SerializeField] float gaugeWidth;
    [SerializeField] GameObject hitEffect; //攻撃を食らったときのエフェクト

    public bool TakeDamage { get => takeDamage; set => takeDamage = value; }

    private void Start()
    {
        //コンポーネントの取得
        playerController = GetComponent<PlayerController>();
        gaugeWidth = hpGaugeMask.GetComponent<RectTransform>().rect.width;
        pRespawn = GetComponent<PlayerRespawn>();
    }
    void Update()
    {
        UpdateHPGauge();
        if (hp <= 0)
        {
            //HPが0になったらリスポーン関数が実行され、体力が回復する
            pRespawn.Respawn();
            hp += 200;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if ((other.CompareTag("Enemy") || other.CompareTag("EnemyBullet")) && !TakeDamage)
        {
            //攻撃が当たるとダメージを受ける

            //敵のダメージ用コンポーネントの取得
            DamageToPlayer damage = other.GetComponent<DamageToPlayer>();

            //エフェクトの発生
            Instantiate(hitEffect, transform.position, Quaternion.identity);
            if (damage != null)
            {
                //damageの値に応じて体力を減らす
                hp -= damage.Damage;

                //効果音を再生する
                AudioManager.Instance.PlaySE(SEManager.SEType.Damage);
                StartCoroutine(playerController.TakeDamageAction());
            }
        }
    }
    void UpdateHPGauge()
    {
        // 現在HPを最大HPで割り、HPの割合（0〜1）を計算する
        float hpRate = (float)hp / maxHP;

        // HPが減った分だけ、右側のマスクを広げるための値を計算する
        float rightPadding = gaugeWidth * (1f - hpRate);

        // RectMask2Dの右側paddingを変更することで、HPゲージが減少しているように見せる
        hpGaugeMask.padding = new Vector4(
            hpGaugeMask.padding.x,     // Left
            hpGaugeMask.padding.y,
            rightPadding,// 右をHPに応じて変化させる
            hpGaugeMask.padding.w
        );
    }
}
