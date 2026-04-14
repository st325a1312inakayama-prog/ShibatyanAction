using UnityEngine;
using UnityEngine.Playables;
/// <summary>
/// ボスを倒したときに発生するイベントのクラス
/// </summary>
public class BossFinishEvent : MonoBehaviour
{
    [Header("ボス")]
    [SerializeField] GameObject boss;
    EnemyHPManager enemyHP; //ボスの体力
    Boss1Controller bossController; //ボスのアクション用クラス
    PlayableDirector playableDirector; //Timeline再生用のコントローラーの取得
    private bool isStarted = false; //タイムラインの再生開始フラグ

    void Start()
    {
        //コンポーネントの取得
        playableDirector = GetComponent<PlayableDirector>();
        enemyHP = boss.GetComponent<EnemyHPManager>();
        //再生時に呼び出す関数
        playableDirector.played += OnTimelineStarted;
        playableDirector.stopped += OnTimelineStopped;
    }
    void Update()
    {
        if(enemyHP.EnemyHP <= 0 && !isStarted)//ボスの体力が0になり、isStartedがfalseになったらTimelineが再生される
        {
            playableDirector.Play();
            bossController.enabled = false;
        }
    }
    private void OnTimelineStarted(PlayableDirector director)
    {
        isStarted = true;
        // ボス行動を終了
        bossController.enabled = false;
    }
    private void OnTimelineStopped(PlayableDirector director)
    {
        //タイムラインの再生終了時にBGMを変える
        AudioManager.Instance.PlayBGM(BGMManager.BGMType.Stage1);
    }
}
