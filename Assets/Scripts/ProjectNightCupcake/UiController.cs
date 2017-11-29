using UnityEngine;
using UnityEngine.UI;

namespace projectnightcupcake
{
    [RequireComponent(typeof(Canvas))]
    public class UiController : MonoBehaviour
    {
        [SerializeField]
        private Text _itemDescriptionText;
        public Text ItemDescriptionText { get { return _itemDescriptionText;  } private set { _itemDescriptionText = value; } }

        public static UiController Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        public void SetItemDescription(string description)
        {
            ItemDescriptionText.text = description;            
        }
    }
}
