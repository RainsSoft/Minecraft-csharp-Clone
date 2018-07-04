namespace net.minecraft.src
{
	public class EntityMagmaCube : EntitySlime
	{
		public EntityMagmaCube(World par1World) : base(par1World)
		{
			Texture = "/mob/lava.png";
			isImmuneToFire_Renamed = true;
			LandMovementFactor = 0.2F;
		}

		/// <summary>
		/// Checks if the entity's current position is a valid location to spawn this entity.
		/// </summary>
		public override bool GetCanSpawnHere()
		{
			return WorldObj.DifficultySetting > 0 && WorldObj.CheckIfAABBIsClear(BoundingBox) && WorldObj.GetCollidingBoundingBoxes(this, BoundingBox).Count == 0 && !WorldObj.IsAnyLiquid(BoundingBox);
		}

		/// <summary>
		/// Returns the current armor value as determined by a call to InventoryPlayer.getTotalArmorValue
		/// </summary>
		public override int GetTotalArmorValue()
		{
			return GetSlimeSize() * 3;
		}

		public override int GetBrightnessForRender(float par1)
		{
			return 0xf000f0;
		}

		/// <summary>
		/// Gets how bright this entity is.
		/// </summary>
		public override float GetBrightness(float par1)
		{
			return 1.0F;
		}

		/// <summary>
		/// Returns the name of a particle effect that may be randomly created by EntitySlime.onUpdate()
		/// </summary>
		protected override string GetSlimeParticle()
		{
			return "flame";
		}

		protected override EntitySlime CreateInstance()
		{
			return new EntityMagmaCube(WorldObj);
		}

		/// <summary>
		/// Returns the item ID for the item the mob drops on death.
		/// </summary>
		protected override int GetDropItemId()
		{
			return Item.MagmaCream.ShiftedIndex;
		}

		/// <summary>
		/// Drop 0-2 items of this living's type
		/// </summary>
		protected override void DropFewItems(bool par1, int par2)
		{
			int i = GetDropItemId();

			if (i > 0 && GetSlimeSize() > 1)
			{
				int j = Rand.Next(4) - 2;

				if (par2 > 0)
				{
					j += Rand.Next(par2 + 1);
				}

				for (int k = 0; k < j; k++)
				{
					DropItem(i, 1);
				}
			}
		}

		/// <summary>
		/// Returns true if the entity is on fire. Used by render to add the fire effect on rendering.
		/// </summary>
		public override bool IsBurning()
		{
			return false;
		}

		protected override int Func_40131_af()
		{
			return base.Func_40131_af() * 4;
		}

		protected override void Func_40136_ag()
		{
			Field_40139_a = Field_40139_a * 0.9F;
		}

		/// <summary>
		/// jump, Causes this entity to do an upwards motion (jumping)
		/// </summary>
		protected override void Jump()
		{
			MotionY = 0.42F + (float)GetSlimeSize() * 0.1F;
			IsAirBorne = true;
		}

		/// <summary>
		/// Called when the mob is falling. Calculates and applies fall damage.
		/// </summary>
		protected override void Fall(float f)
		{
		}

		protected override bool Func_40137_ah()
		{
			return true;
		}

		protected override int Func_40130_ai()
		{
			return base.Func_40130_ai() + 2;
		}

		/// <summary>
		/// Returns the sound this mob makes when it is hurt.
		/// </summary>
		protected override string GetHurtSound()
		{
			return "mob.slime";
		}

		/// <summary>
		/// Returns the sound this mob makes on death.
		/// </summary>
		protected override string GetDeathSound()
		{
			return "mob.slime";
		}

		protected override string Func_40138_aj()
		{
			if (GetSlimeSize() > 1)
			{
				return "mob.magmacube.big";
			}
			else
			{
				return "mob.magmacube.small";
			}
		}

		/// <summary>
		/// Whether or not the current entity is in lava
		/// </summary>
		public override bool HandleLavaMovement()
		{
			return false;
		}

		protected override bool Func_40134_ak()
		{
			return true;
		}
	}
}