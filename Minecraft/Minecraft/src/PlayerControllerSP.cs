namespace net.minecraft.src
{
	using net.minecraft.src;

	public class PlayerControllerSP : PlayerController
	{
		private int CurBlockX;
		private int CurBlockY;
		private int CurBlockZ;
		private float CurBlockDamage;
		private float PrevBlockDamage;
		private float BlockDestroySoundCounter;
		private int BlockHitWait;

		public PlayerControllerSP(Minecraft par1Minecraft) : base(par1Minecraft)
		{
			CurBlockX = -1;
			CurBlockY = -1;
			CurBlockZ = -1;
			CurBlockDamage = 0.0F;
			PrevBlockDamage = 0.0F;
			BlockDestroySoundCounter = 0.0F;
			BlockHitWait = 0;
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
			return true;
		}

		/// <summary>
		/// Called when a player completes the destruction of a block
		/// </summary>
		public override bool OnPlayerDestroyBlock(int par1, int par2, int par3, int par4)
		{
			int i = Mc.TheWorld.GetBlockId(par1, par2, par3);
			int j = Mc.TheWorld.GetBlockMetadata(par1, par2, par3);
			bool flag = base.OnPlayerDestroyBlock(par1, par2, par3, par4);
			ItemStack itemstack = Mc.ThePlayer.GetCurrentEquippedItem();
			bool flag1 = Mc.ThePlayer.CanHarvestBlock(Block.BlocksList[i]);

			if (itemstack != null)
			{
				itemstack.OnDestroyBlock(i, par1, par2, par3, Mc.ThePlayer);

				if (itemstack.StackSize == 0)
				{
					itemstack.OnItemDestroyedByUse(Mc.ThePlayer);
					Mc.ThePlayer.DestroyCurrentEquippedItem();
				}
			}

			if (flag && flag1)
			{
				Block.BlocksList[i].HarvestBlock(Mc.TheWorld, Mc.ThePlayer, par1, par2, par3, j);
			}

			return flag;
		}

		/// <summary>
		/// Called by Minecraft class when the player is hitting a block with an item. Args: x, y, z, side
		/// </summary>
		public override void ClickBlock(int par1, int par2, int par3, int par4)
		{
			if (!Mc.ThePlayer.CanPlayerEdit(par1, par2, par3))
			{
				return;
			}

			Mc.TheWorld.Func_48457_a(Mc.ThePlayer, par1, par2, par3, par4);
			int i = Mc.TheWorld.GetBlockId(par1, par2, par3);

			if (i > 0 && CurBlockDamage == 0.0F)
			{
				Block.BlocksList[i].OnBlockClicked(Mc.TheWorld, par1, par2, par3, Mc.ThePlayer);
			}

			if (i > 0 && Block.BlocksList[i].BlockStrength(Mc.ThePlayer) >= 1.0F)
			{
				OnPlayerDestroyBlock(par1, par2, par3, par4);
			}
		}

		/// <summary>
		/// Resets current block damage and isHittingBlock
		/// </summary>
		public override void ResetBlockRemoving()
		{
			CurBlockDamage = 0.0F;
			BlockHitWait = 0;
		}

		/// <summary>
		/// Called when a player damages a block and updates damage counters
		/// </summary>
		public override void OnPlayerDamageBlock(int par1, int par2, int par3, int par4)
		{
			if (BlockHitWait > 0)
			{
				BlockHitWait--;
				return;
			}

			if (par1 == CurBlockX && par2 == CurBlockY && par3 == CurBlockZ)
			{
				int i = Mc.TheWorld.GetBlockId(par1, par2, par3);

				if (!Mc.ThePlayer.CanPlayerEdit(par1, par2, par3))
				{
					return;
				}

				if (i == 0)
				{
					return;
				}

				Block block = Block.BlocksList[i];
				CurBlockDamage += block.BlockStrength(Mc.ThePlayer);

				if (BlockDestroySoundCounter % 4F == 0.0F && block != null)
				{
					Mc.SndManager.PlaySound(block.StepSound.GetStepSound(), (float)par1 + 0.5F, (float)par2 + 0.5F, (float)par3 + 0.5F, (block.StepSound.GetVolume() + 1.0F) / 8F, block.StepSound.GetPitch() * 0.5F);
				}

				BlockDestroySoundCounter++;

				if (CurBlockDamage >= 1.0F)
				{
					OnPlayerDestroyBlock(par1, par2, par3, par4);
					CurBlockDamage = 0.0F;
					PrevBlockDamage = 0.0F;
					BlockDestroySoundCounter = 0.0F;
					BlockHitWait = 5;
				}
			}
			else
			{
				CurBlockDamage = 0.0F;
				PrevBlockDamage = 0.0F;
				BlockDestroySoundCounter = 0.0F;
				CurBlockX = par1;
				CurBlockY = par2;
				CurBlockZ = par3;
			}
		}

		public override void SetPartialTime(float par1)
		{
			if (CurBlockDamage <= 0.0F)
			{
				Mc.IngameGUI.DamageGuiPartialTime = 0.0F;
				Mc.RenderGlobal.DamagePartialTime = 0.0F;
			}
			else
			{
				float f = PrevBlockDamage + (CurBlockDamage - PrevBlockDamage) * par1;
				Mc.IngameGUI.DamageGuiPartialTime = f;
				Mc.RenderGlobal.DamagePartialTime = f;
			}
		}

		/// <summary>
		/// player reach distance = 4F
		/// </summary>
		public override float GetBlockReachDistance()
		{
			return 4F;
		}

		/// <summary>
		/// Called on world change with the new World as the only parameter.
		/// </summary>
		public override void OnWorldChange(World par1World)
		{
			base.OnWorldChange(par1World);
		}

		public override EntityPlayer CreatePlayer(World par1World)
		{
			EntityPlayer entityplayer = base.CreatePlayer(par1World);
			return entityplayer;
		}

		public override void UpdateController()
		{
			PrevBlockDamage = CurBlockDamage;
			Mc.SndManager.PlayRandomMusicIfReady();
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
				return par3ItemStack.UseItem(par1EntityPlayer, par2World, par4, par5, par6, par7);
			}
		}

		public override bool Func_35642_f()
		{
			return true;
		}
	}
}