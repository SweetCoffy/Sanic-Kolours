using UnityEngine;
public class CameraFixedPositionType : CameraType {
    public float speed = 3;
    float progress = 0;
    Vector3 startPos;
    public override void CameraStart() {
        progress = 0;
        cam.move = false;
        startPos = cam.yetAnotherAxis.position;
    }
    public override void CameraUpdate() {
        /**/
        cam.yetAnotherAxis.position = Vector3.Lerp(startPos, reference.position, progress);
        if (progress < 1) progress += deltaTime * speed;
    }
    public override void CameraEnd() {
        cam.move = cam.shouldMove;
    }
    public override void CameraPostEnd() {}
}