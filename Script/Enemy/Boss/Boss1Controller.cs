using UnityEngine;
using static Boss1Action;
/// <summary>
/// ボスの行動の制御を行うクラス
/// </summary>
public class Boss1Controller : MonoBehaviour
{
    Boss1Action action; //ボスの行動を管理しているクラス
    EnemyHPManager hpManager; //敵の体力を管理しているクラス

    private void Start()
    {
        //コンポーネントの取得
        action = GetComponent<Boss1Action>();
        hpManager = GetComponent<EnemyHPManager>();
    }

    private void Update()
    {
        //体力が0以下になった時に状態をDeadにする関数を実行する
        if (hpManager.EnemyHP <= 0)
        {
            action.SetDeadState();
            return; //死んだら他の処理をさせない
        }

        //ボスの状態によって実行する関数を変える
        switch (action.CurrentState)
        {
            case BossState.Stop:
                action.Stop();
                action.FocusOnPlayer();
                break;

            case BossState.Attack1:
                action.Attack1(); //各攻撃に特別な名前を付けた方が読みやすいし、わかりやすいと思います（例：ShootBulletAttackとか）
                break;

            case BossState.Attack2:
                action.Attack2(); 
                break;
                
            case BossState.Dead:
                action.Dead();
                break;
        }
    }
}
