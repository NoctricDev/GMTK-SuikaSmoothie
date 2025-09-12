using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Dev.Joshua
{
    public class CustomDropdown : TMP_Dropdown
    {
        private GameObject dropDown;
        protected override GameObject CreateDropdownList(GameObject template)
        {
            dropDown = base.CreateDropdownList(template);
            return dropDown;
        }

        protected override GameObject CreateBlocker(Canvas rootCanvas)
        {
            
            RectTransform dropDownRect = dropDown.GetComponent<RectTransform>();
            RectTransform viewPort = dropDown.GetComponent<ScrollRect>().viewport;
            float heightOffset = viewPort.offsetMin.y - viewPort.offsetMax.y;
            dropDownRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, dropDownRect.rect.height + heightOffset);
            return base.CreateBlocker(rootCanvas);
        }
    }
    
}
