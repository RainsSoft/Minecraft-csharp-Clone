namespace net.minecraft.src
{
	using net.minecraft.src;

	public abstract class PlayerController
	{
		/// <summary>
		/// A reference to the Minecraft object. </summary>
		protected readonly Minecraft Mc;
		public bool IsInTestMode;

		public PlayerController(Minecraft par1Minecraft)
		{
			IsInTestMode = false;
			Mc = par1Minecraft;
		}

		/// <summary>
		/// Called on world change with the new World as the only parameter.
		/// </summary>
		public virtual void OnWorldChange(World world)
		{
		}

		/// <summary>
		/// Called by Minecraft class when the player is hitting a block with an item. Args: x, y, z, side
		/// </summary>
		public abstract void ClickBlock(int i, int j, int k, int l);

		/// <summary>
		/// Called when a player completes the destruction of a block
		/// </summary>
		public virtual bool OnPlayerDestroyBlock(int par1, int par2, int par3, int par4)
		{
			World world = Mc.TheWorld;
			Block block = Block.BlocksList[world.GetBlockId(par1, par2, par3)];

			if (block == null)
			{
				return false;
			}

			world.PlayAuxSFX(2001, par1, par2, par3, block.BlockID + (world.GetBlockMetadata(par1, par2, par3) << 12));
			int i = world.GetBlockMetadata(par1, par2, par3);
			bool flag = world.SetBlockWithNotify(par1, par2, par3, 0);

			if (flag)
			{
				block.OnBlockDestroyedByPlayer(world, par1, par2, par3, i);
			}

			return flag;
		}

		/// <summary>
		/// Called when a player damages a block and updates damage counters
		/// </summary>
		public abstract void OnPlayerDamageBlock(int i, int j, int k, int l);

		/// <summary>
		/// Resets current block damage and isHittingBlock
		/// </summary>
		public abstract void ResetBlockRemoving();

		public virtual void SetPartialTime(float f)
		{
		}

		/// <summary>
		/// player reach distance = 4F
		/// </summary>
		public abstract float GetBlockReachDistance();

		/// <summary>
		/// Notifies the server of things like consuming food, etc...
		/// </summary>
		public virtual bool SendUseItem(EntityPlayer par1EntityPlayer, World par2World, ItemStack par3ItemStack)
		{
			int i = par3ItemStack.StackSize;
			ItemStack itemstack = par3ItemStack.UseItemRightClick(par2World, par1EntityPlayer);

			if (itemstack != par3ItemStack || itemstack != null && itemstack.StackSize != i)
			{
				par1EntityPlayer.Inventory.MainInventory[par1EntityPlayer.Inventory.CurrentItem] = itemstack;

				if (itemstack.StackSize == 0)
				{
					par1EntityPlayer.Inventory.MainInventory[par1EntityPlayer.Inventory.CurrentItem] = null;
				}

				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Flips the player around. Args: player
		/// </summary>
		public virtual void FlipPlayer(EntityPlayer entityplayer)
		{
		}

		public virtual void UpdateController()
		{
		}

		public abstract bool ShouldDrawHUD();

		public virtual void Func_6473_b(EntityPlayer par1EntityPlayer)
		{
			PlayerControllerCreative.DisableAbilities(par1EntityPlayer);
		}

		/// <summary>
		/// Handles a players right click
		/// </summary>
		public abstract bool OnPlayerRightClick(EntityPlayer entityplayer, World world, ItemStack itemstack, int i, int j, int k, int l);

		public virtual EntityPlayer CreatePlayer(World par1World)
		{
			return new EntityPlayerSP(Mc, par1World, Mc.Session, par1World.WorldProvider.TheWorldType);
		}

		/// <summary>
		/// Interacts with an entity
		/// </summary>
		public virtual void InteractWithEntity(EntityPlayer par1EntityPlayer, Entity par2Entity)
		{
			par1EntityPlayer.UseCurrentItemOnEntity(par2Entity);
		}

		/// <summary>
		/// Attacks an entity
		/// </summary>
		public virtual void AttackEntity(EntityPlayer par1EntityPlayer, Entity par2Entity)
		{
			par1EntityPlayer.AttackTargetEntityWithCurrentItem(par2Entity);
		}

		public virtual ItemStack WindowClick(int par1, int par2, int par3, bool par4, EntityPlayer par5EntityPlayer)
		{
			return par5EntityPlayer.CraftingInventory.SlotClick(par2, par3, par4, par5EntityPlayer);
		}

		public virtual void Func_20086_a(int par1, EntityPlayer par2EntityPlayer)
		{
			par2EntityPlayer.CraftingInventory.OnCraftGuiClosed(par2EntityPlayer);
			par2EntityPlayer.CraftingInventory = par2EntityPlayer.InventorySlots;
		}

		public virtual void Func_40593_a(int i, int j)
		{
		}

		public virtual bool Func_35643_e()
		{
			return false;
		}

		public virtual void OnStoppedUsingItem(EntityPlayer par1EntityPlayer)
		{
			par1EntityPlayer.StopUsingItem();
		}

		public virtual bool Func_35642_f()
		{
			return false;
		}

		/// <summary>
		/// Checks if the player is not creative, used for checking if it should break a block instantly
		/// </summary>
		public virtual bool IsNotCreative()
		{
			return true;
		}

		/// <summary>
		/// returns true if player is in creative mode
		/// </summary>
		public virtual bool IsInCreativeMode()
		{
			return false;
		}

		/// <summary>
		/// true for hitting entities far away.
		/// </summary>
		public virtual bool ExtendedReach()
		{
			return false;
		}

		/// <summary>
		/// Used in PlayerControllerMP to update the server with an ItemStack in a slot.
		/// </summary>
		public virtual void SendSlotPacket(ItemStack itemstack, int i)
		{
		}

		public virtual void Func_35639_a(ItemStack itemstack)
		{
		}
	}
}