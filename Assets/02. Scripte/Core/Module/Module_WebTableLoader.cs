using IdleGame.Core.Procedure;
using IdleGame.Data;
using IdleGame.Data.Common.Event;
using IdleGame.Data.Common.Log;
using IdleGame.Data.DataTable;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine.Networking;

namespace IdleGame.Core.Module.DataTable
{
    /// <summary>
    /// [기능] 구글 스프레드 시트로부터 데이터를 긁어와서 게임에 적용시킵니다.
    /// </summary>
    public class Module_WebTableLoader : Base_Module
    {
        /// <summary>
        /// [데이터] 게임 데이터 테이블이 존재하는 구글 스프레드시트 기반 주소입니다.
        /// </summary>
        private string DefaultURL = "https://docs.google.com/spreadsheets/d/1xVP1PT_xdm_GWn5_hVe3_m_eymSgrm04ZmbDAxrHXcU/export?format=tsv&gid=0&range=B2:F100";

        /// <summary>
        /// 언어 설정이 포함된 공통 텍스트 주소를 반환합니다.
        /// </summary>
        private string GetGooleCommonURL(eDataTableType m_type)
        {
            switch (m_type)
            {
                case eDataTableType.GameInfo:
                    return DefaultURL;
                case eDataTableType.BasicText:
                case eDataTableType.ShareText:
                    return Library_DataTable.Info.dataTableList[m_type].Item1 + Convert_GetLanguageRange(Library_DataTable.Info.dataTableList[m_type].Item2);
                default:
                    return Library_DataTable.Info.dataTableList[m_type].Item1 + Library_DataTable.Info.dataTableList[m_type].Item2;
            }
        }

        /// <summary>
        /// [변환] 텍스트 같은 경우 언어값이 반영되어야하기때문에 언어에 맞쳐서 범위값을 재조정합니다. 
        /// </summary>
        private string Convert_GetLanguageRange(string m_range)
        {
            Match match = Regex.Match(m_range, @"(\d+):.*?(\d+)");

            if (match.Success)
            {
                return (char)Global_Data.Option.language + match.Groups[1].Value + ":" + (char)Global_Data.Option.language + match.Groups[2].Value;
            }

            Base_Engine.Log.Logic_PutLog(new Data_Log("잘못된 범위값이 입력되었습니다. 변환에 실패하였습니다.", Data_ErrorType.Error_TextLoadFailed, "Loader"));
            return null;
        }

        /// <summary>
        /// [기능] 구글 스프레드 시트에서 데이터 정보를 가져옵니다.
        /// </summary>
        public IEnumerator LoadData(eDataTableType m_kind)
        {
            // returnData :: 스프레드 시트로부터 반환받은 데이터입니다. // 
            string returnData = string.Empty;

            #region 공통 텍스트 설정
            using (UnityWebRequest www = UnityWebRequest.Get(GetGooleCommonURL(m_kind)))
            {
                yield return www.SendWebRequest();
                returnData = www.downloadHandler.text;
                int errorCode = 0;
                string[] dataArray = null;
                switch (www.result)
                {
                    case UnityWebRequest.Result.Success:
                        // -- 데이터 가공 -- //
                        returnData = returnData.Replace("\r\n", "\n");
                        // --------------- // 

                        dataArray = returnData.Split("\n");
                        break;
                    default:
                        dataArray = new string[] { www.result.ToString() };
                        errorCode = -1;
                        break;
                }

                Base_Engine.Event.CallEvent(eGlobalEventType.Table_OnResponseData, m_kind, dataArray, errorCode);
            }
            #endregion
        }
    }
}