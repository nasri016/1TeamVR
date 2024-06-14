using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour 
    //싱글턴 패턴으로, 이벤트를 등록.해제.트리거하는 기능
    //이벤트 매니저는 이벤트를 키로 사용하고 해당 이벤트에 반응하는 핸들러(커맨드)의 리스트를 
    //값으로 사용하는 딕셔너리를 내부적으로 관리한다. 이벤트를 발생시킬 때마다 딕셔너리를 조회하여 해당 이벤트에 등록된 모든 핸들러를 실행한다.
{
    //싱글톤 인스턴스
    private static EventManager _instance;
    public static EventManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // 이벤트 매니저가 존재하지 않는 경우, 새로 생성
                var obj = new GameObject("EventManager");
                _instance = obj.AddComponent<EventManager>();
            }
            return _instance;
        }
    }

    //이벤트 딕셔너리
   // private Dictionary<Type, Action<EventBase>> eventDictionary;


    //인스턴스의 초기화와 딕셔너리의 초기화를 수행
    void Awake()
    {
        
        if (_instance == null)
        {
            _instance = this;
            //eventDictionary = new Dictionary<Type, Action<EventBase>>();
            DontDestroyOnLoad(gameObject); // 씬이 변경되어도 파괴되지 않음
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //특정 이벤트 타입에 대한 리스너를 등록
    
    /*
    public void Subscribe<T>(Action<T> listener) where T : EventBase
    {
        Type eventType = typeof(T);
        if (!eventDictionary.ContainsKey(eventType))
        {
            eventDictionary[eventType] = (x => listener((T)x));
        }
        else
        {
            eventDictionary[eventType] += (x => listener((T)x));
        }
    }

    public void Unsubscribe<T>(Action<T> listener) where T : EventBase
    {
        Type eventType = typeof(T);
        if (eventDictionary.ContainsKey(eventType))
        {
            eventDictionary[eventType] -= (x => listener((T)x));
        }
    }

    public void TriggerEvent(EventBase eventToTrigger)
    {
        Type eventType = eventToTrigger.GetType();
        if (eventDictionary.ContainsKey(eventType))
        {
            eventDictionary[eventType].Invoke(eventToTrigger);
        }
    }
    
    */
}

public abstract class EventBase { }
