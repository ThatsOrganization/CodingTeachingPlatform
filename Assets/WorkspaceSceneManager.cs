using UnityEngine.SceneManagement;

public class WorkspaceSceneManager
{
    static bool _isEditMode = true;

    static LevelInfo _currentLevelInfo;

    public static bool IsEditMode => _isEditMode;

    public static LevelInfo CurrentLevelInfo => _currentLevelInfo;
    
    public static void LoadWorkspaceScene(LevelInfo level, bool editMode)
    {
        _isEditMode = editMode;
        //foreach (var block in level.Blocks)
        //    UnityEngine.Object.DontDestroyOnLoad(block);
        //foreach (var block in level.AvailableBlocks.Keys)
        //    UnityEngine.Object.DontDestroyOnLoad(block);
        _currentLevelInfo = level;
        SceneManager.LoadScene("Workspace");
    }

    public static void LoadMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
