using IdleGame.Data.Base;

namespace IdleGame.Data
{
    /// <summary>
    /// [데이터] 스테이지 정보를 담고 있습니다.
    /// </summary>
    public class Data_Stage
    {
        /// <summary>
        /// [데이터] 스테이지 고유 인덱스입니다.
        /// </summary>
        public int index = -1;

        /// <summary>
        /// [데이터] 스테이지에 할당되는 고유 넘버입니다.
        /// </summary>
        public int stage_id = -1;

        /// <summary>
        /// [데이터] 스테이지에 구성된 웨이브 갯수를 나타냅니다. 
        /// </summary>
        public int wave_num = 0;

        /// <summary>
        /// [데이터] 스테이지에 부여된 고유 효과를 나타냅니다. 
        /// </summary>
        public int stage_effect = -1;

        /// <summary>
        /// [데이터] 시작전 스토리가 반영되는지 유무를 나타냅니다. 
        /// </summary>
        public bool story = false;

        /// <summary>
        /// [데이터] 스테이지 진행시 적용되는 배경 id입니다. 
        /// </summary>
        public int background_id = 0;

        /// <summary>
        /// [데이터] 스테이지 진행동안 등장하는 몬스터 종류입니다.
        /// </summary>
        public int[] monster_id;

        /// <summary>
        /// [데이터] 스테이지를 구성되는 각 몬스터의 학률입니다. 
        /// </summary>
        public float[] monster_num;

        /// <summary>
        /// [데이터] 각 웨이브별 등장하는 최대 몬스터 수입니다. 
        /// </summary>
        public int[] monster_max;

        /// <summary>
        /// [데이터] 최종 웨이브에 등장하는 보스 몬스터의 고유 Id입니다.
        /// </summary>
        public int boss_id;

        /// <summary>
        /// [데이터] 보스 웨이브가 진행되는 제한시간입니다. 
        /// </summary>
        public int boss_battletime;

        /// <summary>
        /// [상태] 현재까지 진행된 상태에 대한 정보를 담습니다. 
        /// </summary>
        public eProcedures procedures = eProcedures.None;

        /// <summary>
        /// [상태] 현재 진행중인 스테이지의 웨이브 단계를 나타냅니다. 
        /// </summary>
        public int currentWave = 0;
    }
}