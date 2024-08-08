using System.Collections.Generic;

namespace IdleGame.Data.Common.Log
{

    /// <summary>
    /// [데이터] 모든 에러 타입의 데이터들이 있습니다. 
    /// </summary>
    public struct Data_ErrorType
    {
        #region 에러 타입 정의

        /// <summary>
        /// [데이터] 에러 타입에대한 데이터입니다. 텍스트 및 인덱스를 가지고 있습니다.
        /// </summary>
        public struct Data
        {
            /// <summary>
            /// [데이터] 데이터 타입의 인덱스를 추출하기위해 카운터되는 데이터입니다.
            /// </summary>
            private static int dataCount = 0;

            /// <summary>
            /// [데이터] 에러 텍스트를 순서대로 나열하여 번호를 매긴 번호입니다. 
            /// </summary>
            private int index { get; }
            /// <summary>
            /// [데이터] 에러 타입이 지니는 텍스트입니다. 
            /// </summary>
            private string text { get; }

            /// <summary>
            /// [생성자] 데이터 타입을 추가합니다. 
            /// </summary>
            /// <param name="m_text"></param>
            public Data(string m_text)
            {
                index = dataCount++;
                text = m_text;

                RegisterData(this);
            }

            /// <summary>
            /// [변환] 해당 에러 텍스트에 대한 인덱스를 int로 변환하여 반환할 수 있습니다.
            /// </summary>
            /// <param name="data"></param>
            public static explicit operator int(Data data) => data.index;

            /// <summary>
            /// [변환] 해당 에러 타입에대한 텍스트 문구를 반환하여 문장을 더합니다. 
            /// </summary>
            /// <param name="data"></param>
            /// <param name="m_text"></param>
            /// <returns></returns>
            public static string operator +(Data data, string m_text) => data.text + m_text;

            /// <summary>
            /// [변환] 해당 에러 타입에대한 텍스트 문구를 반환합니다. 
            /// </summary>
            /// <returns></returns>
            public override string ToString() => $"{text}";
        }
        /// <summary>
        /// [데이터] 인덱스를 통해서 에러 타입을 찾기위해 리스트화하여 관리됩니다. 
        /// </summary>
        private static List<Data> SearchDataList = new List<Data>();

        /// <summary>
        /// [기능] 데이터 리스트에 에러타입을 추가합니다. 
        /// </summary>
        /// <param name="data"></param>
        private static void RegisterData(Data data) => SearchDataList.Add(data);
        /// <summary>
        /// [기능] 인덱스를 통해서 에러타입을 색인할수 있습니다. 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Data FindErrorData(int index) => SearchDataList[index];
        #endregion


        #region 에러 리스트
        public static readonly Data Error_NetworkConnect = new Data("네트워크 연결 상태가 원활하지 않습니다.");
        public static readonly Data Error_TryChangeSceneFail = new Data("씬 전환에 실패하였습니다.");
        public static readonly Data Error_TextLoadFailed = new Data("텍스트를 불러오는데 실패하였습니다.");
        public static readonly Data Error_DataLoadFailed = new Data("데이터를 불러오는데 실패하였습니다.");
        public static readonly Data Error_DataParsingFailed = new Data("데이터를 변환하는 과정에서 문제가 생겼습니다.");
        public static readonly Data Error_NonCompareDataCount = new Data("불러온 데이터가 Global_Data의 형식과 맞지않습니다.");
        #endregion

        #region 경고 리스트
        public static readonly Data Warning_IsSaveing = new Data("이미 세이브가 진행중입니다. 중복으로 진행이 실행되었습니다.");
        public static readonly Data Warning_InsufficientSetting = new Data("미할당된 오브젝트 설정이 존재합니다.");
        public static readonly Data Warning_NullValueSoundPlay = new Data("미할당된 사운드가 재생되었습니다.");
        // 코루틴 중복 실행
        public static readonly Data Warning_FunctionRunInDuplicate = new Data("코루틴 함수가 중복 실행되었습니다.");

        // 텍스트 관련
        public static readonly Data Warning_LoadEmptyText = new Data("비어있는 텍스트를 불러왔습니다.");

        public static readonly Data Warning_InvalidTextLoadType = new Data("텍스트를 불러오는 설정이 잘못되었습니다.");
        #endregion
    }
}