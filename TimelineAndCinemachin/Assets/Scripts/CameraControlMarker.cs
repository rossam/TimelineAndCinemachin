using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable, DisplayName("カメラ制御マーカーを追加する")]
public class CameraControlMarker : Marker, INotification
{
    public PropertyName id => new();

    [Header("LookAt を有効にするかどうか")]
    public bool isEnable = true;

    [Header("カメラの向きを戻すかどうか")]
    public bool isReturnRotation = true;

    [Header("カメラの向きを戻す時間")]
    public float returnDuration = 1.0f;
}