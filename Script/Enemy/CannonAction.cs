using UnityEngine;
/// <summary>
/// 敵(キャノン)の行動パターン制御用クラス
/// </summary>
public class CannonAction : MonoBehaviour
{
    [SerializeField] GameObject bullet; //発射される弾
    [SerializeField] Transform firePoint; // 弾を出す位置
    [SerializeField] float speed = 5f; //弾のスピード
    [SerializeField] float detectRange = 20f; //プレイヤーを検知できる範囲
    [SerializeField] bool playerLook = true; //プレイヤーの方向を向くかどうか

    Transform player; //プレイヤーのTransform
    bool isActive = false; //パターン行動をするかしないか
    bool isRunning = false; //敵行動コルーチンがすでに動いているかを管理（多重起動防止）

    void Start()
    {
        //プレイヤーのTransformを取得
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        //プレイヤーとの距離を取得
        float dist = Vector3.Distance(transform.position, player.position);

        // 範囲内に入ったらアクション開始
        if (dist <= detectRange)
        {
            isActive = true;

            if (!isRunning)
            {
                StartCoroutine(CannonRoutine());
            }
        }
        else
        {
            // プレイヤーが離れたら通常状態に戻る
            isActive = false;
        }
    }

    System.Collections.IEnumerator CannonRoutine()
    {
        isRunning = true;

        while (isActive)
        {
            // ---- ① 2秒間プレイヤーに向かって移動 ----
            float timer = 0;
            while (timer < 2f && isActive)
            {
                if (playerLook)
                {
                    Vector3 dir = player.position - transform.position;
                    dir.y = 0;
                    transform.rotation = Quaternion.LookRotation(dir);
                }

                transform.position += transform.forward * speed * Time.deltaTime;

                timer += Time.deltaTime;
                yield return null;
            }

            // ---- ② 3秒間止まって弾を1発撃つ ----
            // 止まる間も向く
            if (playerLook)
            {
                Vector3 dir = player.position - transform.position;
                dir.y = 0;
                transform.rotation = Quaternion.LookRotation(dir);
            }
            yield return new WaitForSeconds(2f);
            // 弾発射
            Shoot();

            yield return new WaitForSeconds(3f);
        }

        isRunning = false; // Idle に戻ると終了
    }

    // firePointの位置から弾Prefabを生成
    void Shoot()
    {
        if (bullet != null && firePoint != null)
        {
            Instantiate(bullet, firePoint.position, firePoint.rotation);
        }
    }
}
