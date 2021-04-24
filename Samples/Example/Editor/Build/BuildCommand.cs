using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace lus.framework
{
    static class BuildCommand
    {
        static string GetArgument(string name)
        {
            string[] args = Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Contains(name))
                {
                    return args[i + 1];
                }
            }
            return null;
        }

        static string[] GetEnabledScenes()
        {
            return (
                from scene in EditorBuildSettings.scenes
                where scene.enabled
                where !string.IsNullOrEmpty(scene.path)
                select scene.path
            ).ToArray();
        }

        static BuildTarget GetBuildTarget()
        {
            string buildTargetName = GetArgument("customBuildTarget");
            Console.WriteLine(":: Received customBuildTarget " + buildTargetName);
            return ToEnum<BuildTarget>(buildTargetName, BuildTarget.NoTarget);
        }

        static string GetBuildPath()
        {
            string buildPath = GetArgument("customBuildPath");
            Console.WriteLine(":: Received customBuildPath " + buildPath);
            if (buildPath == "")
            {
                throw new Exception("customBuildPath argument is missing");
            }
            return buildPath;
        }

        static string GetBuildName()
        {
            string buildName = GetArgument("customBuildName");
            Console.WriteLine(":: Received customBuildName " + buildName);
            if (buildName == "")
            {
                throw new Exception("customBuildName argument is missing");
            }
            return buildName;
        }

        static string GetFixedBuildPath(BuildTarget buildTarget, string buildPath, string buildName)
        {
            if (buildTarget.ToString().ToLower().Contains("windows"))
            {
                buildName = buildName + ".exe";
            }
            else if (buildTarget.ToString().ToLower().Contains("ios"))
            {
                buildName = buildName + ".ipa";
            }
            else if (buildTarget.ToString().ToLower().Contains("android"))
            {
                buildName = buildName + ".apk";
            }
            else if (buildTarget.ToString().ToLower().Contains("webgl"))
            {
                // webgl produces a folder with index.html inside, there is no executable name for this buildTarget
                // buildName = "WebGL";
            }
            return buildPath + buildName;
        }

        static BuildOptions GetBuildOptions()
        {
            string buildOptions = GetArgument("customBuildOptions");
            return buildOptions == "AcceptExternalModificationsToPlayer" ? BuildOptions.AcceptExternalModificationsToPlayer : BuildOptions.None;
        }

        static TEnum ToEnum<TEnum>(this string strEnumValue, TEnum defaultValue)
        {
            if (!Enum.IsDefined(typeof(TEnum), strEnumValue))
            {
                return defaultValue;
            }

            return (TEnum)Enum.Parse(typeof(TEnum), strEnumValue);
        }

        static string GetEnv(string key, bool secret = false, bool verbose = true)
        {
            var env_var = Environment.GetEnvironmentVariable(key);
            if (verbose)
            {
                if (env_var != null)
                {
                    if (secret)
                    {
                        Console.WriteLine(":: env['" + key + "'] set");
                    }
                    else
                    {
                        Console.WriteLine(":: env['" + key + "'] set to '" + env_var + "'");
                    }
                }
                else
                {
                    Console.WriteLine(":: env['" + key + "'] is null");
                }
            }
            return env_var;
        }

        static void PerformBuild()
        {
            Console.WriteLine(":: Performing asset bundle build");
            PerformAssetBundle();

            Console.WriteLine(":: Performing build");
            var buildTarget = GetBuildTarget();
            var buildPath = GetBuildPath();
            var buildName = GetBuildName();
            var fixedBuildPath = GetFixedBuildPath(buildTarget, buildPath, buildName);

            BuildReport report = BuildPipeline.BuildPlayer(GetEnabledScenes(), fixedBuildPath, buildTarget, GetBuildOptions());
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Console.WriteLine(":: Build succeeded: " + summary.totalSize + " bytes");
                EditorApplication.Exit(0);
            }

            if (summary.result == BuildResult.Failed)
            {
                Console.WriteLine(":: Build failed");
                EditorApplication.Exit(1);
            }

            Console.WriteLine(":: Done with build");
        }

        static void PerformAssetBundle()
        {
            Console.WriteLine(":: Performing asset bundle build");
            Console.WriteLine(string.Format("Platform:{0}", Application.platform));
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    AssetBundlesMenu.BuildAssetBundlesStandaloneWindows64();
                    break;
                case RuntimePlatform.OSXEditor:
                    AssetBundlesMenu.BuildAssetBundlesStandaloneOSX();
                    break;
                case RuntimePlatform.Android:
                    AssetBundlesMenu.BuildAssetBundlesAndroid();
                    break;
                case RuntimePlatform.IPhonePlayer:
                    AssetBundlesMenu.BuildAssetBundlesiOS();
                    break;
            }
        }

        [MenuItem("MyTools/Build/BuildPlayer")]
        public static void DoBuild()
        {
            PerformAssetBundle();
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = GetEnabledScenes();
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    buildPlayerOptions.locationPathName = "StandaloneWindows64Build";
                    buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
                    break;
                case RuntimePlatform.OSXEditor:
                    buildPlayerOptions.locationPathName = "StandaloneOSXBuild";
                    buildPlayerOptions.target = BuildTarget.StandaloneOSX;
                    break;
                case RuntimePlatform.Android:
                    buildPlayerOptions.locationPathName = "AndroidBuild";
                    buildPlayerOptions.target = BuildTarget.Android;
                    break;
                case RuntimePlatform.IPhonePlayer:
                    buildPlayerOptions.locationPathName = "iOSBuild";
                    buildPlayerOptions.target = BuildTarget.iOS;
                    break;
            }
            buildPlayerOptions.options = BuildOptions.None;
            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
            }

            if (summary.result == BuildResult.Failed)
            {
                Debug.Log("Build failed");
            }
        }
    }
}