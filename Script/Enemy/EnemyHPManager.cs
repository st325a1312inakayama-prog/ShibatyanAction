using UnityEngine;
/// <summary>
/// “G‚ÌHP‚Æ“|‚µ‚½Û‚Ìs“®‚ÌŠÇ—
/// </summary>
public class EnemyHPManager : MonoBehaviour
{
    [SerializeField] int enemyHP = 20; //‘Ì—Í
    [SerializeField] float destroyTime = 1.0f; //Á‚¦‚é‚Ü‚Å‚ÌŠÔ
    private bool dead = false; //HP‚ª0‚É‚È‚Á‚½‚ÉŒÄ‚Î‚ê‚éƒtƒ‰ƒO

    public bool Dead { get => dead; set => dead = value; }
    public int EnemyHP { get => enemyHP; set => enemyHP = value; }

    private void Update()
    {
        //“G‚ÌHP‚ª0ˆÈ‰º‚É‚È‚Á‚½‚çˆê’èŠÔ‚ÅÁ‚¦‚é
        if(EnemyHP <= 0)
        {
            Dead = true;
            Destroy(gameObject, destroyTime);
        }
    }
}
