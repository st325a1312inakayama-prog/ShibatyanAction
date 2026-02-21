using UnityEngine;
/// <summary>
/// 攻撃でHPが減る処理を行うクラス
/// </summary>
public class EnemyDamageManager : MonoBehaviour
{
    EnemyHPManager eneHP; //敵の体力を管理しているクラス
    PlayerController pCon; //プレイヤーのアクションを制御しているクラス
    void Start()
    {
        //コンポーネントの取得
        eneHP = GetComponent<EnemyHPManager>();
        pCon = FindAnyObjectByType<PlayerController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        //FireAに当たるとFireA_Power分のダメージが入る
        if(other.CompareTag("FireA"))
        {
            eneHP.EnemyHP = eneHP.EnemyHP - pCon.FireA_Power;
        }
        //FireBに当たるとFireB_Power分のダメージが入る
        if (other.CompareTag("FireB"))
        {
            eneHP.EnemyHP = eneHP.EnemyHP - pCon.FireB_Power;
        }
    }
}
