using UnityEngine;
/// <summary>
/// プレイヤーのセーブポイントを更新するためのクラス
/// </summary>
public class SavePoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // プレイヤーの位置を保存
            PlayerRespawn.LastCheckPointPos = transform.position;
            Debug.Log("セーブポイント更新: " + transform.position);
        }
    }
}
