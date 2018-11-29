using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class StreamingAssetManager : MonoBehaviour {
    /// <summary>
    /// Singleton of the script
    /// </summary>
    public static StreamingAssetManager instance;
    private Image i;
    private AudioSource a;
    /// <summary>
    /// list of pending 
    /// to load
    /// </summary>
    private List<Image> _DestinationImage;
    /// <summary>
    /// list of pending sprites to load
    /// </summary>
    private List<SpriteRenderer> _DestinationSprite;
    /// <summary>
    /// list of pending audiosource to load
    /// </summary>
    private AudioSource _DestiantionAudio;
    

    void Awake()
    {
        instance = this;
        _DestinationImage = new List<Image>();
        _DestinationSprite = new List<SpriteRenderer>();
    }

    /// <summary>
    /// load an audioclip for the streaming asset
    /// </summary>
    /// <param name="folder">the relative path f the folderfrm the streaming asset forler where the file is</param>
    /// <param name="filename">the name of the file (with extension)</param>
    /// <param name="destination">Audiosource where to load the audio file</param>
    public void loadAudioClipFromStreamingAsset(string folder, string filename, AudioSource destination) {
        _DestiantionAudio = destination;
        Debug.Log(filename);
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.streamingAssetsPath + "/" + folder);
        FileInfo[] allFiles = directoryInfo.GetFiles("*.*");
        foreach (FileInfo file in allFiles)
        {
            if (file.Name == filename)
            {
                StartCoroutine("LoadAudioFromAsset", file);
            }
        }
    }
    /// <summary>
    /// load an image from the stremaing asset into the an Image
    /// </summary>
    /// <param name="folder">the relative path f the folderfrm the streaming asset forler where the file is</param>
    /// <param name="filename">the name of the file (with extension)</param>
    /// <param name="destination">Audiosource where to load the image</param>
    public void loadImageFromStreamingAsset(string folder, string filename, Image destination)
    {
        _DestinationImage.Add(destination);
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.streamingAssetsPath + "/" + folder);
        FileInfo[] allFiles = directoryInfo.GetFiles("*.*");
        foreach (FileInfo file in allFiles)
        {
            if (file.Name == filename)
            {
                StartCoroutine("LoadImageFromAsset", file);
            }
        }
    }
    /// <summary>
    /// load an image from the stremaing asset into the an SpriteRenderer
    /// </summary>
    /// <param name="folder">the relative path f the folderfrm the streaming asset forler where the file is</param>
    /// <param name="filename">the name of the file (with extension)</param>
    /// <param name="destination">Audiosource where to load the image</param>
    public void loadSpriteRendererFromStreamingAsset(string folder, string filename, SpriteRenderer destination)
    {
        _DestinationSprite.Add(destination);
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.streamingAssetsPath + "/" + folder);
        FileInfo[] allFiles = directoryInfo.GetFiles("*.*");
        foreach (FileInfo file in allFiles)
        {
            if (file.Name == filename)
            {
                StartCoroutine("LoadSpriteFromAsset", file);
            }
        }
    }
    /// <summary>
    /// get all the files present in the folder, excluded the .meta file
    /// </summary>
    /// <param name="folder">folder where to search</param>
    /// <returns></returns>
    public string[] getFilenamesFromFolderInStreamingAsset(string folder) {
        List<string> names = new List<string>();
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.streamingAssetsPath + "/" + folder);
        FileInfo[] allFiles = directoryInfo.GetFiles("*.*");
        foreach (FileInfo file in allFiles)
        {
            if (!file.Name.Contains("meta"))
            {
                names.Add(file.Name);
            }
        }
        return names.ToArray();
    }

    IEnumerator LoadImageFromAsset(FileInfo playerFile)
    {
        if (playerFile.Name.Contains("meta"))
        {

            yield return "";
        }
        else
        {
            print(playerFile.FullName.ToString());
            string wwwPlayerFilePath = "file://" + playerFile.FullName.ToString();
            WWW www = new WWW(wwwPlayerFilePath);
            yield return www;
            _DestinationImage.ElementAt(0).sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));
            _DestinationImage.ElementAt(0).sprite.name = playerFile.Name;
            _DestinationImage.RemoveAt(0);
        }
    }
    IEnumerator LoadSpriteFromAsset(FileInfo playerFile)
    {
        if (playerFile.Name.Contains("meta"))
        {

            yield return "";
        }
        else
        {
            print(playerFile.FullName.ToString());
            string wwwPlayerFilePath = "file://" + playerFile.FullName.ToString();
            WWW www = new WWW(wwwPlayerFilePath);
            yield return www;
            _DestinationSprite.ElementAt(0).sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));
            _DestinationImage.ElementAt(0).sprite.name = playerFile.Name;
            _DestinationSprite.RemoveAt(0);
        }
    }
    IEnumerator LoadAudioFromAsset(FileInfo playerFile)
    {
        if (playerFile.Name.Contains("meta"))
        {

            yield return "";
        }
        else
        {
            
                print(playerFile.FullName.ToString());
                string wwwPlayerFilePath = "file://" + playerFile.FullName.ToString();
                WWW www = new WWW(wwwPlayerFilePath);
                yield return www;


            if (playerFile.Name.Contains(".wav"))
            {
                _DestiantionAudio.clip = www.GetAudioClip(true, true);
            }
            if (playerFile.Name.Contains(".mp3"))
            {

                _DestiantionAudio.clip = NAudioPlayer.FromMp3Data(www.bytes);//www.GetAudioClip();
                
            }
            _DestiantionAudio.clip.name = playerFile.Name;
            _DestiantionAudio.Play();
        }
    }
}
