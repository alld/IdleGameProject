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
        /// [데이터] 스테이지의 구성 최대웨이브를 나타냅니다.
        /// </summary>
        public int maxWave = 0;
        /// <summary>
        /// [상태] 현재까지 진행중인 웨이브를 나타냅니다. 
        /// <br> (동적 데이터) </br>
        /// </summary>
        public int currentWave = -1;

        /// <summary>
        /// [데이터] 각 웨이브의 타입을 나타냅니다. 
        /// <br> 특정 구간의 보스만 나오거나 모든 필드에서 보스가 나오는등의 형태로 구성할 수 있습니다. </br>
        /// </summary>
        public int[] waveType;

        /// <summary>
        /// [데이터] 각 웨이브별 등장하는 몬스터의 수를 지정합니다.
        /// </summary>
        public int[][] wave_unitCount;

        /// <summary>
        /// [데이터] 각 웨이브별 등장하는 몬스터의 종류를 지정합니다. 
        /// </summary>
        public int[][] wave_unitKind;


        /// <summary>
        /// [데이터] 해당 데이터가 메인 스토리에 해당하는 데이터인지를 나타냅니다. 
        /// </summary>
        public bool isMainStory;

        /// <summary>
        /// [데이터] 해당 스테이지가 메인스토리에 몇번째에 스테이지인지를 나타냅니다. 
        /// </summary>
        public int storyIndex;

        /// <summary>
        /// [상태] 현재까지 진행된 상태에 대한 정보를 담습니다. 
        /// </summary>
        public eProcedures procedures = eProcedures.None;
    }
}