
using UnityEngine;
using Protobuf;          
using Google.Protobuf;     

public class TestFramework : MonoBehaviour {

    void Start()
    {
        //新建一个Person对象，并赋值
        Person p = new Person();
        p.Name = "IongX";
        p.Age = 22;
        p.NameList.Add("熊");
        p.NameList.Add("棒");
        p.NameList.Add("棒");
        //将对象转换成字节数组
        byte[] databytes = p.ToByteArray();

        //将字节数据的数据还原到对象中
        IMessage IMperson = new Person();
        Person p1 = new Person();
        p1 = (Person)IMperson.Descriptor.Parser.ParseFrom(databytes);

        //输出测试
        Debug.Log(p1.Name);
        Debug.Log(p1.Age);
        for (int i = 0; i < p1.NameList.Count; i++)
        {
            Debug.Log(p1.NameList[i]);
        }
    }
}
