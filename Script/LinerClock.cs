using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HaseMikan
{
    /// <summary>
    /// 线性钟，采用线性的逻辑去处理事件
    /// </summary>
    public class LinerClock : MonoBehaviour
    {
        /// <summary>
        /// 单例变量
        /// </summary>
        public static LinerClock instance;

        /// <summary>
        /// 单例属性
        /// </summary>
        public static LinerClock Instance
        {
            get
            {
                if(instance == null)
                {
                    GameObject tempHost = new GameObject();
                    instance = tempHost.AddComponent<LinerClock>();
                    DontDestroyOnLoad(tempHost);
                }
                return instance;
            }
        }

        /// <summary>
        /// 线性时钟事件
        /// </summary>
        private class ClockEvent
        {
            /// <summary>
            /// 如果不是队列中的第一个事件，则表示在前驱事件执行完之后，等待多久执行该事件
            /// 否则表示离当前事件执行还剩下多少时间
            /// </summary>
            public float leftTime;

            /// <summary>
            /// 当前执行的函数，注意，如果使用匿名委托，在调用CancelEvent的时候将无法判断相等
            /// </summary>
            public Action eventAction;
            
            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="leftTime">剩余事件</param>
            /// <param name="eventAction">执行事件</param>
            public ClockEvent(float leftTime, Action eventAction)
            { 
                this.leftTime = leftTime;
                this.eventAction = eventAction;
            }
        }

        /// <summary>
        /// 存储线性时钟事件的链表
        /// </summary>
        private LinkedList<ClockEvent> clockEvents;

        /// <summary>
        /// 初始化
        /// </summary>
        public void Awake()
        {
            clockEvents = new LinkedList<ClockEvent>();
        }

        /// <summary>
        /// 每deltaTime执行的函数
        /// </summary>
        public void Update()
        {
            float leftDeltaTime = Time.deltaTime;

            //依次对队首进行判断，直到deltaTime被耗完或者队列中没有事件
            while(leftDeltaTime > 0 && clockEvents.Count != 0)
            {
                var soloNode = clockEvents.First;

                //如果队首事件剩余时间大于deltaTime，则只减少
                if (soloNode.Value.leftTime > leftDeltaTime)
                {
                    soloNode.Value.leftTime -= leftDeltaTime;
                    leftDeltaTime = 0;
                    /*Debug.Log(soloNode.Value.leftTime);*/
                }
                //如果不是，则执行并退出队首,减少相应的deltaTime
                else
                {
                    leftDeltaTime -= soloNode.Value.leftTime;
                    soloNode.Value.eventAction.Invoke();
                    
                    clockEvents.RemoveFirst();
                    soloNode = clockEvents.First;
                }
            }
        }

        /// <summary>
        /// 插入一个延迟执行的事件，返回插入是否成功
        /// </summary>
        /// <param name="action">延迟执行的事件</param>
        /// <param name="delaySeconds">延迟的时间</param>
        /// <returns></returns>
        public static bool InsertDelayAction(float delaySeconds , System.Action action)
        {
            //排除不成功情况
            if (delaySeconds <= 0) return false;

            //判断是否队列为空
            //如果为空则直接插入
            if (Instance.clockEvents.Count == 0) Instance.clockEvents.AddFirst(new ClockEvent(delaySeconds,action));
            //如果不为空
            else 
            {
                //寻找插入位置
                LinkedListNode<ClockEvent> soloNode = Instance.clockEvents.First;
                float sum = 0;

                while (soloNode != null)
                {
                    sum += soloNode.Value.leftTime;

                    //如果当前累计和小于延迟时间，则说明插入位置在后方
                    if(sum < delaySeconds) soloNode = soloNode.Next;

                    //如果当前累计和大于延迟时间，则说明插入位置就在这个事件前
                    if(sum > delaySeconds)
                    {
                        Instance.clockEvents.AddBefore(soloNode, new ClockEvent(delaySeconds + soloNode.Value.leftTime - sum, action));
                        
                        //修改当前结点的值
                        soloNode.Value.leftTime = sum - delaySeconds;

                        //无需进行其他调整，直接返回成功
                        return true;
                    }

                    //如果相等，则直接加在委托中
                    if(sum == delaySeconds)
                    {
                        soloNode.Value.eventAction += action;

                        //无需进行其他调整，直接返回成功
                        return true;
                    }
                }

                //如果遍历都没有匹配成功，则一定加在最末尾
                if(soloNode == null)
                {
                    Instance.clockEvents.AddLast(new ClockEvent(delaySeconds - sum,action));
                }

            }
            
            return true;
        }
    }
}