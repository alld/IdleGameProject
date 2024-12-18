using IdleGame.Core.Popup;
using IdleGame.Core.Procedure;

public class Panel_GameOption : Base_AnimationPopup
{

    #region 콜백함수
    public void OnClickClosePopup()
    {
        Logic_Close_Base();
    }

    public void OnClickOpenPopoup()
    {
        Logic_Open_Base();
    }

    public void OnClickSoundMute(bool active)
    {
        Base_Engine.Sound.SoundMute(active);
    }

    public void OnClickMusicMute(bool active)
    {
        Base_Engine.Sound.MusicMute(active);
    }
    #endregion
}
