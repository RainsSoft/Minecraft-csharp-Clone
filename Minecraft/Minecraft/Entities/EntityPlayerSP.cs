using System.Text;

namespace net.minecraft.src
{
	public class EntityPlayerSP : EntityPlayer
	{
		public MovementInput MovementInput;
		protected Minecraft Mc;

		/// <summary>
		/// Used to tell if the player pressed forward twice. If this is at 0 and it's pressed (And they are allowed to
		/// sprint, aka enough food on the ground etc) it sets this to 7. If it's pressed and it's greater than 0 enable
		/// sprinting.
		/// </summary>
		protected int SprintToggleTimer;

		/// <summary>
		/// Ticks left before sprinting is disabled. </summary>
		public int SprintingTicksLeft;
		public float RenderArmYaw;
		public float RenderArmPitch;
		public float PrevRenderArmYaw;
		public float PrevRenderArmPitch;
		private MouseFilter Field_21903_bJ;
		private MouseFilter Field_21904_bK;
		private MouseFilter Field_21902_bL;

		public EntityPlayerSP(Minecraft par1Minecraft, World par2World, Session par3Session, int par4) : base(par2World)
		{
			SprintToggleTimer = 0;
			SprintingTicksLeft = 0;
			Field_21903_bJ = new MouseFilter();
			Field_21904_bK = new MouseFilter();
			Field_21902_bL = new MouseFilter();
			Mc = par1Minecraft;
			Dimension = par4;

			if (par3Session != null && par3Session.Username != null && par3Session.Username.Length > 0)
			{
				SkinUrl = (new StringBuilder()).Append("http://s3.amazonaws.com/MinecraftSkins/").Append(par3Session.Username).Append(".png").ToString();
			}

			Username = par3Session.Username;
		}

		/// <summary>
		/// Tries to moves the entity by the passed in displacement. Args: x, y, z
		/// </summary>
        public override void MoveEntity(float par1, float par3, float par5)
		{
			base.MoveEntity(par1, par3, par5);
		}

		public override void UpdateEntityActionState()
		{
			base.UpdateEntityActionState();
			MoveStrafing = MovementInput.MoveStrafe;
			MoveForward = MovementInput.MoveForward;
			IsJumping = MovementInput.Jump;
			PrevRenderArmYaw = RenderArmYaw;
			PrevRenderArmPitch = RenderArmPitch;
			RenderArmPitch += (float)(RotationPitch - RenderArmPitch) * 0.5F;
			RenderArmYaw += (float)(RotationYaw - RenderArmYaw) * 0.5F;
		}

		/// <summary>
		/// Returns whether the entity is in a local (client) world
		/// </summary>
		protected override bool IsClientWorld()
		{
			return true;
		}

		/// <summary>
		/// Called frequently so the entity can update its state every tick as required. For example, zombies and skeletons
		/// use this to react to sunlight and start to burn.
		/// </summary>
		public override void OnLivingUpdate()
		{
			if (SprintingTicksLeft > 0)
			{
				SprintingTicksLeft--;

				if (SprintingTicksLeft == 0)
				{
					SetSprinting(false);
				}
			}

			if (SprintToggleTimer > 0)
			{
				SprintToggleTimer--;
			}

			if (Mc.PlayerController.Func_35643_e())
			{
				PosX = PosZ = 0.5F;
				PosX = 0.0F;
				PosZ = 0.0F;
				RotationYaw = (float)TicksExisted / 12F;
				RotationPitch = 10F;
				PosY = 68.5F;
				return;
			}

			if (!Mc.StatFileWriter.HasAchievementUnlocked(AchievementList.OpenInventory))
			{
				Mc.GuiAchievement.QueueAchievementInformation(AchievementList.OpenInventory);
			}

			PrevTimeInPortal = TimeInPortal;

			if (InPortal)
			{
				if (!WorldObj.IsRemote && RidingEntity != null)
				{
					MountEntity(null);
				}

				if (Mc.CurrentScreen != null)
				{
					Mc.DisplayGuiScreen(null);
				}

				if (TimeInPortal == 0.0F)
				{
					Mc.SndManager.PlaySoundFX("portal.trigger", 1.0F, Rand.NextFloat() * 0.4F + 0.8F);
				}

				TimeInPortal += 0.0125F;

				if (TimeInPortal >= 1.0F)
				{
					TimeInPortal = 1.0F;

					if (!WorldObj.IsRemote)
					{
						TimeUntilPortal = 10;
						Mc.SndManager.PlaySoundFX("portal.travel", 1.0F, Rand.NextFloat() * 0.4F + 0.8F);
						sbyte byte0 = 0;

						if (Dimension == -1)
						{
							byte0 = 0;
						}
						else
						{
							byte0 = -1;
						}

						Mc.UsePortal(byte0);
						TriggerAchievement(AchievementList.Portal);
					}
				}

				InPortal = false;
			}
			else if (IsPotionActive(Potion.Confusion) && GetActivePotionEffect(Potion.Confusion).GetDuration() > 60)
			{
				TimeInPortal += 0.006666667F;

				if (TimeInPortal > 1.0F)
				{
					TimeInPortal = 1.0F;
				}
			}
			else
			{
				if (TimeInPortal > 0.0F)
				{
					TimeInPortal -= 0.05F;
				}

				if (TimeInPortal < 0.0F)
				{
					TimeInPortal = 0.0F;
				}
			}

			if (TimeUntilPortal > 0)
			{
				TimeUntilPortal--;
			}

			bool flag = MovementInput.Jump;
			float f = 0.8F;
			bool flag1 = MovementInput.MoveForward >= f;
			MovementInput.Func_52013_a();

			if (IsUsingItem())
			{
				MovementInput.MoveStrafe *= 0.2F;
				MovementInput.MoveForward *= 0.2F;
				SprintToggleTimer = 0;
			}

			if (MovementInput.Sneak && YSize < 0.2F)
			{
				YSize = 0.2F;
			}

			PushOutOfBlocks(PosX - Width * 0.34999999999999998F, BoundingBox.MinY + 0.5F, PosZ + Width * 0.34999999999999998F);
			PushOutOfBlocks(PosX - Width * 0.34999999999999998F, BoundingBox.MinY + 0.5F, PosZ - Width * 0.34999999999999998F);
			PushOutOfBlocks(PosX + Width * 0.34999999999999998F, BoundingBox.MinY + 0.5F, PosZ - Width * 0.34999999999999998F);
			PushOutOfBlocks(PosX + Width * 0.34999999999999998F, BoundingBox.MinY + 0.5F, PosZ + Width * 0.34999999999999998F);
			bool flag2 = (float)GetFoodStats().GetFoodLevel() > 6F;

			if (OnGround && !flag1 && MovementInput.MoveForward >= f && !IsSprinting() && flag2 && !IsUsingItem() && !IsPotionActive(Potion.Blindness))
			{
				if (SprintToggleTimer == 0)
				{
					SprintToggleTimer = 7;
				}
				else
				{
					SetSprinting(true);
					SprintToggleTimer = 0;
				}
			}

			if (IsSneaking())
			{
				SprintToggleTimer = 0;
			}

			if (IsSprinting() && (MovementInput.MoveForward < f || IsCollidedHorizontally || !flag2))
			{
				SetSprinting(false);
			}

			if (Capabilities.AllowFlying && !flag && MovementInput.Jump)
			{
				if (FlyToggleTimer == 0)
				{
					FlyToggleTimer = 7;
				}
				else
				{
					Capabilities.IsFlying = !Capabilities.IsFlying;
					Func_50009_aI();
					FlyToggleTimer = 0;
				}
			}

			if (Capabilities.IsFlying)
			{
				if (MovementInput.Sneak)
				{
					MotionY -= 0.14999999999999999F;
				}

				if (MovementInput.Jump)
				{
					MotionY += 0.14999999999999999F;
				}
			}

			base.OnLivingUpdate();

			if (OnGround && Capabilities.IsFlying)
			{
				Capabilities.IsFlying = false;
				Func_50009_aI();
			}
		}

		public override void TravelToTheEnd(int par1)
		{
			if (WorldObj.IsRemote)
			{
				return;
			}

			if (Dimension == 1 && par1 == 1)
			{
				TriggerAchievement(AchievementList.TheEnd2);
				Mc.DisplayGuiScreen(new GuiWinGame());
			}
			else
			{
				TriggerAchievement(AchievementList.TheEnd);
				Mc.SndManager.PlaySoundFX("portal.travel", 1.0F, Rand.NextFloat() * 0.4F + 0.8F);
				Mc.UsePortal(1);
			}
		}

		/// <summary>
		/// Gets the player's field of view multiplier. (ex. when flying)
		/// </summary>
		public virtual float GetFOVMultiplier()
		{
			float f = 1.0F;

			if (Capabilities.IsFlying)
			{
				f *= 1.1F;
			}

			f *= ((LandMovementFactor * GetSpeedModifier()) / SpeedOnGround + 1.0F) / 2.0F;

			if (IsUsingItem() && GetItemInUse().ItemID == Item.Bow.ShiftedIndex)
			{
				int i = GetItemInUseDuration();
				float f1 = (float)i / 20F;

				if (f1 > 1.0F)
				{
					f1 = 1.0F;
				}
				else
				{
					f1 *= f1;
				}

				f *= 1.0F - f1 * 0.15F;
			}

			return f;
		}

		/// <summary>
		/// (abstract) Protected helper method to write subclass entity data to NBT.
		/// </summary>
		public override void WriteEntityToNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.WriteEntityToNBT(par1NBTTagCompound);
			par1NBTTagCompound.SetInteger("Score", Score);
		}

		/// <summary>
		/// (abstract) Protected helper method to read subclass entity data from NBT.
		/// </summary>
		public override void ReadEntityFromNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.ReadEntityFromNBT(par1NBTTagCompound);
			Score = par1NBTTagCompound.GetInteger("Score");
		}

		/// <summary>
		/// sets current screen to null (used on escape buttons of GUIs)
		/// </summary>
		public override void CloseScreen()
		{
			base.CloseScreen();
			Mc.DisplayGuiScreen(null);
		}

		/// <summary>
		/// Displays the GUI for editing a sign. Args: tileEntitySign
		/// </summary>
		public override void DisplayGUIEditSign(TileEntitySign par1TileEntitySign)
		{
			Mc.DisplayGuiScreen(new GuiEditSign(par1TileEntitySign));
		}

		/// <summary>
		/// Displays the GUI for interacting with a chest inventory. Args: chestInventory
		/// </summary>
		public override void DisplayGUIChest(IInventory par1IInventory)
		{
			Mc.DisplayGuiScreen(new GuiChest(Inventory, par1IInventory));
		}

		/// <summary>
		/// Displays the crafting GUI for a workbench.
		/// </summary>
		public override void DisplayWorkbenchGUI(int par1, int par2, int par3)
		{
			Mc.DisplayGuiScreen(new GuiCrafting(Inventory, WorldObj, par1, par2, par3));
		}

		public override void DisplayGUIEnchantment(int par1, int par2, int par3)
		{
			Mc.DisplayGuiScreen(new GuiEnchantment(Inventory, WorldObj, par1, par2, par3));
		}

		/// <summary>
		/// Displays the furnace GUI for the passed in furnace entity. Args: tileEntityFurnace
		/// </summary>
		public override void DisplayGUIFurnace(TileEntityFurnace par1TileEntityFurnace)
		{
			Mc.DisplayGuiScreen(new GuiFurnace(Inventory, par1TileEntityFurnace));
		}

		/// <summary>
		/// Displays the GUI for interacting with a brewing stand.
		/// </summary>
		public override void DisplayGUIBrewingStand(TileEntityBrewingStand par1TileEntityBrewingStand)
		{
			Mc.DisplayGuiScreen(new GuiBrewingStand(Inventory, par1TileEntityBrewingStand));
		}

		/// <summary>
		/// Displays the dipsenser GUI for the passed in dispenser entity. Args: TileEntityDispenser
		/// </summary>
		public override void DisplayGUIDispenser(TileEntityDispenser par1TileEntityDispenser)
		{
			Mc.DisplayGuiScreen(new GuiDispenser(Inventory, par1TileEntityDispenser));
		}

		/// <summary>
		/// is called when the player performs a critical hit on the Entity. Args: entity that was hit critically
		/// </summary>
		public override void OnCriticalHit(Entity par1Entity)
		{
			Mc.EffectRenderer.AddEffect(new EntityCrit2FX(Mc.TheWorld, par1Entity));
		}

		public override void OnEnchantmentCritical(Entity par1Entity)
		{
			EntityCrit2FX entitycrit2fx = new EntityCrit2FX(Mc.TheWorld, par1Entity, "magicCrit");
			Mc.EffectRenderer.AddEffect(entitycrit2fx);
		}

		/// <summary>
		/// Called whenever an item is picked up from walking over it. Args: pickedUpEntity, stackSize
		/// </summary>
		public override void OnItemPickup(Entity par1Entity, int par2)
		{
			Mc.EffectRenderer.AddEffect(new EntityPickupFX(Mc.TheWorld, par1Entity, this, -0.5F));
		}

		/// <summary>
		/// Sends a chat message from the player. Args: chatMessage
		/// </summary>
		public virtual void SendChatMessage(string s)
		{
		}

		/// <summary>
		/// Returns if this entity is sneaking.
		/// </summary>
		public override bool IsSneaking()
		{
			return MovementInput.Sneak && !Sleeping;
		}

		/// <summary>
		/// Updates health locally.
		/// </summary>
		public virtual void SetHealth(int par1)
		{
			int i = GetHealth() - par1;

			if (i <= 0)
			{
				SetEntityHealth(par1);

				if (i < 0)
				{
					HeartsLife = HeartsHalvesLife / 2;
				}
			}
			else
			{
				NaturalArmorRating = i;
				SetEntityHealth(GetHealth());
				HeartsLife = HeartsHalvesLife;
				DamageEntity(DamageSource.Generic, i);
				HurtTime = MaxHurtTime = 10;
			}
		}

		public override void RespawnPlayer()
		{
			Mc.Respawn(false, 0, false);
		}

		public override void Func_6420_o()
		{
		}

		/// <summary>
		/// Add a chat message to the player
		/// </summary>
		public override void AddChatMessage(string par1Str)
		{
			Mc.IngameGUI.AddChatMessageTranslate(par1Str);
		}

		/// <summary>
		/// Adds a value to a statistic field.
		/// </summary>
		public override void AddStat(StatBase par1StatBase, int par2)
		{
			if (par1StatBase == null)
			{
				return;
			}

			if (par1StatBase.IsAchievement())
			{
				Achievement achievement = (Achievement)par1StatBase;

				if (achievement.ParentAchievement == null || Mc.StatFileWriter.HasAchievementUnlocked(achievement.ParentAchievement))
				{
					if (!Mc.StatFileWriter.HasAchievementUnlocked(achievement))
					{
						Mc.GuiAchievement.QueueTakenAchievement(achievement);
					}

					Mc.StatFileWriter.ReadStat(par1StatBase, par2);
				}
			}
			else
			{
				Mc.StatFileWriter.ReadStat(par1StatBase, par2);
			}
		}

		private bool IsBlockTranslucent(int par1, int par2, int par3)
		{
			return WorldObj.IsBlockNormalCube(par1, par2, par3);
		}

		/// <summary>
		/// Adds velocity to push the entity out of blocks at the specified x, y, z position Args: x, y, z
		/// </summary>
        protected override bool PushOutOfBlocks(float par1, float par3, float par5)
		{
			int i = MathHelper2.Floor_double(par1);
			int j = MathHelper2.Floor_double(par3);
			int k = MathHelper2.Floor_double(par5);
			float d = par1 - i;
            float d1 = par5 - k;

			if (IsBlockTranslucent(i, j, k) || IsBlockTranslucent(i, j + 1, k))
			{
				bool flag = !IsBlockTranslucent(i - 1, j, k) && !IsBlockTranslucent(i - 1, j + 1, k);
				bool flag1 = !IsBlockTranslucent(i + 1, j, k) && !IsBlockTranslucent(i + 1, j + 1, k);
				bool flag2 = !IsBlockTranslucent(i, j, k - 1) && !IsBlockTranslucent(i, j + 1, k - 1);
				bool flag3 = !IsBlockTranslucent(i, j, k + 1) && !IsBlockTranslucent(i, j + 1, k + 1);
				sbyte byte0 = -1;
				double d2 = 9999D;

				if (flag && d < d2)
				{
					d2 = d;
					byte0 = 0;
				}

				if (flag1 && 1.0D - d < d2)
				{
					d2 = 1.0D - d;
					byte0 = 1;
				}

				if (flag2 && d1 < d2)
				{
					d2 = d1;
					byte0 = 4;
				}

				if (flag3 && 1.0D - d1 < d2)
				{
					double d3 = 1.0D - d1;
					byte0 = 5;
				}

				float f = 0.1F;

				if (byte0 == 0)
				{
					MotionX = -f;
				}

				if (byte0 == 1)
				{
					MotionX = f;
				}

				if (byte0 == 4)
				{
					MotionZ = -f;
				}

				if (byte0 == 5)
				{
					MotionZ = f;
				}
			}

			return false;
		}

		/// <summary>
		/// Set sprinting switch for Entity.
		/// </summary>
		public override void SetSprinting(bool par1)
		{
			base.SetSprinting(par1);
			SprintingTicksLeft = par1 ? 600 : 0;
		}

		/// <summary>
		/// Sets the current XP, total XP, and level number.
		/// </summary>
		public virtual void SetXPStats(float par1, int par2, int par3)
		{
			Experience = par1;
			ExperienceTotal = par2;
			ExperienceLevel = par3;
		}
	}
}