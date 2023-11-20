using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T> //����Լ����T������
{
    private static T instance; // ���ɸ���ĵ���ʵ��

    public static T Instance //����get�ⲿ����
    {
        get { return instance; }
    }

    //ϣ���̳и�������������඼�ܸ����ڲ��ķ���������ʹ��protected
    protected virtual void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = (T)this;
        }
    }

    // ���ص��������Ƿ�����
    public static bool IsInitialized
    {
        get { return instance != null;  }
    }

    // ���ٸõ������͵ķ�ʽ
    protected virtual void OnDestroy()
    {
        if(instance == this)
        {
            instance = null;
        }
    }
}
