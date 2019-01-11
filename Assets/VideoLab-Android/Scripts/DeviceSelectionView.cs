using nateturley.midible;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DeviceSelectionView : MonoBehaviour
{
    [SerializeField]
    GameObject _listItemPrefab;
    AndroidMidiBleShim bleHelper;

    protected GameObject CreateCollectionItem(object ListItem, Transform parent)
    {
        var newDevice = ListItem as MidiBleDevice;
        var go = GameObject.Instantiate(_listItemPrefab, transform);

        go.name = newDevice.Name;
        go.GetComponentInChildren<Text>().text = string.Format("Name: {0} Addr: {1}", newDevice.Name, newDevice.Address);
        go.GetComponentInChildren<Button>().onClick.AddListener(() =>
        {
            FindObjectOfType<AndroidMidiBleShim>().OpenConnection(newDevice);
        });

        return go;
    }
    void Awake()
    {
        bleHelper = FindObjectOfType<AndroidMidiBleShim>();
        AndroidMidiBleShim.OnDeviceAdded += (MidiBleDevice newDevice) =>
        {
            var go = CreateCollectionItem(newDevice, transform);
        };
    }

    // Update is called once per frame
    void Update()
    {

    }
}
