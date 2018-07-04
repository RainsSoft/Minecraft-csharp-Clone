using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public abstract class GuiContainer : GuiScreen
	{
		/// <summary>
		/// Stacks renderer. Icons, stack size, health, etc... </summary>
		protected static RenderItem ItemRenderer = new RenderItem();

		/// <summary>
		/// The X size of the inventory window in pixels. </summary>
		protected int XSize;

		/// <summary>
		/// The Y size of the inventory window in pixels. </summary>
		protected int YSize;

		/// <summary>
		/// A list of the players inventory slots. </summary>
		public Container InventorySlots;

		/// <summary>
		/// Starting X position for the Gui. Inconsistent use for Gui backgrounds.
		/// </summary>
		protected int GuiLeft;

		/// <summary>
		/// Starting Y position for the Gui. Inconsistent use for Gui backgrounds.
		/// </summary>
		protected int GuiTop;

		public GuiContainer(Container par1Container)
		{
			XSize = 176;
			YSize = 166;
			InventorySlots = par1Container;
		}

		/// <summary>
		/// Adds the buttons (and other controls) to the screen in question.
		/// </summary>
		public override void InitGui()
		{
			base.InitGui();
			Mc.ThePlayer.CraftingInventory = InventorySlots;
			GuiLeft = (Width - XSize) / 2;
			GuiTop = (Height - YSize) / 2;
		}

		/// <summary>
		/// Draws the screen and all the components in it.
		/// </summary>
		public override void DrawScreen(int par1, int par2, float par3)
		{
			DrawDefaultBackground();
			int i = GuiLeft;
			int j = GuiTop;
			DrawGuiContainerBackgroundLayer(par3, par1, par2);
			RenderHelper.EnableGUIStandardItemLighting();
			//GL.PushMatrix();
			//GL.Translate(i, j, 0.0F);
			//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
			//GL.Enable(EnableCap.RescaleNormal);
			Slot slot = null;
			int k = 240;
			int i1 = 240;
			OpenGlHelper.SetLightmapTextureCoords(OpenGlHelper.LightmapTexUnit, (float)k / 1.0F, (float)i1 / 1.0F);
			//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);

			for (int l = 0; l < InventorySlots.InventorySlots.Count; l++)
			{
				Slot slot1 = (Slot)InventorySlots.InventorySlots[l];
				DrawSlotInventory(slot1);

				if (IsMouseOverSlot(slot1, par1, par2))
				{
					slot = slot1;
					//GL.Disable(EnableCap.Lighting);
					//GL.Disable(EnableCap.DepthTest);
					int j1 = slot1.XDisplayPosition;
					int k1 = slot1.YDisplayPosition;
					DrawGradientRect(j1, k1, j1 + 16, k1 + 16, 0x80fffff, 0x80fffff);
					//GL.Enable(EnableCap.Lighting);
					//GL.Enable(EnableCap.DepthTest);
				}
			}

			DrawGuiContainerForegroundLayer();
			InventoryPlayer inventoryplayer = Mc.ThePlayer.Inventory;

			if (inventoryplayer.GetItemStack() != null)
			{
				//GL.Translate(0.0F, 0.0F, 32F);
				ZLevel = 200F;
				ItemRenderer.ZLevel = 200F;
				ItemRenderer.RenderItemIntoGUI(FontRenderer, Mc.RenderEngineOld, inventoryplayer.GetItemStack(), par1 - i - 8, par2 - j - 8);
				ItemRenderer.RenderItemOverlayIntoGUI(FontRenderer, Mc.RenderEngineOld, inventoryplayer.GetItemStack(), par1 - i - 8, par2 - j - 8);
				ZLevel = 0.0F;
				ItemRenderer.ZLevel = 0.0F;
			}

			//GL.Disable(EnableCap.RescaleNormal);
			RenderHelper.DisableStandardItemLighting();
			//GL.Disable(EnableCap.Lighting);
			//GL.Disable(EnableCap.DepthTest);

			if (inventoryplayer.GetItemStack() == null && slot != null && slot.GetHasStack())
			{
				ItemStack itemstack = slot.GetStack();
                List<string> list = itemstack.GetItemNameandInformation();

				if (list.Count > 0)
				{
					int l1 = 0;

					for (int i2 = 0; i2 < list.Count; i2++)
					{
						int k2 = FontRenderer.GetStringWidth(list[i2]);

						if (k2 > l1)
						{
							l1 = k2;
						}
					}

					int j2 = (par1 - i) + 12;
					int l2 = par2 - j - 12;
					int i3 = l1;
					int j3 = 8;

					if (list.Count > 1)
					{
						j3 += 2 + (list.Count - 1) * 10;
					}

					ZLevel = 300F;
					ItemRenderer.ZLevel = 300F;
					int k3 = 0xf010001;
					DrawGradientRect(j2 - 3, l2 - 4, j2 + i3 + 3, l2 - 3, k3, k3);
					DrawGradientRect(j2 - 3, l2 + j3 + 3, j2 + i3 + 3, l2 + j3 + 4, k3, k3);
					DrawGradientRect(j2 - 3, l2 - 3, j2 + i3 + 3, l2 + j3 + 3, k3, k3);
					DrawGradientRect(j2 - 4, l2 - 3, j2 - 3, l2 + j3 + 3, k3, k3);
					DrawGradientRect(j2 + i3 + 3, l2 - 3, j2 + i3 + 4, l2 + j3 + 3, k3, k3);
					int l3 = 0x505000ff;
					int i4 = (l3 & 0xfefefe) >> 1 | l3 & 0xff00000;
					DrawGradientRect(j2 - 3, (l2 - 3) + 1, (j2 - 3) + 1, (l2 + j3 + 3) - 1, l3, i4);
					DrawGradientRect(j2 + i3 + 2, (l2 - 3) + 1, j2 + i3 + 3, (l2 + j3 + 3) - 1, l3, i4);
					DrawGradientRect(j2 - 3, l2 - 3, j2 + i3 + 3, (l2 - 3) + 1, l3, l3);
					DrawGradientRect(j2 - 3, l2 + j3 + 2, j2 + i3 + 3, l2 + j3 + 3, i4, i4);

					for (int j4 = 0; j4 < list.Count; j4++)
					{
						string s = list[j4];

						if (j4 == 0)
						{
							//s = (new StringBuilder()).Append((char)0xa7).Append(int.ToHexString(itemstack.GetRarity().NameColor)).Append(s).ToString();
						}
						else
						{
							s = (new StringBuilder()).Append((char)0xa7).Append(s).ToString();
						}

						FontRenderer.DrawStringWithShadow(s, j2, l2, -1);

						if (j4 == 0)
						{
							l2 += 2;
						}

						l2 += 10;
					}

					ZLevel = 0.0F;
					ItemRenderer.ZLevel = 0.0F;
				}
			}

			//GL.PopMatrix();
			base.DrawScreen(par1, par2, par3);
			//GL.Enable(EnableCap.Lighting);
			//GL.Enable(EnableCap.DepthTest);
		}

		/// <summary>
		/// Draw the foreground layer for the GuiContainer (everythin in front of the items)
		/// </summary>
		protected virtual void DrawGuiContainerForegroundLayer()
		{
		}

		/// <summary>
		/// Draw the background layer for the GuiContainer (everything behind the items)
		/// </summary>
		protected abstract void DrawGuiContainerBackgroundLayer(float f, int i, int j);

		/// <summary>
		/// Draws an inventory slot
		/// </summary>
		private void DrawSlotInventory(Slot par1Slot)
		{
			int i = par1Slot.XDisplayPosition;
			int j = par1Slot.YDisplayPosition;
			ItemStack itemstack = par1Slot.GetStack();
			bool flag = false;
			int k = i;
			int l = j;
			ZLevel = 100F;
			ItemRenderer.ZLevel = 100F;

			if (itemstack == null)
			{
				int i1 = par1Slot.GetBackgroundIconIndex();

				if (i1 >= 0)
				{
					//GL.Disable(EnableCap.Lighting);
					Mc.RenderEngineOld.BindTexture(Mc.RenderEngineOld.GetTexture("/gui/items.png"));
					DrawTexturedModalRect(k, l, (i1 % 16) * 16, (i1 / 16) * 16, 16, 16);
					//GL.Enable(EnableCap.Lighting);
					flag = true;
				}
			}

			if (!flag)
			{
				ItemRenderer.RenderItemIntoGUI(FontRenderer, Mc.RenderEngineOld, itemstack, k, l);
				ItemRenderer.RenderItemOverlayIntoGUI(FontRenderer, Mc.RenderEngineOld, itemstack, k, l);
			}

			ItemRenderer.ZLevel = 0.0F;
			ZLevel = 0.0F;
		}

		/// <summary>
		/// Returns the slot at the given coordinates or null if there is none.
		/// </summary>
		private Slot GetSlotAtPosition(int par1, int par2)
		{
			for (int i = 0; i < InventorySlots.InventorySlots.Count; i++)
			{
				Slot slot = (Slot)InventorySlots.InventorySlots[i];

				if (IsMouseOverSlot(slot, par1, par2))
				{
					return slot;
				}
			}

			return null;
		}

		/// <summary>
		/// Called when the mouse is clicked.
		/// </summary>
		protected override void MouseClicked(int par1, int par2, int par3)
		{
			base.MouseClicked(par1, par2, par3);

			if (par3 == 0 || par3 == 1)
			{
				Slot slot = GetSlotAtPosition(par1, par2);
				int i = GuiLeft;
				int j = GuiTop;
				bool flag = par1 < i || par2 < j || par1 >= i + XSize || par2 >= j + YSize;
				int k = -1;

				if (slot != null)
				{
					k = slot.SlotNumber;
				}

				if (flag)
				{
					k = -999;
				}

				if (k != -1)
				{
					bool flag1 = k != -999;// && (Keyboard.isKeyDown(42) || Keyboard.isKeyDown(54));
					HandleMouseClick(slot, k, par3, flag1);
				}
			}
		}

		/// <summary>
		/// Returns if the passed mouse position is over the specified slot.
		/// </summary>
		private bool IsMouseOverSlot(Slot par1Slot, int par2, int par3)
		{
			int i = GuiLeft;
			int j = GuiTop;
			par2 -= i;
			par3 -= j;
			return par2 >= par1Slot.XDisplayPosition - 1 && par2 < par1Slot.XDisplayPosition + 16 + 1 && par3 >= par1Slot.YDisplayPosition - 1 && par3 < par1Slot.YDisplayPosition + 16 + 1;
		}

		protected virtual void HandleMouseClick(Slot par1Slot, int par2, int par3, bool par4)
		{
			if (par1Slot != null)
			{
				par2 = par1Slot.SlotNumber;
			}

			Mc.PlayerController.WindowClick(InventorySlots.WindowId, par2, par3, par4, Mc.ThePlayer);
		}

		/// <summary>
		/// Fired when a key is typed. This is the equivalent of KeyListener.keyTyped(KeyEvent e).
		/// </summary>
		protected override void KeyTyped(char par1, int par2)
		{
			if (par2 == 1 || par2 == Mc.GameSettings.KeyBindInventory.KeyCode)
			{
				Mc.ThePlayer.CloseScreen();
			}
		}

		/// <summary>
		/// Called when the screen is unloaded. Used to disable keyboard repeat events
		/// </summary>
		public override void OnGuiClosed()
		{
			if (Mc.ThePlayer == null)
			{
				return;
			}
			else
			{
				InventorySlots.OnCraftGuiClosed(Mc.ThePlayer);
				Mc.PlayerController.Func_20086_a(InventorySlots.WindowId, Mc.ThePlayer);
				return;
			}
		}

		/// <summary>
		/// Returns true if this GUI should pause the game when it is displayed in single-player
		/// </summary>
		public override bool DoesGuiPauseGame()
		{
			return false;
		}

		/// <summary>
		/// Called from the main game loop to update the screen.
		/// </summary>
		public override void UpdateScreen()
		{
			base.UpdateScreen();

			if (!Mc.ThePlayer.IsEntityAlive() || Mc.ThePlayer.IsDead)
			{
				Mc.ThePlayer.CloseScreen();
			}
		}
	}
}