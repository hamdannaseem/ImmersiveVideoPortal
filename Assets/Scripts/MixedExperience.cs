using UnityEngine;
using UnityEngine.Video;

public class MixedExperience : MonoBehaviour
{
    [System.Serializable]
    public class FrameRange
    {
        public long startFrame;
        public long endFrame;
        public bool is180Mode;
    }
    
    [Header("References")]
    public VideoPlayer videoPlayer;
    public Transform cameraTransform;
    
    [Header("Frame Ranges")]
    public FrameRange[] frameRanges;
    
    [Header("180° Constraints")]
    public float minYRotation = -90f;
    public float maxYRotation = 90f;
    public float minXRotation = -45f;
    public float maxXRotation = 45f;
    
    private bool is180ModeActive = false;
    private Vector3 lastValidRotation;
    
    void Start()
    {
        lastValidRotation = transform.eulerAngles;
    }
    
    void LateUpdate()
    {
        if (videoPlayer == null) return;
        
        long currentFrame = videoPlayer.frame;
        is180ModeActive = ShouldUse180Mode(currentFrame);
        
        if (is180ModeActive)
        {
            ConstrainCameraRotation();
        }
        else
        {
            lastValidRotation = GetCombinedRotation();
        }
    }
    
    bool ShouldUse180Mode(long currentFrame)
    {
        foreach (var range in frameRanges)
        {
            if (currentFrame >= range.startFrame && currentFrame <= range.endFrame)
            {
                return range.is180Mode;
            }
        }
        return false;
    }
    
    void ConstrainCameraRotation()
    {
        Vector3 combinedRotation = GetCombinedRotation();
        float yAngle = NormalizeAngle(combinedRotation.y);
        float xAngle = NormalizeAngle(combinedRotation.x);
        bool yExceedsLimit = yAngle < minYRotation || yAngle > maxYRotation;
        bool xExceedsLimit = xAngle < minXRotation || xAngle > maxXRotation;
        
        if (yExceedsLimit || xExceedsLimit)
        {
            float constrainedY = Mathf.Clamp(yAngle, minYRotation, maxYRotation);
            float constrainedX = Mathf.Clamp(xAngle, minXRotation, maxXRotation);
            Vector3 targetParentRotation = new Vector3(
                constrainedX - NormalizeAngle(cameraTransform.localEulerAngles.x),
                constrainedY - NormalizeAngle(cameraTransform.localEulerAngles.y),
                0
            );
            
            transform.eulerAngles = targetParentRotation;
        }
    }
    
    Vector3 GetCombinedRotation()
    {
        Quaternion combinedRotation = transform.rotation * cameraTransform.localRotation;
        return combinedRotation.eulerAngles;
    }
    
    float NormalizeAngle(float angle)
    {
        while (angle > 180f) angle -= 360f;
        while (angle < -180f) angle += 360f;
        return angle;
    }
}
