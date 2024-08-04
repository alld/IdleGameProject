namespace IdleGame.Data.Pool
{
    /// <summary>
    /// [종류] 오브젝트 풀에서 관리되는 오브젝트 종류입니다. 
    /// </summary>
    public enum ePoolType
    {
        None = 0,
        /// <summary> [종류] 플레이어 유닛 </summary>
        Player,
        /// <summary> [종류] 동료 유닛 </summary>
        Party,
        /// <summary> [종류] 적 유닛 ]</summary>
        Enemy,

        /// <summary> [종류] 보상 오브젝트 </summary>
        Currency,

    }

    public class Library_Pool
    {

    }
}