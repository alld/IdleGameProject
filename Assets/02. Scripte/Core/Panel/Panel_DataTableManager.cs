using IdleGame.Core.Module.DataTable;
using IdleGame.Core.Procedure;
using IdleGame.Data;
using IdleGame.Data.Common.Event;
using IdleGame.Data.Common.Log;
using IdleGame.Data.DataTable;
using System.ComponentModel;
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
        private Module_WebTableLoader _tableLoader = new Module_WebTableLoader();
#else
        [SerializeField]
        private Module_TSVConvert _tableLoader = new Module_TSVConvert();
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
                Base_Engine.Log.Logic_PutLog(new Data_Log($"데이터를 불러오는데 실패하였습니다.\n {m_dataArray[0]}", Data_ErrorType.Error_DataLoadFailed, _tag.tag));
                return;
            }

            switch (m_type)
            {
                case eDataTableType.GameInfo:
                    Convert_GameInfo(m_dataArray);

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
                    Base_Engine.Log.Logic_PutLog(new Data_Log($"미 할당된 데이터 로드를 시도하였습니다.\n {m_type}", Data_ErrorType.Error_DataLoadFailed, _tag.tag));
                    // TODO :: 에러메시지 출력 (설정되지않은 데이터 타입이 지정되었습니다.)
                    break;
            }
        }

        #region 데이터 파싱
        private void Convert_GameInfo(string[] m_dataArray)
        {
            string[] resultData = m_dataArray[0].Split("\t");
            Library_DataTable.Info.isDataExists = true;

            Library_DataTable.Info.version = resultData[0];

            for (int i = 1; i < resultData.Length; i += 2)
            {
                Library_DataTable.Info.dataTableList.Add((eDataTableType)((i + 1) / 2), (resultData[i], resultData[i + 1]));
            }
        }


        private void Convert_StageTable(string[] m_dataArray)
        {
            Library_DataTable.stage.Clear();

            for (int i = 0; i < m_dataArray.Length; i++)
            {
                Data_Stage parsingData = new Data_Stage();
                string[] dataSegment = m_dataArray[i].Split("\t");

                parsingData.index = int.Parse(dataSegment[0]);
                parsingData.maxWave = int.Parse(dataSegment[1]);
                parsingData.isMainStory = bool.Parse(dataSegment[2]);
                Convert_Array(ref parsingData.waveType, dataSegment[3]);
                parsingData.storyIndex = int.Parse(dataSegment[4]);
                Convert_Array(ref parsingData.wave_unitKind, dataSegment[5]);
                Convert_Array(ref parsingData.wave_unitCount, dataSegment[5]);

                Library_DataTable.stage.Add(parsingData.index, parsingData);
            }
        }

        /// <summary>
        /// [변환] 1차 배열의 값을 적절하게 파싱해줍니다.
        /// </summary>
        private void Convert_Array<T>(ref T[] m_parsingData, string m_dataSegment)
        {
            if (m_dataSegment == "") return;

            string[] arrayData = m_dataSegment.Split(",");
            m_parsingData = new T[arrayData.Length];
            for (int i = 0; i < arrayData.Length; i++)
            {
                m_parsingData[i] = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(arrayData[i]);
            }
        }

        /// <summary>
        /// [변환] 2차 배열의 값을 적절하게 파싱해줍니다.
        /// </summary>
        private void Convert_Array<T>(ref T[][] m_parsingData, string m_dataSegment)
        {
            if (m_dataSegment == "") return;

            string[] arrayData = m_dataSegment.Split("//");
            m_parsingData = new T[arrayData.Length][];
            for (int i = 0; i < arrayData.Length; i++)
            {
                string[] arrayData_N = arrayData[i].Split(",");
                m_parsingData[i] = new T[arrayData_N.Length];
                for (int j = 0; j < arrayData_N.Length; j++)
                {
                    m_parsingData[i][j] = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(arrayData_N[j]);
                }
            }
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