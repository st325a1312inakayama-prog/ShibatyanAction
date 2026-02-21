using System.Collections;
using UnityEngine;
/// <summary>
/// 敵(キャノン)の弾の動きの制御クラス
/// </summary>
public class EnemyBulletMove : MonoBehaviour
{
    [SerializeField] float speed = 10f; //弾のスピード
    [SerializeField] float moveTime = 6; //動ける時間
    [SerializeField] GameObject explosion; //ヒットした際に発動する爆発エフェクト

    void Update()
    {
        StartCoroutine(BulletCoroutine());
    }

    private void OnTriggerEnter(Collider other)
    {
        // プレイヤー(無敵時間中も含む)と地面に当たったら爆発して削除する処理
        if (other.CompareTag("Player") || other.CompareTag("Ground") || other.CompareTag("PlayerInvincible"))
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    IEnumerator BulletCoroutine()
    {
        // 前方向に進む
        transform.position += transform.forward * speed * Time.deltaTime;
        yield return new WaitForSeconds(moveTime);
        //時間がたったら消す
        Destroy(gameObject);
    }
}
