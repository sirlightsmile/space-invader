using System.Threading.Tasks;
using UnityEngine;

namespace SmileProject.SpaceInvader.UI
{
    public abstract class BaseUIComponent : MonoBehaviour
    {
        private const string HideAnimState = "Hide";

        [SerializeField]
        private Animator _animator;

        /// <summary>
        /// Show UI and auto hide after show time
        /// </summary>
        /// <param name="showTime">show time in milliseconds</param>
        /// <returns></returns>
        public async virtual void Show(int showTime)
        {
            SetActive(true);
            await Task.Delay(showTime);
            Hide();
        }

        /// <summary>
        /// Set UI active status
        /// </summary>
        /// <param name="isActive">is active</param>
        public virtual void SetActive(bool isActive)
        {
            this.gameObject?.SetActive(isActive);
        }

        /// <summary>
        /// Show UI instant
        /// </summary>
        public virtual void Show()
        {
            SetActive(true);
        }

        /// <summary>
        /// Hide UI with or without animation
        /// </summary>
        /// <param name="isForce">if false, will trigger hide animation</param>
        public virtual void Hide(bool isForce = false)
        {
            if (!isForce && _animator != null)
            {
                _animator.SetTrigger(HideAnimState);
            }
            else
            {
                SetActive(false);
            }
        }

        /// <summary>
        /// Trigger by hide animation
        /// </summary>
        private void OnHide()
        {
            SetActive(false);
        }
    }
}