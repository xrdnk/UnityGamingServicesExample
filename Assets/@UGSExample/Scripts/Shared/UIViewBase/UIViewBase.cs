using UnityEngine.EventSystems;

namespace Shared.UIViewBase
{
    public abstract class UIViewBase : UIBehaviour
    {
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}