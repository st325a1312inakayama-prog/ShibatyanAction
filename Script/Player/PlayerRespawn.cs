using UnityEngine;

/// <summary>
/// プレイヤーの死亡時(体力が0になった時)にプレイヤーをセーブポイントの位置に転送するクラス
/// </summary>
public class PlayerRespawn : MonoBehaviour
{
    // 最後のセーブポイント位置
    [SerializeField] static Vector3 lastCheckPointPos;
    Vector3 startPos;

    public static Vector3 LastCheckPointPos { get => lastCheckPointPos; set => lastCheckPointPos = value; }

    void Start()
    {
        // 最初の位置を初期値にする
        startPos = transform.position;

        // まだセーブポイントを通ってなければ初期位置をセット
        if (LastCheckPointPos == Vector3.zero)
        {
            LastCheckPointPos = startPos;
        }
    }
    public void Respawn()
    {
        transform.position = LastCheckPointPos;
    }
}
