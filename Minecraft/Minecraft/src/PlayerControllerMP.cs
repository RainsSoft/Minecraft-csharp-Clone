namespace net.minecraft.src
{
	using net.minecraft.src;

	public class PlayerControllerMP : PlayerController
	{
		/// <summary>
		/// PosX of the current block being destroyed </summary>
		private int CurrentBlockX;

		/// <summary>
		/// PosY of the current block being destroyed </summary>
		private int CurrentBlockY;

		/// <summary>
		/// PosZ of the current block being destroyed </summary>
		private int CurrentblockZ;

		/// <summary>
		/// Current block damage (MP) </summary>
		private float CurBlockDamageMP;

		/// <summary>
		/// Previous block damage (MP) </summary>
		private float PrevBlockDamageMP;

		/// <summary>
		/// Tick counter, when it hits 4 it resets back to 0 and plays the step sound
		/// </summary>
		private float StepSoundTickCounter;

		/// <summary>
		/// Delays the first damage on the block after the first click on the block
		/// </summary>
		private int BlockHitDelay;

		/// <summary>
		/// Tells if the player is hitting a block </summary>
		private bool IsHittingBlock;
		private bool CreativeMode;
		private NetClientHandler NetClientHandler;

		/// <summary>
		/// Index of the current item held by the player in the inventory hotbar </summary>
		private int CurrentPlayerItem;

		public PlayerControllerMP(Minecraft par1Minecraft, NetClientHandler par2NetClientHandler) : base(par1Minecraft)
		{
			CurrentBlockX = -1;
			CurrentBlockY = -1;
			CurrentblockZ = -1;
			CurBlockDamageMP = 0.0F;
			PrevBlockDamageMP = 0.0F;
			StepSoundTickCounter = 0.0F;
			BlockHitDelay = 0;
			IsHittingBlock = false;
			CurrentPlayerItem = 0;
			NetClientHandler = par2NetClientHandler;
		}

		public virtual void SetCreative(bool par1)
		{
			CreativeMode = par1;

			if (CreativeMode)
			{
				PlayerControllerCreative.EnableAbilities(Mc.ThePlayer);
			}
			else
			{
				PlayerControllerCreative.DisableAbilities(Mc.ThePlayer);
			}
		}

		/// <summary>
		/// Flips the player around. Args: player
		/// </summary>
		public override void FlipPlayer(EntityPlayer par1EntityPlayer)
		{
			par1EntityPlayer.RotationYaw = -180F;
		}

		public override bool ShouldDrawHUD()
		{
			return !CreativeMode;
		}

		/// <summary>
		/// Called when a player completes the destruction of a block
		/// </summary>
		public override bool OnPlayerDestroyBlock(int par1, int par2, int par3, int par4)
		{
			if (CreativeMode)
			{
				return base.OnPlayerDestroyBlock(par1, par2, par3, par4);
			}

			int i = Mc.TheWorld.GetBlockId(par1, par2, par3);
			bool flag = base.OnPlayerDestroyBlock(par1, par2, par3, par4);
			ItemStack itemstack = Mc.ThePlayer.GetCurrentEquippedItem();

			if (itemstack != null)
			{
				itemstack.OnDestroyBlock(i, par1, par2, par3, Mc.ThePlayer);

				if (itemstack.StackSize == 0)
				{
					itemstack.OnItemDestroyedByUse(Mc.ThePlayer);
					Mc.ThePlayer.DestroyCurrentEquippedItem();
				}
			}

			return flag;
		}

		/// <summary>
		/// Called by Minecraft class when the player is hitting a block with an item. Args: x, y, z, side
		/// </summary>
		public override void ClickBlock(int par1, int par2, int par3, int par4)
		{
			if (CreativeMode)
			{
				NetClientHandler.AddToSendQueue(new Packet14BlockDig(0, par1, par2, par3, par4));
				PlayerControllerCreative.ClickBlockCreative(Mc, this, par1, par2, par3, par4);
				BlockHitDelay = 5;
			}
			else if (!IsHittingBlock || par1 != CurrentBlockX || par2 != CurrentBlockY || par3 != CurrentblockZ)
			{
				NetClientHandler.AddToSendQueue(new Packet14BlockDig(0, par1, par2, par3, par4));
				int i = Mc.TheWorld.GetBlockId(par1, par2, par3);

				if (i > 0 && CurBlockDamageMP == 0.0F)
				{
					Block.BlocksList[i].OnBlockClicked(Mc.TheWorld, par1, par2, par3, Mc.ThePlayer);
				}

				if (i > 0 && Block.BlocksList[i].BlockStrength(Mc.ThePlayer) >= 1.0F)
				{
					OnPlayerDestroyBlock(par1, par2, par3, par4);
				}
				else
				{
					IsHittingBlock = true;
					CurrentBlockX = par1;
					CurrentBlockY = par2;
					CurrentblockZ = par3;
					CurBlockDamageMP = 0.0F;
					PrevBlockDamageMP = 0.0F;
					StepSoundTickCounter = 0.0F;
				}
			}
		}

		/// <summary>
		/// Resets current block damage and isHittingBlock
		/// </summary>
		public override void ResetBlockRemoving()
		{
			CurBlockDamageMP = 0.0F;
			IsHittingBlock = false;
		}

		/// <summary>
		/// Called when a player damages a block and updates damage counters
		/// </summary>
		public override void OnPlayerDamageBlock(int par1, int par2, int par3, int par4)
		{
			SyncCurrentPlayItem();

			if (BlockHitDelay > 0)
			{
				BlockHitDelay--;
				return;
			}

			if (CreativeMode)
			{
				BlockHitDelay = 5;
				NetClientHandler.AddToSendQueue(new Packet14BlockDig(0, par1, par2, par3, par4));
				PlayerControllerCreative.ClickBlockCreative(Mc, this, par1, par2, par3, par4);
				return;
			}

			if (par1 == CurrentBlockX && par2 == CurrentBlockY && par3 == CurrentblockZ)
			{
				int i = Mc.TheWorld.GetBlockId(par1, par2, par3);

				if (i == 0)
				{
					IsHittingBlock = false;
					return;
				}

				Block block = Block.BlocksList[i];
				CurBlockDamageMP += block.BlockStrength(Mc.ThePlayer);

				if (StepSoundTickCounter % 4F == 0.0F && block != null)
				{
					Mc.SndManager.PlaySound(block.StepSound.GetStepSound(), (float)par1 + 0.5F, (float)par2 + 0.5F, (float)par3 + 0.5F, (block.StepSound.GetVolume() + 1.0F) / 8F, block.StepSound.GetPitch() * 0.5F);
				}

				StepSoundTickCounter++;

				if (CurBlockDamageMP >= 1.0F)
				{
					IsHittingBlock = false;
					NetClientHandler.AddToSendQueue(new Packet14BlockDig(2, par1, par2, par3, par4));
					OnPlayerDestroyBlock(par1, par2, par3, par4);
					CurBlockDamageMP = 0.0F;
					PrevBlockDamageMP = 0.0F;
					StepSoundTickCounter = 0.0F;
					BlockHitDelay = 5;
				}
			}
			else
			{
				ClickBlock(par1, par2, par3, par4);
			}
		}

		public override void SetPartialTime(float par1)
		{
			if (CurBlockDamageMP <= 0.0F)
			{
				Mc.IngameGUI.DamageGuiPartialTime = 0.0F;
				Mc.RenderGlobal.DamagePartialTime = 0.0F;
			}
			else
			{
				float f = PrevBlockDamageMP + (CurBlockDamageMP - PrevBlockDamageMP) * par1;
				Mc.IngameGUI.DamageGuiPartialTime = f;
				Mc.RenderGlobal.DamagePartialTime = f;
			}
		}

		/// <summary>
		/// player reach distance = 4F
		/// </summary>
		public override float GetBlockReachDistance()
		{
			return !CreativeMode ? 4.5F : 5F;
		}

		/// <summary>
		/// Called on world change with the new World as the only parameter.
		/// </summary>
		public override void OnWorldChange(World par1World)
		{
			base.OnWorldChange(par1World);
		}

		public override void UpdateController()
		{
			SyncCurrentPlayItem();
			PrevBlockDamageMP = CurBlockDamageMP;
			Mc.SndManager.PlayRandomMusicIfReady();
		}

		/// <summary>
		/// Syncs the current player item with the server
		/// </summary>
		private void SyncCurrentPlayItem()
		{
			int i = Mc.ThePlayer.Inventory.CurrentItem;

			if (i != CurrentPlayerItem)
			{
				CurrentPlayerItem = i;
				NetClientHandler.AddToSendQueue(new Packet16BlockItemSwitch(CurrentPlayerItem));
			}
		}

		/// <summary>
		/// Handles a players right click
		/// </summary>
		public override bool OnPlayerRightClick(EntityPlayer par1EntityPlayer, World par2World, ItemStack par3ItemStack, int par4, int par5, int par6, int par7)
		{
			SyncCurrentPlayItem();
			NetClientHandler.AddToSendQueue(new Packet15Place(par4, par5, par6, par7, par1EntityPlayer.Inventory.GetCurrentItem()));
			int i = par2World.GetBlockId(par4, par5, par6);

			if (i > 0 && Block.BlocksList[i].BlockActivated(par2World, par4, par5, par6, par1EntityPlayer))
			{
				return true;
			}

			if (par3ItemStack == null)
			{
				return false;
			}

			if (CreativeMode)
			{
				int j = par3ItemStack.GetItemDamage();
				int k = par3ItemStack.StackSize;
				bool flag = par3ItemStack.UseItem(par1EntityPlayer, par2World, par4, par5, par6, par7);
				par3ItemStack.SetItemDamage(j);
				par3ItemStack.StackSize = k;
				return flag;
			}
			else
			{
				return par3ItemStack.UseItem(par1EntityPlayer, par2World, par4, par5, par6, par7);
			}
		}

		/// <summary>
		/// Notifies the server of things like consuming food, etc...
		/// </summary>
		public override bool SendUseItem(EntityPlayer par1EntityPlayer, World par2World, ItemStack par3ItemStack)
		{
			SyncCurrentPlayItem();
			NetClientHandler.AddToSendQueue(new Packet15Place(-1, -1, -1, 255, par1EntityPlayer.Inventory.GetCurrentItem()));
			bool flag = base.SendUseItem(par1EntityPlayer, par2World, par3ItemStack);
			return flag;
		}

		public override EntityPlayer CreatePlayer(World par1World)
		{
			return new EntityClientPlayerMP(Mc, par1World, Mc.Session, NetClientHandler);
		}

		/// <summary>
		/// Attacks an entity
		/// </summary>
		public override void AttackEntity(EntityPlayer par1EntityPlayer, Entity par2Entity)
		{
			SyncCurrentPlayItem();
			NetClientHandler.AddToSendQueue(new Packet7UseEntity(par1EntityPlayer.EntityId, par2Entity.EntityId, 1));
			par1EntityPlayer.AttackTargetEntityWithCurrentItem(par2Entity);
		}

		/// <summary>
		/// Interacts with an entity
		/// </summary>
		public override void InteractWithEntity(EntityPlayer par1EntityPlayer, Entity par2Entity)
		{
			SyncCurrentPlayItem();
			NetClientHandler.AddToSendQueue(new Packet7UseEntity(par1EntityPlayer.EntityId, par2Entity.EntityId, 0));
			par1EntityPlayer.UseCurrentItemOnEntity(par2Entity);
		}

		public override ItemStack WindowClick(int par1, int par2, int par3, bool par4, EntityPlayer par5EntityPlayer)
		{
			short word0 = par5EntityPlayer.CraftingInventory.GetNextTransactionID(par5EntityPlayer.Inventory);
			ItemStack itemstack = base.WindowClick(par1, par2, par3, par4, par5EntityPlayer);
			NetClientHandler.AddToSendQueue(new Packet102WindowClick(par1, par2, par3, par4, itemstack, word0));
			return itemstack;
		}

		public override void Func_40593_a(int par1, int par2)
		{
			NetClientHandler.AddToSendQueue(new Packet108EnchantItem(par1, par2));
		}

		/// <summary>
		/// Used in PlayerControllerMP to update the server with an ItemStack in a slot.
		/// </summary>
		public override void SendSlotPacket(ItemStack par1ItemStack, int par2)
		{
			if (CreativeMode)
			{
				NetClientHandler.AddToSendQueue(new Packet107CreativeSetSlot(par2, par1ItemStack));
			}
		}

		public override void Func_35639_a(ItemStack par1ItemStack)
		{
			if (CreativeMode && par1ItemStack != null)
			{
				NetClientHandler.AddToSendQueue(new Packet107CreativeSetSlot(-1, par1ItemStack));
			}
		}

		public override void Func_20086_a(int par1, EntityPlayer par2EntityPlayer)
		{
			if (par1 == -9999)
			{
				return;
			}
			else
			{
				return;
			}
		}

		public override void OnStoppedUsingItem(EntityPlayer par1EntityPlayer)
		{
			SyncCurrentPlayItem();
			NetClientHandler.AddToSendQueue(new Packet14BlockDig(5, 0, 0, 0, 255));
			base.OnStoppedUsingItem(par1EntityPlayer);
		}

		public override bool Func_35642_f()
		{
			return true;
		}

		/// <summary>
		/// Checks if the player is not creative, used for checking if it should break a block instantly
		/// </summary>
		public override bool IsNotCreative()
		{
			return !CreativeMode;
		}

		/// <summary>
		/// returns true if player is in creative mode
		/// </summary>
		public override bool IsInCreativeMode()
		{
			return CreativeMode;
		}

		/// <summary>
		/// true for hitting entities far away.
		/// </summary>
		public override bool ExtendedReach()
		{
			return CreativeMode;
		}
	}
}