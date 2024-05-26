using IdleGame.Core.GameInfo;
using System;
using UnityEngine;


namespace IdleGame.Data.Common.Log
{
    /// <summary>
    /// [종류] 기록되는 로그의 타입을 지정합니다.
    /// </summary>
    public enum eLogType
    {
        None = 0,
        /// <summary>[종류] 에러 발생 로그 </summary>
        Error,
        /// <summary>[종류] 클라이언트 커스텀 로그 </summary>
        Client,
    }

    public struct Data_Log
    {
        /// <summary>
        /// [데이터] 최종적으로 기록되는 로그 내용입니다. 
        /// </summary>
        public string text;

        /// <summary>
        /// [종류] 선택된 타입에 따라서 저장되는 경로나 환경이 달라집니다. 
        /// </summary>
        public eLogType type;

        /// <summary>
        /// [기능] 에러 타입에대한 로그 포멧을 만듭니다. 
        /// </summary>
        /// <param name="m_data"></param>
        /// <param name="m_tag"></param>
        /// <param name="m_type"></param>
        public Data_Log(Data_ErrorType.Data m_data, string m_tag = null, eLogType m_type = eLogType.Error)
        {
            type = m_type;

            if (string.IsNullOrEmpty(m_tag))
            {
                text =
                    $"발생 지점 : " + Application.productName + "\n" +
                    $"게임 버전 : " + Global_GameInfo.version + "::" + Application.version + "v\n" +
                    $"발생 시간 : " + DateTime.Now.ToString() + "\n" +
                    $"플랫폼 : " + Application.platform + "\n" +
                    $"현재 씬 : " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + "\n" +
                    $"내용 : ({((int)m_data)})\n" +
                    m_data.ToString();
            }
            else
            {
                text =
                    $"발생 지점 : " + Application.productName + "::" + m_tag + "\n" +
                    $"게임 버전 : " + Global_GameInfo.version + "::" + Application.version + "v\n" +
                    $"발생 시간 : " + DateTime.Now.ToString() + "\n" +
                    $"플랫폼 : " + Application.platform + "\n" +
                    $"현재 씬 : " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + "\n" +
                    $"내용 : ({((int)m_data)})\n" +
                    m_data.ToString();
            }
        }

        /// <summary>
        /// [기능] 에러 타입과 추가 내용을 담은 로그 포멧을 만듭니다.  
        /// </summary>
        /// <param name="m_data"></param>
        /// <param name="m_tag"></param>
        /// <param name="m_type"></param>
        public Data_Log(string m_contents, Data_ErrorType.Data m_data, string m_tag = null, eLogType m_type = eLogType.Error)
        {
            type = m_type;

            if (string.IsNullOrEmpty(m_tag))
            {
                text =
                    $"발생 지점 : " + Application.productName + "\n" +
                    $"게임 버전 : " + Global_GameInfo.version + "::" + Application.version + "v\n" +
                    $"발생 시간 : " + DateTime.Now.ToString() + "\n" +
                    $"플랫폼 : " + Application.platform + "\n" +
                    $"현재 씬 : " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + "\n" +
                    $"에러내용 : \n" +
                    m_data.ToString() + "\n" +
                    $"내용 : ({((int)m_data)})\n" +
                    m_contents;
            }
            else
            {
                text =
                    $"발생 지점 : " + Application.productName + "::" + m_tag + "\n" +
                    $"게임 버전 : " + Global_GameInfo.version + "::" + Application.version + "v\n" +
                    $"발생 시간 : " + DateTime.Now.ToString() + "\n" +
                    $"플랫폼 : " + Application.platform + "\n" +
                    $"현재 씬 : " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + "\n" +
                    $"에러내용 : \n" +
                    m_data.ToString() + "\n" +
                    $"내용 : ({((int)m_data)})\n" +
                    m_contents;
            }
        }

        /// <summary>
        /// [기능] 커스텀 타입에 로그 포멧을 만듭니다. 
        /// </summary>
        /// <param name="m_logContents"></param>
        /// <param name="m_tag"></param>
        /// <param name="m_type"></param>
        public Data_Log(string m_logContents, string m_tag = null, eLogType m_type = eLogType.Client)
        {
            type = m_type;

            if (string.IsNullOrEmpty(m_tag))
            {
                text =
                    $"발생 지점 : " + Application.productName + "\n" +
                    $"게임 버전 : " + Global_GameInfo.version + "::" + Application.version + "v\n" +
                    $"발생 시간 : " + DateTime.Now.ToString() + "\n" +
                    $"플랫폼 : " + Application.platform + "\n" +
                    $"현재 씬 : " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + "\n" +
                    $"내용 : \n" +
                    m_logContents;
            }
            else
            {
                text =
                    $"발생 지점 : " + Application.productName + "::" + m_tag + "\n" +
                    $"게임 버전 : " + Global_GameInfo.version + "::" + Application.version + "v\n" +
                    $"발생 시간 : " + DateTime.Now.ToString() + "\n" +
                    $"플랫폼 : " + Application.platform + "\n" +
                    $"현재 씬 : " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + "\n" +
                    $"내용 : \n" +
                    m_logContents;
            }
        }

        /// <summary>
        /// [기능] 예외에 대한 로그 포멧을 만듭니다. 
        /// </summary>
        /// <param name="m_logContents"></param>
        /// <param name="m_tag"></param>
        /// <param name="m_type"></param>
        public Data_Log(Exception e, string m_tag = null)
        {
            type = eLogType.Error;

            if (string.IsNullOrEmpty(m_tag))
            {
                text =
                    $"예외 발생 \n" +
                    $"발생 지점 : " + Application.productName + "\n" +
                    $"게임 버전 : " + Global_GameInfo.version + "::" + Application.version + "v\n" +
                    $"발생 시간 : " + DateTime.Now.ToString() + "\n" +
                    $"플랫폼 : " + Application.platform + "\n" +
                    $"현재 씬 : " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + "\n" +
                    $"내용 : \n" +
                    e.ToString();
            }
            else
            {
                text =
                    $"예외 발생 \n" +
                    $"발생 지점 : " + Application.productName + "::" + m_tag + "\n" +
                    $"게임 버전 : " + Global_GameInfo.version + "::" + Application.version + "::" + Application.version + "v\n" +
                    $"발생 시간 : " + DateTime.Now.ToString() + "\n" +
                    $"플랫폼 : " + Application.platform + "\n" +
                    $"현재 씬 : " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + "\n" +
                    $"내용 : \n" +
                    e.ToString();
            }
        }
    }
}