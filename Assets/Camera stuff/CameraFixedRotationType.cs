using UnityEngine;
public class CameraFixedRotationType : CameraType {
    public float speed = 3;
    float progress = 0;
    Quaternion startA;
    Quaternion startXA;
    Quaternion startYA;
    public Transform directionsTransform;
    public override void CameraStart() {
        progress = 0;
        cam.rotate = false;
        startA = cam.yetAnotherAxis.rotation;
        startXA = cam.xAxis.localRotation;
        startYA = cam.yAxis.localRotation;
        cam.updateDirections = false;
    }
    public override void CameraUpdate() {
        cam.forward = directionsTransform.forward;
        cam.right = directionsTransform.right;
        cam.yetAnotherAxis.rotation = Quaternion.Lerp(startA, reference.rotation, progress);
        cam.xAxis.localRotation = Quaternion.Lerp(startXA, Quaternion.identity, progress);
        cam.yAxis.localRotation = Quaternion.Lerp(startYA, Quaternion.identity, progress);
        if (progress < 1) progress += deltaTime * speed;
    }
    public override void CameraEnd() {
        cam.rotate = cam.shouldRotate;
        cam.xAxis.localRotation = startXA;
        cam.yAxis.localRotation = startYA;
        cam.updateDirections = true;
    }
    public override void CameraPostEnd() {}
}