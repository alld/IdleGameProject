using IdleGame.Core.Unit;
using System.Collections.Generic;

namespace IdleGame.Data.DataTable
{
    /// <summary>
    /// [데이터] 정적으로 사용되어지는 모든 데이터 테이블을 담고 있습니다. 
    /// </summary>
    public static class Library_DataTable
    {
        /// <summary>
        /// [데이터] 데이터 테이블에대한 기본적인 정보를 담습니다.
        /// </summary>
        public static Data_DataTableInfo Info = new Data_DataTableInfo();

        /// <summary>
        /// [데이터] 데이터 테이블의 총 갯수를 나타냅니다. 
        /// </summary>
        public const int DataTableCount = 12;

        /// <summary>
        /// [데이터] 스테이지에대한 모든 정보를 담고 있습니다. 인덱스로 스테이지를 찾을 수 있습니다. 
        /// </summary>
        public static Dictionary<int, Data_Stage> stage = new Dictionary<int, Data_Stage>();

        /// <summary>
        /// [데이터[ 몬스터에대한 모든 테이블 정보를 담고 있습니다. 
        /// </summary>
        public static Dictionary<int, Data_Monster> monster = new Dictionary<int, Data_Monster>();

        /// <summary>
        /// [데이터] 퀘스트에대한 모든 테이블 정보를 담고 있습니다. 
        /// </summary>
        public static Dictionary<int, Data_Quest> quest = new Dictionary<int, Data_Quest>();

        /// <summary>
        /// [데이터] 퀘스트에대한 모든 테이블 정보를 담고 있습니다. 
        /// </summary>
        public static Dictionary<int, Data_Character> character = new Dictionary<int, Data_Character>();

        /// <summary>
        /// [데이터] 퀘스트에대한 모든 테이블 정보를 담고 있습니다. 
        /// </summary>
        public static Dictionary<int, Data_Item> item = new Dictionary<int, Data_Item>();

        /// <summary>
        /// [데이터] 퀘스트에대한 모든 테이블 정보를 담고 있습니다. 
        /// </summary>
        public static Dictionary<int, Data_Skill> skill = new Dictionary<int, Data_Skill>();

        /// <summary>
        /// [데이터] 능력의 상승 테이블 정보를 담고 있습니다. 
        /// </summary>
        public static Dictionary<eAbilityType, Data_AbilitySlotInfo[]> abilitySlot = new Dictionary<eAbilityType, Data_AbilitySlotInfo[]>();

        /// <summary>
        /// [데이터] 퀘스트에대한 모든 테이블 정보를 담고 있습니다. 
        /// </summary>
        public static Dictionary<(eUnitProperty, eUnitProperty), float> property = new Dictionary<(eUnitProperty, eUnitProperty), float>();
    }
}