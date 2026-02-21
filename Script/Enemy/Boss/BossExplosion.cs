using UnityEngine;
/// <summary>
/// ボスを倒した際に爆発エフェクトを再生させるためのクラス
/// </summary>
public class BossExplosion : MonoBehaviour
{
    [SerializeField] GameObject explosion; //爆発エフェクト
    [SerializeField] GameObject boss; //ボス
    private bool isExplode = false; //爆発を行うためのフラグ

    public bool IsExplode { get => isExplode; set => isExplode = value; }

    private void FixedUpdate()
    {
        //ボスが消えた際に爆発演出を再生させる
        if(boss == null)
        {
            if (IsExplode == false)
            {
                Instantiate(explosion,transform.position,Quaternion.identity);
                AudioManager.Instance.PlaySE(SEManager.SEType.EnemyExplode);
                AudioManager.Instance.StopBGM();
                IsExplode = true;
            }
            return;
        }
    }
}
