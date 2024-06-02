using System.Collections.Generic;


namespace IdleGame.Data.DataTable
{
    /// <summary>
    /// 데이터 테이블의 종류를 나타냅니다.
    /// </summary>
    public enum eDataTableType
    {
        None = -1,
        GameInfo = 0,
        Stage = 1,
        Monsters = 2,
        Weapon = 3,
        Character = 4,
        WeaponProperty = 5,
        Temp_6,
        Temp_7,
        Temp_8,
        CommonText = 9,
        BasicText = 10,
    }


    /// <summary>
    /// [데이터] 데이터 테이블의 종류와 할당되는 데이터 정보를 담고 있습니다.
    /// </summary>
    public class Data_DataTableInfo
    {
        #region 파싱 데이터
        /// <summary>
        /// 데이터 테이블의 버전 정보를 나타냅니다.
        /// </summary>
        public string version;

        public Dictionary<eDataTableType, (string, string)> dataTableList = new Dictionary<eDataTableType, (string, string)>();

        /// <summary>
        /// 공통 텍스트 테이블의 총 갯수입니다. 
        /// </summary>
        public int[] commonTextTableCount;

        public string commonTextTableURL;


        /// <summary>
        /// 기본 텍스트 테이블의 총 갯수입니다. 
        /// </summary>
        public int[] basicTextTableCount;

        public string basicTextTableURL;
        #endregion
    }
}