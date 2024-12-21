using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(Animator))]
public sealed class CameraControlMarkerReceiver : MonoBehaviour, INotificationReceiver
{
    [SerializeField]
    public Transform headTarget;

    private CinemachineBrain _cinemachineBrain;
    private Quaternion  _prevRotation;
    private bool _isReturningRotation;
    private float _rotationLerpTime;
    CameraControlMarker _cameraControlMarker;

    private void Awake()
    {
        if (Camera.main != null)
        {
            _cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        }
    }

    public void OnNotify(Playable _origin, INotification _notification, object _context)
    {
        _cameraControlMarker = _notification as CameraControlMarker;
        if (_cameraControlMarker == null)
        {
            return;
        }

        if (_cameraControlMarker.isEnable)
        {
            StartLookAt();
        }
        else
        {
            StopLookAt();
        }
    }

    private void StartLookAt()
    {
        var activeCamera = _cinemachineBrain.ActiveVirtualCamera as CinemachineCamera;
        if (activeCamera == null)
        {
            return;
        }

        _prevRotation = activeCamera.transform.rotation;
        activeCamera.LookAt = headTarget;
        _isReturningRotation = false;
    }

    private void StopLookAt()
    {
        var activeCamera = _cinemachineBrain.ActiveVirtualCamera as CinemachineCamera;
        if (activeCamera == null)
        {
            return;
        }

        activeCamera.LookAt = null;
        // 補間用のフラグとタイマーをリセット
        _isReturningRotation = true;
        _rotationLerpTime = 0f;
    }

    private void Update()
    {
        if (!_isReturningRotation)
        {
            return;
        }

        var activeCamera = _cinemachineBrain.ActiveVirtualCamera as CinemachineCamera;
        if (activeCamera == null)
        {
            return;
        }

        // Lerp で回転を滑らかに戻す
        _rotationLerpTime += Time.deltaTime / _cameraControlMarker.returnDuration;
        activeCamera.transform.rotation = Quaternion.Slerp(activeCamera.transform.rotation, _prevRotation, _rotationLerpTime);

        // 補間が完了したら終了
        if (_rotationLerpTime >= 1.0f)
        {
            _isReturningRotation = false;
        }
    }
}