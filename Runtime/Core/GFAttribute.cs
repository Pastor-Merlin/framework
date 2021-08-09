using System;
using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;


namespace PureMVC.Core
{
    [AttributeUsage(AttributeTargets.Field |
                    AttributeTargets.Method |
                    AttributeTargets.Class)]
    public class GFAttribute : Attribute
    {
        public string Description { get; private set; }

        public GFType lusType { get; private set; }

        public enum GFType
        {
            None,
            Class,
            Method,
            Field
        }

        public static string Detect(Type _type, object _obj)
        {
            string value = "";
            if (_obj != null)
            {
                //获取指定的属性
                PropertyInfo pi0 = _type.GetProperty("Description");
                //获取指定属性的值
                if (pi0 != null)
                    value += pi0.GetValue(_obj, null).ToString();

                value += "_";
                //获取指定的属性
                PropertyInfo pi1 = _type.GetProperty("GFType");
                //获取指定属性的值
                if (pi1 != null)
                    value += pi1.GetValue(_obj, null).ToString();
            }
            return value;
        }

        public GFAttribute()
        {
            Description = "";
            lusType = GFType.None;
        }

        public GFAttribute(string _Description)
        {
            Description = _Description;
            lusType = GFType.None;
        }

        public GFAttribute(string _Description, GFType _lusType)
        {
            Description = _Description;
            lusType = _lusType;
        }
    }

}


