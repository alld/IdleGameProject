
namespace IdleGame.Data.Base
{
    #region 대리자 선언
    /// <summary>
    /// [대리자] 게임내에서 가장 기본적으로 사용되는 통용 액션입니다. 
    /// </summary>
    public delegate void Dele_Action();
    /// <summary>
    /// [대리자] 매개변수가 없는 이벤트 타입입니다.
    /// </summary>
    public delegate void Dele_EventFunc();
    /// <summary>
    /// [대리자] 매개변수가 한개인 이벤트 타입입니다.
    /// </summary>
    public delegate void Dele_EventFunc<T>(T param1);
    /// <summary>
    /// [대리자] 매개변수가 두개인 이벤트 타입입니다.
    /// </summary>
    public delegate void Dele_EventFunc<T1, T2>(T1 param1, T2 param2);
    /// <summary>
    /// [대리자] 매개변수가 세개인 이벤트 타입입니다.
    /// </summary>
    public delegate void Dele_EventFunc<T1, T2, T3>(T1 param1, T2 param2, T3 param3);
    #endregion


    namespace Language
    {
        /// <summary>
        /// [종류] 언어 종류입니다.
        /// </summary>
        public enum eLanguage
        {
            /// <summary> [언어] 한국어 </summary>
            Kr = 65,
            /// <summary> [언어] 영어</summary>
            En,
            /// <summary> [언어] 일본어</summary>
            Jp,
        }
    }


    namespace Scene
    {
        /// <summary>
        /// [종류] 현재 게임에 적용된 씬의 종류를 나타냅니다.
        /// <br> 해당 씬의 이름은 씬과 동일해야합니다. </br>
        /// </summary>
        public enum eSceneKind
        {
            None = -1,
            /// <summary> [종류] 로드 씬 </summary>
            Load,
            /// <summary> [종류] 인트로 씬 </summary>
            Intro,


            /// <summary> [종류] 로딩 씬 </summary>
            Loading = 50,
        }
    }


    /// <summary>
    /// [정보] 로직의 대한 정보가 담겨 있습니다.
    /// </summary>
    [System.Serializable]
    public struct TagInfo
    {
        /// <summary>
        /// [정보] 로직을 구분하는 명칭을 담습니다. 
        /// </summary>
        public string tag;

        /// <summary>
        /// [데이터] 로직이 어떤 형태인지에대한 속성을 구분합니다. 
        /// </summary>
        public eLogicType type;

        /// <summary>
        /// [상태] 현재 로직의 상태를 나타냅니다. 
        /// </summary>
        public eProcedures procedure;
    }

    /// <summary>
    /// [데이터] 구조체 형태의 자료형을 기분 구조로 생성자를 잡기위해 선언된 빈 타입입니다. 
    /// </summary>
    public struct DefaultData { }

    /// <summary>
    /// [종류] 로직의 취급 형태를 구분하는 데이터입니다. 
    /// </summary>
    public enum eLogicType
    {
        None = 0,
        /// <summary>
        /// [종류] 2가지 형태중 하나를 의미합니다. 
        /// <br> - 로직 또는 데이터의 기반</br>
        /// <br> - 가장 기본적으로 사용되어지는 공통 로직 또는 데이터 </br></summary>
        Base,
        /// <summary>
        /// [종류] 가장 보편적으로 사용되어지는 로직 또는 데이터
        /// </summary>
        Basic,
        /// <summary>
        /// [종류] 독립형 로직 또는 데이터입니다. 다른 시스템에 종속되지않습니다. 
        /// </summary>
        Independent
    }

    /// <summary>
    /// [종류] 로직의 현재 상태를 나타냅니다.
    /// </summary>
    public enum eProcedures
    {
        /// <summary>
        /// [상태] 비정상 중지 상태입니다.
        /// </summary>
        Stop = -1,

        /// <summary>
        /// [상태] 중지 중입니다. 
        /// </summary>
        Stopping = -11,

        /// <summary>
        /// [상태] 미설정 상태입니다. 
        /// </summary>
        None = 0,

        /// <summary>
        /// [상태] 사용이 가능한 기본 상태입니다.
        /// </summary>
        Stay = 1,

        /// <summary>
        /// [상태] 초기화가 진행중입니다.
        /// </summary>
        Initing = -3,

        /// <summary>
        /// [상태] 초기화가 완료된 상태입니다.
        /// </summary>
        Init_Complete = 3,

        /// <summary>
        /// [상태] 일시정지 된 상태입니다.
        /// </summary>
        Pause = -2,

        /// <summary>
        /// [상태] 정상 로직이 작동중인 상태입니다.
        /// </summary>
        Running = 5,

        /// <summary>
        /// [상태] 미사용중인 기능입니다.
        /// </summary>
        NotUsed = -10,
    }
}