using UnityEngine;
/// <summary>
/// 常に横に動く床のクラス
/// </summary>
public class MoveSide : MonoBehaviour
{
    #region 横に動く床
    [SerializeField] float moveX = 2f;     // 移動速度
    [SerializeField] float moveTime = 2f;  // 地面が動く時間
    [SerializeField] float stopTime = 1f;  // 地面が止まる時間
    [SerializeField] Transform playerTransform; // プレイヤー
    float timer = 0f; //タイマー
    int moveCount = 0; //動きのパターン変化
    #endregion
    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;

        switch (moveCount)
        {
            // 横に動く
            case 0:
                transform.Translate(Vector3.right * moveX * Time.fixedDeltaTime);
                if (timer >= moveTime)
                {
                    timer = 0f;
                    moveCount = 1;
                }
                break;

            // 停止
            case 1:
                if (timer >= stopTime)
                {
                    timer = 0f;
                    moveCount = 2;
                }
                break;

            // 元の場所に動く
            case 2:
                transform.Translate(Vector3.left * moveX * Time.fixedDeltaTime);
                if (timer >= moveTime)
                {
                    timer = 0f;
                    moveCount = 3;
                }
                break;

            // 停止
            case 3:
                if (timer >= stopTime)
                {
                    timer = 0f;
                    moveCount = 0;
                }
                break;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //プレイヤーを床の子にする
            playerTransform = collision.transform;
            playerTransform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //親子関係を解除
            if (playerTransform != null)
            {
                playerTransform.SetParent(null);
                playerTransform = null;
            }
        }
    }
}
