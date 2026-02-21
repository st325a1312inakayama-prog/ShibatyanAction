using System.Collections;
using UnityEngine;
/// <summary>
/// 乗ると右に向かって動く床のクラス
/// </summary>
public class FloorMoveOnPlayer : MonoBehaviour
{
    #region 動く床
    [SerializeField] bool onPlayer = false; //プレイヤーが乗ったか
    [SerializeField] bool isReturn = false; //床が戻っている状態か
    [SerializeField] bool isMoveForward = false; //床が目的地に向かっているか
    [SerializeField] bool isStopped = false; //床が初期位置で止まっているか
    [SerializeField] float moveX = 2f;     // 移動速度
    [SerializeField] float moveTime = 2f;  // 地面が動く時間
    [SerializeField] float stopTime = 0.5f;  // 地面が止まる時間
    [SerializeField] Transform playerTransform; // プレイヤー
    float timer = 0f; //タイマー
    public int moveCount = 0; //動きのパターン変化
    #endregion

    private void FixedUpdate()
    {
        if(!onPlayer && !isMoveForward && !isReturn)
        {
            isStopped = true;
        }
        //プレイヤーが乗る＆動きのパターンが2以上
        if (!isStopped || moveCount >= 2)
        {
            //MoveFloorが実行される
            MoveFloor();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isReturn)
        {
            isStopped = false;
            onPlayer = true;
            //プレイヤーを床の子にする
            playerTransform = collision.transform;
            playerTransform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            onPlayer = false;
        }
        //プレイヤーが離れた場合は親子関係を解除する
        if(collision.gameObject.CompareTag("Player") && !onPlayer)
        {
            if (playerTransform != null)
            {
                playerTransform.SetParent(null);
                playerTransform = null;
            }
        }
    }

    //床を動かす関数
    public void MoveFloor()
    {
        //時間で変わるようにしているため、実行時からタイマーをカウントさせている
        timer += Time.fixedDeltaTime;
        switch(moveCount)
        {
            case 0:
                isMoveForward = true;
                //タイマーがストップタイム以上になった場合
                if (timer >= stopTime)
                {
                    //タイマーを0にし、moveCountを1に進める
                    timer = 0;
                    moveCount = 1;
                }
                break;
            case 1:
                //床を右にmoveX分進ませる
                transform.Translate(Vector3.right * moveX * Time.fixedDeltaTime);
                //タイマーがムーブタイム以上になった場合
                if(timer >= moveTime)
                {
                    //タイマーを0にし、moveCountを2に進める
                    timer = 0;
                    moveCount = 2;
                }
                break;
            case 2:
                isMoveForward = false;
                //タイマーがストップタイム以上になった場合
                if (timer >= stopTime)
                {
                    //タイマーを0にする
                    timer = 0;
                    //プレイヤーが乗っていない場合
                    if (!onPlayer && !isMoveForward)
                    {
                        isReturn = true;
                        //ムーブカウントを3に進める
                        moveCount = 3;
                    }
                }
                break;
            case 3:
                //反対方向に向かって戻らせる
                transform.Translate(Vector3.left * moveX * Time.fixedDeltaTime);
                //タイマーがムーブタイム以上になった場合
                if (timer >= moveTime)
                {
                    //タイマーを0にし、isReturnをfalseにしてmoveCountを0にする
                    timer = 0;
                    isReturn = false;
                    moveCount = 0;
                }
                break;
        }
    }
}
