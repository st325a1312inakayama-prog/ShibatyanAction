using UnityEngine;
using UnityEngine.Playables;
/// <summary>
/// イベントを発生させてタイムラインを再生させるクラス
/// </summary>
public class RockEvent : MonoBehaviour
{
    [Header("イベント")]
    [SerializeField]private bool isRockEvent = false; //プレイヤー接触時にイベントを発生させるためのフラグ
    PlayableDirector playableDirector; //Timeline再生用のコントローラーの取得
    [SerializeField]GameObject rockEventController; //タイムラインのついているオブジェクト
    void Start()
    {
        //コンポーネントの取得
        playableDirector = GetComponent<PlayableDirector>();
    }

    void Update()
    {
        if(isRockEvent)
        {
            //タイムラインの再生
            PlayTimeline();
            isRockEvent = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") || other.CompareTag("PlayerInvincible"))
        {
            //イベント発生フラグ
            isRockEvent = true;
        }
    }

    void OnEnable()
    {
        playableDirector.stopped += OnPlayableDirectorStopped;
    }
    void PlayTimeline()
    {
        rockEventController.SetActive(true);//タイムラインのついているオブジェクトを出現させる
    }
    void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (playableDirector == aDirector)
        {
            rockEventController.SetActive(false);//タイムラインが終了するとオブジェクトを非表示にする
        }
    }
    void OnDisable()
    {
        playableDirector.stopped -= OnPlayableDirectorStopped;
    }
}
