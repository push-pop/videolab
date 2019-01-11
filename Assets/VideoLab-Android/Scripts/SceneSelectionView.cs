using nateturley.midible;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSelectionView : MonoBehaviour
{

    [SerializeField]
    GameObject _listItemPrefab;

    AndroidMidiBleShim bleHelper;

    protected GameObject CreateCollectionItem(int sceneIdx, Transform parent)
    {
        var path = SceneUtility.GetScenePathByBuildIndex(sceneIdx);
        var go = GameObject.Instantiate(_listItemPrefab, transform);

        go.name = System.IO.Path.GetFileNameWithoutExtension(path);
        go.GetComponentInChildren<Text>().text = go.name;

        go.GetComponentInChildren<Button>().onClick.AddListener(() =>
        {
            var idx = sceneIdx;
            SceneManager.LoadScene(idx);
            GetComponentInParent<MenuController>().GoToState(MenuState.Closed);
        });

        return go;
    }

    protected GameObject CreateCollectionItem(FileInfo file, Transform parent)
    {

        var go = GameObject.Instantiate(_listItemPrefab, transform);
        go.GetComponentInChildren<Text>().text = file.Name;
        go.name = file.Name;
        go.GetComponent<Scene>().File = file;

        go.GetComponentInChildren<Button>().onClick.AddListener(() =>
        {
            GameObject g = go;
            var f = g.GetComponent<Scene>().File;
            AssetBundleProvider.Instance.LoadSceneFromFile(f);


        });

        return go;
    }

    void Awake()
    {
        AssetBundleProvider.OnAssetBundlesFound += (assetBundles) =>
        {
            foreach (var fInfo in assetBundles)
            {
                var go = CreateCollectionItem(fInfo, transform);
            }
        };
    }

    // Update is called once per frame
    void Update()
    {

    }
}
