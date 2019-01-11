using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AssetBundleProvider : MonoBehaviour
{

    public static AssetBundleProvider Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<AssetBundleProvider>();

            return _instance;
        }
    }
    static AssetBundleProvider _instance;

    public static Action<List<FileInfo>> OnAssetBundlesFound;

    Dictionary<string, AssetBundle> _loadedBundles = new Dictionary<string, AssetBundle>();
    public List<FileInfo> AssetBundleFiles = new List<FileInfo>();

    internal void LoadSceneFromFile(FileInfo f)
    {
        string[] scenes;

        if (_loadedBundles.ContainsKey(f.Name))
            scenes = _loadedBundles[f.Name].GetAllScenePaths();

        else
        {
            var bundle = AssetBundle.LoadFromFile(f.FullName);
            scenes = bundle.GetAllScenePaths();

            _loadedBundles.Add(f.Name, bundle);
        }
        SceneManager.LoadScene(scenes[0]);
    }

    // Use this for initialization
    void Start()
    {
        var baseDir = Application.platform.Equals(RuntimePlatform.WindowsEditor) ?
             Application.streamingAssetsPath :
             Application.persistentDataPath;

        var dir = Path.Combine(baseDir
            , "VideoPaks");

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        Debug.Log("Asset bundle path: " + dir);

        var folder = Directory.GetDirectories(dir);

        foreach (var f in folder)
        {
            var dirInfo = new DirectoryInfo(f);

            var path = Path.Combine(f, VideopakManager.GetPlatformString(Application.platform));
            path = Path.Combine(path, dirInfo.Name);

            AssetBundleFiles.Add(new FileInfo(path));
        }

        OnAssetBundlesFound(AssetBundleFiles);
    }

}
