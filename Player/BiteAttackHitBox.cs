using System.Collections;
using UnityEngine;
/// <summary>
/// 噛みつき攻撃の当たり判定(攻撃発生時の牙オブジェクトにアタッチする)
/// </summary>
public class BiteAttackHitBox : MonoBehaviour
{
    [SerializeField] private BoxCollider fireA; //攻撃Aの当たり判定
    [SerializeField] GameObject hitEffect; //当たった際のエフェクト
    PlayerController pCon; //プレイヤーのアクションを制御するクラス
    private Transform player; //プレイヤーのTransform

    void Start()
    {
        //コンポーネントの取得
        fireA = GetComponent<BoxCollider>();
        pCon = FindAnyObjectByType<PlayerController>();

        //初めは判定を非表示にする
        fireA.enabled = false;
        player = pCon.transform;
        if (pCon.FireA_Push)
        {
            StartFireA();
        }
    }
    private void Update()
    {
        //FIreA_Point(プレイヤーの攻撃の出現ポイント)の場所と軸
        transform.position = pCon.FireA_Point.position;
        transform.rotation = pCon.FireA_Point.rotation;

    }

    public void StartFireA()
    {
        //FireATimeを発生させるメソッド
        StartCoroutine(FireATime());
    }

    IEnumerator FireATime()
    {
        float firstTime = 0.48f; //攻撃の発生時間
        float finishedTime = 0.5f; //攻撃の終了時間
        yield return new WaitForSeconds(firstTime);

        //攻撃Aの当たり判定を表示させる
        fireA.enabled = true;

        yield return new WaitForSeconds(finishedTime);

        //オブジェクトを削除する
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy") || other.CompareTag("Boss"))
        {
            //効果音を再生する
            AudioManager.Instance.PlaySE(SEManager.SEType.Attack1);

            //当たり判定を消す
            fireA.enabled = false;

            //攻撃ヒット時のエフェクトを出現させる
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }
    }
}
