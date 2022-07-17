using UnityEditor;

public class WavePlanner : EditorWindow
{
    [MenuItem("Window/Wave Planner")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(WavePlanner));
    }
}