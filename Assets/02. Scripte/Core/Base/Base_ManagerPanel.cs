namespace IdleGame.Core
{
    /// <summary>
    /// [기능] 매니저에서 통합관리될 수 있는 매니저 패널입니다. 
    /// <br> 해당 패널은 모든 절차가 매니저에서 관리됩니다. </br>
    /// </summary>
    public class Base_ManagerPanel : Base_Panel
    {
        protected override void Awake() { }

        /// <summary>
        /// [초기화] 로직이 실행되면서 초기 필요한 설정들을 지정해주는 초기화 로직입니다.
        /// </summary>
        internal void Logic_Init()
        {
            Logic_Init_Custom();

            Logic_RegisterEvent_Custom();
        }
    }
}