namespace net.minecraft.src
{
	using net.minecraft.src;

	public class PlayerControllerCreative : PlayerController
	{
		private int Field_35647_c;

		public PlayerControllerCreative(Minecraft par1Minecraft) : base(par1Minecraft)
		{
			IsInTestMode = true;
		}

		/// <summary>
		/// Enables creative abilities to the player
		/// </summary>
		public static void EnableAbilities(EntityPlayer par0EntityPlayer)
		{
			par0EntityPlayer.Capabilities.AllowFlying = true;
			par0EntityPlayer.Capabilities.IsCreativeMode = true;
			par0EntityPlayer.Capabilities.DisableDamage = true;
		}

		/// <summary>
		/// Disables creative abilities to the player.
		/// </summary>
		public static void DisableAbilities(EntityPlayer par0EntityPlayer)
		{
			par0EntityPlayer.Capabilities.AllowFlying = false;
			par0EntityPlayer.Capabilities.IsFlying = false;
			par0EntityPlayer.Capabilities.IsCreativeMode = false;
			par0EntityPlayer.Capabilities.DisableDamage = false;
		}

		public override void Func_6473_b(EntityPlayer par1EntityPlayer)
		{
			EnableAbilities(par1EntityPlayer);

			for (int i = 0; i < 9; i++)
			{
				if (par1EntityPlayer.Inventory.MainInventory[i] == null)
				{
					par1EntityPlayer.Inventory.MainInventory[i] = new ItemStack((Block)Session.RegisteredBlocksList[i]);
				}
			}
		}

		/// <summary>
		/// Called from a PlayerController when the player is hitting a block with an item in Creative mode. Args: Minecraft
		/// instance, player controller, x, y, z, side
		/// </summary>
		public static void ClickBlockCreative(Minecraft par0Minecraft, PlayerController par1PlayerController, int par2, int par3, int par4, int par5)
		{
			if (!par0Minecraft.TheWorld.Func_48457_a(par0Minecraft.ThePlayer, par2, par3, par4, par5))
			{
				par1PlayerController.OnPlayerDestroyBlock(par2, par3, par4, par5);
			}
		}

		/// <summary>
		/// Handles a players right click
		/// </summary>
		public override bool OnPlayerRightClick(EntityPlayer par1EntityPlayer, World par2World, ItemStack par3ItemStack, int par4, int par5, int par6, int par7)
		{
			int i = par2World.GetBlockId(par4, par5, par6);

			if (i > 0 && Block.BlocksList[i].BlockActivated(par2World, par4, par5, par6, par1EntityPlayer))
			{
				return true;
			}

			if (par3ItemStack == null)
			{
				return false;
			}
			else
			{
				int j = par3ItemStack.GetItemDamage();
				int k = par3ItemStack.StackSize;
				bool flag = par3ItemStack.UseItem(par1EntityPlayer, par2World, par4, par5, par6, par7);
				par3ItemStack.SetItemDamage(j);
				par3ItemStack.StackSize = k;
				return flag;
			}
		}

		/// <summary>
		/// Called by Minecraft class when the player is hitting a block with an item. Args: x, y, z, side
		/// </summary>
		public override void ClickBlock(int par1, int par2, int par3, int par4)
		{
			ClickBlockCreative(Mc, this, par1, par2, par3, par4);
			Field_35647_c = 5;
		}

		/// <summary>
		/// Called when a player damages a block and updates damage counters
		/// </summary>
		public override void OnPlayerDamageBlock(int par1, int par2, int par3, int par4)
		{
			Field_35647_c--;

			if (Field_35647_c <= 0)
			{
				Field_35647_c = 5;
				ClickBlockCreative(Mc, this, par1, par2, par3, par4);
			}
		}

		/// <summary>
		/// Resets current block damage and isHittingBlock
		/// </summary>
		public override void ResetBlockRemoving()
		{
		}

		public override bool ShouldDrawHUD()
		{
			return false;
		}

		/// <summary>
		/// Called on world change with the new World as the only parameter.
		/// </summary>
		public override void OnWorldChange(World par1World)
		{
			base.OnWorldChange(par1World);
		}

		/// <summary>
		/// player reach distance = 4F
		/// </summary>
		public override float GetBlockReachDistance()
		{
			return 5F;
		}

		/// <summary>
		/// Checks if the player is not creative, used for checking if it should break a block instantly
		/// </summary>
		public override bool IsNotCreative()
		{
			return false;
		}

		/// <summary>
		/// returns true if player is in creative mode
		/// </summary>
		public override bool IsInCreativeMode()
		{
			return true;
		}

		/// <summary>
		/// true for hitting entities far away.
		/// </summary>
		public override bool ExtendedReach()
		{
			return true;
		}
	}
}