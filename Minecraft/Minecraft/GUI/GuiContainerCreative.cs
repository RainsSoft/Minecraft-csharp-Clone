namespace net.minecraft.src
{
    	using net.minecraft.src;
	using Microsoft.Xna.Framework;

	public class GuiContainerCreative : GuiContainer
	{
		private static InventoryBasic Inventory = new InventoryBasic("tmp", 72);

		/// <summary>
		/// Amount scrolled in Creative mode inventory (0 = top, 1 = bottom) </summary>
		private float CurrentScroll;

		/// <summary>
		/// True if the scrollbar is being dragged </summary>
		private bool IsScrolling;

		/// <summary>
		/// True if the left mouse button was held down last time drawScreen was called.
		/// </summary>
		private bool WasClicking;

		public GuiContainerCreative(EntityPlayer par1EntityPlayer) : base(new ContainerCreative(par1EntityPlayer))
		{
			CurrentScroll = 0.0F;
			IsScrolling = false;
			par1EntityPlayer.CraftingInventory = InventorySlots;
			AllowUserInput = true;
			par1EntityPlayer.AddStat(AchievementList.OpenInventory, 1);
			YSize = 208;
		}

		/// <summary>
		/// Called from the main game loop to update the screen.
		/// </summary>
		public override void UpdateScreen()
		{
			if (!Mc.PlayerController.IsInCreativeMode())
			{
				Mc.DisplayGuiScreen(new GuiInventory(Mc.ThePlayer));
			}
		}

		protected override void HandleMouseClick(Slot par1Slot, int par2, int par3, bool par4)
		{
			if (par1Slot != null)
			{
				if (par1Slot.Inventory == Inventory)
				{
					InventoryPlayer inventoryplayer = Mc.ThePlayer.Inventory;
					ItemStack itemstack1 = inventoryplayer.GetItemStack();
					ItemStack itemstack4 = par1Slot.GetStack();

					if (itemstack1 != null && itemstack4 != null && itemstack1.ItemID == itemstack4.ItemID)
					{
						if (par3 == 0)
						{
							if (par4)
							{
								itemstack1.StackSize = itemstack1.GetMaxStackSize();
							}
							else if (itemstack1.StackSize < itemstack1.GetMaxStackSize())
							{
								itemstack1.StackSize++;
							}
						}
						else if (itemstack1.StackSize <= 1)
						{
							inventoryplayer.SetItemStack(null);
						}
						else
						{
							itemstack1.StackSize--;
						}
					}
					else if (itemstack1 != null)
					{
						inventoryplayer.SetItemStack(null);
					}
					else if (itemstack4 == null)
					{
						inventoryplayer.SetItemStack(null);
					}
					else if (itemstack1 == null || itemstack1.ItemID != itemstack4.ItemID)
					{
						inventoryplayer.SetItemStack(ItemStack.CopyItemStack(itemstack4));
						ItemStack itemstack2 = inventoryplayer.GetItemStack();

						if (par4)
						{
							itemstack2.StackSize = itemstack2.GetMaxStackSize();
						}
					}
				}
				else
				{
					InventorySlots.SlotClick(par1Slot.SlotNumber, par3, par4, Mc.ThePlayer);
					ItemStack itemstack = InventorySlots.GetSlot(par1Slot.SlotNumber).GetStack();
					Mc.PlayerController.SendSlotPacket(itemstack, (par1Slot.SlotNumber - InventorySlots.InventorySlots.Count) + 9 + 36);
				}
			}
			else
			{
				InventoryPlayer inventoryplayer1 = Mc.ThePlayer.Inventory;

				if (inventoryplayer1.GetItemStack() != null)
				{
					if (par3 == 0)
					{
						Mc.ThePlayer.DropPlayerItem(inventoryplayer1.GetItemStack());
						Mc.PlayerController.Func_35639_a(inventoryplayer1.GetItemStack());
						inventoryplayer1.SetItemStack(null);
					}

					if (par3 == 1)
					{
						ItemStack itemstack3 = inventoryplayer1.GetItemStack().SplitStack(1);
						Mc.ThePlayer.DropPlayerItem(itemstack3);
						Mc.PlayerController.Func_35639_a(itemstack3);

						if (inventoryplayer1.GetItemStack().StackSize == 0)
						{
							inventoryplayer1.SetItemStack(null);
						}
					}
				}
			}
		}

		/// <summary>
		/// Adds the buttons (and other controls) to the screen in question.
		/// </summary>
		public override void InitGui()
		{
			if (!Mc.PlayerController.IsInCreativeMode())
			{
				Mc.DisplayGuiScreen(new GuiInventory(Mc.ThePlayer));
			}
			else
			{
				base.InitGui();
				ControlList.Clear();
			}
		}

		/// <summary>
		/// Draw the foreground layer for the GuiContainer (everythin in front of the items)
		/// </summary>
		protected override void DrawGuiContainerForegroundLayer()
		{
			FontRenderer.DrawString(StatCollector.TranslateToLocal("container.creative"), 8, 6, 0x404040);
		}

		/// <summary>
		/// Handles mouse input.
		/// </summary>
		public override void HandleMouseInput()
		{
			base.HandleMouseInput();
            int i = 0;// Mouse.getEventDWheel();

			if (i != 0)
			{
				int j = (((ContainerCreative)InventorySlots).ItemList.Count / 8 - 8) + 1;

				if (i > 0)
				{
					i = 1;
				}

				if (i < 0)
				{
					i = -1;
				}

				CurrentScroll -= (float)i / (float)j;

				if (CurrentScroll < 0.0F)
				{
					CurrentScroll = 0.0F;
				}

				if (CurrentScroll > 1.0F)
				{
					CurrentScroll = 1.0F;
				}

				((ContainerCreative)InventorySlots).ScrollTo(CurrentScroll);
			}
		}

		/// <summary>
		/// Draws the screen and all the components in it.
		/// </summary>
		public override void DrawScreen(int par1, int par2, float par3)
		{
            bool flag = false;// Mouse.isButtonDown(0);
			int i = GuiLeft;
			int j = GuiTop;
			int k = i + 155;
			int l = j + 17;
			int i1 = k + 14;
			int j1 = l + 160 + 2;

			if (!WasClicking && flag && par1 >= k && par2 >= l && par1 < i1 && par2 < j1)
			{
				IsScrolling = true;
			}

			if (!flag)
			{
				IsScrolling = false;
			}

			WasClicking = flag;

			if (IsScrolling)
			{
				CurrentScroll = (float)(par2 - (l + 8)) / ((float)(j1 - l) - 16F);

				if (CurrentScroll < 0.0F)
				{
					CurrentScroll = 0.0F;
				}

				if (CurrentScroll > 1.0F)
				{
					CurrentScroll = 1.0F;
				}

				((ContainerCreative)InventorySlots).ScrollTo(CurrentScroll);
			}

			base.DrawScreen(par1, par2, par3);
			//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
			//GL.Disable(EnableCap.Lighting);
		}

		/// <summary>
		/// Draw the background layer for the GuiContainer (everything behind the items)
		/// </summary>
		protected override void DrawGuiContainerBackgroundLayer(float par1, int par2, int par3)
		{
			//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
			int i = Mc.RenderEngineOld.GetTexture("/gui/allitems.png");
			Mc.RenderEngineOld.BindTexture(i);
			int j = GuiLeft;
			int k = GuiTop;
			DrawTexturedModalRect(j, k, 0, 0, XSize, YSize);
			int l = j + 155;
			int i1 = k + 17;
			int j1 = i1 + 160 + 2;
			DrawTexturedModalRect(j + 154, k + 17 + (int)((float)(j1 - i1 - 17) * CurrentScroll), 0, 208, 16, 16);
		}

		/// <summary>
		/// Fired when a control is clicked. This is the equivalent of ActionListener.actionPerformed(ActionEvent e).
		/// </summary>
		protected override void ActionPerformed(GuiButton par1GuiButton)
		{
			if (par1GuiButton.Id == 0)
			{
				Mc.DisplayGuiScreen(new GuiAchievements(Mc.StatFileWriter));
			}

			if (par1GuiButton.Id == 1)
			{
				Mc.DisplayGuiScreen(new GuiStats(this, Mc.StatFileWriter));
			}
		}

		/// <summary>
		/// Returns the creative inventory
		/// </summary>
		public static InventoryBasic GetInventory()
		{
			return Inventory;
		}
	}
}