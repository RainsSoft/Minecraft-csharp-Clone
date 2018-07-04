namespace net.minecraft.src
{
	using net.minecraft.src;

	public class GuiSleepMP : GuiChat
	{
		public GuiSleepMP()
		{
		}

		/// <summary>
		/// Adds the buttons (and other controls) to the screen in question.
		/// </summary>
		public override void InitGui()
		{
			base.InitGui();
			StringTranslate stringtranslate = StringTranslate.GetInstance();
			ControlList.Add(new GuiButton(1, Width / 2 - 100, Height - 40, stringtranslate.TranslateKey("multiplayer.stopSleeping")));
		}

		/// <summary>
		/// Fired when a key is typed. This is the equivalent of KeyListener.keyTyped(KeyEvent e).
		/// </summary>
		protected override void KeyTyped(char par1, int par2)
		{
			if (par2 == 1)
			{
				WakeEntity();
			}
			else if (par2 == 28)
			{
				string s = Field_50064_a.GetText().Trim();

				if (s.Length > 0)
				{
					Mc.ThePlayer.SendChatMessage(s);
				}

				Field_50064_a.SetText("");
				Mc.IngameGUI.Func_50014_d();
			}
			else
			{
				base.KeyTyped(par1, par2);
			}
		}

		/// <summary>
		/// Fired when a control is clicked. This is the equivalent of ActionListener.actionPerformed(ActionEvent e).
		/// </summary>
		protected override void ActionPerformed(GuiButton par1GuiButton)
		{
			if (par1GuiButton.Id == 1)
			{
				WakeEntity();
			}
			else
			{
				base.ActionPerformed(par1GuiButton);
			}
		}

		/// <summary>
		/// Wakes the entity from the bed
		/// </summary>
		private void WakeEntity()
		{
			if (Mc.ThePlayer is EntityClientPlayerMP)
			{
				NetClientHandler netclienthandler = ((EntityClientPlayerMP)Mc.ThePlayer).SendQueue;
				netclienthandler.AddToSendQueue(new Packet19EntityAction(Mc.ThePlayer, 3));
			}
		}
	}
}