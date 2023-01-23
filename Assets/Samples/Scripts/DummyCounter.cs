using TMPro;
using UnityEngine;

namespace Samples.Scripts
{
    public class DummyCounter : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text label;

        private float i;
        // Update is called once per frame
        void Update()
        {
            i += Time.deltaTime;
            label.text = i.ToString("00");
        }
    }
}
