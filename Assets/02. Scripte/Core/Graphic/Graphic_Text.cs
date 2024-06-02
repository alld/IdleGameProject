using System;
using TMPro;
using UnityEngine;

public class Graphic_Text : MonoBehaviour
{
    [Header("Base")]
    /// <summary>
    /// [캐시] Graphic_Text는 Manager_Text에서 관리되어지는 텍스트의 형태입니다.
    /// <br> 기존 유니티에서 사용되어지는 TMP_Text와 연동되어지기 위해 캐시처리되었습니다.</br>
    /// </summary>
    [SerializeField] private TMP_Text t_text = null;

    [Header("Graphic_Text Setting")]

    /// <summary>
    /// 텍스트의 내용을 설정합니다. 해당 내용을 None이외의 값을 사용하면 kind_basic이 -1로 취급됩니다.
    /// </summary>
    [SerializeField] private eTextKind kind_base = eTextKind.None;

    /// <summary>
    /// 텍스트의 내용을 설정합니다. base가 None의외의 값으로 설정된 경우 해당 값을 변경하더라도 -1로 적용됩니다. 
    /// </summary>
    [SerializeField] private int kind_basic_index = -1;

    /// <summary>
    /// 커스텀하게 텍스트값이 설정된 경우에는 업데이트 대상에서 제외됩니다. 
    /// <br> 언어 변경의 대상이 되지않습니다. </br>
    /// </summary>
    private bool isCustomText = false;

    /// <summary>
    /// 기존 t_text.text에 접근하지않더라도 Graphic_Text.text에서 바로 수정할수 있도록 캐시처리되었습니다.
    /// <br> 해당 값을 직접 설정할 경우 커스텀 텍스트로 지정됩니다. </br>
    /// </summary>
    public string text
    {
        get { return t_text.text; }
        set
        {
            isCustomText = true;
            t_text.text = value;
        }
    }

    private void Start()
    {
        RegisterText();

        TextResetting();
    }

    private void OnEnable()
    {
        TextResetting();
    }

    private void OnDestroy()
    {
        Global_TextData.RemoveText(this);
    }

    /// <summary>
    /// [기능] 최초 1회 실행되며, 텍스트를 텍스트매니저에 등록시킵니다.
    /// </summary>
    private void RegisterText()
    {
        if (t_text == null) t_text = GetComponent<TMP_Text>();
        Global_TextData.RegisterText(this);
    }

    /// <summary>
    /// [기능] 설정된 데이터를 기반으로 값을 재조정합니다. 
    /// </summary>
    private void TextResetting()
    {
        if (kind_base != eTextKind.None) kind_basic_index = -1;

        UpdateText();
    }

    /// <summary>
    /// [기능] 현재 설정된 텍스트 설정에 맞쳐서 텍스트를 갱신합니다.
    /// </summary>
    public void UpdateText()
    {
        if (isCustomText) return;

        if (kind_base != eTextKind.None)
        {
            t_text.text = Global_TextData.GetText(kind_base);
        }
        else if (kind_basic_index != -1)
        {
            t_text.text = Global_TextData.GetText(kind_basic_index);
        }
    }

    /// <summary>
    /// [기능] 텍스트를 새롭게 설정합니다. 커스텀 텍스트가 적용되면 언어 변경의 적용을 받지않습니다.
    /// </summary>
    public void SetText(string m_text)
    {
        text = m_text;
    }

    /// <summary>
    /// [기능] 공통 텍스트 타입으로 텍스트를 새롭게 설정합니다.
    /// </summary>
    public void SetText(eTextKind m_kind)
    {
        isCustomText = false;
        kind_base = m_kind;

        UpdateText();
    }

    /// <summary>
    /// [기능] 기본 텍스트의 형태로 새롭게 설정합니다.
    /// </summary>
    /// <param name="m_index"></param>
    public void SetText(int m_index)
    {
        isCustomText = false;
        kind_basic_index = m_index;

        UpdateText();
    }

    /// <summary>
    /// [기능] 기본 텍스트를 게임 텍스트 타입의 텍스트로 새롭게 설정합니다.
    /// <br> 게임에서 사용되어지는 별도의 타입이 존재한다면 해당 함수를 통해서 사용하세요. </br>
    /// </summary>
    /// <typeparam name="T">[종류] 열거형이 사용되어야합니다.</typeparam>
    public void SetText<T>(T m_kind) where T : Enum
    {
        isCustomText = false;
        kind_basic_index = Convert.ToInt32(m_kind);

        UpdateText();
    }
}
