#region

using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#endregion

namespace GreenerGames
{
    public class ImprovedButton : Button
    {
        TextMeshProUGUI buttonText;

        public Action onButtonClicked;
        public Action onButtonDoubleClick;
        public Action onHoldClick;


        public Action onPointerUp;
        public Action onPointerDown;
        public Action onPointerEnter;
        public Action onPointerExit;
        
        
        Coroutine holdCheck;
        bool holdCheckFired;
        float holdClickTime = 1;
        
        int clickCount;
        readonly float interval = 0.5f;
        bool readyForDoubleTap;
        Coroutine doubleChecker;

        /// <summary>
        ///     Clear all actions assigned on a button
        /// </summary>
        public void Clear()
        {
            onButtonClicked = null;
            onPointerUp = null;
            onPointerDown = null;
            onPointerEnter = null;
            onPointerExit = null;
        }

        public void SetButtonText(string text)
        {
            if (buttonText == null)
            {
                buttonText = GetComponentInChildren<TextMeshProUGUI>(true);
            }

            buttonText.text = text;
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            onPointerEnter?.Invoke();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            onPointerExit?.Invoke();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            onPointerUp?.Invoke();

            if (holdCheck != null)
            {
                StopCoroutine(holdCheck);
            }
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            onPointerDown?.Invoke();

            holdCheckFired = false;
            holdCheck = StartCoroutine(HoldCheck());
        }

        
        IEnumerator HoldCheck()
        {
            yield return new WaitForSeconds(holdClickTime);
            
            // Here type the code for what you want to happen at the end of the long press
            onHoldClick?.Invoke();
            holdCheck = null;
            holdCheckFired = true;
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            if (holdCheckFired)
            {
                return;
            }
            if (holdCheck != null)
            {
                StopCoroutine(holdCheck);
            }
            
            clickCount++;

            if (clickCount == 1)
            {
                //do stuff
                readyForDoubleTap = true;
                doubleChecker = StartCoroutine(DoubleTapInterval());
            }

            else if (clickCount > 1 && readyForDoubleTap)
            {
                //do stuff
                readyForDoubleTap = false;
                onButtonDoubleClick?.Invoke();
                clickCount = 0;
                StopCoroutine(doubleChecker);
            }
        }

        IEnumerator DoubleTapInterval()
        {
            yield return new WaitForSeconds(interval);
            readyForDoubleTap = false;
            clickCount = 0;
            
            onButtonClicked?.Invoke();
        }
    }
}