using UnityEngine;
public class CameraControlType : CameraType {
    public bool controlEnabled = false;
    public override void CameraEnd() {
        cam.controllable = cam.shouldBeControllable;
    }
    public override void CameraStart() {
        cam.controllable = controlEnabled;
    }
}