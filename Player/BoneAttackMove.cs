using UnityEngine;
using System.Collections;
/// <summary>
/// 骨攻撃の発動時の動き
/// </summary>
public class BoneAttackMove : MonoBehaviour
{
    [Header("移動速度"), SerializeField] private float moveSpeed = 10f;
    [Header("生存時間"), SerializeField] private float lifeTime = 3f;
    [Header("停滞時間"), SerializeField] private float stayTime = 1f;
    [Header("上昇量"), SerializeField] private float riseHeight = 1f;
    [Header("斜め移動"), SerializeField] private float side = 0f;
    [SerializeField] private BoxCollider fireB; //骨プレハブ

    private bool isFlying = false; // 発射状態フラグ

    void Start()
    {
        //コンポーネントの取得
        fireB = GetComponent<BoxCollider>();

        //発射時の動き
        StartCoroutine(StayAndThenFly());
    }

    void Update()
    {
        // 飛行中は前方に直進
        if (isFlying)
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
    }

    IEnumerator StayAndThenFly()
    {
        //初期のポジション
        Vector3 startPos = transform.position;

        //上昇の最大値
        Vector3 targetPos = startPos + Vector3.up * riseHeight + transform.right * side;

        //効果音を流す
        AudioManager.Instance.PlaySE(SEManager.SEType.Attack2);

        //経過時間
        float elapsed = 0f;
        fireB.enabled = false;

        // ⬆ 停滞時間中にゆっくり上昇
        while (elapsed < stayTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / stayTime);
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        //発射開始
        isFlying = true;
        fireB.enabled = true;
        //一定時間後に自動で消滅
        Destroy(gameObject, lifeTime);
    }

    //敵かボスに当たると消える
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Boss"))
        {
            Destroy(gameObject);
        }
    }
}
