using UnityEngine;
/// <summary>
/// 敵の動きの制御
/// </summary>
public class EnemyMove : MonoBehaviour
{
    [SerializeField] float speed = 5f;          // 移動速度
    [SerializeField] float directionChangeTime = 2f; // 方向転換までの時間

    private Vector3 moveDir = Vector3.forward;  // 現在の進行方向
    private float timer = 0f;                   // 経過時間

    void Update()
    {
        // 前方に進む
        transform.Translate(moveDir * speed * Time.deltaTime, Space.World);

        // 経過時間を加算
        timer += Time.deltaTime;

        // 一定時間経過したら方向転換
        if (timer >= directionChangeTime)
        {
            moveDir = -moveDir; // 進行方向を反転
            timer = 0f;         // タイマーをリセット

            // 見た目の向きも反転させたい場合
            transform.rotation = Quaternion.LookRotation(moveDir);
        }
    }
}
