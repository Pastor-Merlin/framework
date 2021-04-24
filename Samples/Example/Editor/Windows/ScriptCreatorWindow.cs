using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class ScriptCreatorWindow : EditorWindow
{
    [MenuItem("MyTools/Framework/ScriptCreatorWindow", false, 1)]
    public static void ShowWindow()
    {
        //打开窗口
        var window = EditorWindow.GetWindow(typeof(ScriptCreatorWindow));
        var screenResolution = Screen.currentResolution;
        var w = 500;
        var h = 300;
        var r = new Rect(screenResolution.width * 0.5f - w * 0.5f, screenResolution.height * 0.5f - h * 0.5f, w, h);
        window.position = r;
    }

    private List<string> nameList = new List<string>();
    private string scriptCreatorClassFlag = "_lusClassName";
    private string scriptCreatorName = "";
    private string scriptTemplateExtName = "bytes";
    private string scriptOutputPath = "Assets";
    private string fullPath = "Assets/Editor Default Resources/DC/";

    private void Awake()
    {
        if (Directory.Exists(fullPath))
        {
            nameList.Clear();
            DirectoryInfo direction = new DirectoryInfo(fullPath);
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".meta"))
                {
                    continue;
                }
                var strList = files[i].Name.Split('.');
                if (strList.Length > 1)
                {
                    var name = strList[0];
                    var ext = strList[1];
                    if (ext == scriptTemplateExtName)
                    {
                        nameList.Add(name);
                    }
                }
            }
        }
    }

    private void OnSelectionChange()
    {
        Repaint();
    }

    private void OnGUI()
    {
    
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);

        //模版名字
        scriptCreatorName = EditorGUILayout.TextField("Name", scriptCreatorName);
        if (string.IsNullOrEmpty(scriptCreatorName))
        {
            scriptCreatorName = "Template";
        }

        EditorGUI.BeginDisabledGroup(true);
        {
            //模板默认目录
            EditorGUILayout.TextField("TemplatePath", fullPath);

            //模板输出目录
            scriptOutputPath = EditorGUILayout.TextField("OutPut", scriptOutputPath);
            scriptOutputPath = GetSelectedPathOrFallback();
        }
        EditorGUI.EndDisabledGroup();

        //ButtonGroup
        if (nameList.Count > 0)
        {
            if (GUILayout.Button("All"))
            {
                for (int i = 0; i < nameList.Count; i++)
                {
                    var go = nameList[i];
                    EditorUtility.DisplayProgressBar("ScriptCreator", "Compile", i * 1.0f / nameList.Count);
                    DoCreate(go);
                }
                EditorUtility.ClearProgressBar();

                foreach (var go in nameList)
                    DoCreate(go);

                EditorUtility.ClearProgressBar();
            }

            foreach (var go in nameList)
            {
                if (GUILayout.Button(go))
                {
                    EditorUtility.DisplayProgressBar("ScriptCreator", "Compile", 0);
                    DoCreate(go);
                    EditorUtility.ClearProgressBar();
                }
            }

        }
    }

    private void DoCreate(string name)
    {
        var templateSource = EditorGUIUtility.Load(string.Format("DC/{0}." + scriptTemplateExtName, name)) as TextAsset;
        if (templateSource == null)
        {
            Debug.Log("no template file");
            return;
        }

        string codeSrc = templateSource.text;
        if (string.IsNullOrEmpty(codeSrc))
        {
            Debug.Log("no template content");
            return;
        }

        //替换类名
        codeSrc = templateSource.text.Replace(scriptCreatorClassFlag, scriptCreatorName);

        //导出目录
        var assetPath = string.Format("{0}/{1}{2}", scriptOutputPath, scriptCreatorName + name, ".cs");
        var filePath = Application.dataPath.Replace("Assets", assetPath);
        File.WriteAllText(filePath, codeSrc);
        AssetDatabase.ImportAsset(assetPath);
    }

    public static string GetSelectedPathOrFallback()
    {
        string path = "Assets";

        foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
        {
            path = AssetDatabase.GetAssetPath(obj);
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                path = Path.GetDirectoryName(path);
                break;
            }
        }
        return path;
    }
}
