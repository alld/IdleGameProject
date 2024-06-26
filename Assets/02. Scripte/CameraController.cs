using IdleGame.Core.Procedure;
using IdleGame.Data.Common.Log;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CameraController : MonoBehaviour
{
    private Transform _targetTransform;
    private float _cameraDistance = -10f;    // z축만 조절 

    [SerializeField] private SpriteRenderer _mapSprite;
    private Vector2 _mapMin;
    private Vector2 _mapMax;

    private Vector3 velocity = Vector3.zero;
    private float smoothTime = 0.2f;

    private void Start()
    {
        _targetTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        transform.SetParent(_targetTransform);

        #region 맵의 크기 구하기
        _mapSprite = _mapSprite.GetComponent<SpriteRenderer>();
        if(_mapSprite != null)
        {
            Bounds bounds = _mapSprite.bounds;
            _mapMin = new Vector2(bounds.min.x, bounds.min.y);
            _mapMax = new Vector2(bounds.max.x / 1.5f, bounds.max.y);
        }
        else
        {
            Base_Engine.Log.Logic_PutLog(new Data_Log("맵이 존재하지 않습니다"));
        }
        #endregion
    }

    private void LateUpdate()
    {
        TargetFollow();
    }

    private void TargetFollow()
    {
        if (_targetTransform == null)
            return;

        Vector3 targetPosition = _targetTransform.position;
        targetPosition.z = _cameraDistance;

        float clampedX = Mathf.Clamp(targetPosition.x, _mapMin.x, _mapMax.x);
        float clampedY = Mathf.Clamp(targetPosition.y, _mapMin.y, _mapMax.y);
        targetPosition = new Vector3(clampedX, clampedY, targetPosition.z);

        //Vector3 smoothPosition = Vector3.Lerp(transform.position, targetPosition, 0.1f);
        //transform.position = smoothPosition;
    
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
