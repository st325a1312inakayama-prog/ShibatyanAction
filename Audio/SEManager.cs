using UnityEngine;
/// <summary>
/// SEType に応じて効果音を再生するサウンド管理クラス
/// </summary>
public class SEManager : MonoBehaviour
{
    [SerializeField] AudioSource seSource; //SE再生用のAudioSource

    [Header("SE Clips")]
    [SerializeField] AudioClip cursorSE; //カーソル移動音
    [SerializeField] AudioClip clickSE; //クリック音
    [SerializeField] AudioClip cancelSE; //キャンセル音
    [SerializeField] AudioClip gaugeSE; //ゲージ増加音
    [SerializeField] AudioClip sceneChangeSE; //シーン移行音
    [SerializeField] AudioClip jumpSE; //ジャンプ音
    [SerializeField] AudioClip damageSE; //ダメージ音
    [SerializeField] AudioClip attackSE1; //攻撃1音
    [SerializeField] AudioClip attackSE2; //攻撃2音
    [SerializeField] AudioClip explodeSE; //爆発音
    [SerializeField] AudioClip enemyExplodeSE; //敵の爆発音
 
    public enum SEType// 再生する効果音の種類
    {
        Cursor,
        Click,
        Cancel,
        Gauge,
        SceneChange,
        Jump,
        Damage,
        Attack1,
        Attack2,
        Explode,
        EnemyExplode
    }
    public void Play(SEType type)// 指定されたSETypeに対応した効果音を再生する
    {
        AudioClip clip = GetClip(type);//SETypeから対応するAudioClipを取得
        if (clip == null) return;

        seSource.PlayOneShot(clip);//取得したSEを再生
    }

    private AudioClip GetClip(SEType type)// SETypeに応じて対応するAudioClipを返す
    {
        switch (type)
        {
            case SEType.Cursor: return cursorSE;
            case SEType.Click: return clickSE;
            case SEType.Cancel: return cancelSE;
            case SEType.Gauge: return gaugeSE;
            case SEType.SceneChange: return sceneChangeSE;
            case SEType.Jump: return jumpSE;
            case SEType.Damage: return damageSE;
            case SEType.Attack1: return attackSE1;
            case SEType.Attack2: return attackSE2;
            case SEType.Explode: return explodeSE;
            case SEType.EnemyExplode: return enemyExplodeSE;
            default: return null;
        }
    }
}
