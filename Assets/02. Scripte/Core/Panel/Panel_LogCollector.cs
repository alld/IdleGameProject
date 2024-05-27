using IdleGame.Data.Common.Log;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace IdleGame.Core.Panel.LogCollector
{
    /// <summary>
    /// [모듈] 에러 수집기는 발생한 에러들을 수집하여 처리하는 방법을 정의합니다. 
    /// </summary>
    public class Panel_LogCollector : Base_Panel
    {
        /// <summary>
        /// [상태] 해당 에러가 수집되는 장소를 지정합니다. 
        /// </summary>
        [Header("LogCollecter")]
        [SerializeField]
        [Tooltip("\nTrue : 로컬 환경에 에러가 수집됩니다. \nFalse : 네트워크 통신을 통한 외부 환경에 에러가 수집됩니다.")]
        private bool _isLocalSave = true;

        [SerializeField]
        private string exportPath_Error = string.Empty;

        [SerializeField]
        private string exportPath_Client = string.Empty;




        /// <summary>
        /// [기능] 클라이언트 내부 환경에서 발생한 로그를 외부 환경에서 수집합니다.
        /// </summary>
        public void Logic_PutLog(Data_Log m_log)
        {
            if (_isLocalSave)
            {

            }
            else
            {
                StartCoroutine(Post(m_log));
            }
        }

        /// <summary>
        /// [기능] 외부로 로그를 내보냅니다. 
        /// </summary>
        /// <param name="m_log"></param>
        /// <returns></returns>
        internal IEnumerator Post(Data_Log m_log)
        {
            string URL = null;
            switch (m_log.type)
            {
                case eLogType.Error:
                    URL = exportPath_Error;
                    break;
                case eLogType.Client:
                    URL = exportPath_Client;
                    break;
            }

            if (string.IsNullOrEmpty(URL))
            {
                yield break;
            }

            yield return null;

            using (UnityWebRequest www = new UnityWebRequest(URL, "POST"))
            {
                www.SetRequestHeader("Content-Type", "application/json");

                var json = JsonUtility.ToJson(m_log);
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
                www.uploadHandler = new UploadHandlerRaw(bodyRaw);

                yield return www.SendWebRequest();
                if (www.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.LogWarning((new Data_Log(www.error, Data_ErrorType.Error_NetworkConnect, "Logic_ExportLog")));
                }
            }
        }
    }
}