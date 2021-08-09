using System;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

namespace PureMVC.Core
{
    public class GFAttributeHelper<T> where T : GFAttribute
    {
        #region Public 

        public static void Register(object p)
        {
            RegisterClass(p);
            RegisterField(p);
            RegisterMethod(p);

            Log();
        }

        public static void UnRegister(object p)
        {
            UnRegisterClass(p);
            UnRegisterField(p);
            UnRegisterMethod(p);

            Log();
        }

        public static void ClearCache()
        {
            Instance._ClassCache.Clear();
            Instance._ClassFieldCache.Clear();
            Instance._ClassMethodCache.Clear();
        }

        /// <summary>
        /// 反射调用函数 【所有注册对象】
        /// </summary>
        /// <param name="_param"></param>
        /// <returns></returns>
        public static bool InvokeAllMethod(params object[] _param)
        {
            //所有类型
            foreach (var type in Instance._ClassMethodCache)
            {
                //所有对象
                foreach (var obj in type.Value)
                {
                    Dictionary<MethodInfo, string> outMethod = obj.Value;
                    foreach (var method in outMethod)
                    {
                        //参数信息
                        ParameterInfo[] parameterInfos = method.Key.GetParameters();
                        //参数个数
                        if (parameterInfos.Length == _param.Length)
                        {
                            method.Key.Invoke(obj.Key, _param as object[]);
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 反射调用函数 【无形参类型检测】
        /// </summary>
        /// <param name="p"></param>
        /// <param name="_param"></param>
        /// <returns></returns>
        public static bool InvokeMethodWithOutCheck(object p, params object[] _param)
        {
            //类型检查
            Type t = p.GetType();
            if (Instance._ClassMethodCache.ContainsKey(t))
            {
                //对象检查
                if (Instance._ClassMethodCache[t].ContainsKey(p))
                {
                    Dictionary<MethodInfo, string> outMethod = Instance._ClassMethodCache[t][p];

                    foreach (var method in outMethod)
                    {
                        //参数信息
                        ParameterInfo[] parameterInfos = method.Key.GetParameters();
                        //参数个数
                        if (parameterInfos.Length == _param.Length)
                        {
                            method.Key.Invoke(p, _param as object[]);
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 反射调用函数 【形参类型检测】
        /// </summary>
        /// <param name="p"></param>
        /// <param name="_param"></param>
        /// <returns></returns>
        public static bool InvokeMethod<T1>(object p, params T1[] _param)
        {
            //类型检查
            Type t = p.GetType();
            if (Instance._ClassMethodCache.ContainsKey(t))
            {
                //对象检查
                if (Instance._ClassMethodCache[t].ContainsKey(p))
                {
                    Dictionary<MethodInfo, string> outMethod = Instance._ClassMethodCache[t][p];

                    foreach (var method in outMethod)
                    {
                        //参数信息
                        ParameterInfo[] parameterInfos = method.Key.GetParameters();

                        //参数个数
                        if (parameterInfos.Length == _param.Length)
                        {
                            Type tt = typeof(T1);
                            //参数类型
                            foreach (var parameterInfo in parameterInfos)
                            {
                                if (parameterInfo.ParameterType != typeof(T1))
                                    return false;
                            }
                            method.Key.Invoke(p, _param as object[]);

                        }
                    }
                    return true;
                }
            }
            return false;
        }

        public static void Log()
        {
            int nClassNumber = 0;
            foreach (var obj in Instance._ClassCache)
            {
                Dictionary<object, string> dic = obj.Value;
                nClassNumber += dic.Count;
            }

            int nMethodNumber = 0;
            foreach (var obj in Instance._ClassMethodCache)
            {
                Dictionary<object, Dictionary<MethodInfo, string>> dic = obj.Value;
                foreach (var methods in dic)
                {
                    Dictionary<MethodInfo, string> methodDic = methods.Value;
                    nMethodNumber += methodDic.Count;
                }
            }


            int nFieldNumber = 0;
            foreach (var obj in Instance._ClassFieldCache)
            {
                Dictionary<object, Dictionary<FieldInfo, string>> dic = obj.Value;
                foreach (var fieldInfos in dic)
                {
                    Dictionary<FieldInfo, string> methodDic = fieldInfos.Value;
                    nFieldNumber += methodDic.Count;
                }
            }
            Debug.Log(" C:" + nClassNumber + " M:" + nMethodNumber + " F:" + nFieldNumber);
        }

        #endregion

        #region Register

        public static void RegisterClass(object p)
        {
            Type t = p.GetType();
            var classAttributes = t.GetCustomAttributes(typeof(T), false);
            foreach (var attributes in classAttributes)
            {
                //指定类型
                if (attributes.GetType() == typeof(T))
                {
                    T obj = (T)attributes;
                    Type attrType = obj.GetType();


                    string value = GFAttribute.Detect(attrType, obj);

                    //先分类型
                    if (Instance._ClassCache.ContainsKey(t))
                    {
                        //再分对象
                        if (Instance._ClassCache[t].ContainsKey(p))
                            Instance._ClassCache[t].Add(p, value);
                        else
                            Instance._ClassCache[t][p] = value;
                    }
                    else
                    {
                        Dictionary<object, string> dir = new Dictionary<object, string>();
                        dir[p] = value;
                        Instance._ClassCache.Add(t, dir);
                    }
                }
            }

            //Debug.Log(p + "类探测完毕！");
        }

        public static void RegisterField(object p)
        {
            Type t = p.GetType();
            ////公有变量
            //var publicfields = t.GetFields(BindingFlags.Public);
            ////私有变量
            //var nonPublicfields = t.GetFields(BindingFlags.NonPublic);
            ////静态变量
            //var staticfields = t.GetFields(BindingFlags.Static);
            FieldInfo[] fields = t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var field in fields)
            {
                var classAttributes = field.GetCustomAttributes(typeof(T), false);
                foreach (var attributes in classAttributes)
                {
                    //指定类型
                    if (attributes.GetType() == typeof(T))
                    {
                        T obj = (T)attributes;
                        Type attrType = obj.GetType();

                        string value = GFAttribute.Detect(attrType, obj);

                        //先分类型
                        if (Instance._ClassFieldCache.ContainsKey(t))
                        {
                            //再分对象
                            if (Instance._ClassFieldCache[t].ContainsKey(p))
                            {
                                //再分属性
                                if (Instance._ClassFieldCache[t][p].ContainsKey(field))
                                    Instance._ClassFieldCache[t][p][field] = value;
                                else
                                    Instance._ClassFieldCache[t][p].Add(field, value);
                            }
                            else
                                Instance._ClassFieldCache[t][p][field] = value;
                        }
                        else
                        {
                            Dictionary<FieldInfo, string> fieldDic = new Dictionary<FieldInfo, string>();
                            fieldDic.Add(field, value);
                            Dictionary<object, Dictionary<FieldInfo, string>> dir = new Dictionary<object, Dictionary<FieldInfo, string>>();
                            dir.Add(p, fieldDic);
                            Instance._ClassFieldCache.Add(t, dir);
                        }
                    }
                }
            }

            //Debug.Log(p + "属性探测完毕！");
        }

        public static void RegisterMethod(object p)
        {
            Type t = p.GetType();
            MethodInfo[] methods = t.GetMethods(BindingFlags.Public | BindingFlags.Instance);

            foreach (var method in methods)
            {
                var classAttributes = method.GetCustomAttributes(typeof(T), false);
                foreach (var attributes in classAttributes)
                {
                    //指定类型
                    if (attributes.GetType() == typeof(T))
                    {
                        T obj = (T)attributes;
                        Type attrType = obj.GetType();

                        string value = GFAttribute.Detect(attrType, obj);

                        //先分类型
                        if (Instance._ClassMethodCache.ContainsKey(t))
                        {
                            //再分对象
                            if (Instance._ClassMethodCache[t].ContainsKey(p))
                            {
                                //再分方法
                                if (Instance._ClassMethodCache[t][p].ContainsKey(method))
                                    Instance._ClassMethodCache[t][p][method] = value;
                                else
                                    Instance._ClassMethodCache[t][p].Add(method, value);
                            }
                            else
                                Instance._ClassMethodCache[t][p][method] = value;
                        }
                        else
                        {
                            Dictionary<MethodInfo, string> methodDic = new Dictionary<MethodInfo, string>();
                            methodDic.Add(method, value);
                            Dictionary<object, Dictionary<MethodInfo, string>> dir = new Dictionary<object, Dictionary<MethodInfo, string>>();
                            dir.Add(p, methodDic);
                            Instance._ClassMethodCache.Add(t, dir);
                        }
                    }
                }
            }

            //Debug.Log(p + "方法探测完毕！");
        }

        #endregion

        #region UnRegister

        public static void UnRegisterClass(object p)
        {
            Type t = p.GetType();
            if (Instance._ClassCache.ContainsKey(t))
            {
                //对象检查
                if (Instance._ClassCache[t].ContainsKey(p))
                {
                    Dictionary<object, string> objDic = Instance._ClassCache[t];
                    if (objDic.Remove(p))
                    {
                        // Debug.Log(p + "类移除成功！");
                    }
                    else
                    {
                        Debug.Log(p + "类移除失败！");
                    }
                }
            }
        }

        public static void UnRegisterField(object p)
        {
            //类型检查
            Type t = p.GetType();
            if (Instance._ClassFieldCache.ContainsKey(t))
            {
                //对象检查
                if (Instance._ClassFieldCache[t].ContainsKey(p))
                {
                    Instance._ClassFieldCache[t].Remove(p);
                    //Debug.Log(p + "属性移除完毕！");
                }
            }
        }

        public static void UnRegisterMethod(object p)
        {
            //类型检查
            Type t = p.GetType();
            if (Instance._ClassMethodCache.ContainsKey(t))
            {
                //对象检查
                if (Instance._ClassMethodCache[t].ContainsKey(p))
                {
                    Instance._ClassMethodCache[t].Remove(p);
                    //Debug.Log(p + "方法移除完毕！");
                }
            }
        }

        #endregion

        #region FindMothod

        public static bool GetClass(object p, ref string _out)
        {
            //类型检查
            Type t = p.GetType();
            if (Instance._ClassCache.ContainsKey(t))
            {
                //对象检查
                if (Instance._ClassCache[t].ContainsKey(p))
                {
                    _out = Instance._ClassCache[t][p];
                    return true;
                }
            }

            return false;
        }

        public static bool GetClassField(object p, ref Dictionary<FieldInfo, string> _out)
        {
            //类型检查
            Type t = p.GetType();
            if (Instance._ClassFieldCache.ContainsKey(t))
            {
                //对象检查
                if (Instance._ClassFieldCache[t].ContainsKey(p))
                {
                    _out = Instance._ClassFieldCache[t][p];
                    return true;
                }
            }
            return false;
        }

        public static bool GetClassMethod(object p, ref Dictionary<MethodInfo, string> _out)
        {
            //类型检查
            Type t = p.GetType();
            if (Instance._ClassMethodCache.ContainsKey(t))
            {
                //对象检查
                if (Instance._ClassMethodCache[t].ContainsKey(p))
                {
                    _out = Instance._ClassMethodCache[t][p];
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region 私有属性
        private Dictionary<Type, Dictionary<object, string>> _ClassCache = new Dictionary<Type, Dictionary<object, string>>();
        private Dictionary<Type, Dictionary<object, Dictionary<FieldInfo, string>>> _ClassFieldCache = new Dictionary<Type, Dictionary<object, Dictionary<FieldInfo, string>>>();
        private Dictionary<Type, Dictionary<object, Dictionary<MethodInfo, string>>> _ClassMethodCache = new Dictionary<Type, Dictionary<object, Dictionary<MethodInfo, string>>>();
        #endregion

        #region Singleton

        private static GFAttributeHelper<T> _instance = null;

        public static GFAttributeHelper<T> Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GFAttributeHelper<T>();
                return _instance;
            }
        }

        private GFAttributeHelper()
        {

        }

        #endregion
    }
}


