using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] private GameObject XROrigin;
    [SerializeField] private Vector3 DefaultPosition,TunnelEnd;
    [SerializeField] private ParticleSystem WarpDrive;
    bool isMoving = false;
    public void Journey360()
    {
        if (!isMoving)
        {
            StartCoroutine(ChangeScene("360Journey"));
        }
    }
    public void PanoramicView()
    {
        if (!isMoving)
        {
            StartCoroutine(ChangeScene("PanoramicView"));
        }
    }
    public void ClassicStory()
    {
        if (!isMoving)
        {
            StartCoroutine(ChangeScene("ClassicStory"));
        }
    }
    IEnumerator ChangeScene(string sceneToLoad)
    { 
        isMoving = true;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
        asyncLoad.allowSceneActivation = false;
        WarpDrive.Play();
        
        while (XROrigin.transform.position != TunnelEnd)
        {
            XROrigin.transform.position = Vector3.MoveTowards(XROrigin.transform.position, TunnelEnd, Speed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        asyncLoad.allowSceneActivation = true;
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        transform.position = DefaultPosition;
        WarpDrive.Stop();
        isMoving = false;
    }
    public void Quit()
    {
        Application.Quit();
    }

}
