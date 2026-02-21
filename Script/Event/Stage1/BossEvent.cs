using UnityEngine;
using UnityEngine.Playables;
/// <summary>
/// ボスの出現イベントのクラス
/// </summary>
public class BossEvent : MonoBehaviour
{
    [Header("ボス")]
    private bool isStarted = false;//タイムライン再生時のフラグ

    PlayableDirector playableDirector;//Timeline再生用のコントローラーの取得

    [SerializeField] GameObject bossAppearance; //ボスの出現ポイント
    [SerializeField] Boss1Controller bossController; //ボスのアクション用クラス

    void Start()
    {
        //コンポーネントの取得
        playableDirector = GetComponent<PlayableDirector>();

        // Timeline終了時のイベント登録
        playableDirector.stopped += OnTimelineStopped;

        // 最初はボス行動を止める
        bossController.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Player") || other.CompareTag("PlayerInvincible")) && !isStarted)
        {
            //BGMを止めてTimelineを再生させる
            AudioManager.Instance.StopBGM();

            //タイムライン再生
            playableDirector.Play();
        }
    }

    // Timelineが止まったら呼ばれる
    private void OnTimelineStopped(PlayableDirector director)
    {
        isStarted = true;
        AudioManager.Instance.PlayBGM(BGMManager.BGMType.Boss1);
        //Timeline終了後にボス行動を解放
        bossController.enabled = true;
    }
}
