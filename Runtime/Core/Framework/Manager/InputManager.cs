/*
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace Framework
{
    public delegate void TouchDelegate(Gesture gesture);

    public class InputManager : Manager
    {
        // Subscribe to events  
        //鼠标在屏幕上点击时，On_TouchStart响应一次，On_TouchDown至少响应一次；松开时On_TouchUp响应一次 
        //鼠标在屏幕上拖拽时，On_SwipeStart响应一次，On_Swipe至少响应一次；松开时On_SwipeEnd响应一次  
        public event TouchDelegate start2FingersEvent;
        public event TouchDelegate up2FingersEvent;
        public event TouchDelegate touchStartEvent;
        public event TouchDelegate touchUpEvent;
        public event TouchDelegate touchDownEvent;
        public event TouchDelegate swipeStartEvent;
        public event TouchDelegate swipeEvent;
        public event TouchDelegate swipeEndEvent;
        public event TouchDelegate pinchInEvent;
        public event TouchDelegate pinchOutEvent;
        public event TouchDelegate longTapEvent;
        public event TouchDelegate simpleTapEvent;
        static List<RaycastResult> results = new List<RaycastResult>();


        public static bool IsTouchOnUI
        {
            get
            {
#if UNITY_EDITOR
                if (EventSystem.current.IsPointerOverGameObject())
                    return true;
                else
                    return false;
#else
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            results.Clear();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
#endif
            }
        }

        void Awake()
        {
            EasyTouch.On_TouchStart += OnTouchStart;
            EasyTouch.On_TouchUp += OnTouchUp;
            EasyTouch.On_TouchDown += OnTouchDown;
            EasyTouch.On_SwipeStart += OnSwipeStart;
            EasyTouch.On_Swipe += OnSwipe;
            EasyTouch.On_SwipeEnd += OnSwipeEnd;//手指滑动结束 

            EasyTouch.On_PinchIn += OnPinchIn;//两个指头滑动缩小
            EasyTouch.On_PinchOut += OnPinchOut;//两个指头滑动放大
                                                //EasyTouch.On_DragStart += On_DragStart;
                                                //EasyTouch.On_Drag += On_Drag;
            EasyTouch.On_TouchStart2Fingers += OnTouchStart2Fingers;//两个指头触摸
                                                                    //EasyTouch.On_TouchDown2Fingers += On_TouchDown2Fingers;
            EasyTouch.On_TouchUp2Fingers += OnTouchUp2Fingers;
            //EasyTouch.On_SwipeStart2Fingers += On_SwipeStart2Fingers;//两个指头滑动开始
            //EasyTouch.On_Swipe2Fingers += On_Swipe2Fingers;//两个指头滑动
            //EasyTouch.On_SwipeEnd2Fingers += On_SwipeEnd2Fingers;//两个指头滑动结束
            //EasyTouch.On_SimpleTap2Fingers += On_SimpleTap2Fingers;//两个指头轻敲
            //EasyTouch.On_Twist += On_Twist;
            //EasyTouch.On_Cancel2Fingers += On_Cancel2Fingers;
            EasyTouch.On_SimpleTap += OnSimpleTap;//轻敲事件
                                                  //EasyTouch.On_DoubleTap += On_DoubleTap;//双击时间
            EasyTouch.On_LongTapStart += OnLongTapStart;//长按事件
        }

        void OnLongTapStart(Gesture gesture)
        {
            if (IsTouchOnUI || gesture.touchCount > 1)
                return;
            if (longTapEvent != null)
                longTapEvent(gesture);
        }

        void OnTouchStart2Fingers(Gesture gesture)
        {
            if (IsTouchOnUI)
                return;
            if (start2FingersEvent != null)
                start2FingersEvent(gesture);
        }

        void OnTouchUp2Fingers(Gesture gesture)
        {
            if (IsTouchOnUI)
                return;
            if (up2FingersEvent != null)
                up2FingersEvent(gesture);
        }

        public void OnTouchStart(Gesture gesture)
        {
            if (gesture.touchCount > 1)
                return;
            if (touchStartEvent != null)
                touchStartEvent(gesture);
        }

        public void OnTouchUp(Gesture gesture)
        {
            if (gesture.touchCount > 1)
                return;
            //if (IsTouchOnUI)
            //    return;
            if (touchUpEvent != null)
                touchUpEvent(gesture);
        }
        public void OnTouchDown(Gesture gesture)
        {
            if (gesture.touchCount > 1)
                return;
            if (IsTouchOnUI)
                return;
            if (touchDownEvent != null)
                touchDownEvent(gesture);
        }
        private void OnSwipeStart(Gesture gesture)
        {
            if (swipeStartEvent != null)
                swipeStartEvent(gesture);
        }

        private void OnSwipe(Gesture gesture)
        {
            if (gesture.position == gesture.startPosition)
                return;
            if (swipeEvent != null)
                swipeEvent(gesture);
        }

        private void OnSwipeEnd(Gesture gesture)
        {
            //if (gesture.touchCount > 1)
            //    return;

            if (swipeEndEvent != null)
                swipeEndEvent(gesture);
        }

        private void OnPinchIn(Gesture gesture)
        {
            //if (IsTouchOnUI)
            //    return;
            if (pinchInEvent != null)
                pinchInEvent(gesture);
        }

        private void OnSimpleTap(Gesture gesture)
        {
            if (simpleTapEvent != null)
                simpleTapEvent(gesture);
        }

        private void OnPinchOut(Gesture gesture)
        {
            //if (IsTouchOnUI)
            //    return;
            if (pinchOutEvent != null)
                pinchOutEvent(gesture);
        }
    }
}

*/