using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Threading.Tasks;

namespace lus.framework
{
    public class AssetBundlesMenu : AssetBundleBase
    {
        [MenuItem("MyTools/AssetBundle/Clear All AssetBundle Names")]
        public static void ClearAllAssetBundlesName()
        {
            AssetBundleBase.DoClearAllAssetBundlesName();
            AssetDatabase.Refresh();
        }

        [MenuItem("MyTools/AssetBundle/Set AssetBundle Names")]
        public static void SetAssetBundleNames()
        {
            AssetBundleBase.SetAssetBundleName(folderDirMame, "Atlas", false, true, true);
            AssetBundleBase.SetAssetBundleName(folderDirMame, "Textures", false);
            AssetBundleBase.SetAssetBundleName(folderDirMame, "Prefabs", true);
            AssetDatabase.Refresh();
        }

        [MenuItem("MyTools/AssetBundle/Get AssetBundle Names")]
        public static void GetAssetBundleNames()
        {
            var names = AssetDatabase.GetAllAssetBundleNames();
            Debug.Log("<color=#009400>" + "All AssetBundle Number: " + "</color>" + string.Format("<color=#0000D3>" + "{0}" + "</color>", names.Length));
            foreach (var name in names)
                Debug.Log($"AssetBundle: {name}");
        }

        [MenuItem("MyTools/AssetBundle/Delete AssetBundle")]
        public static void DeleteTempleAssetBundles()
        {
            if (Directory.Exists("Assets/StreamingAssets/AssetBundles.meta"))
            {
                Directory.Delete("Assets/StreamingAssets/AssetBundles.meta");
            }
            if (Directory.Exists("Assets/StreamingAssets/AssetBundles"))
            {
                Directory.Delete("Assets/StreamingAssets/AssetBundles", true);
            }
            System.Func<Task> CallDelay = async () =>
            {
                await Task.Delay(System.TimeSpan.FromSeconds(2));
                AssetDatabase.Refresh();

            };
            CallDelay();
        }

        [MenuItem("MyTools/AssetBundle/Build AssetBundles/StandaloneWindows64")]
        public static void BuildAssetBundlesStandaloneWindows64()
        {
            //1.设置打包名字
            SetAssetBundleNames();

            //2.打包资源
            string assetBundleDirectory = "Assets/StreamingAssets/AssetBundles/StandaloneWindows64";
            if (!Directory.Exists(assetBundleDirectory))
            {
                Directory.CreateDirectory(assetBundleDirectory);
            }
            BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
            DeleteAllManifest(assetBundleDirectory);
            BuildFileIndex(assetBundleDirectory);
            CopyWorkingToStreamingAssets(assetBundleDirectory);

            //3.清空打包名字
            ClearAllAssetBundlesName();

            //4.清空打包资源
            DeleteTempleAssetBundles();
        }

        [MenuItem("MyTools/AssetBundle/Build AssetBundles/StandaloneOSX")]
        public static void BuildAssetBundlesStandaloneOSX()
        {
            //1.设置打包名字
            SetAssetBundleNames();

            //2.打包资源
            string assetBundleDirectory = "Assets/StreamingAssets/AssetBundles/StandaloneOSX";
            if (!Directory.Exists(assetBundleDirectory))
            {
                Directory.CreateDirectory(assetBundleDirectory);
            }
            BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.StandaloneOSX);
            DeleteAllManifest(assetBundleDirectory);
            BuildFileIndex(assetBundleDirectory);
            CopyWorkingToStreamingAssets(assetBundleDirectory);

            //3.清空打包名字
            ClearAllAssetBundlesName();

            //4.清空打包资源
            DeleteTempleAssetBundles();
        }

        [MenuItem("MyTools/AssetBundle/Build AssetBundles/Android")]
        public static void BuildAssetBundlesAndroid()
        {
            //1.设置打包名字
            SetAssetBundleNames();

            //2.打包资源
            string assetBundleDirectory = "Assets/StreamingAssets/AssetBundles/Android";
            if (!Directory.Exists(assetBundleDirectory))
            {
                Directory.CreateDirectory(assetBundleDirectory);
            }
            BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.Android);
            DeleteAllManifest(assetBundleDirectory);
            BuildFileIndex(assetBundleDirectory);
            CopyWorkingToStreamingAssets(assetBundleDirectory);

            //3.清空打包名字
            ClearAllAssetBundlesName();

            //4.清空打包资源
            DeleteTempleAssetBundles();
        }

        [MenuItem("MyTools/AssetBundle/Build AssetBundles/iOS")]
        public static void BuildAssetBundlesiOS()
        {
            //1.设置打包名字
            SetAssetBundleNames();

            //2.打包资源
            string assetBundleDirectory = "Assets/StreamingAssets/AssetBundles/iOS";
            if (!Directory.Exists(assetBundleDirectory))
            {
                Directory.CreateDirectory(assetBundleDirectory);
            }
            BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.iOS);
            DeleteAllManifest(assetBundleDirectory);
            BuildFileIndex(assetBundleDirectory);
            CopyWorkingToStreamingAssets(assetBundleDirectory);

            //3.清空打包名字
            ClearAllAssetBundlesName();

            //4.清空打包资源
            DeleteTempleAssetBundles();
        }
    }
}