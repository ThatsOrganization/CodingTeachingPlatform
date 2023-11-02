using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class LoadSceneTest : MonoBehaviour
{
    [SerializeField]
    GameObject startBlockPrefab;

    [SerializeField]
    GameObject finishBlockPrefab;

    [SerializeField]
    GameObject storageBlockPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadSceneInEditMode()
    {
        var level = new LevelInfo();
        WorkspaceSceneManager.LoadWorkspaceScene(level, true);
    }

    public void LoadSceneInPlayMode()
    {
        var serializer = new BinaryFormatter();
        var stream = new FileStream("test", FileMode.Open, FileAccess.Read);
        var level = serializer.Deserialize(stream) as LevelInfo;
        stream.Close();
        WorkspaceSceneManager.LoadWorkspaceScene(level, false);
    }
}
