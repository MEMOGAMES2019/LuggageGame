using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    class PanelInfo : MonoBehaviour
    {
        [SerializeField]
        public GameObject _panelInfo;

        public GameObject PanelInfor { get => _panelInfo; set => _panelInfo = value; }

        public Text TextPanelInfo { get; set; }

        void Start()
        {
            TextPanelInfo = PanelInfor.GetComponentInChildren<Text>();
        }

        private void OnMouseEnter()
        {
            if (!Input.GetMouseButtonUp(0))
            {
                PanelInfor.SetActive(true);
                TextPanelInfo.text = name;
            }
        }
        private void OnMouseExit()
        {
            PanelInfor.SetActive(false);
        }
        private void OnMouseDrag()
        {
            PanelInfor.SetActive(false);
        }
    }
}
