using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nekoyume
{
    /// <summary>
    /// Customed Debug Panel for Android Dev
    /// </summary>
    [RequireComponent(typeof(Text)), DisallowMultipleComponent]
    public class DebugPanel : MonoBehaviour
    {
        private static Text TextInstance = null;
        private static ScrollRect ScrollRectInstance = null;
        private static Toggle ScrollLockToggle = null;
        private static Queue<string> messages = new Queue<string>();
        private static int MaxQueueSize = 64;
        void Start()
        {
            TextInstance = this.GetComponent<Text>();
            ScrollRectInstance = GetComponentInParent<ScrollRect>();
            ScrollLockToggle = transform.parent.parent.parent.gameObject.GetComponentInChildren<Toggle>();
            Log("debug panel initialized, \nwait for log");
        }

        void Update()
        {
            // Lock the scroll to the bottom
            if (ScrollLockToggle != null && ScrollLockToggle.isOn == true)
            {
                ScrollRectInstance.verticalScrollbar.value = 0;
            }
        }

        /// <summary>
        /// Output log to screen debug panel
        /// </summary>
        /// <param name="message">"\n" is valid</param>
        static public void Log(string message)
        {
            if (TextInstance == null || ScrollRectInstance == null)
            {
                return;
            }

            messages.Enqueue("[row]" + message);
            if (messages.Count > MaxQueueSize)
            {
                messages.Dequeue();
            }
            string[] rows = messages.ToArray();
            string finalText = "";
            foreach (string row in rows)
            {
                finalText += row;
                finalText += "\n";
            }
            finalText += new string('-', 32);
            TextInstance.text = finalText;
        }
    }
}
