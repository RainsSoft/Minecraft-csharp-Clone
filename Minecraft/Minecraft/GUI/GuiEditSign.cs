using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public class GuiEditSign : GuiScreen
	{
		/// <summary>
		/// This String is just a local copy of the characters allowed in text rendering of minecraft.
		/// </summary>
		private static readonly string AllowedCharacters;

		/// <summary>
		/// The title string that is displayed in the top-center of the screen. </summary>
		protected string ScreenTitle;

		/// <summary>
		/// Reference to the sign object. </summary>
		private TileEntitySign EntitySign;

		/// <summary>
		/// Counts the number of screen updates. </summary>
		private int UpdateCounter;

		/// <summary>
		/// The number of the line that is being edited. </summary>
		private int EditLine;

		public GuiEditSign(TileEntitySign par1TileEntitySign)
		{
			ScreenTitle = "Edit sign message:";
			EditLine = 0;
			EntitySign = par1TileEntitySign;
		}

		/// <summary>
		/// Adds the buttons (and other controls) to the screen in question.
		/// </summary>
		public override void InitGui()
		{
			ControlList.Clear();
			//Keyboard.enableRepeatEvents(true);
			ControlList.Add(new GuiButton(0, Width / 2 - 100, Height / 4 + 120, "Done"));
			EntitySign.Func_50006_a(false);
		}

		/// <summary>
		/// Called when the screen is unloaded. Used to disable keyboard repeat events
		/// </summary>
		public override void OnGuiClosed()
		{
			//Keyboard.enableRepeatEvents(false);

			if (Mc.TheWorld.IsRemote)
			{
				Mc.GetSendQueue().AddToSendQueue(new Packet130UpdateSign(EntitySign.XCoord, EntitySign.YCoord, EntitySign.ZCoord, EntitySign.SignText));
			}

			EntitySign.Func_50006_a(true);
		}

		/// <summary>
		/// Called from the main game loop to update the screen.
		/// </summary>
		public override void UpdateScreen()
		{
			UpdateCounter++;
		}

		/// <summary>
		/// Fired when a control is clicked. This is the equivalent of ActionListener.actionPerformed(ActionEvent e).
		/// </summary>
		protected override void ActionPerformed(GuiButton par1GuiButton)
		{
			if (!par1GuiButton.Enabled)
			{
				return;
			}

			if (par1GuiButton.Id == 0)
			{
				EntitySign.OnInventoryChanged();
				Mc.DisplayGuiScreen(null);
			}
		}

		/// <summary>
		/// Fired when a key is typed. This is the equivalent of KeyListener.keyTyped(KeyEvent e).
		/// </summary>
		protected override void KeyTyped(char par1, int par2)
		{
			if (par2 == 200)
			{
				EditLine = EditLine - 1 & 3;
			}

			if (par2 == 208 || par2 == 28)
			{
				EditLine = EditLine + 1 & 3;
			}

			if (par2 == 14 && EntitySign.SignText[EditLine].Length > 0)
			{
				EntitySign.SignText[EditLine] = EntitySign.SignText[EditLine].Substring(0, EntitySign.SignText[EditLine].Length - 1);
			}

			if (!(AllowedCharacters.IndexOf(par1) < 0 || EntitySign.SignText[EditLine].Length >= 15))
			{
				EntitySign.SignText[EditLine] += par1;
			}
		}

		/// <summary>
		/// Draws the screen and all the components in it.
		/// </summary>
		public override void DrawScreen(int par1, int par2, float par3)
		{
			DrawDefaultBackground();
			DrawCenteredString(FontRenderer, ScreenTitle, Width / 2, 40, 0xffffff);
			//GL.PushMatrix();
			//GL.Translate(Width / 2, 0.0F, 50F);
			float f = 93.75F;
			//GL.Scale(-f, -f, -f);
			//GL.Rotate(180F, 0.0F, 1.0F, 0.0F);
			Block block = EntitySign.GetBlockType();

			if (block == Block.SignPost)
			{
				float f1 = (float)(EntitySign.GetBlockMetadata() * 360) / 16F;
				//GL.Rotate(f1, 0.0F, 1.0F, 0.0F);
				//GL.Translate(0.0F, -1.0625F, 0.0F);
			}
			else
			{
				int i = EntitySign.GetBlockMetadata();
				float f2 = 0.0F;

				if (i == 2)
				{
					f2 = 180F;
				}

				if (i == 4)
				{
					f2 = 90F;
				}

				if (i == 5)
				{
					f2 = -90F;
				}

				//GL.Rotate(f2, 0.0F, 1.0F, 0.0F);
				//GL.Translate(0.0F, -1.0625F, 0.0F);
			}

			if ((UpdateCounter / 6) % 2 == 0)
			{
				EntitySign.LineBeingEdited = EditLine;
			}

			TileEntityRenderer.Instance.RenderTileEntityAt(EntitySign, -0.5F, -0.75F, -0.5F, 0.0F);
			EntitySign.LineBeingEdited = -1;
			//GL.PopMatrix();
			base.DrawScreen(par1, par2, par3);
		}

		static GuiEditSign()
		{
			AllowedCharacters = ChatAllowedCharacters.AllowedCharacters;
		}
	}
}