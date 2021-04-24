using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

namespace lus.framework
{
    internal class Utility
    {
        /// <summary>
        /// 读取本地资源文件
        /// </summary>
        public static Dictionary<string, string> ReadLocalFilesTxt(string localFiles, string AssetResRoot, bool isToLow, out string[] fileInfo, int readType = 0)
        {
            string localPersistentFiles = Application.persistentDataPath + "/" + AssetResRoot + "/" + localFiles;
            string localStreamingAssetsFiles = Application.streamingAssetsPath + "/" + AssetResRoot + "/" + localFiles;
            //可读写目录
            string localFiles1 = GetFileText(localPersistentFiles);
            string[] localFiles1Lines = null;
            long localFiles1Version = 0;
            //应用程序目录
            string localFiles2 = GetFileText(localStreamingAssetsFiles);
            string[] localFiles2Lines = null;
            long localFiles2Version = 0;

            if (localFiles1 != null && localFiles1 != "")
            {
                localFiles1Lines = localFiles1.Split(new String[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                string info1 = localFiles1Lines[0];
                if (info1 != null)
                {
                    //Debug.Log("可读写目录首行" + info1);
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
                Debug.Log(string.Format("<color=#ff0000>{0}</color>", "PersistentDataPath not find files.txt"));
            }

            if (localFiles2 != null && localFiles2 != "")
            {
                localFiles2Lines = localFiles2.Split(new String[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                string info2 = localFiles2Lines[0];
                if (info2 != null)
                {
                    //Debug.Log("应用程序目首行" + info2);
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
                Debug.Log(string.Format("<color=#ff0000>{0}</color>", "StreamingAssetsPath not find files.txt"));
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

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileText(string path)
        {
            if (File.Exists(path))
                return File.ReadAllText(path);
            else
                return "";
        }

    }
}