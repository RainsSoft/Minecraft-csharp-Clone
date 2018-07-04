using System;
using System.Text;

namespace net.minecraft.src
{
	public class EntityOtherPlayerMP : EntityPlayer
	{
		private bool IsItemInUse;
		private int OtherPlayerMPPosRotationIncrements;
        private float OtherPlayerMPX;
        private float OtherPlayerMPY;
        private float OtherPlayerMPZ;
        private float OtherPlayerMPYaw;
        private float OtherPlayerMPPitch;

		public EntityOtherPlayerMP(World par1World, string par2Str) : base(par1World)
		{
			IsItemInUse = false;
			Username = par2Str;
			YOffset = 0.0F;
			StepHeight = 0.0F;

			if (par2Str != null && par2Str.Length > 0)
			{
				SkinUrl = (new StringBuilder()).Append("http://s3.amazonaws.com/MinecraftSkins/").Append(par2Str).Append(".png").ToString();
			}

			NoClip = true;
			Field_22062_y = 0.25F;
			RenderDistanceWeight = 10;
		}

		/// <summary>
		/// sets the players height back to normal after doing things like sleeping and dieing
		/// </summary>
		protected override void ResetHeight()
		{
			YOffset = 0.0F;
		}

		/// <summary>
		/// Called when the entity is attacked.
		/// </summary>
		public override bool AttackEntityFrom(DamageSource par1DamageSource, int par2)
		{
			return true;
		}

		/// <summary>
		/// Sets the position and rotation. Only difference from the other one is no bounding on the rotation. Args: posX,
		/// posY, posZ, yaw, pitch
		/// </summary>
        public override void SetPositionAndRotation2(float par1, float par3, float par5, float par7, float par8, int par9)
		{
			OtherPlayerMPX = par1;
			OtherPlayerMPY = par3;
			OtherPlayerMPZ = par5;
			OtherPlayerMPYaw = par7;
			OtherPlayerMPPitch = par8;
			OtherPlayerMPPosRotationIncrements = par9;
		}

		/// <summary>
		/// Called to update the entity's position/logic.
		/// </summary>
		public override void OnUpdate()
		{
			Field_22062_y = 0.0F;
			base.OnUpdate();
			Field_705_Q = Field_704_R;
			double d = PosX - PrevPosX;
			double d1 = PosZ - PrevPosZ;
			float f = MathHelper2.Sqrt_double(d * d + d1 * d1) * 4F;

			if (f > 1.0F)
			{
				f = 1.0F;
			}

			Field_704_R += (f - Field_704_R) * 0.4F;
			Field_703_S += Field_704_R;

			if (!IsItemInUse && IsEating() && Inventory.MainInventory[Inventory.CurrentItem] != null)
			{
				ItemStack itemstack = Inventory.MainInventory[Inventory.CurrentItem];
				SetItemInUse(Inventory.MainInventory[Inventory.CurrentItem], Item.ItemsList[itemstack.ItemID].GetMaxItemUseDuration(itemstack));
				IsItemInUse = true;
			}
			else if (IsItemInUse && !IsEating())
			{
				ClearItemInUse();
				IsItemInUse = false;
			}
		}

		public override float GetShadowSize()
		{
			return 0.0F;
		}

		/// <summary>
		/// Called frequently so the entity can update its state every tick as required. For example, zombies and skeletons
		/// use this to react to sunlight and start to burn.
		/// </summary>
		public override void OnLivingUpdate()
		{
			base.UpdateEntityActionState();

			if (OtherPlayerMPPosRotationIncrements > 0)
			{
                float d = PosX + (OtherPlayerMPX - PosX) / OtherPlayerMPPosRotationIncrements;
                float d1 = PosY + (OtherPlayerMPY - PosY) / OtherPlayerMPPosRotationIncrements;
                float d2 = PosZ + (OtherPlayerMPZ - PosZ) / OtherPlayerMPPosRotationIncrements;
				double d3;

				for (d3 = OtherPlayerMPYaw - RotationYaw; d3 < -180D; d3 += 360D)
				{
				}

				for (; d3 >= 180D; d3 -= 360D)
				{
				}

				RotationYaw += (float)d3 / OtherPlayerMPPosRotationIncrements;
				RotationPitch += (float)(OtherPlayerMPPitch - RotationPitch) / OtherPlayerMPPosRotationIncrements;
				OtherPlayerMPPosRotationIncrements--;
				SetPosition(d, d1, d2);
				SetRotation(RotationYaw, RotationPitch);
			}

			PrevCameraYaw = CameraYaw;
			float f = MathHelper2.Sqrt_double(MotionX * MotionX + MotionZ * MotionZ);
			float f1 = (float)Math.Atan(-MotionY * 0.20000000298023224D) * 15F;

			if (f > 0.1F)
			{
				f = 0.1F;
			}

			if (!OnGround || GetHealth() <= 0)
			{
				f = 0.0F;
			}

			if (OnGround || GetHealth() <= 0)
			{
				f1 = 0.0F;
			}

			CameraYaw += (f - CameraYaw) * 0.4F;
			CameraPitch += (f1 - CameraPitch) * 0.8F;
		}

		/// <summary>
		/// Parameters: item slot, item ID, item damage. If slot >= 0 a new item will be generated with the specified item ID
		/// damage.
		/// </summary>
		public override void OutfitWithItem(int par1, int par2, int par3)
		{
			ItemStack itemstack = null;

			if (par2 >= 0)
			{
				itemstack = new ItemStack(par2, 1, par3);
			}

			if (par1 == 0)
			{
				Inventory.MainInventory[Inventory.CurrentItem] = itemstack;
			}
			else
			{
				Inventory.ArmorInventory[par1 - 1] = itemstack;
			}
		}

		public override void Func_6420_o()
		{
		}

		public override float GetEyeHeight()
		{
			return 1.82F;
		}
	}
}