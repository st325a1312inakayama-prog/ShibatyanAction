using UnityEngine;
/// <summary>
/// /プレイヤーの与える攻撃の数値(インスペクターで変更可能)
/// </summary>
public class DamageToPlayer : MonoBehaviour
{
    [Header("与えるダメージ")][SerializeField] int damage = 0;

    public int Damage { get => damage; set => damage = value; }
}
