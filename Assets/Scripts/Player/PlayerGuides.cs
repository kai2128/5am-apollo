using TMPro;
using UnityEngine;

namespace Player
{
    public class PlayerGuides : MonoBehaviour
    {
        // Start is called before the first frame update
        public TextMeshProUGUI guideText;
        public Canvas guideCanvas;
        
        private bool addedSwordText;
        private string swordText = "\nQ to switch weapons \nFor sword, Z to attack, X to ranged attack, Z + S in middle air to perform ground attack\n";
        
        private bool addedFlyText;
        private string flyText = "\nHold C to Fly\n";
        
        private bool addedGrowShrinkText;
        private string growShrinkText = "\nV to Grow, B to Shrinks\n";
        
        void Start()
        {
            guideText = GetComponent<TextMeshProUGUI>();
            guideCanvas = GetComponentInParent<Canvas>();
        }

        // Update is called once per frame
        void Update()
        {
            CheckTextAdded(PlayerManager.Instance.unlockedSword, ref addedSwordText, swordText, 1);
            CheckTextAdded(PlayerManager.Instance.unlockedFly, ref addedFlyText, flyText, 1);
            CheckTextAdded(PlayerManager.Instance.unlockedGrowShrink, ref addedGrowShrinkText, growShrinkText, 1);
        }

        private void CheckTextAdded(bool playerUnlockable, ref bool flag, string text, float height)
        {
            if (!flag && playerUnlockable )
            {
                guideText.text += text;
                guideCanvas.transform.position += Vector3.up * height;  
                flag = true;
            }
        } 
    }
}
