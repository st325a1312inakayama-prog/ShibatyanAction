using System.Collections;
using UnityEngine;
/// <summary>
/// まっすぐ飛ぶ弾幕のクラス
/// </summary>
public class StraightBullet : MonoBehaviour
{
    [SerializeField]float speed = 5.0f; //弾幕のスピード
    [SerializeField]float time = 3.0f; //弾幕が消える処理に移るまでの時間
    bool dissappear_Bullet = false; //弾幕が消え始めるためのフラグ
    bool isScaling = false;   // 多重実行防止

    private void Start()
    {
        // 3秒後に消滅開始
        StartCoroutine(TimeDisappearBullet());
    }

    private void FixedUpdate()
    {
        //弾の移動処理
        transform.position += transform.forward * speed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Playerタグに当たると消える処理が行われる
        if (other.CompareTag("Player"))
        {
            StartDisappear();
        }
    }

    //一定時間がたつと消えるようにする
    IEnumerator TimeDisappearBullet()
    {
        yield return new WaitForSeconds(time);
        StartDisappear();
    }

    //弾を消す関数
    void StartDisappear()
    {
        if (isScaling) return;

        dissappear_Bullet = true;
        isScaling = true;
        StartCoroutine(ScaleDownAndDestroy());
    }

    //時間がたつごとに小さくなってから消す処理
    IEnumerator ScaleDownAndDestroy()
    {
        //縮小アニメーションにかける時間
        float duration = 0.8f;

        //経過時間
        float elapsed = 0f;

        //開始時のスケールを保存
        Vector3 startScale = transform.localScale;

        //duration秒かけて徐々にスケールを0に近づける
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            //進行度（0〜1）
            float t = elapsed / duration;

            //現在のスケールを補間で計算
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);
            yield return null;
        }

        // 完全に縮小
        transform.localScale = Vector3.zero;

        //最後にオブジェクトを削除
        Destroy(gameObject);
    }
}
