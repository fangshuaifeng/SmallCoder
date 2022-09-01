using System;
using System.Windows.Forms;

namespace SmallCoder
{
    internal class HotKeyManager
    {
        /// <summary>
        /// Forms.Control 绑定热键
        /// </summary>
        public static void AddFormControlHotKey<TControl>(TControl control, Action function, Keys key, bool ctrl = false, bool shift = false, bool alt = false) where TControl : System.Windows.Forms.Control
        {
            control.KeyDown += delegate (object sender, System.Windows.Forms.KeyEventArgs e)
            {
                if (IsHotkey(e, key, ctrl, shift, alt))
                {
                    function();
                }
            };
        }

        private static bool IsHotkey(KeyEventArgs eventData, Keys key, bool ctrl = false, bool shift = false, bool alt = false)
        {
            return eventData.KeyCode == key && eventData.Control == ctrl && eventData.Shift == shift && eventData.Alt == alt;
        }

        /// <summary>
        /// Controls.Control 绑定热键
        /// </summary>
        public static void AddControlHotKey<TControl>(TControl control, Action function, System.Windows.Input.Key key, bool ctrl = false, bool shift = false, bool alt = false) where TControl : System.Windows.Controls.Control
        {
            control.KeyDown += delegate (object sender, System.Windows.Input.KeyEventArgs e)
            {
                if (IsHotkey2(e, key, ctrl, shift, alt))
                {
                    function();
                }
            };
        }

        private static bool IsHotkey2(System.Windows.Input.KeyEventArgs eventData, System.Windows.Input.Key key, bool ctrl = false, bool shift = false, bool alt = false)
        {
            var isControl = (System.Windows.Input.Keyboard.Modifiers & System.Windows.Input.ModifierKeys.Control) == System.Windows.Input.ModifierKeys.Control;
            var isShift = (System.Windows.Input.Keyboard.Modifiers & System.Windows.Input.ModifierKeys.Shift) == System.Windows.Input.ModifierKeys.Shift;
            var isAlter = (System.Windows.Input.Keyboard.Modifiers & System.Windows.Input.ModifierKeys.Alt) == System.Windows.Input.ModifierKeys.Alt;
            return System.Windows.Input.Keyboard.IsKeyDown(key) && isControl == ctrl && isShift == shift && isAlter == alt;
        }
    }
}
