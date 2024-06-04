using IdleGame.Core.Module.DataTable;
using IdleGame.Core.Procedure;
using IdleGame.Data.Common.Event;
using IdleGame.Data.DataTable;
using System.Text.RegularExpressions;
using UnityEngine;


namespace IdleGame.Core.Panel.DataTable
{
    /// <summary>
    /// [기능] 데이터 테이블을 구성합니다. 
    /// </summary>
    public class Panel_DataTableManager : Base_Panel
    {
#if DevelopeMode
        [SerializeField]
        private Module_WebTableLoader _tableLoader;
#else
        [SerializeField]
        private Module_TSVConvert _tableLoader;
#endif

        protected override void Logic_Init_Custom()
        {
            if (_tableLoader != null) Logic_TryLoadData(eDataTableType.GameInfo);
        }

        protected override void Logic_RegisterEvent_Custom()
        {
            Base_Engine.Event.RegisterEvent<eDataTableType, string[], int>(eGlobalEventType.Table_OnResponseData, OnResponseData);
            Base_Engine.Event.RegisterEvent(eGlobalEventType.Option_OnChangeLanguage, Logic_LoadData_TextTable);
        }

        /// <summary>
        /// [기능] 모든 데이터 테이블을 불러옵니다. 
        /// </summary>
        private void Logic_LoadAllData()
        {
            Logic_LoadData_FristInit();
            Logic_LoadData_TextTable();
        }

        /// <summary>
        /// [기능] 게임 전반적으로 구성에 필요한 데이터 테이블을 불러옵니다. 
        /// <br> 가장 먼저 초기화 되어 추가로 초기화가 이루어질 필요가 없는 데이터 테이블들로 구성됩니다. </br>
        /// </summary>
        private void Logic_LoadData_FristInit()
        {
            Logic_TryLoadData(eDataTableType.Stage);
        }


        /// <summary>
        /// [기능] 텍스트 테이블로부터 새로운 텍스트리스트를 받아옵니다.
        /// <br> 텍스트 테이블은 언어 변경에 따라 반복적으로 호출이 필요할 수 있습니다. </br>
        /// </summary>
        public void Logic_LoadData_TextTable()
        {
            Logic_TryLoadData(eDataTableType.ShareText);
            Logic_TryLoadData(eDataTableType.BasicText);
        }

        /// <summary>
        /// [기능] 테이블 로더기로부터 특정 타입의 데이터를 읽어오도록 시도합니다. 
        /// </summary>
        public void Logic_TryLoadData(eDataTableType m_kind)
        {
            StartCoroutine(_tableLoader.LoadData(m_kind));
        }

        private void OnResponseData(eDataTableType m_type, string[] m_dataArray, int m_errorCode)
        {
            if (m_errorCode < 0)
            {
                // TODO :: 에러메시지 출력 (데이터를 불러오는 과정에서 에러가 발생하였습니다.)
                // m_dataArray[0]에 메시지 타입이 담겨있음.
                return;
            }

            switch (m_type)
            {
                case eDataTableType.GameInfo:
                    _tableLoader.LoaderSetting(Convert_GameInfo(m_dataArray));

                    Logic_LoadAllData();
                    break;
                case eDataTableType.Stage:
                    Convert_StageTable(m_dataArray);
                    break;
                case eDataTableType.ShareText:
                    Convert_CommonTextTable(m_dataArray);
                    Global_TextData.OnChangeLanguage();
                    break;
                case eDataTableType.BasicText:
                    Convert_BasicTextTable(m_dataArray);
                    Global_TextData.OnChangeLanguage();
                    break;
                default:
                    // TODO :: 에러메시지 출력 (설정되지않은 데이터 타입이 지정되었습니다.)
                    break;
            }
        }

        #region 데이터 파싱
        private Data_DataTableInfo Convert_GameInfo(string[] m_dataArray)
        {
            string[] resultData = m_dataArray[0].Split("\t");
            Library_DataTable.Info.isDataExists = true;

            Library_DataTable.Info.version = resultData[0];
            return null;
            //_dataTableInfo.version = resultData[0];

            //for (int i = 1; i < resultData.Length - 4; i += 2)
            //{
            //    _dataTableInfo.dataTableList.Add((eDataTableType)((i + 1) / 2), (resultData[i], resultData[i + 1]));
            //}

            //GetCountData(resultData[17], out _dataTableInfo.commonTextTableCount);
            //_dataTableInfo.commonTextTableURL = resultData[18];
            //GetCountData(resultData[19], out _dataTableInfo.basicTextTableCount);
            //_dataTableInfo.basicTextTableURL = resultData[20];
            //return _dataTableInfo;
        }


        private void Convert_StageTable(string[] m_dataArray)
        {
            //Global_Data.stageTable.Clear();

            //for (int i = 0; i < m_dataArray.Length; i++)
            //{
            //    Data_Stage parsingData = new Data_Stage();
            //    string[] dataSegment = m_dataArray[i].Split("\t");

            //    parsingData.index = int.Parse(dataSegment[0]);
            //    parsingData.tileKind = (eMapTileKind)Enum.Parse(typeof(eMapTileKind), dataSegment[1]);

            //    tempStringData = dataSegment[16].Split(",");
            //    parsingData.startEndSpawnDelay = new float[tempStringData.Length];
            //    for (int j = 0; j < tempStringData.Length; j++)
            //    {
            //        parsingData.startEndSpawnDelay[j] = float.Parse(tempStringData[j]);
            //    }

            //    parsingData.spawnDelayInterval = float.Parse(dataSegment[17]);

            //    Global_Data.stageTable.Add(parsingData.index, parsingData);
            //}
        }

        private void Convert_CommonTextTable(string[] m_dataArray)
        {
            Global_TextData.SetCommonText(m_dataArray);
        }

        private void Convert_BasicTextTable(string[] m_dataArray)
        {
            Global_TextData.SetBasicText(m_dataArray);
        }

        /// <summary>
        /// 범위 데이터를 사용하여 데이터의 총 크기를 구합니다.
        /// <br> 입력되는 데이터 양식은 A2:B10 같은 형태여야합니다. </br>
        /// </summary>
        private int GetCountData(string m_data, out int[] m_count)
        {
            string[] tempData = m_data.Split(":");
            m_count = new int[2];

            // 설명 :: A2:B10 데이터 A2부분의 숫자만을 추출하여 totalCount에 담습니다.
            m_count[0] = int.Parse(Regex.Replace(tempData[0], @"\D", ""));
            // 설명 :: A2:B10 데이터 중 B10 부분의 숫자만을 추출하여 기존 길이값을 빼서 총 길이값을 구합니다.
            m_count[1] = int.Parse(Regex.Replace(tempData[1], @"\D", ""));

            return m_count[1] - m_count[0];
        }
        #endregion
    }
}