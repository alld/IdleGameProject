using IdleGame.Data.Base;

namespace IdleGame.Data.Popup
{
    /// <summary>
    /// [데이터] 공통 팝업을 구성할때 사용되는 팝업 데이터입니다. 
    /// </summary>
    public struct Data_Popup
    {
        /// <summary>
        /// [데이터] 팝업 제목에 들어갈 텍스트입니다.
        /// </summary>
        public string title;

        /// <summary>
        /// [데이터] 팝업 내용에 들어갈 텍스트입니다. 
        /// </summary>
        public string content;

        /// <summary>
        /// [캐시] 확인 버튼이 눌렸을때 호출되는 콜백 함수입니다.
        /// </summary>
        public Dele_Action callback_Ok;


        /// <summary>
        /// [생성자] 제목, 내용, 콜백 함수를 입력하여 팝업 데이터를 구성합니다. 
        /// </summary>
        public Data_Popup(string m_title, string m_content, Dele_Action m_callback = null)
        {
            title = m_title;
            content = m_content;
            callback_Ok = m_callback;
        }

        /// <summary>
        /// [생성자] 내용, 콜백 함수를 입력하여 팝업 데이터를 구성합니다. 제목은 빈값이 들어갑니다.
        /// </summary>
        public Data_Popup(string m_content, Dele_Action m_callback = null)
        {
            title = null;
            content = m_content;
            callback_Ok = m_callback;
        }
    }
}