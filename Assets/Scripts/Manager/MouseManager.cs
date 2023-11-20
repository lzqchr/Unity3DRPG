using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

//��ק�ķ����������������
//[System.Serializable]

//public class EventVector3 : UnityEvent<Vector3> { };
////һ���̳���UnityEvent���࣬����Ӧ����vector3��
////��Ϊ���class���Ǽ̳�monobehaviour�ģ����Ա��뱻���л�������ʾ����

public class MouseManager : Singleton<MouseManager>
{
    ////����ģʽ��Ҫ�ȴ���һ��static������ı�����ͨ��ȡ��ΪInstance
    //public static MouseManager Instance;

    //��ȡ���Ĳ�ͬ��ͼ
    public Texture2D point, doorway, attack, target, arrow;

    //����һ���¼�
    public event Action<Vector3> OnMouseClicked;

    public event Action<GameObject> OnEnemyClicked;

    public event Action<GameObject> OnMousedPushed;

    //��������߲��񵽵�����
    RaycastHit hitinfo;

    ////����һ��EventVector3�ı��������ꣻ
    //public EventVector3 OnMouseClicked;


    // �ڵ�������дAwake()
    protected override void Awake()
    {
        base.Awake(); //����ԭ�и��෽��֮��
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        SetCurserTexture();
        MouseControl();
    }

    
    void SetCurserTexture()
    {
        //һ�����ߣ��������������������������λ�÷���
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if(Physics.Raycast(ray,out hitinfo))
        {
            //������ƶ���ĳЩλ��ʱ�ı������ͼ
            switch (hitinfo.collider.gameObject.tag)
            {
                case "Ground":
                    Cursor.SetCursor(target, new Vector2(16, 16), CursorMode.Auto);
                    break;

                case "Enemy":
                    Cursor.SetCursor(attack, new Vector2(16, 16), CursorMode.Auto);
                    break;

                case "Portal":
                    Cursor.SetCursor(doorway, new Vector2(16, 16), CursorMode.Auto);
                    break;

                default:
                    Cursor.SetCursor(arrow, new Vector2(16, 16), CursorMode.Auto);
                    break;
            }

        }
    }

    void MouseControl()
    {
        //����������˵������������ײ������ײ�岻Ϊ��
        if (Input.GetMouseButtonDown(0) && hitinfo.collider != null)
        {
            //�����ײ���������tagΪ����
            if(hitinfo.collider.gameObject.CompareTag("Ground"))
            {
                //������˵Ļ����������¼�
                OnMouseClicked?.Invoke(hitinfo.point);
                //�ͻ�ִ��ע����OnMouseClicked����������¼�
            }

            //�����ײ������tagΪEnemy
            if(hitinfo.collider.gameObject.CompareTag("Enemy"))
            {
                //ִ��ע����OnEnemyClicked����������¼���
                //���ﴫ��ȥ�������������ײ����collider��gameObject
                OnEnemyClicked?.Invoke(hitinfo.collider.gameObject);
            }

            //�����
            //if(hitinfo.collider.gameObject.CompareTag("Player"))
            //{
            //    OnSelfClicked?.Invoke(hitinfo.collider.gameObject);
            //}

            //��ײ����Ϊʯͷ�ȿɹ���ʵ��
            if(hitinfo.collider.gameObject.CompareTag("Attackable"))
            {
                OnEnemyClicked?.Invoke(hitinfo.collider.gameObject);
            }

            //��ײ����Ϊ������
            if (hitinfo.collider.gameObject.CompareTag("Portal"))
            {
                OnMouseClicked?.Invoke(hitinfo.point);
              
            }
        }
    }
}
