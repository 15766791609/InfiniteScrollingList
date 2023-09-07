using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfiniteScrolling : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    //�Ƿ���
    bool isPress;
    //��ǰ���λ��
    Vector2 curPos;
    // item�б�
    public List<Transform> items;
    //���ݿ�߶�
    public float height;
    // ��βλ��
    public float startY = 0f;
    public float endY = 0f;
    // ���
    public float interval = 5f;

    public float maxScale = 1f; // ������ű���
    public float minScale = 0.5f; // ��С���ű���

    public Transform middlePos;
    private void Start()
    {
        height = items[0].GetComponent<RectTransform>().sizeDelta.y;
        startY = items[0].GetComponent<RectTransform>().localPosition.y;
        endY = items[items.Count - 1].GetComponent<RectTransform>().localPosition.y;
    }

    void Update()
    {
        if (isPress) MoveItems();
        ZoomButton();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        isPress = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPress = true;
        curPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPress = false;
        curPos = Vector2.zero;
    }

    private void MoveItems()
    {
        float y = Input.mousePosition.y - curPos.y;
        curPos.y = Input.mousePosition.y;

        for (int i = 0; i < items.Count; i++)
        {
            items[i].localPosition = new Vector3(items[i].localPosition.x, items[i].localPosition.y + y, 0);
        }
        SwitchPos();
    }

    private void SwitchPos()
    {
        //�����ϱ߽�
        if( items[0].localPosition.y >= startY + height)
        {
            items[0].localPosition = new Vector3(items[items.Count - 1].localPosition.x, items[items.Count - 1].localPosition.y - height - interval, 0);
            var item = items[0];
            items.RemoveAt(0);
            items.Add(item);
        }
        //�����±߽�
        if (items[items.Count - 1].localPosition.y <= endY)
        {
            items[items.Count - 1].localPosition = new Vector3(items[0].localPosition.x, items[0].localPosition.y + height + interval, 0);
            var item = items[items.Count - 1];
            items.RemoveAt(items.Count - 1);
            items.Insert(0, item);
        }
    }

    //��ť����
    private void ZoomButton()
    {
        foreach (var data in items)
        {
            float distance = Vector3.Distance(middlePos.position, data.transform.position);
            float scale = Mathf.Lerp(maxScale, minScale, distance / Mathf.Abs(startY  - middlePos.localPosition.y));
            //����ԶС
            data.transform.localScale = new Vector3(scale, scale, 1f);

        }

    }
}
