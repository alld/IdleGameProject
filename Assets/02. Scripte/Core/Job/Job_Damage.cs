using IdleGame.Core.Unit;
using IdleGame.Data.Numeric;
using Unity.Collections;
using Unity.Jobs;

namespace IdleGame.Core.Job
{
    /// <summary>
    /// [기능] 데미지 계산을 잡시스템을 통해서 진행합니다.
    /// </summary>
    public struct Job_Damage : IJob
    {
        /// <summary>
        /// [캐시] 계산된 또는 계산할 데미지입니다.
        /// </summary>
        public ExactInt damage;

        /// <summary>
        /// [캐시] 공격하는 본인입니다.
        /// </summary>
        public Data_UnitAbility attaker;

        /// <summary>
        /// [캐시] 공격 대상입니다.
        /// </summary>
        public Data_UnitAbility target;

        /// <summary>
        /// [데이터] 계산 결과입니다.
        /// </summary>
        public ExactInt result;

        public void Execute()
        {
            result = Global_DamageEngine.Logic_Calculator(attaker, target, damage);
        }

        /// <summary>
        /// [초기화] 모든 데이터를 지웁니다.
        /// </summary>
        public void Clear()
        {
            attaker = null;
            target = null;
        }
    }
}