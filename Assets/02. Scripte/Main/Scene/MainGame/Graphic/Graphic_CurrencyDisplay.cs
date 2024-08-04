using IdleGame.Core.Panel;
using IdleGame.Core.Procedure;
using UnityEngine;

namespace IdleGame.Main.UI
{
    /// <summary>
    /// [기능] 특정 재화를 보여주는 디스플레이 패널입니다.
    /// </summary>
    public class Graphic_CurrencyDisplay : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _rect;

        [SerializeField]
        private Panel_CurrencyManager.Data_DisplayComponent _dc;


        private void OnDisable()
        {
            Base_Engine.Currency.Logic_RemoveCurrencyType(_dc.type);
        }

        private void OnEnable()
        {
            //Ray ray = Camera.main.ScreenPointToRay(RectTransformUtility.WorldToScreenPoint(null, _rect.position));
            //RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.origin + ray.direction * 100, Mathf.Infinity, 1 << LayerMask.NameToLayer("Plane"));

            //// UI 요소의 위치를 스크린 좌표로 변환
            //Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(null, _rect.position);

            //// 스크린 좌표를 사용하여 레이 생성
            //Ray ray = Camera.main.ScreenPointToRay(screenPoint);

            //// 레이캐스트를 통해 바닥면의 충돌 지점 찾기
            //RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, 1 << LayerMask.NameToLayer("Plane"));

            //// UI 요소의 월드 좌표를 가져옴
            //Vector3 uiWorldPosition = _rect.position;

            //// 레이 방향을 아래쪽으로 설정
            //Vector2 direction = Vector2.down;

            //// 레이캐스트를 통해 바닥면의 충돌 지점 찾기
            //RaycastHit2D hit = Physics2D.Raycast(uiWorldPosition, direction, Mathf.Infinity, 1 << LayerMask.NameToLayer("Plane"));

            ////// 레이캐스트 결과 출력
            //Debug.DrawRay(_rect.position, direction * 10, Color.red, 2f); // 레이 방향을 시각적으로 표시


            Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, _rect.position);

            // 스크린 좌표를 월드 좌표로 변환 (Z값은 카메라와의 거리로 설정)
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, Camera.main.nearClipPlane));

            // 레이 방향 설정
            Vector2 direction = Vector2.down; // 아래쪽으로 레이캐스트

            // 레이캐스트를 통해 BoxCollider2D의 충돌 지점 찾기
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, direction, Mathf.Infinity, 1 << LayerMask.NameToLayer("Plane"));

            if (hit.collider != null)
                _dc.endPoint = hit.point;
            else
                _dc.endPoint = Vector3.zero;



            Base_Engine.Currency.Logic_SetCurrencyType(_dc.type);
            Base_Engine.Currency.Logic_RegisterPanel(_dc);
        }

        private void OnDrawGizmos()
        {
            if (_rect != null)
            {
                Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(null, _rect.position);
                Ray ray = Camera.main.ScreenPointToRay(screenPoint);

                // 레이캐스트 경로 그리기
                Gizmos.color = Color.red; // 레이의 색상
                Gizmos.DrawLine(ray.origin, ray.direction * 100); // 100은 임의의 거리
            }
        }
    }
}