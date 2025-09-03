using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    InputAction Exit;
    void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        Exit =new InputAction("Exit", binding: "<Keyboard>/escape");
    }
    void Start()
    {
        videoPlayer.Play();
        videoPlayer.loopPointReached += OnVideoFinished;
    }
    void OnEnable()
    {
        Exit.Enable();
    }

    void OnDisable()
    {
        Exit.Disable();
    }
    void Update()
    {
        if (Exit.WasPressedThisFrame())
        {
            OnVideoFinished(videoPlayer);
        }
    }
    void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene("Lobby");
    }
}
