using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GreenerGames
{
    public class ImprovedSlider : Slider, IEndDragHandler
    {
        public UnityAction<float> onDragEnd;
        
        public void OnEndDrag(PointerEventData eventData)
        {
            onDragEnd?.Invoke(value);
        }
    }
}