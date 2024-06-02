using IdleGame.Core.Module;
using System;
using System.Collections.Generic;

/// <summary>
/// [종류] text_common에 해당하는 텍스트 종류입니다.
/// </summary>
public enum eTextKind
{
    None = 0,
    #region 공용팝업
    /// <summary> 안내</summary>
    CommonP_Information = 1,
    /// <summary> 확인 </summary>
    CommonP_Check,
    /// <summary> 닫기</summary>
    CommonP_Close,
    /// <summary> 텍스트를 입력해주세요.</summary>
    CommonP_EnterText,

    #endregion

    #region 인트로
    /// <summary> 진행을 하려면 화면을 클릭하세요.</summary>
    Intro_TuchScreen = 9,
    /// <summary> 오프라인</summary>
    Intro_Offline,
    #endregion

    #region 메인메뉴 패널
    /// <summary> 게임시작</summary>
    MainMenu_Start = 13,
    /// <summary> 콜렉션 </summary>
    MainMenu_Collection,
    /// <summary> 환경설정 </summary>
    MainMenu_Option,
    /// <summary> 재능 </summary>
    MainMenu_Possibility,
    /// <summary> 강화 </summary>
    MainMenu_Upgrade,
    /// <summary> 크레딧</summary>
    MainMenu_Credit,
    /// <summary> 종료</summary>
    MainMenu_Quit,
    #endregion

    #region 옵션 팝업
    /// <summary> 게임설정 </summary>
    Option_GameSetting,
    /// <summary> 사운드 </summary>
    Option_Sound,
    #endregion

    #region 슬롯 선택 팝업
    /// <summary> 슬롯 선택</summary>
    Select_SlotSelect,
    #endregion

    #region 옵션 메뉴
    /// <summary> 저장하기 </summary>
    OptionMenu_Save,
    /// <summary> 도움말 </summary>
    OptionMenu_Help,
    /// <summary> 메인메뉴 </summary>
    OptionMenu_MainMenu,
    #endregion

}


public static class Global_TextData 
{
    /// <summary>
    /// [데이터] 공통으로 사용되어지는 고정 텍스트입니다. 
    /// <br> 해당 텍스트들은 enum으로 관리될수 있습니다. </br>
    /// </summary>
    internal static Dictionary<int, string> text_common = new Dictionary<int, string>();

    /// <summary>
    /// [데이터] 일반적으로 사용되어지는 텍스트입니다.
    /// </summary>
    internal static Dictionary<int, string> text_basic = new Dictionary<int, string>();

    /// <summary>
    /// [캐시] 모든 텍스트를 담는 리스트입니다.
    /// </summary>
    private static List<Graphic_Text> _textList = new List<Graphic_Text>();


    public static bool IsSettingData() => text_common.Count != 0 && text_basic.Count != 0;

    /// <summary>
    /// [기능] 현재의 언어 설정에 맞쳐서 모든 텍스트를 업데이트합니다.
    /// <br> 언어 설정이 바뀐 경우 호출됩니다.</br>
    /// </summary>
    public static void OnChangeLanguage()
    {
        UpdateAllText();
    }


    /// <summary>
    /// [기능] 텍스트를 매니저에 등록시킵니다.
    /// </summary>
    public static void RegisterText(Graphic_Text m_text)
    {
        _textList.Add(m_text);
    }

    /// <summary>
    /// [기능] 등록된 텍스트를 제거합니다. 
    /// </summary>
    /// <param name="m_text"></param>
    public static void RemoveText(Graphic_Text m_text)
    {
        _textList.Remove(m_text);
    }

    /// <summary>
    /// [기능] 모든 텍스트를 갱신합니다. 
    /// </summary>
    public static void UpdateAllText()
    {
        foreach (var text in _textList)
        {
            text.UpdateText();
        }
    }

    /// <summary>
    /// [설정] 공통 텍스트 리스트를 지정합니다. 
    /// </summary>
    internal static void SetCommonText(string[] m_textList)
    {
        text_common.Clear();
        int index = 0;
        foreach (var text in m_textList)
        {
            text_common.Add(index++, text);
        }
    }

    /// <summary>
    /// [설정] 일반 텍스트 리스트를 지정합니다. 
    /// </summary>
    internal static void SetBasicText(string[] m_textList)
    {
        text_basic.Clear();
        int index = 0;
        foreach (var text in m_textList)
        {
            text_basic.Add(index++, text);
        }
    }

    /// <summary>
    /// [데이터] 일반적으로 사용되어지는 텍스트를 불러옵니다.
    /// </summary>
    public static string GetText(int m_index)
    {
        try
        {
            return text_basic[m_index];
        }
        catch (System.Exception)
        {
            return "Empty Text";
        }
    }

    /// <summary>
    /// [데이터] 공통적으로 사용되어지는 텍스트를 불러옵니다. 
    /// </summary>
    public static string GetText(eTextKind m_kind)
    {
        try
        {
            return text_common[Convert.ToInt32(m_kind)];
        }
        catch (System.Exception)
        {
            return "Empty Text";
        }
    }

    /// <summary>
    /// [기능] 데이터를 모두 정리합니다.
    /// </summary>
    internal static void ClearData()
    {
        text_common = null;
        text_basic = null;
    }
}
