using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace lus.framework
{
    internal class ABFileMD5
    {
        public string md5Info;
        public long fileSize;
        public string fileDownloadType;

        public ABFileMD5(string md5, long size, string fileDownType)
        {
            md5Info = md5;
            fileSize = size;
            fileDownloadType = fileDownType;
        }
    }
    public class AssetBundleBase : AssetBundleConfig
    {
        private static string assetsPath { get { return Application.dataPath + "/"; } }
        private static string myStreamingAssetPath { get { return StreamingAssetsPath + resourceName + "/"; } }
        private static string buildTime = "";
        private static List<string> paths = new List<string>();
        private static List<string> files = new List<string>();
        private static Dictionary<string, bool> folderFile2AB = new Dictionary<string, bool>();
        private static Dictionary<string, string> folder2AB = new Dictionary<string, string>();
        private static Dictionary<string, ABFileMD5> ABFileMD5Dic = new Dictionary<string, ABFileMD5>();

        /// <summary>
        /// 清除AssetName名称
        /// </summary>
        protected static void DoClearAllAssetBundlesName()
        {
            //显示进程加载条
            EditorUtility.DisplayProgressBar("清除AssetName名称", "正在清除AssetName名称中...", 0f);

            AssetDatabase.StartAssetEditing();

            foreach (var assetPath in AssetDatabase.GetAllAssetPaths())
            {
                var assetImporter = AssetImporter.GetAtPath(assetPath);

                if (string.IsNullOrWhiteSpace(assetImporter.assetBundleName))
                    continue;

                assetImporter.SetAssetBundleNameAndVariant(null, null);
                assetImporter.SaveAndReimport();
            }

            AssetDatabase.StopAssetEditing();

            int total = AssetDatabase.GetAllAssetBundleNames().Length;
            int cur = 0;
            foreach (var n in AssetDatabase.GetAllAssetBundleNames())
            {
                cur++;
                AssetDatabase.RemoveAssetBundleName(n, true);
                //显示进程加载条
                EditorUtility.DisplayProgressBar("清除AssetName名称", "正在清除AssetName名称中...", 1f * cur / total);
            }
            AssetDatabase.SaveAssets();

            //清除进度条
            EditorUtility.ClearProgressBar();
        }

        /// <summary>
        /// 设置AssetName名称
        /// </summary>
        protected static void SetAssetBundleName(string path, string name, bool bDependences = false, bool bRecusion = true, bool bUseFolderName = false)
        {
            var absolutelypath = assetsPath + path + "/" + name;
            if (Directory.Exists(absolutelypath))
            {
                EditorUtility.DisplayProgressBar("设置AssetName名称", "正在设置AssetName名称中...", 0f);
                DirectoryInfo dir = new DirectoryInfo(absolutelypath);
                //递归调用
                FileInfo[] files = bRecusion ? dir.GetFiles("*", SearchOption.AllDirectories) : dir.GetFiles("*", SearchOption.TopDirectoryOnly);

                for (var i = 0; i < files.Length; ++i)
                {
                    FileInfo fileInfo = files[i];
                    EditorUtility.DisplayProgressBar("设置AssetName名称", "正在设置AssetName名称中...", 1f * i / files.Length);

                    //判断去除掉扩展名为“.meta .json .xls的资源
                    if (!fileInfo.Name.EndsWith(".meta") &&
                        !fileInfo.Name.EndsWith(".json") &&
                        !fileInfo.Name.EndsWith(".xls"))
                    {
                        string basePath = "Assets" + fileInfo.FullName.Substring(Application.dataPath.Length);
                        basePath = basePath.Replace('\\', '/');

                        //预设的Assetbundle名字，带上一级目录名称
                        string assetName = fileInfo.FullName.Substring(assetsPath.Length);
                        if (assetName.LastIndexOf('.') != -1)
                            assetName = assetName.Substring(0, assetName.LastIndexOf('.'));

                        //注意此处的斜线一定要改成反斜线，否则不能设置名称
                        assetName = assetName.Replace('\\', '/');

                        //使用文件夹名替代资源
                        if (bUseFolderName)
                            assetName = assetName.Substring(0, assetName.LastIndexOf('/'));

                        //设置预设的AssetBundleName名称
                        AssetImporter importer = AssetImporter.GetAtPath(basePath);
                        if (importer && string.IsNullOrEmpty(importer.assetBundleName))
                        {
                            importer.assetBundleName = assetName;
                            //importer.assetBundleVariant = name;
                        }

                        if (bDependences)
                        {
                            bool isFolderFile2AB = false;
                            foreach (KeyValuePair<string, bool> kvp1 in folderFile2AB)
                            {
                                if (basePath.Contains(kvp1.Key))
                                {
                                    isFolderFile2AB = true;
                                    break;
                                }
                            }

                            SetAssetBundleName(assetName, basePath, isFolderFile2AB);
                        }
                    }
                }
                EditorUtility.ClearProgressBar();   //清除进度条
            }
        }

        /// <summary>
        /// 删除所有manifest文件
        /// </summary>
        /// <param name="dirPath"></param>
        protected static void DeleteAllManifest(string dirPath)
        {
            DirectoryInfo dir = new DirectoryInfo(dirPath);
            FileInfo[] files = dir.GetFiles("*.manifest", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].FullName.Contains("AssetsResources.manifest"))
                    continue;
                if (File.Exists(files[i].FullName + ".meta"))
                    File.Delete(files[i].FullName + ".meta");
                if (File.Exists(files[i].FullName))
                    File.Delete(files[i].FullName);
            }
        }

        /// <summary>
        /// 资源文件列表
        /// </summary>
        protected static void BuildFileIndex(string path)
        {
            EditorUtility.DisplayProgressBar("资源文件列表", "正在创建资源文件列表...", 0f);
            buildTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            buildTime = buildTime.Replace("-", "");
            buildTime = buildTime.Replace(" ", "");
            buildTime = buildTime.Replace(":", "");
            buildTime = buildTime.Replace("/", "");
            string tempPath = path.Replace('\\', '/');
            string newFilePath = path + "/" + filesName + "." + buildTime;

            if (File.Exists(newFilePath))
                File.Delete(newFilePath);

            paths.Clear();
            files.Clear();
            Recursive(path + "/" + folderDirMame, ref files, ref paths);

            ABFileMD5Dic.Clear();

            FileStream fs = new FileStream(newFilePath, FileMode.CreateNew);
            var encoding = new UTF8Encoding(false);
            StreamWriter sw = new StreamWriter(fs, encoding);

            List<string> infoList = new List<string>();
            string headInfo = fileStyleVersion.ToString() + "#" + buildTime + "#";
            infoList.Add(headInfo);
            int fileCount = 0;
            for (int i = 0; i < files.Count; i++)
            {
                string file = files[i];
                string ext = Path.GetExtension(file);
                string value = file.Replace(tempPath, string.Empty);
                if (file.EndsWith(".meta") ||
                    file.Contains(".DS_Store") ||
                    file.Contains("filesSize.csv") ||
                    value.StartsWith("lua/"))
                    continue;

                string md5 = Md5file(file, out long fileSize);
                string updateType = "def";
                ABFileMD5Dic.Add(value, new ABFileMD5(md5, fileSize, updateType));
                string data = value + "|" + md5 + "|" + fileSize + "|" + updateType;
                infoList.Add(data);
                fileCount++;
                EditorUtility.DisplayProgressBar("资源文件列表", "正在创建资源文件列表...", 1f * i / files.Count);   //显示进程加载条
            }
            headInfo += fileCount.ToString();
            infoList[0] = headInfo;
            for (int i = 0; i < infoList.Count; i++)
            {
                sw.WriteLine(infoList[i]);
            }

            sw.Close();
            fs.Close();
            EditorUtility.ClearProgressBar();
        }

        /// <summary>
        /// 拷贝打包到App里的资源到StreamingAssets里
        /// </summary>
        protected static void CopyWorkingToStreamingAssets(string streamingAssetPath)
        {
            //清空
            if (Directory.Exists(myStreamingAssetPath))
            {
                Directory.Delete(myStreamingAssetPath, true);
            }
            Directory.CreateDirectory(myStreamingAssetPath);

            //拷贝资源
            foreach (KeyValuePair<string, ABFileMD5> kvp in ABFileMD5Dic)
            {
                string outfile = myStreamingAssetPath + kvp.Value.md5Info;
                string dir = Path.GetDirectoryName(outfile);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                File.Copy(streamingAssetPath + kvp.Key, outfile, true);
            }

            //拷贝files.txt
            File.Copy(streamingAssetPath + "/" + filesName + "." + buildTime, myStreamingAssetPath + "/" + filesName, true);
        }

        #region  PRIVATE
        private static void SetAssetBundleName(string assetName, string assetPath, bool isFolderFile2AB)
        {
            string[] dps = new string[] { };
            try
            {
                dps = AssetDatabase.GetDependencies(assetPath);
            }
            catch
            {
                Debug.LogError("ABDependencies ERR===" + assetPath);
            }

            //对依赖资源排序 需要递归的排到前面
            Array.Sort(dps, delegate (string s1, string s2)
            {
                int i1 = 0;
                int i2 = 0;
                for (int i = 0; i < assetSingleAB.Length; i++)
                {
                    if (s1.Contains(assetSingleAB[i]))
                        i1++;

                    if (s2.Contains(assetSingleAB[i]))
                        i2++;
                }

                return i2 - i1;
            });

            for (int j = 0; j < dps.Length; j++)
            {
                //要过滤掉依赖的自己本身、脚本文件、文件夹，自己本身的名称已设置，而脚本不能打包
                if (dps[j].Contains(assetName + ".") ||
                    dps[j].Contains(".cs") ||
                    dps[j].Contains(".xls") ||
                    !dps[j].Contains("."))
                    continue;

                for (int i = 0; i < assetSingleAB.Length; i++)
                {
                    if (dps[j].Contains(assetSingleAB[i]))
                    {
                        string aName = dps[j];

                        if (!isFolderFile2AB)
                        {
                            aName = aName.Substring("Assets/".Length);
                            aName = aName.Substring(0, aName.LastIndexOf('.'));
                        }
                        else
                        {
                            aName = assetName;
                        }

                        aName = ResetABName(dps[j], aName);

                        AssetImporter importer = AssetImporter.GetAtPath(dps[j]);
                        if (importer && string.IsNullOrEmpty(importer.assetBundleName))                         //已经设置过bundle名称略过
                        {
                            importer.assetBundleName = aName;
                            SetAssetBundleName(aName, dps[j], isFolderFile2AB);
                        }

                        break;
                    }
                }

                AssetImporter importer1 = AssetImporter.GetAtPath(dps[j]);
                if (importer1 && string.IsNullOrEmpty(importer1.assetBundleName))                             //已经设置过bundle名称略过
                {
                    string aName = assetName;
                    aName = ResetABName(dps[j], aName);
                    importer1.assetBundleName = aName;
                }
            }
        }

        private static string ResetABName(string path, string aName)
        {
            bool isCheckFolder2AB = true;

            //Art 下 UI Effect 打入一个包
            if (path.Contains("Art/Effect/UI"))
            {
                aName = "Art/EffectUi";
                isCheckFolder2AB = false;
            }

            //所有特效噪声贴图打入一个包
            if (path.Contains("Art/Effect/Textures/Noise"))
            {
                aName = "Art/EffectNoise";
                isCheckFolder2AB = false;
            }

            if (isCheckFolder2AB)
            {
                foreach (KeyValuePair<string, string> kvp in folder2AB)
                {
                    if (path.Contains(kvp.Key))
                    {
                        aName = kvp.Value;
                        break;
                    }
                }
            }

            return aName;
        }

        public static string StreamingAssetsPath
        {
            get
            {
#if UNITY_EDITOR || UNITY_STANDALONE
                return Application.dataPath + "/StreamingAssets/";
#elif UNITY_ANDROID
            return Application.dataPath + "!/assets/";
#elif UNITY_IOS || UNITY_IPHONE
            return Application.dataPath + "/Raw/";
#else
                return Application.dataPath + "/StreamingAssets/";
#endif
            }
        }

        public static void Recursive(string path, ref List<string> files, ref List<string> paths)
        {
            string[] names = Directory.GetFiles(path);
            string[] dirs = Directory.GetDirectories(path);
            foreach (string filename in names)
            {
                string ext = Path.GetExtension(filename);
                if (ext.Equals(".meta")) continue;
                files.Add(filename.Replace('\\', '/'));
            }
            foreach (string dir in dirs)
            {
                paths.Add(dir.Replace('\\', '/'));
                Recursive(dir, ref files, ref paths);
            }
        }

        public static string Md5file(string file, out long fileSize)
        {
            try
            {
                FileStream fs = new FileStream(file, FileMode.Open);
                fileSize = fs.Length;
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(fs);
                fs.Close();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }

                return sb.ToString();

            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5File Fail Error:" + ex.Message);
            }
        }
        #endregion
    }
}