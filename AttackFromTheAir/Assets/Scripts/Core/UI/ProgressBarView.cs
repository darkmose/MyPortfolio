using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{

    public class ProgressBarView : MonoBehaviour
    {
        [SerializeField] private Image _value;

        public void SetValue(float value) 
        {
            _value.fillAmount = value;
        }
    }
}