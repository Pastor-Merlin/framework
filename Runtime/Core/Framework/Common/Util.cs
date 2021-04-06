using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using LitJson;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Framework
{
    public class Util
    {
        private static List<string> luaPaths = new List<string>();

        public static int Int(object o)
        {
            return Convert.ToInt32(o);
        }

        public static float Float(object o)
        {
            return (float)Math.Round(Convert.ToSingle(o), 2);
        }

        public static long Long(object o)
        {
            return Convert.ToInt64(o);
        }

        public static int Random(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        public static float Random(float min, float max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        public static string Uid(string uid)
        {
            int position = uid.LastIndexOf('_');
            return uid.Remove(0, position + 1);
        }

        public static long GetTime()
        {
            TimeSpan ts = new TimeSpan(DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
            return (long)ts.TotalMilliseconds;
        }

        /// <summary>
        /// 搜索子物体组件-GameObject版
        /// </summary>
        public static T Get<T>(GameObject go, string subnode) where T : Component
        {
            if (go != null)
            {
                Transform sub = go.transform.Find(subnode);
                if (sub != null) return sub.GetComponent<T>();
            }
            return null;
        }

        /// <summary>
        /// 搜索子物体组件-Transform版
        /// </summary>
        public static T Get<T>(Transform go, string subnode) where T : Component
        {
            if (go != null)
            {
                Transform sub = go.Find(subnode);
                if (sub != null) return sub.GetComponent<T>();
            }
            return null;
        }

        /// <summary>
        /// 搜索子物体组件-Component版
        /// </summary>
        public static T Get<T>(Component go, string subnode) where T : Component
        {
            return go.transform.Find(subnode).GetComponent<T>();
        }

        /// <summary>
        /// 添加组件
        /// </summary>
        public static T Add<T>(GameObject go) where T : Component
        {
            if (go != null)
            {
                T[] ts = go.GetComponents<T>();
                for (int i = 0; i < ts.Length; i++)
                {
                    if (ts[i] != null) GameObject.Destroy(ts[i]);
                }
                return go.gameObject.AddComponent<T>();
            }
            return null;
        }

        /// <summary>
        /// 添加组件
        /// </summary>
        public static T Add<T>(Transform go) where T : Component
        {
            return Add<T>(go.gameObject);
        }

        /// <summary>
        /// 查找子对象
        /// </summary>
        public static GameObject Child(GameObject go, string subnode)
        {
            return Child(go.transform, subnode);
        }

        /// <summary>
        /// 查找子对象
        /// </summary>
        public static GameObject Child(Transform go, string subnode)
        {
            Transform tran = go.Find(subnode);
            if (tran == null) return null;
            return tran.gameObject;
        }

        /// <summary>
        /// 取平级对象
        /// </summary>
        public static GameObject Peer(GameObject go, string subnode)
        {
            return Peer(go.transform, subnode);
        }

        /// <summary>
        /// 取平级对象
        /// </summary>
        public static GameObject Peer(Transform go, string subnode)
        {
            Transform tran = go.parent.Find(subnode);
            if (tran == null) return null;
            return tran.gameObject;
        }

        /// <summary>
        /// 计算字符串在指定text控件中的长度
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static int CalculateLengthOfText(string message, Text tex)
        {
            int totalLength = 0;
            Font myFont = tex.font;  //chatText is my Text component
            myFont.RequestCharactersInTexture(message, tex.fontSize, tex.fontStyle);
            CharacterInfo characterInfo = new CharacterInfo();

            char[] arr = message.ToCharArray();

            foreach (char c in arr)
            {
                myFont.GetCharacterInfo(c, out characterInfo, tex.fontSize);

                totalLength += characterInfo.advance;
            }

            return totalLength;
        }

        /// <summary>
        /// 计算字符串的MD5值
        /// </summary>
        public static string md5(string source)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(source);
            byte[] md5Data = md5.ComputeHash(data, 0, data.Length);
            md5.Clear();

            string destString = "";
            for (int i = 0; i < md5Data.Length; i++)
            {
                destString += System.Convert.ToString(md5Data[i], 16).PadLeft(2, '0');
            }
            destString = destString.PadLeft(32, '0');
            return destString;
        }

        /// <summary>
        /// 计算文件的MD5值
        /// </summary>
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

        /// <summary>
        /// 清除所有子节点
        /// </summary>
        public static void ClearChild(Transform go)
        {
            if (go == null) return;
            for (int i = go.childCount - 1; i >= 0; i--)
            {
                GameObject.Destroy(go.GetChild(i).gameObject);
            }
        }

        /// <summary>
        /// 清理内存
        /// </summary>
        public static void ClearMemory()
        {
            GC.Collect(); Resources.UnloadUnusedAssets();
        }

        /// <summary>
        /// 取得数据存放目录,此为各个平台默认目录，Editor模式为StreamingAsset目录，移动平台为持久化可读写目录
        /// </summary>
        public static string DataPath
        {
            get
            {
                //移动平台默认使用可读写的持久化目录
                if (Application.isMobilePlatform)
                    return Application.persistentDataPath + "/";
#if UNITY_EDITOR
                if (AppConst.UseStreamingAssets)
                    return Application.persistentDataPath + "/";

                return Application.dataPath + "/" + AppConst.AssetDir + "/";
#else
                string game = AppConst.AppName.ToLower();
                if (Application.platform == RuntimePlatform.OSXEditor)
                {
                    int i = Application.dataPath.LastIndexOf('/');
                    return Application.dataPath.Substring(0, i + 1) + game + "/";
                }

                return "c:/" + game + "/";
#endif
            }
        }

        public static string GetStreamingAssetsPath()
        {
            if (Application.isEditor)
                return Application.streamingAssetsPath + "/";
            else if (Application.isMobilePlatform || Application.isConsolePlatform)
                return Application.streamingAssetsPath + "/";
            else // For standalone player.
                return Application.streamingAssetsPath + "/";
        }

        /// <summary>
        /// 取得行文本
        /// </summary>
        public static string GetFileText(string path)
        {
            if (File.Exists(path))
                return File.ReadAllText(path);
            else
                return "";
        }

        /// <summary>
        /// 网络可用
        /// </summary>
        public static bool NetAvailable
        {
            get
            {
                return Application.internetReachability != NetworkReachability.NotReachable;
            }
        }

        /// <summary>
        /// 是否是无线
        /// </summary>
        public static bool IsWifi
        {
            get
            {
                return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
            }
        }

        /// <summary>
        /// 打开浏览器
        /// </summary>
        public static void OpenBrowser(string url)
        {
            //System.Diagnostics.Process.Start(url); 
            WWW www = new WWW(url);
            Application.OpenURL(www.url);
        }

        public static void ClearLocalfileNameInfoDic()
        {
            localfileNameInfoDic = null;
        }

        private static Dictionary<string, string> localfileNameInfoDic = null;

        /// <summary>
        /// 检查 use 文件是否存在可读写目录
        /// </summary>
        /// <param name="filePathName">从AssetsResources下一级开始，直到文件名</param>
        /// <returns>result -1:此use 文件 不存在 0:files 文件里不存在， 1：此use文件存在 2：此为def文件 </returns>
        public static int CheckUseFileIsExit(string filePathName, out string fileMd5Str)
        {
            int result = 0;
            fileMd5Str = "";

            if (ResourcesManager.USE_AB)
            {
                string md5FileInfo = Util.GetUseDownLodFilesInfo(filePathName, out string[] fileInfoArr);
                fileMd5Str = md5FileInfo;

                //原始包里有这个文件，则此文件存在
                if (fileInfoArr != null && ChekPkgFileExit(fileInfoArr[1]))
                    return 1;

                //file.txt里有此文件，且 更新方式为use 则进行此文件是否在读写目录检查
                if (md5FileInfo != "" && fileInfoArr.Length >= 3)
                {
                    if (fileInfoArr[3] == "use")
                    {
                        filePathName = filePathName.ToLower();
                        string url = ResourcesManager.Instance.GetExitFileUrl(filePathName, false);
                        if (url != string.Empty)
                        {
                            result = 1;
                        }
                        else
                        {
                            result = -1;
                        }
                    }
                    else if (fileInfoArr[3] == "def")
                    {
                        result = 2;
                    }
                }
            }

            return result;
        }

        //获取本地存储更新信息
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePathName">从AssetsResources下一级开始，直到文件名</param>
        /// <param name="fileInfoArr"></param>
        /// <returns></returns>
        public static string GetUseDownLodFilesInfo(string filePathName, out string[] fileInfoArr)
        {
            fileInfoArr = null;
            if (localfileNameInfoDic == null)
                localfileNameInfoDic = Util.ReadLocalFilesTxt(true, out string[] fileInfos, 1);

            if (localfileNameInfoDic == null)
                return "";

            filePathName = AppConst.AssetResRoot + "/" + filePathName;
            string fileNameLow = filePathName.ToLower();

            foreach (KeyValuePair<string, string> kv in localfileNameInfoDic)
            {
                if (kv.Key == fileNameLow)
                {
                    string s = kv.Value;
                    s = s.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");
                    fileInfoArr = s.Split('|');

                    return kv.Value;
                }
            }

            return "";
        }

        public static string pkgFilesData = "";
        /// <summary>
        /// 读取本地files.txt
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ReadLocalFilesTxt(bool isToLow, out string[] fileInfo, int readType = 0)
        {
            string localFiles = "files.txt";
            string localPersistentFiles = Application.persistentDataPath + "/" + AppConst.AssetResRoot + "/" + localFiles;

            //可读写目录
            string localFiles1 = Util.GetFileText(localPersistentFiles);
            string[] localFiles1Lines = null;
            long localFiles1Version = 0;

            //应用程序目录
            string localFiles2 = pkgFilesData;//Util.GetFileText(Util.GetStreamingAssetsPath() + AppConst.AssetResRoot + "/" + localFiles);//Adroid不可这样读取
            string[] localFiles2Lines = null;
            long localFiles2Version = 0;

            if (localFiles1 != null && localFiles1 != "")
            {
                localFiles1Lines = localFiles1.Split(new String[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                string info1 = localFiles1Lines[0];
                if (info1 != null)
                {
                    Debug.Log("可读写目录首行" + info1);
                    string[] info1s = info1.Split('#');
                    if (info1s.Length >= 3)
                    {
                        if (Convert.ToInt32(info1s[2]) == localFiles1Lines.Length - 1)  //文件数量校验
                            localFiles1Version = Convert.ToInt64(info1s[1]);
                    }
                }
                else
                {
                    Debug.Log("可读写目录首行空");
                }
            }
            else
            {
                Debug.Log("可读写目录无 files.txt");
            }

            if (localFiles2 != null && localFiles2 != "")
            {
                localFiles2Lines = localFiles2.Split(new String[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                string info2 = localFiles2Lines[0];
                if (info2 != null)
                {
                    Debug.Log("应用程序目首行" + info2);
                    string[] info2s = info2.Split('#');
                    if (info2s.Length >= 3)
                    {
                        localFiles2Version = Convert.ToInt64(info2s[1]);
                    }
                }
                else
                {
                    Debug.Log("应用程序目首行空");
                }
            }
            else
            {
                Debug.Log("应用程序目录无 files.txt：" + Util.GetStreamingAssetsPath() + AppConst.AssetResRoot + "/" + localFiles);
            }

            if (localFiles1Version == 0 && localFiles2Version == 0)
            {
                fileInfo = null;
                return null;
            }

            string[] localFilesLines = null;
            if (localFiles2Version >= localFiles1Version || localFiles1Version == 0)
                localFilesLines = localFiles2Lines;
            else
                localFilesLines = localFiles1Lines;

            Dictionary<string, string> filesLinesDic = new Dictionary<string, string>();
            for (int i = 1; i < localFilesLines.Length; i++)
            {
                string s = localFilesLines[i];
                s = s.Replace("\n", "").Replace("\t", "").Replace("\r", "");
                string[] strs = s.Split('|');

                if (isToLow)
                    strs[0] = strs[0].ToLower();

                if (!filesLinesDic.ContainsKey(strs[0]))
                {
                    if (readType == 0)
                        filesLinesDic.Add(strs[0], strs[1]);
                    else if (readType == 1)
                        filesLinesDic.Add(strs[0], s);
                }

            }

            string info = localFilesLines[0];
            if (info != null)
                fileInfo = info.Split('#');
            else
                fileInfo = null;

            return filesLinesDic;
        }

        private static Dictionary<string, string> pkgFilesDic = null;
        public static bool ChekPkgFileExit(string fileMd5)
        {
            if (pkgFilesData == null)
                return false;

            if (pkgFilesDic != null)
                return pkgFilesDic.ContainsKey(fileMd5);

            string[] localFilesLines = pkgFilesData.Split(new String[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 1; i < localFilesLines.Length; i++)
            {
                string s = localFilesLines[i];
                s = s.Replace("\n", "").Replace("\t", "").Replace("\r", "");
                string[] strs = s.Split('|');

                pkgFilesDic = new Dictionary<string, string>();
                if (!pkgFilesDic.ContainsKey(strs[0]))
                {
                    pkgFilesDic.Add(strs[1], s);
                }
            }

            return pkgFilesDic.ContainsKey(fileMd5);
        }

        /// <summary>
        /// 应用程序内容路径
        /// </summary>
        public static string AppContentPath()
        {
            string path = string.Empty;
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    path = "jar:file://" + Application.dataPath + "!/assets/";
                    break;
                case RuntimePlatform.IPhonePlayer:
                    path = Application.dataPath + "/Raw/";
                    break;
                default:
                    path = Application.dataPath + "/" + AppConst.AssetDir + "/";
                    break;
            }
            return path;
        }

        // public static string ToJson(object data, Type type)
        // {
        //     JsonData json = new JsonData();
        //     json["type"] = type.FullName;
        //     json["data"] = JsonUtility.ToJson(data);
        //     return json.ToJson();
        // }

        // public static T ToObject<T>(string json)
        // {
        //     JsonData jsonData = JsonMapper.ToObject(json);
        //     string type = jsonData["type"].ToString();
        //     string buff = jsonData["data"].ToString();
        //     Assembly assembly = Assembly.GetExecutingAssembly();
        //     dynamic obj = assembly.CreateInstance(type);
        //     JsonUtility.FromJsonOverwrite(buff, obj);
        //     return (T)obj;
        // }

        public static Vector3 RadiusBetweenPos(Vector3 pos, float minRadius, float maxRadius)
        {
            float angle = UnityEngine.Random.Range(0, 360);
            float r = UnityEngine.Random.Range(minRadius, maxRadius);
            pos.x = pos.x + r * Mathf.Cos(angle * Mathf.PI / 180);
            pos.y = 0;
            pos.z = pos.z + r * Mathf.Sin(angle * Mathf.PI / 180);

            return pos;
        }

        public static Vector3 FBV32UV3(MyFlatBuffer.Vector3 fbV3)
        {
            return new Vector3(fbV3.X,fbV3.Y,fbV3.Z);
        }

        public static FlatBuffers.Offset<MyFlatBuffer.Vector3> UV32FBV3(FlatBuffers.FlatBufferBuilder buffer, Vector3 uV3)
        {
            return MyFlatBuffer.Vector3.CreateVector3(buffer, uV3.x,uV3.y,uV3.z);
        }

        public static void Log(string str)
        {
            Debug.Log(str);
        }

        public static void LogWarning(string str)
        {
            Debug.LogWarning(str);
        }

        public static void LogError(string str)
        {
            Debug.LogError(str);
        }
    }
}