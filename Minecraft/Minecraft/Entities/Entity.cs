using System;
using System.Collections.Generic;
using System.Text;

namespace net.minecraft.src
{
	public abstract class Entity
	{
		private static int NextEntityID = 0;
		public int EntityId;
        public float RenderDistanceWeight;

		/// <summary>
		/// Blocks entities from spawning when they do their AABB check to make sure the spot is clear of entities that can
		/// prevent spawning.
		/// </summary>
		public bool PreventEntitySpawning;

		/// <summary>
		/// The entity that is riding this entity </summary>
		public Entity RiddenByEntity;

		/// <summary>
		/// The entity we are currently riding </summary>
		public Entity RidingEntity;

		/// <summary>
		/// Reference to the World object. </summary>
		public World WorldObj;
        public float PrevPosX;
        public float PrevPosY;
        public float PrevPosZ;

		/// <summary>
		/// Entity position X </summary>
        public float PosX;

		/// <summary>
		/// Entity position Y </summary>
        public float PosY;

		/// <summary>
		/// Entity position Z </summary>
        public float PosZ;

		/// <summary>
		/// Entity motion X </summary>
        public float MotionX;

		/// <summary>
		/// Entity motion Y </summary>
        public float MotionY;

		/// <summary>
		/// Entity motion Z </summary>
        public float MotionZ;

		/// <summary>
		/// Entity rotation Yaw </summary>
		public float RotationYaw;

		/// <summary>
		/// Entity rotation Pitch </summary>
		public float RotationPitch;
		public float PrevRotationYaw;
		public float PrevRotationPitch;

		/// <summary>
		/// Axis aligned bounding box. </summary>
		public readonly AxisAlignedBB BoundingBox = AxisAlignedBB.GetBoundingBox(0.0F, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F);
		public bool OnGround;

		/// <summary>
		/// True if after a move this entity has collided with something on X- or Z-axis
		/// </summary>
		public bool IsCollidedHorizontally;

		/// <summary>
		/// True if after a move this entity has collided with something on Y-axis
		/// </summary>
		public bool IsCollidedVertically;

		/// <summary>
		/// True if after a move this entity has collided with something either vertically or horizontally
		/// </summary>
		public bool IsCollided;
		public bool VelocityChanged;
		protected bool IsInWeb;
		public bool Field_9293_aM;

		/// <summary>
		/// gets set by setEntityDead, so this must be the flag whether an Entity is dead (inactive may be better term)
		/// </summary>
		public bool IsDead;
		public float YOffset;

		/// <summary>
		/// How wide this entity is considered to be </summary>
		public float Width;

		/// <summary>
		/// How high this entity is considered to be </summary>
		public float Height;

		/// <summary>
		/// The previous ticks distance walked multiplied by 0.6 </summary>
		public float PrevDistanceWalkedModified;

		/// <summary>
		/// The distance walked multiplied by 0.6 </summary>
		public float DistanceWalkedModified;
		public float FallDistance;

		/// <summary>
		/// The distance that has to be exceeded in order to triger a new step sound and an onEntityWalking event on a block
		/// </summary>
		private int NextStepDistance;

		/// <summary>
		/// The entity's X coordinate at the previous tick, used to calculate position during rendering routines
		/// </summary>
        public float LastTickPosX;

		/// <summary>
		/// The entity's Y coordinate at the previous tick, used to calculate position during rendering routines
		/// </summary>
        public float LastTickPosY;

		/// <summary>
		/// The entity's Z coordinate at the previous tick, used to calculate position during rendering routines
		/// </summary>
        public float LastTickPosZ;

		public float YSize;

		/// <summary>
		/// How high this entity can step up when running into a block to try to get over it (currently make note the entity
		/// will always step up this amount and not just the amount needed)
		/// </summary>
		public float StepHeight;

		/// <summary>
		/// Whether this entity won't clip with collision or not (make note it won't disable gravity)
		/// </summary>
		public bool NoClip;

		/// <summary>
		/// Reduces the velocity applied by entity collisions by the specified percent.
		/// </summary>
		public float EntityCollisionReduction;
		protected Random Rand;

		/// <summary>
		/// How many ticks has this entity had ran since being alive </summary>
		public int TicksExisted;

		/// <summary>
		/// The amount of ticks you have to stand inside of fire before be set on fire
		/// </summary>
		public int FireResistance;
		private int Fire;

		/// <summary>
		/// Whether this entity is currently inside of water (if it handles water movement that is)
		/// </summary>
		protected bool InWater;
		public int HeartsLife;
		private bool FirstUpdate;

		/// <summary>
		/// downloadable location of player's skin </summary>
		public string SkinUrl;

		/// <summary>
		/// downloadable location of player's cloak </summary>
		public string CloakUrl;
		protected bool isImmuneToFire_Renamed;
		protected DataWatcher DataWatcher;
        private float EntityRiderPitchDelta;
        private float EntityRiderYawDelta;

		/// <summary>
		/// Has this entity been added to the chunk its within </summary>
		public bool AddedToChunk;
		public int ChunkCoordX;
		public int ChunkCoordY;
		public int ChunkCoordZ;
		public int ServerPosX;
		public int ServerPosY;
		public int ServerPosZ;

		/// <summary>
		/// Render entity even if it is outside the camera frustum. Only true in EntityFish for now. Used in RenderGlobal:
		/// render if ignoreFrustumCheck or in frustum.
		/// </summary>
		public bool IgnoreFrustumCheck;
		public bool IsAirBorne;

		public Entity(World par1World)
		{
			EntityId = NextEntityID++;
			RenderDistanceWeight = 1.0F;
			PreventEntitySpawning = false;
			OnGround = false;
			IsCollided = false;
			VelocityChanged = false;
			Field_9293_aM = true;
			IsDead = false;
			YOffset = 0.0F;
			Width = 0.6F;
			Height = 1.8F;
			PrevDistanceWalkedModified = 0.0F;
			DistanceWalkedModified = 0.0F;
			FallDistance = 0.0F;
			NextStepDistance = 1;
			YSize = 0.0F;
			StepHeight = 0.0F;
			NoClip = false;
			EntityCollisionReduction = 0.0F;
			Rand = new Random();
			TicksExisted = 0;
			FireResistance = 1;
			Fire = 0;
			InWater = false;
			HeartsLife = 0;
			FirstUpdate = true;
			isImmuneToFire_Renamed = false;
			DataWatcher = new DataWatcher();
			AddedToChunk = false;
			WorldObj = par1World;
			SetPosition(0.0F, 0.0F, 0.0F);
			DataWatcher.AddObject(0, (byte)0);
			DataWatcher.AddObject(1, (short)300);
			EntityInit();
		}

		protected abstract void EntityInit();

		public virtual DataWatcher GetDataWatcher()
		{
			return DataWatcher;
		}

		public override bool Equals(object par1Obj)
		{
			if (par1Obj is Entity)
			{
				return ((Entity)par1Obj).EntityId == EntityId;
			}
			else
			{
				return false;
			}
		}

		public override int GetHashCode()
		{
			return EntityId;
		}

		/// <summary>
		/// Keeps moving the entity up so it isn't colliding with blocks and other requirements for this entity to be spawned
		/// (only actually used on players though its also on Entity)
		/// </summary>
        public virtual void PreparePlayerToSpawn()
		{
			if (WorldObj == null)
			{
				return;
			}

			do
			{
				if (PosY <= 0.0F)
				{
					break;
				}

				SetPosition(PosX, PosY, PosZ);

				if (WorldObj.GetCollidingBoundingBoxes(this, BoundingBox).Count == 0)
				{
					break;
				}

				PosY++;
			}
			while (true);

			MotionX = MotionY = MotionZ = 0.0F;
			RotationPitch = 0.0F;
		}

		/// <summary>
		/// Will get destroyed next tick.
		/// </summary>
		public virtual void SetDead()
		{
			IsDead = true;
		}

		/// <summary>
		/// Sets the width and height of the entity. Args: width, height
		/// </summary>
		protected virtual void SetSize(float par1, float par2)
		{
			Width = par1;
			Height = par2;
		}

		/// <summary>
		/// Sets the rotation of the entity
		/// </summary>
		protected virtual void SetRotation(float par1, float par2)
		{
			RotationYaw = par1 % 360F;
			RotationPitch = par2 % 360F;
		}

		/// <summary>
		/// Sets the x,y,z of the entity from the given parameters. Also seems to set up a bounding box.
		/// </summary>
        public virtual void SetPosition(float par1, float par3, float par5)
		{
			PosX = par1;
			PosY = par3;
			PosZ = par5;
			float f = Width / 2.0F;
			float f1 = Height;
			BoundingBox.SetBounds(par1 - f, (par3 - YOffset) + YSize, par5 - f, par1 + f, (par3 - YOffset) + YSize + f1, par5 + f);
		}

		public virtual void SetAngles(float par1, float par2)
		{
			float f = RotationPitch;
			float f1 = RotationYaw;
			RotationYaw += par1 * 0.14999999999999999F;
			RotationPitch -= par2 * 0.14999999999999999F;

			if (RotationPitch < -90F)
			{
				RotationPitch = -90F;
			}

			if (RotationPitch > 90F)
			{
				RotationPitch = 90F;
			}

			PrevRotationPitch += RotationPitch - f;
			PrevRotationYaw += RotationYaw - f1;
		}

		/// <summary>
		/// Called to update the entity's position/logic.
		/// </summary>
		public virtual void OnUpdate()
		{
			OnEntityUpdate();
		}

		/// <summary>
		/// Gets called every tick from main Entity class
		/// </summary>
		public virtual void OnEntityUpdate()
		{
			Profiler.StartSection("entityBaseTick");

			if (RidingEntity != null && RidingEntity.IsDead)
			{
				RidingEntity = null;
			}

			TicksExisted++;
			PrevDistanceWalkedModified = DistanceWalkedModified;
			PrevPosX = PosX;
			PrevPosY = PosY;
			PrevPosZ = PosZ;
			PrevRotationPitch = RotationPitch;
			PrevRotationYaw = RotationYaw;

			if (IsSprinting() && !IsInWater())
			{
				int i = MathHelper2.Floor_double(PosX);
				int j = MathHelper2.Floor_double(PosY - 0.20000000298023224D - (double)YOffset);
				int k = MathHelper2.Floor_double(PosZ);
				int j1 = WorldObj.GetBlockId(i, j, k);

				if (j1 > 0)
				{
					WorldObj.SpawnParticle((new StringBuilder()).Append("tilecrack_").Append(j1).ToString(), PosX + (Rand.NextFloat() - 0.5F) * Width, BoundingBox.MinY + 0.10000000000000001F, PosZ + (Rand.NextFloat() - 0.5F) * Width, -MotionX * 4F, 1.5F, -MotionZ * 4F);
				}
			}

			if (HandleWaterMovement())
			{
				if (!InWater && !FirstUpdate)
				{
					float f = MathHelper2.Sqrt_double(MotionX * MotionX * 0.20000000298023224D + MotionY * MotionY + MotionZ * MotionZ * 0.20000000298023224D) * 0.2F;

					if (f > 1.0F)
					{
						f = 1.0F;
					}

					WorldObj.PlaySoundAtEntity(this, "random.splash", f, 1.0F + (Rand.NextFloat() - Rand.NextFloat()) * 0.4F);
					float f1 = MathHelper2.Floor_double(BoundingBox.MinY);

					for (int l = 0; (float)l < 1.0F + Width * 20F; l++)
					{
						float f2 = (Rand.NextFloat() * 2.0F - 1.0F) * Width;
						float f4 = (Rand.NextFloat() * 2.0F - 1.0F) * Width;
						WorldObj.SpawnParticle("bubble", PosX + f2, f1 + 1.0F, PosZ + f4, MotionX, MotionY - (Rand.NextFloat() * 0.2F), MotionZ);
					}

					for (int i1 = 0; (float)i1 < 1.0F + Width * 20F; i1++)
					{
						float f3 = (Rand.NextFloat() * 2.0F - 1.0F) * Width;
						float f5 = (Rand.NextFloat() * 2.0F - 1.0F) * Width;
						WorldObj.SpawnParticle("splash", PosX + f3, f1 + 1.0F, PosZ + f5, MotionX, MotionY, MotionZ);
					}
				}

				FallDistance = 0.0F;
				InWater = true;
				Fire = 0;
			}
			else
			{
				InWater = false;
			}

			if (WorldObj.IsRemote)
			{
				Fire = 0;
			}
			else if (Fire > 0)
			{
				if (isImmuneToFire_Renamed)
				{
					Fire -= 4;

					if (Fire < 0)
					{
						Fire = 0;
					}
				}
				else
				{
					if (Fire % 20 == 0)
					{
						AttackEntityFrom(DamageSource.OnFire, 1);
					}

					Fire--;
				}
			}

			if (HandleLavaMovement())
			{
				SetOnFireFromLava();
				FallDistance *= 0.5F;
			}

			if (PosY < -64D)
			{
				Kill();
			}

			if (!WorldObj.IsRemote)
			{
				SetFlag(0, Fire > 0);
				SetFlag(2, RidingEntity != null);
			}

			FirstUpdate = false;
			Profiler.EndSection();
		}

		/// <summary>
		/// Called whenever the entity is walking inside of lava.
		/// </summary>
		protected virtual void SetOnFireFromLava()
		{
			if (!isImmuneToFire_Renamed)
			{
				AttackEntityFrom(DamageSource.Lava, 4);
				SetFire(15);
			}
		}

		/// <summary>
		/// Sets entity to burn for x amount of seconds, cannot lower amount of existing fire.
		/// </summary>
		public virtual void SetFire(int par1)
		{
			int i = par1 * 20;

			if (Fire < i)
			{
				Fire = i;
			}
		}

		/// <summary>
		/// Removes fire from entity.
		/// </summary>
		public virtual void Extinguish()
		{
			Fire = 0;
		}

		/// <summary>
		/// sets the dead flag. Used when you fall off the bottom of the world.
		/// </summary>
		protected virtual void Kill()
		{
			SetDead();
		}

		/// <summary>
		/// Checks if the offset position from the entity's current position is inside of liquid. Args: x, y, z
		/// </summary>
        public virtual bool IsOffsetPositionInLiquid(float par1, float par3, float par5)
		{
			AxisAlignedBB axisalignedbb = BoundingBox.GetOffsetBoundingBox(par1, par3, par5);
			List<AxisAlignedBB> list = WorldObj.GetCollidingBoundingBoxes(this, axisalignedbb);

			if (list.Count > 0)
			{
				return false;
			}

			return !WorldObj.IsAnyLiquid(axisalignedbb);
		}

		/// <summary>
		/// Tries to moves the entity by the passed in displacement. Args: x, y, z
		/// </summary>
        public virtual void MoveEntity(float x, float y, float z)
		{
			if (NoClip)
			{
				BoundingBox.Offset(x, y, z);
				PosX = (BoundingBox.MinX + BoundingBox.MaxX) / 2F;
				PosY = (BoundingBox.MinY + YOffset) - YSize;
				PosZ = (BoundingBox.MinZ + BoundingBox.MaxZ) / 2F;
				return;
			}

			Profiler.StartSection("move");
			YSize *= 0.4F;
            float oldX = PosX;
            float oldZ = PosZ;

			if (IsInWeb)
			{
				IsInWeb = false;
				x *= 0.25F;
				y *= 0.05000000074505806F;
				z *= 0.25F;
				MotionX = 0.0F;
				MotionY = 0.0F;
				MotionZ = 0.0F;
			}

            float d2 = x;
            float d3 = y;
            float d4 = z;
			AxisAlignedBB axisalignedbb = BoundingBox.Copy();
			bool flag = OnGround && IsSneaking() && (this is EntityPlayer);

			if (flag)
			{
                float d5 = 0.050000000000000003F;

				for (; x != 0.0F && WorldObj.GetCollidingBoundingBoxes(this, BoundingBox.GetOffsetBoundingBox(x, -1, 0.0F)).Count == 0; d2 = x)
				{
					if (x < d5 && x >= -d5)
					{
						x = 0.0F;
						continue;
					}

					if (x > 0.0F)
					{
						x -= d5;
					}
					else
					{
						x += d5;
					}
				}

				for (; z != 0.0F && WorldObj.GetCollidingBoundingBoxes(this, BoundingBox.GetOffsetBoundingBox(0.0F, -1, z)).Count == 0; d4 = z)
				{
					if (z < d5 && z >= -d5)
					{
						z = 0.0F;
						continue;
					}

					if (z > 0.0F)
					{
						z -= d5;
					}
					else
					{
						z += d5;
					}
				}

				while (x != 0.0F && z != 0.0F && WorldObj.GetCollidingBoundingBoxes(this, BoundingBox.GetOffsetBoundingBox(x, -1, z)).Count == 0)
				{
					if (x < d5 && x >= -d5)
					{
						x = 0.0F;
					}
					else if (x > 0.0F)
					{
						x -= d5;
					}
					else
					{
						x += d5;
					}

					if (z < d5 && z >= -d5)
					{
						z = 0.0F;
					}
					else if (z > 0.0F)
					{
						z -= d5;
					}
					else
					{
						z += d5;
					}

					d2 = x;
					d4 = z;
				}
			}

			List<AxisAlignedBB> list = WorldObj.GetCollidingBoundingBoxes(this, BoundingBox.AddCoord(x, y, z));

			for (int i = 0; i < list.Count; i++)
			{
				y = list[i].CalculateYOffset(BoundingBox, y);
			}

			BoundingBox.Offset(0.0F, y, 0.0F);

			if (!Field_9293_aM && d3 != y)
			{
				x = y = z = 0.0F;
			}

			bool flag1 = OnGround || d3 != y && d3 < 0.0F;

			for (int j = 0; j < list.Count; j++)
			{
				x = list[j].CalculateXOffset(BoundingBox, x);
			}

			BoundingBox.Offset(x, 0.0F, 0.0F);

			if (!Field_9293_aM && d2 != x)
			{
				x = y = z = 0.0F;
			}

			for (int k = 0; k < list.Count; k++)
			{
				z = list[k].CalculateZOffset(BoundingBox, z);
			}

			BoundingBox.Offset(0.0F, 0.0F, z);

			if (!Field_9293_aM && d4 != z)
			{
				x = y = z = 0.0F;
			}

			if (StepHeight > 0.0F && flag1 && (flag || YSize < 0.05F) && (d2 != x || d4 != z))
			{
                float d6 = x;
                float d8 = y;
                float d10 = z;
				x = d2;
				y = StepHeight;
				z = d4;
				AxisAlignedBB axisalignedbb1 = BoundingBox.Copy();
				BoundingBox.SetBB(axisalignedbb);
				List<AxisAlignedBB> list1 = WorldObj.GetCollidingBoundingBoxes(this, BoundingBox.AddCoord(x, y, z));

				for (int j2 = 0; j2 < list1.Count; j2++)
				{
					y = list1[j2].CalculateYOffset(BoundingBox, y);
				}

				BoundingBox.Offset(0.0F, y, 0.0F);

				if (!Field_9293_aM && d3 != y)
				{
					x = y = z = 0.0F;
				}

				for (int k2 = 0; k2 < list1.Count; k2++)
				{
					x = list1[k2].CalculateXOffset(BoundingBox, x);
				}

				BoundingBox.Offset(x, 0.0F, 0.0F);

				if (!Field_9293_aM && d2 != x)
				{
					x = y = z = 0.0F;
				}

				for (int l2 = 0; l2 < list1.Count; l2++)
				{
					z = list1[l2].CalculateZOffset(BoundingBox, z);
				}

				BoundingBox.Offset(0.0F, 0.0F, z);

				if (!Field_9293_aM && d4 != z)
				{
					x = y = z = 0.0F;
				}

				if (!Field_9293_aM && d3 != y)
				{
					x = y = z = 0.0F;
				}
				else
				{
					y = -StepHeight;

					for (int i3 = 0; i3 < list1.Count; i3++)
					{
						y = list1[i3].CalculateYOffset(BoundingBox, y);
					}

					BoundingBox.Offset(0.0F, y, 0.0F);
				}

				if (d6 * d6 + d10 * d10 >= x * x + z * z)
				{
					x = d6;
					y = d8;
					z = d10;
					BoundingBox.SetBB(axisalignedbb1);
				}
				else
				{
                    float d11 = BoundingBox.MinY - (float)(int)BoundingBox.MinY;

					if (d11 > 0.0F)
					{
						YSize += (float)d11 + 0.01F;
					}
				}
			}

			Profiler.EndSection();
			Profiler.StartSection("rest");
			PosX = (BoundingBox.MinX + BoundingBox.MaxX) / 2F;
			PosY = (BoundingBox.MinY + YOffset) - YSize;
			PosZ = (BoundingBox.MinZ + BoundingBox.MaxZ) / 2F;
			IsCollidedHorizontally = d2 != x || d4 != z;
			IsCollidedVertically = d3 != y;
			OnGround = d3 != y && d3 < 0.0F;
			IsCollided = IsCollidedHorizontally || IsCollidedVertically;
			UpdateFallState(y, OnGround);

			if (d2 != x)
			{
				MotionX = 0.0F;
			}

			if (d3 != y)
			{
				MotionY = 0.0F;
			}

			if (d4 != z)
			{
				MotionZ = 0.0F;
			}

            float diffX = PosX - oldX;
            float diffZ = PosZ - oldZ;

			if (CanTriggerWalking() && !flag && RidingEntity == null)
			{
				DistanceWalkedModified += (float)MathHelper2.Sqrt_double(diffX * diffX + diffZ * diffZ) * 0.59999999999999998F;
				int l = MathHelper2.Floor_double(PosX);
				int j1 = MathHelper2.Floor_double(PosY - 0.20000000298023224D - YOffset);
				int l1 = MathHelper2.Floor_double(PosZ);
				int j3 = WorldObj.GetBlockId(l, j1, l1);

				if (j3 == 0 && WorldObj.GetBlockId(l, j1 - 1, l1) == Block.Fence.BlockID)
				{
					j3 = WorldObj.GetBlockId(l, j1 - 1, l1);
				}

				if (DistanceWalkedModified > (float)NextStepDistance && j3 > 0)
				{
					NextStepDistance = (int)DistanceWalkedModified + 1;
					PlayStepSound(l, j1, l1, j3);
					Block.BlocksList[j3].OnEntityWalking(WorldObj, l, j1, l1, this);
				}
			}

			int i1 = MathHelper2.Floor_double(BoundingBox.MinX + 0.001D);
			int k1 = MathHelper2.Floor_double(BoundingBox.MinY + 0.001D);
			int i2 = MathHelper2.Floor_double(BoundingBox.MinZ + 0.001D);
			int k3 = MathHelper2.Floor_double(BoundingBox.MaxX - 0.001D);
			int l3 = MathHelper2.Floor_double(BoundingBox.MaxY - 0.001D);
			int i4 = MathHelper2.Floor_double(BoundingBox.MaxZ - 0.001D);

			if (WorldObj.CheckChunksExist(i1, k1, i2, k3, l3, i4))
			{
				for (int j4 = i1; j4 <= k3; j4++)
				{
					for (int k4 = k1; k4 <= l3; k4++)
					{
						for (int l4 = i2; l4 <= i4; l4++)
						{
							int i5 = WorldObj.GetBlockId(j4, k4, l4);

							if (i5 > 0)
							{
								Block.BlocksList[i5].OnEntityCollidedWithBlock(WorldObj, j4, k4, l4, this);
							}
						}
					}
				}
			}

			bool flag2 = IsWet();

			if (WorldObj.IsBoundingBoxBurning(BoundingBox.Contract(0.001F, 0.001F, 0.001F)))
			{
				DealFireDamage(1);

				if (!flag2)
				{
					Fire++;

					if (Fire == 0)
					{
						SetFire(8);
					}
				}
			}
			else if (Fire <= 0)
			{
				Fire = -FireResistance;
			}

			if (flag2 && Fire > 0)
			{
				WorldObj.PlaySoundAtEntity(this, "random.fizz", 0.7F, 1.6F + (Rand.NextFloat() - Rand.NextFloat()) * 0.4F);
				Fire = -FireResistance;
			}

			Profiler.EndSection();
		}

		/// <summary>
		/// Plays step sound at given x, y, z for the entity
		/// </summary>
		protected virtual void PlayStepSound(int par1, int par2, int par3, int par4)
		{
			StepSound stepsound = Block.BlocksList[par4].StepSound;

			if (WorldObj.GetBlockId(par1, par2 + 1, par3) == Block.Snow.BlockID)
			{
				stepsound = Block.Snow.StepSound;
				WorldObj.PlaySoundAtEntity(this, stepsound.GetStepSound(), stepsound.GetVolume() * 0.15F, stepsound.GetPitch());
			}
			else if (!Block.BlocksList[par4].BlockMaterial.IsLiquid())
			{
				WorldObj.PlaySoundAtEntity(this, stepsound.GetStepSound(), stepsound.GetVolume() * 0.15F, stepsound.GetPitch());
			}
		}

		/// <summary>
		/// returns if this entity triggers Block.onEntityWalking on the blocks they walk on. used for spiders and wolves to
		/// prevent them from trampling crops
		/// </summary>
		protected virtual bool CanTriggerWalking()
		{
			return true;
		}

		/// <summary>
		/// Takes in the distance the entity has fallen this tick and whether its on the ground to update the fall distance
		/// and deal fall damage if landing on the ground.  Args: distanceFallenThisTick, onGround
		/// </summary>
        protected virtual void UpdateFallState(float par1, bool par3)
		{
			if (par3)
			{
				if (FallDistance > 0.0F)
				{
					if (this is EntityLiving)
					{
						int i = MathHelper2.Floor_double(PosX);
						int j = MathHelper2.Floor_double(PosY - 0.20000000298023224D - YOffset);
						int k = MathHelper2.Floor_double(PosZ);
						int l = WorldObj.GetBlockId(i, j, k);

						if (l == 0 && WorldObj.GetBlockId(i, j - 1, k) == Block.Fence.BlockID)
						{
							l = WorldObj.GetBlockId(i, j - 1, k);
						}

						if (l > 0)
						{
							Block.BlocksList[l].OnFallenUpon(WorldObj, i, j, k, this, FallDistance);
						}
					}

					Fall(FallDistance);
					FallDistance = 0.0F;
				}
			}
			else if (par1 < 0.0F)
			{
				FallDistance -= par1;
			}
		}

		/// <summary>
		/// returns the bounding box for this entity
		/// </summary>
		public virtual AxisAlignedBB GetBoundingBox()
		{
			return null;
		}

		/// <summary>
		/// Will deal the specified amount of damage to the entity if the entity isn't immune to fire damage. Args:
		/// amountDamage
		/// </summary>
		protected virtual void DealFireDamage(int par1)
		{
			if (!isImmuneToFire_Renamed)
			{
				AttackEntityFrom(DamageSource.InFire, par1);
			}
		}

		public bool IsImmuneToFire()
		{
			return isImmuneToFire_Renamed;
		}

		/// <summary>
		/// Called when the mob is falling. Calculates and applies fall damage.
		/// </summary>
		protected virtual void Fall(float par1)
		{
			if (RiddenByEntity != null)
			{
				RiddenByEntity.Fall(par1);
			}
		}

		/// <summary>
		/// Checks if this entity is either in water or on an open air block in rain (used in wolves).
		/// </summary>
		public virtual bool IsWet()
		{
			return InWater || WorldObj.CanLightningStrikeAt(MathHelper2.Floor_double(PosX), MathHelper2.Floor_double(PosY), MathHelper2.Floor_double(PosZ));
		}

		/// <summary>
		/// Checks if this entity is inside water (if inWater field is true as a result of handleWaterMovement() returning
		/// true)
		/// </summary>
		public virtual bool IsInWater()
		{
			return InWater;
		}

		/// <summary>
		/// Returns if this entity is in water and will end up adding the waters velocity to the entity
		/// </summary>
		public virtual bool HandleWaterMovement()
		{
			return WorldObj.HandleMaterialAcceleration(BoundingBox.Expand(0.0F, -0.40000000596046448F, 0.0F).Contract(0.001F, 0.001F, 0.001F), Material.Water, this);
		}

		/// <summary>
		/// Checks if the current block the entity is within of the specified material type
		/// </summary>
		public virtual bool IsInsideOfMaterial(Material par1Material)
		{
            float d = PosY + GetEyeHeight();
			int i = MathHelper2.Floor_double(PosX);
			int j = MathHelper2.Floor_float(MathHelper2.Floor_double(d));
			int k = MathHelper2.Floor_double(PosZ);
			int l = WorldObj.GetBlockId(i, j, k);

			if (l != 0 && Block.BlocksList[l].BlockMaterial == par1Material)
			{
				float f = BlockFluid.GetFluidHeightPercent(WorldObj.GetBlockMetadata(i, j, k)) - 0.1111111F;
				float f1 = (j + 1) - f;
				return d < f1;
			}
			else
			{
				return false;
			}
		}

		public virtual float GetEyeHeight()
		{
			return 0.0F;
		}

		/// <summary>
		/// Whether or not the current entity is in lava
		/// </summary>
		public virtual bool HandleLavaMovement()
		{
			return WorldObj.IsMaterialInBB(BoundingBox.Expand(-0.10000000149011612F, -0.40000000596046448F, -0.10000000149011612F), Material.Lava);
		}

		/// <summary>
		/// Used in both water and by flying objects
		/// </summary>
		public virtual void MoveFlying(float par1, float par2, float par3)
		{
			float f = MathHelper2.Sqrt_float(par1 * par1 + par2 * par2);

			if (f < 0.01F)
			{
				return;
			}

			if (f < 1.0F)
			{
				f = 1.0F;
			}

			f = par3 / f;
			par1 *= f;
			par2 *= f;
			float f1 = MathHelper2.Sin((RotationYaw * (float)Math.PI) / 180F);
			float f2 = MathHelper2.Cos((RotationYaw * (float)Math.PI) / 180F);
			MotionX += par1 * f2 - par2 * f1;
			MotionZ += par2 * f2 + par1 * f1;
		}

		public virtual int GetBrightnessForRender(float par1)
		{
			int i = MathHelper2.Floor_double(PosX);
			int j = MathHelper2.Floor_double(PosZ);

			if (WorldObj.BlockExists(i, 0, j))
			{
                float d = (BoundingBox.MaxY - BoundingBox.MinY) * 0.66000000000000003F;
				int k = MathHelper2.Floor_double((PosY - YOffset) + d);
				return WorldObj.GetLightBrightnessForSkyBlocks(i, k, j, 0);
			}
			else
			{
				return 0;
			}
		}

		/// <summary>
		/// Gets how bright this entity is.
		/// </summary>
		public virtual float GetBrightness(float par1)
		{
			int i = MathHelper2.Floor_double(PosX);
			int j = MathHelper2.Floor_double(PosZ);

			if (WorldObj.BlockExists(i, 0, j))
			{
                float d = (BoundingBox.MaxY - BoundingBox.MinY) * 0.66000000000000003F;
				int k = MathHelper2.Floor_double((PosY - YOffset) + d);
				return WorldObj.GetLightBrightness(i, k, j);
			}
			else
			{
				return 0.0F;
			}
		}

		/// <summary>
		/// Sets the reference to the World object.
		/// </summary>
		public virtual void SetWorld(World par1World)
		{
			WorldObj = par1World;
		}

		/// <summary>
		/// Sets the entity's position and rotation. Args: posX, posY, posZ, yaw, pitch
		/// </summary>
        public virtual void SetPositionAndRotation(float par1, float par3, float par5, float par7, float par8)
		{
			PrevPosX = PosX = par1;
			PrevPosY = PosY = par3;
			PrevPosZ = PosZ = par5;
			PrevRotationYaw = RotationYaw = par7;
			PrevRotationPitch = RotationPitch = par8;
			YSize = 0.0F;
            float d = PrevRotationYaw - par7;

			if (d < -180D)
			{
				PrevRotationYaw += 360F;
			}

			if (d >= 180D)
			{
				PrevRotationYaw -= 360F;
			}

			SetPosition(PosX, PosY, PosZ);
			SetRotation(par7, par8);
		}

		/// <summary>
		/// Sets the location and Yaw/Pitch of an entity in the world
		/// </summary>
        public virtual void SetLocationAndAngles(float par1, float par3, float par5, float par7, float par8)
		{
			LastTickPosX = PrevPosX = PosX = par1;
			LastTickPosY = PrevPosY = PosY = par3 + YOffset;
			LastTickPosZ = PrevPosZ = PosZ = par5;
			RotationYaw = par7;
			RotationPitch = par8;
			SetPosition(PosX, PosY, PosZ);
		}

		/// <summary>
		/// Returns the distance to the entity. Args: entity
		/// </summary>
		public virtual float GetDistanceToEntity(Entity par1Entity)
		{
			float f = (PosX - par1Entity.PosX);
			float f1 = (PosY - par1Entity.PosY);
			float f2 = (PosZ - par1Entity.PosZ);
			return MathHelper2.Sqrt_float(f * f + f1 * f1 + f2 * f2);
		}

		/// <summary>
		/// Gets the squared distance to the position. Args: x, y, z
		/// </summary>
        public virtual float GetDistanceSq(float par1, float par3, float par5)
		{
            float d = PosX - par1;
            float d1 = PosY - par3;
            float d2 = PosZ - par5;
			return d * d + d1 * d1 + d2 * d2;
		}

		/// <summary>
		/// Gets the distance to the position. Args: x, y, z
		/// </summary>
        public virtual float GetDistance(float par1, float par3, float par5)
		{
            float d = PosX - par1;
            float d1 = PosY - par3;
            float d2 = PosZ - par5;
			return MathHelper2.Sqrt_double(d * d + d1 * d1 + d2 * d2);
		}

		/// <summary>
		/// Returns the squared distance to the entity. Args: entity
		/// </summary>
        public virtual float GetDistanceSqToEntity(Entity par1Entity)
		{
            float d = PosX - par1Entity.PosX;
            float d1 = PosY - par1Entity.PosY;
            float d2 = PosZ - par1Entity.PosZ;
			return d * d + d1 * d1 + d2 * d2;
		}

		/// <summary>
		/// Called by a player entity when they collide with an entity
		/// </summary>
		public virtual void OnCollideWithPlayer(EntityPlayer entityplayer)
		{
		}

		/// <summary>
		/// Applies a velocity to each of the entities pushing them away from each other. Args: entity
		/// </summary>
		public virtual void ApplyEntityCollision(Entity par1Entity)
		{
			if (par1Entity.RiddenByEntity == this || par1Entity.RidingEntity == this)
			{
				return;
			}

            float d = par1Entity.PosX - PosX;
            float d1 = par1Entity.PosZ - PosZ;
            float d2 = (float)MathHelper2.Abs_max(d, d1);

			if (d2 >= 0.0099999997764825821D)
			{
				d2 = MathHelper2.Sqrt_double(d2);
				d /= d2;
				d1 /= d2;
                float d3 = 1.0F / d2;

				if (d3 > 1.0F)
				{
					d3 = 1.0F;
				}

				d *= d3;
				d1 *= d3;
				d *= 0.05000000074505806F;
				d1 *= 0.05000000074505806F;
				d *= 1.0F - EntityCollisionReduction;
				d1 *= 1.0F - EntityCollisionReduction;
				AddVelocity(-d, 0.0F, -d1);
				par1Entity.AddVelocity(d, 0.0F, d1);
			}
		}

		/// <summary>
		/// Adds to the current velocity of the entity. Args: x, y, z
		/// </summary>
        public virtual void AddVelocity(float par1, float par3, float par5)
		{
			MotionX += par1;
			MotionY += par3;
			MotionZ += par5;
			IsAirBorne = true;
		}

		/// <summary>
		/// Sets that this entity has been attacked.
		/// </summary>
		protected virtual void SetBeenAttacked()
		{
			VelocityChanged = true;
		}

		/// <summary>
		/// Called when the entity is attacked.
		/// </summary>
		public virtual bool AttackEntityFrom(DamageSource par1DamageSource, int par2)
		{
			SetBeenAttacked();
			return false;
		}

		/// <summary>
		/// Returns true if other Entities should be prevented from moving through this Entity.
		/// </summary>
		public virtual bool CanBeCollidedWith()
		{
			return false;
		}

		/// <summary>
		/// Returns true if this entity should push and be pushed by other entities when colliding.
		/// </summary>
		public virtual bool CanBePushed()
		{
			return false;
		}

		/// <summary>
		/// Adds a value to the player score. Currently not actually used and the entity passed in does nothing. Args:
		/// entity, scoreToAdd
		/// </summary>
		public virtual void AddToPlayerScore(Entity entity, int i)
		{
		}

		/// <summary>
		/// Checks using a Vec3d to determine if this entity is within range of that vector to be rendered. Args: vec3D
		/// </summary>
		public virtual bool IsInRangeToRenderVec3D(Vec3D par1Vec3D)
		{
            float d = PosX - (float)par1Vec3D.XCoord;
            float d1 = PosY - (float)par1Vec3D.YCoord;
            float d2 = PosZ - (float)par1Vec3D.ZCoord;
            float d3 = d * d + d1 * d1 + d2 * d2;
			return IsInRangeToRenderDist(d3);
		}

		/// <summary>
		/// Checks if the entity is in range to render by using the past in distance and comparing it to its average edge
		/// length * 64 * renderDistanceWeight Args: distance
		/// </summary>
        public virtual bool IsInRangeToRenderDist(float par1)
		{
            float d = BoundingBox.GetAverageEdgeLength();
			d *= 64F * RenderDistanceWeight;
			return par1 < d * d;
		}

		/// <summary>
		/// Returns the texture's file path as a String.
		/// </summary>
		public virtual string GetTexture()
		{
			return null;
		}

		/// <summary>
		/// adds the ID of this entity to the NBT given
		/// </summary>
		public virtual bool AddEntityID(NBTTagCompound par1NBTTagCompound)
		{
			string s = GetEntityString();

			if (IsDead || s == null)
			{
				return false;
			}
			else
			{
				par1NBTTagCompound.SetString("id", s);
				WriteToNBT(par1NBTTagCompound);
				return true;
			}
		}

		/// <summary>
		/// Save the entity to NBT (calls an abstract helper method to write extra data)
		/// </summary>
		public virtual void WriteToNBT(NBTTagCompound par1NBTTagCompound)
		{
			par1NBTTagCompound.SetTag("Pos", NewDoubleNBTList(new double[] { PosX, PosY + (double)YSize, PosZ }));
			par1NBTTagCompound.SetTag("Motion", NewDoubleNBTList(new double[] { MotionX, MotionY, MotionZ }));
			par1NBTTagCompound.SetTag("Rotation", NewFloatNBTList(new float[] { RotationYaw, RotationPitch }));
			par1NBTTagCompound.SetFloat("FallDistance", FallDistance);
			par1NBTTagCompound.SetShort("Fire", (short)Fire);
			par1NBTTagCompound.SetShort("Air", (short)GetAir());
			par1NBTTagCompound.Setbool("OnGround", OnGround);
			WriteEntityToNBT(par1NBTTagCompound);
		}

		/// <summary>
		/// Reads the entity from NBT (calls an abstract helper method to read specialized data)
		/// </summary>
		public virtual void ReadFromNBT(NBTTagCompound par1NBTTagCompound)
		{
			NBTTagList nbttaglist = par1NBTTagCompound.GetTagList("Pos");
			NBTTagList nbttaglist1 = par1NBTTagCompound.GetTagList("Motion");
			NBTTagList nbttaglist2 = par1NBTTagCompound.GetTagList("Rotation");
			MotionX = (float)((NBTTagDouble)nbttaglist1.TagAt(0)).Data;
			MotionY = (float)((NBTTagDouble)nbttaglist1.TagAt(1)).Data;
			MotionZ = (float)((NBTTagDouble)nbttaglist1.TagAt(2)).Data;

			if (Math.Abs(MotionX) > 10F)
			{
				MotionX = 0.0F;
			}

			if (Math.Abs(MotionY) > 10F)
			{
				MotionY = 0.0F;
			}

			if (Math.Abs(MotionZ) > 10F)
			{
				MotionZ = 0.0F;
			}

			PrevPosX = LastTickPosX = PosX = (float)((NBTTagDouble)nbttaglist.TagAt(0)).Data;
			PrevPosY = LastTickPosY = PosY = (float)((NBTTagDouble)nbttaglist.TagAt(1)).Data;
			PrevPosZ = LastTickPosZ = PosZ = (float)((NBTTagDouble)nbttaglist.TagAt(2)).Data;
			PrevRotationYaw = RotationYaw = ((NBTTagFloat)nbttaglist2.TagAt(0)).Data;
			PrevRotationPitch = RotationPitch = ((NBTTagFloat)nbttaglist2.TagAt(1)).Data;
			FallDistance = par1NBTTagCompound.GetFloat("FallDistance");
			Fire = par1NBTTagCompound.GetShort("Fire");
			SetAir(par1NBTTagCompound.GetShort("Air"));
			OnGround = par1NBTTagCompound.Getbool("OnGround");
			SetPosition(PosX, PosY, PosZ);
			SetRotation(RotationYaw, RotationPitch);
			ReadEntityFromNBT(par1NBTTagCompound);
		}

		/// <summary>
		/// Returns the string that identifies this Entity's class
		/// </summary>
		protected string GetEntityString()
		{
			return EntityList.GetEntityString(this);
		}

		/// <summary>
		/// (abstract) Protected helper method to read subclass entity data from NBT.
		/// </summary>
		public abstract void ReadEntityFromNBT(NBTTagCompound nbttagcompound);

		/// <summary>
		/// (abstract) Protected helper method to write subclass entity data to NBT.
		/// </summary>
		public abstract void WriteEntityToNBT(NBTTagCompound nbttagcompound);

		/// <summary>
		/// creates a NBT list from the array of doubles passed to this function
		/// </summary>
		protected virtual NBTTagList NewDoubleNBTList(double[] par1ArrayOfDouble)
		{
			NBTTagList nbttaglist = new NBTTagList();
			double[] ad = par1ArrayOfDouble;
			int i = ad.Length;

			for (int j = 0; j < i; j++)
			{
				double d = ad[j];
				nbttaglist.AppendTag(new NBTTagDouble(null, d));
			}

			return nbttaglist;
		}

		/// <summary>
		/// Returns a new NBTTagList filled with the specified floats
		/// </summary>
		protected virtual NBTTagList NewFloatNBTList(float[] par1ArrayOfFloat)
		{
			NBTTagList nbttaglist = new NBTTagList();
			float[] af = par1ArrayOfFloat;
			int i = af.Length;

			for (int j = 0; j < i; j++)
			{
				float f = af[j];
				nbttaglist.AppendTag(new NBTTagFloat(null, f));
			}

			return nbttaglist;
		}

		public virtual float GetShadowSize()
		{
			return Height / 2.0F;
		}

		/// <summary>
		/// Drops an item stack at the entity's position. Args: itemID, count
		/// </summary>
		public virtual EntityItem DropItem(int par1, int par2)
		{
			return DropItemWithOffset(par1, par2, 0.0F);
		}

		/// <summary>
		/// Drops an item stack with a specified y offset. Args: itemID, count, yOffset
		/// </summary>
		public virtual EntityItem DropItemWithOffset(int par1, int par2, float par3)
		{
			return EntityDropItem(new ItemStack(par1, par2, 0), par3);
		}

		/// <summary>
		/// Drops an item at the position of the entity.
		/// </summary>
		public virtual EntityItem EntityDropItem(ItemStack par1ItemStack, float par2)
		{
			EntityItem entityitem = new EntityItem(WorldObj, PosX, PosY + par2, PosZ, par1ItemStack);
			entityitem.DelayBeforeCanPickup = 10;
			WorldObj.SpawnEntityInWorld(entityitem);
			return entityitem;
		}

		/// <summary>
		/// Checks whether target entity is alive.
		/// </summary>
		public virtual bool IsEntityAlive()
		{
			return !IsDead;
		}

		/// <summary>
		/// Checks if this entity is inside of an opaque block
		/// </summary>
		public virtual bool IsEntityInsideOpaqueBlock()
		{
			for (int i = 0; i < 8; i++)
			{
				float f = ((float)((i >> 0) % 2) - 0.5F) * Width * 0.8F;
				float f1 = ((float)((i >> 1) % 2) - 0.5F) * 0.1F;
				float f2 = ((float)((i >> 2) % 2) - 0.5F) * Width * 0.8F;
				int j = MathHelper2.Floor_double(PosX + f);
				int k = MathHelper2.Floor_double(PosY + GetEyeHeight() + f1);
				int l = MathHelper2.Floor_double(PosZ + f2);

				if (WorldObj.IsBlockNormalCube(j, k, l))
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Called when a player interacts with a mob. e.g. gets milk from a cow, gets into the saddle on a pig.
		/// </summary>
		public virtual bool Interact(EntityPlayer par1EntityPlayer)
		{
			return false;
		}

		/// <summary>
		/// Returns a boundingBox used to collide the entity with other entities and blocks. This enables the entity to be
		/// pushable on contact, like boats or minecarts.
		/// </summary>
		public virtual AxisAlignedBB GetCollisionBox(Entity par1Entity)
		{
			return null;
		}

		/// <summary>
		/// Handles updating while being ridden by an entity
		/// </summary>
		public virtual void UpdateRidden()
		{
			if (RidingEntity.IsDead)
			{
				RidingEntity = null;
				return;
			}

			MotionX = 0.0F;
			MotionY = 0.0F;
			MotionZ = 0.0F;
			OnUpdate();

			if (RidingEntity == null)
			{
				return;
			}

			RidingEntity.UpdateRiderPosition();
			EntityRiderYawDelta += RidingEntity.RotationYaw - RidingEntity.PrevRotationYaw;
			EntityRiderPitchDelta += RidingEntity.RotationPitch - RidingEntity.PrevRotationPitch;

			for (; EntityRiderYawDelta >= 180F; EntityRiderYawDelta -= 360F)
			{
			}

			for (; EntityRiderYawDelta < -180F; EntityRiderYawDelta += 360F)
			{
			}

			for (; EntityRiderPitchDelta >= 180F; EntityRiderPitchDelta -= 360F)
			{
			}

			for (; EntityRiderPitchDelta < -180F; EntityRiderPitchDelta += 360F)
			{
			}

            float d = EntityRiderYawDelta * 0.5F;
            float d1 = EntityRiderPitchDelta * 0.5F;
			float f = 10F;

			if (d > f)
			{
				d = f;
			}

			if (d < -f)
			{
				d = -f;
			}

			if (d1 > f)
			{
				d1 = f;
			}

			if (d1 < -f)
			{
				d1 = -f;
			}

			EntityRiderYawDelta -= d;
			EntityRiderPitchDelta -= d1;
			RotationYaw += d;
			RotationPitch += d1;
		}

		public virtual void UpdateRiderPosition()
		{
			RiddenByEntity.SetPosition(PosX, PosY + GetMountedYOffset() + RiddenByEntity.GetYOffset(), PosZ);
		}

		/// <summary>
		/// Returns the Y Offset of this entity.
		/// </summary>
        public virtual float GetYOffset()
		{
			return YOffset;
		}

		/// <summary>
		/// Returns the Y offset from the entity's position for any entity riding this one.
		/// </summary>
        public virtual float GetMountedYOffset()
		{
			return Height * 0.75F;
		}

		/// <summary>
		/// Called when a player mounts an entity. e.g. mounts a pig, mounts a boat.
		/// </summary>
		public virtual void MountEntity(Entity par1Entity)
		{
			EntityRiderPitchDelta = 0.0F;
			EntityRiderYawDelta = 0.0F;

			if (par1Entity == null)
			{
				if (RidingEntity != null)
				{
					SetLocationAndAngles(RidingEntity.PosX, RidingEntity.BoundingBox.MinY + RidingEntity.Height, RidingEntity.PosZ, RotationYaw, RotationPitch);
					RidingEntity.RiddenByEntity = null;
				}

				RidingEntity = null;
				return;
			}

			if (RidingEntity == par1Entity)
			{
				RidingEntity.RiddenByEntity = null;
				RidingEntity = null;
				SetLocationAndAngles(par1Entity.PosX, par1Entity.BoundingBox.MinY + par1Entity.Height, par1Entity.PosZ, RotationYaw, RotationPitch);
				return;
			}

			if (RidingEntity != null)
			{
				RidingEntity.RiddenByEntity = null;
			}

			if (par1Entity.RiddenByEntity != null)
			{
				par1Entity.RiddenByEntity.RidingEntity = null;
			}

			RidingEntity = par1Entity;
			par1Entity.RiddenByEntity = this;
		}

		/// <summary>
		/// Sets the position and rotation. Only difference from the other one is no bounding on the rotation. Args: posX,
		/// posY, posZ, yaw, pitch
		/// </summary>
        public virtual void SetPositionAndRotation2(float par1, float par3, float par5, float par7, float par8, int par9)
		{
			SetPosition(par1, par3, par5);
			SetRotation(par7, par8);
			List<AxisAlignedBB> list = WorldObj.GetCollidingBoundingBoxes(this, BoundingBox.Contract(0.03125F, 0.0F, 0.03125F));

			if (list.Count > 0)
			{
                float d = 0.0F;

				for (int i = 0; i < list.Count; i++)
				{
					AxisAlignedBB axisalignedbb = list[i];

					if (axisalignedbb.MaxY > d)
					{
						d = axisalignedbb.MaxY;
					}
				}

				par3 += d - BoundingBox.MinY;
				SetPosition(par1, par3, par5);
			}
		}

		public virtual float GetCollisionBorderSize()
		{
			return 0.1F;
		}

		/// <summary>
		/// returns a (normalized) vector of where this entity is looking
		/// </summary>
		public virtual Vec3D GetLookVec()
		{
			return null;
		}

		/// <summary>
		/// Called by portal blocks when an entity is within it.
		/// </summary>
		public virtual void SetInPortal()
		{
		}

		/// <summary>
		/// Sets the velocity to the args. Args: x, y, z
		/// </summary>
        public virtual void SetVelocity(float par1, float par3, float par5)
		{
			MotionX = par1;
			MotionY = par3;
			MotionZ = par5;
		}

		public virtual void HandleHealthUpdate(byte byte0)
		{
		}

		/// <summary>
		/// Setups the entity to do the hurt animation. Only used by packets in multiplayer.
		/// </summary>
		public virtual void PerformHurtAnimation()
		{
		}

		public virtual void UpdateCloak()
		{
		}

		/// <summary>
		/// Parameters: item slot, item ID, item damage. If slot >= 0 a new item will be generated with the specified item ID
		/// damage.
		/// </summary>
		public virtual void OutfitWithItem(int i, int j, int k)
		{
		}

		/// <summary>
		/// Returns true if the entity is on fire. Used by render to add the fire effect on rendering.
		/// </summary>
		public virtual bool IsBurning()
		{
			return Fire > 0 || GetFlag(0);
		}

		/// <summary>
		/// Returns true if the entity is riding another entity, used by render to rotate the legs to be in 'sit' position
		/// for players.
		/// </summary>
		public virtual bool IsRiding()
		{
			return RidingEntity != null || GetFlag(2);
		}

		/// <summary>
		/// Returns if this entity is sneaking.
		/// </summary>
		public virtual bool IsSneaking()
		{
			return GetFlag(1);
		}

		/// <summary>
		/// Sets the sneaking flag.
		/// </summary>
		public virtual void SetSneaking(bool par1)
		{
			SetFlag(1, par1);
		}

		/// <summary>
		/// Get if the Entity is sprinting.
		/// </summary>
		public virtual bool IsSprinting()
		{
			return GetFlag(3);
		}

		/// <summary>
		/// Set sprinting switch for Entity.
		/// </summary>
		public virtual void SetSprinting(bool par1)
		{
			SetFlag(3, par1);
		}

		public virtual bool IsEating()
		{
			return GetFlag(4);
		}

		public virtual void SetEating(bool par1)
		{
			SetFlag(4, par1);
		}

		/// <summary>
		/// Returns true if the flag is active for the entity. Known flags: 0) is burning; 1) is sneaking; 2) is riding
		/// something; 3) is sprinting; 4) is eating
		/// </summary>
		protected virtual bool GetFlag(int par1)
		{
			return (DataWatcher.GetWatchableObjectByte(0) & 1 << par1) != 0;
		}

		/// <summary>
		/// Enable or disable a entity flag, see getEntityFlag to read the know flags.
		/// </summary>
		protected virtual void SetFlag(int par1, bool par2)
		{
			byte byte0 = DataWatcher.GetWatchableObjectByte(0);

			if (par2)
			{
				DataWatcher.UpdateObject(0, (byte)(byte0 | 1 << par1));
			}
			else
			{
				DataWatcher.UpdateObject(0, (byte)(byte0 & ~(1 << par1)));
			}
		}

		public virtual int GetAir()
		{
			return DataWatcher.GetWatchableObjectShort(1);
		}

		public virtual void SetAir(int par1)
		{
			DataWatcher.UpdateObject(1, (short)par1);
		}

		/// <summary>
		/// Called when a lightning bolt hits the entity.
		/// </summary>
		public virtual void OnStruckByLightning(EntityLightningBolt par1EntityLightningBolt)
		{
			DealFireDamage(5);
			Fire++;

			if (Fire == 0)
			{
				SetFire(8);
			}
		}

		/// <summary>
		/// This method gets called when the entity kills another one.
		/// </summary>
		public virtual void OnKillEntity(EntityLiving entityliving)
		{
		}

		/// <summary>
		/// Adds velocity to push the entity out of blocks at the specified x, y, z position Args: x, y, z
		/// </summary>
        protected virtual bool PushOutOfBlocks(float par1, float par3, float par5)
		{
			int i = MathHelper2.Floor_double(par1);
			int j = MathHelper2.Floor_double(par3);
			int k = MathHelper2.Floor_double(par5);
            float d = par1 - i;
            float d1 = par3 - j;
            float d2 = par5 - k;

			if (WorldObj.IsBlockNormalCube(i, j, k))
			{
				bool flag = !WorldObj.IsBlockNormalCube(i - 1, j, k);
				bool flag1 = !WorldObj.IsBlockNormalCube(i + 1, j, k);
				bool flag2 = !WorldObj.IsBlockNormalCube(i, j - 1, k);
				bool flag3 = !WorldObj.IsBlockNormalCube(i, j + 1, k);
				bool flag4 = !WorldObj.IsBlockNormalCube(i, j, k - 1);
				bool flag5 = !WorldObj.IsBlockNormalCube(i, j, k + 1);
				sbyte byte0 = -1;
                float d3 = 9999F;

				if (flag && d < d3)
				{
					d3 = d;
					byte0 = 0;
				}

				if (flag1 && 1.0F - d < d3)
				{
					d3 = 1.0F - d;
					byte0 = 1;
				}

				if (flag2 && d1 < d3)
				{
					d3 = d1;
					byte0 = 2;
				}

				if (flag3 && 1.0F - d1 < d3)
				{
					d3 = 1.0F - d1;
					byte0 = 3;
				}

				if (flag4 && d2 < d3)
				{
					d3 = d2;
					byte0 = 4;
				}

				if (flag5 && 1.0F - d2 < d3)
				{
                    float d4 = 1.0F - d2;
					byte0 = 5;
				}

				float f = Rand.NextFloat() * 0.2F + 0.1F;

				if (byte0 == 0)
				{
					MotionX = -f;
				}

				if (byte0 == 1)
				{
					MotionX = f;
				}

				if (byte0 == 2)
				{
					MotionY = -f;
				}

				if (byte0 == 3)
				{
					MotionY = f;
				}

				if (byte0 == 4)
				{
					MotionZ = -f;
				}

				if (byte0 == 5)
				{
					MotionZ = f;
				}

				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Sets the Entity inside a web block.
		/// </summary>
		public virtual void SetInWeb()
		{
			IsInWeb = true;
			FallDistance = 0.0F;
		}

		/// <summary>
		/// Return the Entity parts making up this Entity (currently only for dragons)
		/// </summary>
		public virtual Entity[] GetParts()
		{
			return null;
		}

		/// <summary>
		/// Returns true if Entity argument is equal to this Entity
		/// </summary>
		public virtual bool IsEntityEqual(Entity par1Entity)
		{
			return this == par1Entity;
		}

		public virtual void Func_48079_f(float f)
		{
		}

		/// <summary>
		/// If returns false, the item will not inflict any damage against entities.
		/// </summary>
		public virtual bool CanAttackWithItem()
		{
			return true;
		}
	}
}