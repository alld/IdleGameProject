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
        ShareText = 1,
        BasicText = 2,
        Stage = 3,
        Monster = 4,
        Quest = 5,
        Temp_6 = 6,
        Temp_7 = 7,
        Temp_8,
        Temp_9,
    }


    /// <summary>
    /// [데이터] 데이터 테이블의 종류와 할당되는 데이터 정보를 담고 있습니다.
    /// </summary>
    public class Data_DataTableInfo
    {
        #region 파싱 데이터
        /// <summary>
        /// [데이터] 데이터 테이블의 버전 정보를 나타냅니다.
        /// </summary>
        public string version = "EmptyTable";

        /// <summary>
        /// [상태] 파싱된 데이터 테이블이 존재하는지 확인합니다.
        /// <br> 해당 상태값은 실제 성공유무와 상관없이 응답 또는 변환 과정이 시도되었는지 까지만 보장됩니다. </br>
        /// </summary>
        public bool isDataExists = false;


        /// <summary>
        /// [데이터] 데이터 테이블에대한 기본적인 정보를 담고 있습니다. URL 주소, 데이터의 범위입니다. 
        /// <br> 해당 데이터는 개발용에서만 사용되는 데이터입니다. </br>
        /// </summary>
        public Dictionary<eDataTableType, (string, string)> dataTableList = new Dictionary<eDataTableType, (string, string)>();
        #endregion
    }
}