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
        /// [데이터] 공격하는 본인의 인스턴스 인덱스입니다.
        /// </summary>
        public int attaker;

        /// <summary>
        /// [데이터] 공격대상의 인스턴스 인덱스입니다.
        /// </summary>
        public int target;

        /// <summary>
        /// [데이터] 계산 결과입니다.
        /// </summary>
        public NativeList<int> result;

        /// <summary>
        /// [데이터] 결과값의 스케일입니다.
        /// </summary>
        public int result_sacle;

        /// <summary>
        /// [데이터] 결과값의 양수유무를 판단하는 데이터입니다. 
        /// </summary>
        public bool result_isPositive;

        public void Execute()
        {
            SetResult(Global_DamageEngine.Logic_Calculator(Base_Unit.GetUsedUnitList(attaker).ability, Base_Unit.GetUsedUnitList(target).ability, GetResult()));
        }


        /// <summary>
        /// [기능] 데이터를 설정합니다.
        /// </summary>
        public void SetResult(ExactInt m_data, int m_attker, int m_target)
        {
            attaker = m_attker;
            target = m_target;

            SetResult(m_data);
        }

        /// <summary>
        /// [기능] 데이터를 설정합니다.
        /// </summary>
        public void SetResult(ExactInt m_data)
        {
            if (!result.IsCreated)
                result = new NativeList<int>(Allocator.TempJob);

            result.Clear();

            for (int i = 0; i < m_data.scale + 1; i++)
            {
                result.Add(m_data.value[i]);
            }
            result_isPositive = m_data.isPositive;
            result_sacle = m_data.scale;
        }

        /// <summary>
        /// [기능] 결과값을 반환합니다.
        /// </summary>
        public ExactInt GetResult()
        {
            int[] value = new int[result.Length];

            for (int i = 0; i < value.Length; i++)
            {
                value[i] = result[i];
            }

            return new ExactInt(value, result_isPositive, result_sacle);
        }

        /// <summary>
        /// [초기화] 모든 데이터를 지웁니다.
        /// </summary>
        public void Clear()
        {
            if (result.IsCreated)
                result.Dispose();

            attaker = -1;
            target = -1;
        }
    }
}