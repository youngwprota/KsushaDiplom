using UnityEngine;

namespace Sort
{
    public class SortGameManager : MonoBehaviour
    {
        public static SortGameManager Instance;
        public GameObject successPanel;
        private int totalItems;
        private int correctItems = 0;

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            totalItems = FindObjectsOfType<DraggableItem>().Length;
            successPanel.SetActive(false);
        }

        public void RegisterCorrectItem()
        {
            correctItems++;
            if (correctItems >= totalItems)
            {
                successPanel.SetActive(true);
                Debug.Log("All items sorted!");
            }
        }
    }
}