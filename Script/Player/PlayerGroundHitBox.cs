using UnityEngine;
/// <summary>
/// 地面判定用のコライダーが地面に当たるとプレイヤーがジャンプできるようになるためのクラス
/// </summary>
public class PlayerGroundHitBox : MonoBehaviour
{
    [SerializeField] bool isGrounded; //接地判定フラグ

    public bool IsGrounded { get => isGrounded; set => isGrounded = value; }

    //Groundタグに触れると接地判定をtrueで返す
    private void OnTriggerStay(Collider collision)
    {
        if(collision.CompareTag("Ground"))
        {
            IsGrounded = true;
        }
    }
    //Groundタグから離れると接地判定をfalseで返す
    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Ground"))
        {
            IsGrounded = false;
        }
    }
}
