using IdleGame.Core.Procedure;
using IdleGame.Data;
using IdleGame.Data.Common.Event;
using IdleGame.Data.Common.Log;
using IdleGame.Data.NSave;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace IdleGame.Core.Panel.SaveEngine
{
    /// <summary>
    /// [기능] 실질적으로 세이브와 로드의 기능을 담당하는 로직입니다.
    /// </summary>
    public class Logic_Save : MonoBehaviour
    {
        /// <summary>
        /// [상태] 현재 사용가능한 로드데이터가 있는지를 나타냅니다. 
        /// </summary>
        private bool _usedLoadData = false;

        /// <summary>
        /// [상태] 로드 데이터 검증 절차를 진행했는지 확인합니다. 
        /// </summary>
        private bool _checkedLoadData = false;

        private const string _SaveFileName = "TempSave";

        /// <summary>
        /// [경로] 세이브 파일에 대한 확장자명입니다.
        /// </summary>
        private const string _SaveDataExtension = ".save";

        /// <summary>
        /// [변환] 입력한 파일 이름을 기반으로 파일 경로를 반환합니다.
        /// </summary>
        private string GetFilePath() => Path.Combine(Application.persistentDataPath, _SaveFileName + _SaveDataExtension).Replace("\\", "/");

        /// <summary>
        /// [초기화] 세이브 로직을 초기화시킵니다. 
        /// </summary>
        public void init()
        {

        }

        /// <summary>
        /// [기능] 비동기 절차를 생략하고 즉시 세이브를 저장시킵니다.
        /// </summary>
        public void SyncSave()
        {
            List<Interface_SaveData> datas = Global_Data.GetSaveDatas();

            RecordData(GetFilePath(), ref datas);
        }

        /// <summary>
        /// [기능] 세이브 파일을 삭제합니다. 
        /// <br> 개발용 기능입니다.</br>
        /// </summary>
        public void DeleteSaveFile()
        {
            _checkedLoadData = false;
            _usedLoadData = false;

            if (!File.Exists(GetFilePath())) return;

            File.Delete(GetFilePath());
        }

        /// <summary>
        /// [기능] 세이브 로직을 진행시킵니다.
        /// </summary>
        public void Save()
        {
            StartCoroutine(SaveData());
        }

        /// <summary>
        /// [기능] 세이브를 절차적으로 진행시킵니다.
        /// </summary>
        private IEnumerator SaveData()
        {
            List<Interface_SaveData> datas = Global_Data.GetSaveDatas();

            yield return null;

            RecordData(GetFilePath(), ref datas);
            Base_Engine.Event.CallEvent(eGlobalEventType.Save_OnResponseSave);
        }


        /// <summary>
        /// [기능] 텍스트 파일을 열고 데이터를 저장시킵니다.
        /// </summary>
        private void RecordData<T>(string m_filePath, ref T m_dataList)
        {
            using FileStream fileStream = new FileStream(m_filePath, FileMode.Create, FileAccess.Write);
            {
                StreamWriter writer = new StreamWriter(fileStream);

                writer.Write(JsonConvert.SerializeObject(m_dataList));

                writer.Close();
                fileStream.Close();
            }
        }

        /// <summary>
        /// [기능] 데이터를 불러옵니다.
        /// </summary>
        public void Load()
        {
            StartCoroutine(LoadData());
        }

        /// <summary>
        /// [기능] 절차적으로 데이터를 불러옵니다.
        /// </summary>

        private IEnumerator LoadData()
        {
            object[] datas = new object[Global_Data.DataCount];

            Base_Engine.Event.CallEvent(eGlobalEventType.Save_OnResponseStep);
            yield return null;

            ReadData(GetFilePath(), ref datas);

            if (datas.Length != Global_Data.DataCount)
            {
                Base_Engine.Log.Logic_PutLog(new Data_Log(Data_ErrorType.Error_NonCompareDataCount));
                yield break;
            }

            ConvertLoadData(datas);

            Base_Engine.Event.CallEvent(eGlobalEventType.Save_OnResponseStep);
            Base_Engine.Event.CallEvent(eGlobalEventType.Save_OnResponseLoad);
        }

        /// <summary>
        /// [기능] 키와 값으로 분류되어있는 로드 데이터를 합칩니다.
        /// </summary>
        private void ConvertLoadData(object[] m_datas)
        {
            var types = Global_Data.GetTypeList();
            List<Interface_SaveData> datas = new List<Interface_SaveData>();

            for (int i = 0; i < m_datas.Length; i++)
            {
                datas.Add((Interface_SaveData)JsonConvert.DeserializeObject(m_datas[i].ToString(), types[i], (JsonSerializerSettings)null));
            }

            Global_Data.SetSaveDatas(datas);
        }


        /// <summary>
        /// [기능] 세이브 파일을 열어 데이터를 불러옵니다.
        /// </summary>
        private void ReadData(string m_filePath, ref object[] m_dataList)
        {
            if (!Check_UsedLoadData()) return;


            using FileStream fileStream = new FileStream(m_filePath, FileMode.Open, FileAccess.Read);
            {
                StreamReader reader = new StreamReader(fileStream);
                try
                {
                    m_dataList = JsonConvert.DeserializeObject<object[]>(reader.ReadToEnd());
                }
                catch (System.Exception)
                {
                    Base_Engine.Log.Logic_PutLog(new Data_Log(Data_ErrorType.Error_DataLoadFailed, "세이브 로드"));
                }
                reader.Close();
                fileStream.Close();
            }
        }


        /// <summary>
        /// [검사] 현재 로드할 세이브 파일이 존재하는지를 확인합니다. 
        /// </summary>
        public bool Check_UsedLoadData()
        {
            if (_checkedLoadData)
                return _usedLoadData;

            _checkedLoadData = true;
            _usedLoadData = File.Exists(GetFilePath());
            return _usedLoadData;
        }
    }
}