using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class YoutubeVideoPlayerForMagiKRoom : MonoBehaviour {

    public static YoutubeVideoPlayerForMagiKRoom instance;
    public GameObject[] canvases;
    public SimplePlayback playersimpleFront;
    HighQualityPlayback playerhighquality;
    public VideoPlayer player;
    public Camera[] cameras;
    string path;
	// Use this for initialization
	void Start () {
        instance = this;
        playersimpleFront = GameObject.FindObjectOfType<SimplePlayback>();
        playerhighquality = GameObject.FindObjectOfType<HighQualityPlayback>();
    }

    /// <summary>
    /// Play a video on the screen
    /// </summary>
    /// <param name="url">the url of the video on youtube or the file path</param>
    /// <param name="frontscreen">true if the video should be played on the front screen (element 0 in camras array), false to show it on the floor screen (element 1 in camras array)</param>
    public void PlayFromInput(string url, bool frontscreen)
    {
        if (frontscreen)
        {
            player.targetCamera = cameras[0];
        }
        else {
            player.targetCamera = cameras[1];
        }

        if (url != null && url != "")
        {
            path = url;
        }
        else
        {
            return;
        }
        //search for the low quality if not find search for highquality
        if (playersimpleFront != null)
        {
            player.targetCameraAlpha = 1;
            playersimpleFront.PlayYoutubeVideo(path);
            playersimpleFront.unityVideoPlayer.loopPointReached += OnVideoFinished;
        }
        else if (playerhighquality != null)
        {
            player.targetCameraAlpha = 1;
            playerhighquality.PlayYoutubeVideo(path);
            playerhighquality.unityVideoPlayer.loopPointReached += OnVideoFinished;
        }


        if (frontscreen)
        {
            canvases[0].SetActive(false) ;
        }
        else
        {
            canvases[1].SetActive(false);
        }
        
    }

    public bool deactvate = false;
    private void Update()
    {
        if (deactvate)
        {
            player.targetCameraAlpha = Mathf.Lerp(player.targetCameraAlpha, 0, Time.deltaTime * 2);
            if (player.targetCameraAlpha < 0.05f)
            {
                player.targetCameraAlpha = 0.01f;
                deactvate = false;
            }
        }
    }

    private void OnVideoFinished(VideoPlayer vPlayer)
    {
        if (playersimpleFront != null)
        {
            //player.targetCameraAlpha = 0;
            deactvate = true;
            playersimpleFront.unityVideoPlayer.loopPointReached -= OnVideoFinished;
        }
        else if (playerhighquality != null)
        {
            player.targetCameraAlpha = 0.01f;
            playerhighquality.unityVideoPlayer.loopPointReached -= OnVideoFinished;
        }
        foreach (GameObject g in canvases)
        {
            g.SetActive(true);
        }
    }
}
