using System.Collections.Generic;

namespace net.minecraft.src
{
	public class KeyBinding
	{
        public static List<KeyBinding> KeybindArray = new List<KeyBinding>();
		public static IntHashMap Hash = new IntHashMap();
		public string KeyDescription;
		public int KeyCode;

		/// <summary>
		/// because _303 wanted me to call it that(Caironater) </summary>
		public bool Pressed;
		public int PressTime;

		public static void OnTick(int par0)
		{
			KeyBinding keybinding = (KeyBinding)Hash.Lookup(par0);

			if (keybinding != null)
			{
				keybinding.PressTime++;
			}
		}

		public static void SetKeyBindState(int par0, bool par1)
		{
			KeyBinding keybinding = (KeyBinding)Hash.Lookup(par0);

			if (keybinding != null)
			{
				keybinding.Pressed = par1;
			}
		}

		public static void UnPressAllKeys()
		{
			KeyBinding keybinding;

			for (IEnumerator<KeyBinding> iterator = KeybindArray.GetEnumerator(); iterator.MoveNext(); keybinding.UnpressKey())
			{
				keybinding = iterator.Current;
			}
		}

		public static void ResetKeyBindingArrayAndHash()
		{
			Hash.ClearMap();
			KeyBinding keybinding;

			for (IEnumerator<KeyBinding> iterator = KeybindArray.GetEnumerator(); iterator.MoveNext(); Hash.AddKey(keybinding.KeyCode, keybinding))
			{
				keybinding = iterator.Current;
			}
		}

		public KeyBinding(string par1Str, int par2)
		{
			PressTime = 0;
			KeyDescription = par1Str;
			KeyCode = par2;
			KeybindArray.Add(this);
			Hash.AddKey(par2, this);
		}

		public virtual bool IsPressed()
		{
			if (PressTime == 0)
			{
				return false;
			}
			else
			{
				PressTime--;
				return true;
			}
		}

		private void UnpressKey()
		{
			PressTime = 0;
			Pressed = false;
		}
	}
}