using System;
using System.Text;

namespace net.minecraft.src
{
	using net.minecraft.src;

	public class StatStringFormatKeyInv : IStatStringFormat
	{
		/// <summary>
		/// Minecraft instance </summary>
		readonly Minecraft Mc;

		public StatStringFormatKeyInv(Minecraft par1Minecraft)
		{
			Mc = par1Minecraft;
		}

		/// <summary>
		/// Formats the strings based on 'IStatStringFormat' interface.
		/// </summary>
		public virtual string FormatString(string par1Str)
		{
			try
			{
				return string.Format(par1Str, new object[] { GameSettings.GetKeyDisplayString(Mc.GameSettings.KeyBindInventory.KeyCode) });
			}
			catch (Exception exception)
			{
				return (new StringBuilder()).Append("Error: ").Append(exception.Message).ToString();
			}
		}
	}
}