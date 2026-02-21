using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
/// <summary>
/// スイッチに触れるイベントのクラス
/// </summary>
public class BombSwitchAction : MonoBehaviour
{
    [SerializeField] GameObject explosionEffect;//爆発エフェクト
    [SerializeField] GameObject explosionPoint;//爆発エフェクト出現場所
    [SerializeField] GameObject rock; //岩オブジェクト
    [SerializeField] bool switchOn = false; //スイッチオブジェクトを振れたか確認するフラグ
    [SerializeField] Animator animator; //Animatorの取得
    GameObject explosionInstance; //生成された爆発エフェクトのインスタンス
    [SerializeField] GameObject timeline; //再生させるタイムライン
    [SerializeField] float explosionTime = 1.0f; //爆発するまでの時間
    PlayableDirector playableDirector;//Timeline再生用のコントローラーの取得

    void Start()
    {
        //コンポーネントの取得
        animator = FindAnyObjectByType<Animator>();
        playableDirector = timeline.GetComponent<PlayableDirector>();
        //Timeline終了時のイベント登録
        playableDirector.stopped += OnTimelineStopped;
    }
    IEnumerator rockExploaion()
    {
        yield return new WaitForSeconds(explosionTime);
        //効果音を再生
        AudioManager.Instance.PlaySE(SEManager.SEType.Explode);
        //爆発エフェクトの生成
        explosionInstance = Instantiate(explosionEffect.gameObject, explosionPoint.transform.position, explosionPoint.transform.rotation);
        //岩を消す
        Destroy(rock);
    }

    private void OnTriggerStay(Collider other)
    {
        if(((other.CompareTag("Player") || other.CompareTag("PlayerInvinsible")) && !switchOn))
        {
            //ボタンのアニメーションとTimelineを再生させる
            animator.SetBool("pushButton", true);
            playableDirector.Play();
            StartCoroutine(rockExploaion());
        }
    }
    private void OnTimelineStopped(PlayableDirector director)
    {
        switchOn = true;
    }
}
