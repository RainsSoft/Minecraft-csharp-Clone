using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class EntityPainting : Entity
	{
		private int TickCounter1;

		/// <summary>
		/// the direction the painting faces </summary>
		public int Direction;
		public int XPosition;
		public int YPosition;
		public int ZPosition;
		public Art Art;

		public EntityPainting(World par1World) : base(par1World)
		{
			TickCounter1 = 0;
			Direction = 0;
			YOffset = 0.0F;
			SetSize(0.5F, 0.5F);
		}

		public EntityPainting(World par1World, int par2, int par3, int par4, int par5) : this(par1World)
		{
			XPosition = par2;
			YPosition = par3;
			ZPosition = par4;
			List<Art> arraylist = new List<Art>();
			Art[] aenumart = Art.ArtArray;
			int i = aenumart.Length;

			for (int j = 0; j < i; j++)
			{
				Art enumart = aenumart[j];
				Art = enumart;
				Func_412_b(par5);

				if (OnValidSurface())
				{
					arraylist.Add(enumart);
				}
			}

			if (arraylist.Count > 0)
			{
				Art = (Art)arraylist[Rand.Next(arraylist.Count)];
			}

			Func_412_b(par5);
		}

		public EntityPainting(World par1World, int par2, int par3, int par4, int par5, string par6Str) : this(par1World)
		{
			XPosition = par2;
			YPosition = par3;
			ZPosition = par4;
			Art[] aenumart = Art.ArtArray;
			int i = aenumart.Length;
			int j = 0;

			do
			{
				if (j >= i)
				{
					break;
				}

				Art enumart = aenumart[j];

				if (enumart.Title.Equals(par6Str))
				{
					Art = enumart;
					break;
				}

				j++;
			}
			while (true);

			Func_412_b(par5);
		}

		protected override void EntityInit()
		{
		}

		public virtual void Func_412_b(int par1)
		{
			Direction = par1;
			PrevRotationYaw = RotationYaw = par1 * 90;
			float f = Art.SizeX;
			float f1 = Art.SizeY;
			float f2 = Art.SizeX;

			if (par1 == 0 || par1 == 2)
			{
				f2 = 0.5F;
			}
			else
			{
				f = 0.5F;
			}

			f /= 32F;
			f1 /= 32F;
			f2 /= 32F;
			float f3 = (float)XPosition + 0.5F;
			float f4 = (float)YPosition + 0.5F;
			float f5 = (float)ZPosition + 0.5F;
			float f6 = 0.5625F;

			if (par1 == 0)
			{
				f5 -= f6;
			}

			if (par1 == 1)
			{
				f3 -= f6;
			}

			if (par1 == 2)
			{
				f5 += f6;
			}

			if (par1 == 3)
			{
				f3 += f6;
			}

			if (par1 == 0)
			{
				f3 -= Func_411_c(Art.SizeX);
			}

			if (par1 == 1)
			{
				f5 += Func_411_c(Art.SizeX);
			}

			if (par1 == 2)
			{
				f3 += Func_411_c(Art.SizeX);
			}

			if (par1 == 3)
			{
				f5 -= Func_411_c(Art.SizeX);
			}

			f4 += Func_411_c(Art.SizeY);
			SetPosition(f3, f4, f5);
			float f7 = -0.00625F;
			BoundingBox.SetBounds(f3 - f - f7, f4 - f1 - f7, f5 - f2 - f7, f3 + f + f7, f4 + f1 + f7, f5 + f2 + f7);
		}

		private float Func_411_c(int par1)
		{
			if (par1 == 32)
			{
				return 0.5F;
			}

			return par1 != 64 ? 0.0F : 0.5F;
		}

		/// <summary>
		/// Called to update the entity's position/logic.
		/// </summary>
		public override void OnUpdate()
		{
			if (TickCounter1++ == 100 && !WorldObj.IsRemote)
			{
				TickCounter1 = 0;

				if (!IsDead && !OnValidSurface())
				{
					SetDead();
					WorldObj.SpawnEntityInWorld(new EntityItem(WorldObj, PosX, PosY, PosZ, new ItemStack(Item.Painting)));
				}
			}
		}

		/// <summary>
		/// checks to make sure painting can be placed there
		/// </summary>
		public virtual bool OnValidSurface()
		{
			if (WorldObj.GetCollidingBoundingBoxes(this, BoundingBox).Count > 0)
			{
				return false;
			}

			int i = Art.SizeX / 16;
			int j = Art.SizeY / 16;
			int k = XPosition;
			int l = YPosition;
			int i1 = ZPosition;

			if (Direction == 0)
			{
				k = MathHelper2.Floor_double(PosX - (double)((float)Art.SizeX / 32F));
			}

			if (Direction == 1)
			{
				i1 = MathHelper2.Floor_double(PosZ - (double)((float)Art.SizeX / 32F));
			}

			if (Direction == 2)
			{
				k = MathHelper2.Floor_double(PosX - (double)((float)Art.SizeX / 32F));
			}

			if (Direction == 3)
			{
				i1 = MathHelper2.Floor_double(PosZ - (double)((float)Art.SizeX / 32F));
			}

			l = MathHelper2.Floor_double(PosY - (double)((float)Art.SizeY / 32F));

			for (int j1 = 0; j1 < i; j1++)
			{
				for (int k1 = 0; k1 < j; k1++)
				{
					Material material;

					if (Direction == 0 || Direction == 2)
					{
						material = WorldObj.GetBlockMaterial(k + j1, l + k1, ZPosition);
					}
					else
					{
						material = WorldObj.GetBlockMaterial(XPosition, l + k1, i1 + j1);
					}

					if (!material.IsSolid())
					{
						return false;
					}
				}
			}

            List<Entity> list = WorldObj.GetEntitiesWithinAABBExcludingEntity(this, BoundingBox);

			for (int l1 = 0; l1 < list.Count; l1++)
			{
				if (list[l1] is EntityPainting)
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Returns true if other Entities should be prevented from moving through this Entity.
		/// </summary>
		public override bool CanBeCollidedWith()
		{
			return true;
		}

		/// <summary>
		/// Called when the entity is attacked.
		/// </summary>
		public override bool AttackEntityFrom(DamageSource par1DamageSource, int par2)
		{
			if (!IsDead && !WorldObj.IsRemote)
			{
				SetDead();
				SetBeenAttacked();
				WorldObj.SpawnEntityInWorld(new EntityItem(WorldObj, PosX, PosY, PosZ, new ItemStack(Item.Painting)));
			}

			return true;
		}

		/// <summary>
		/// (abstract) Protected helper method to write subclass entity data to NBT.
		/// </summary>
		public override void WriteEntityToNBT(NBTTagCompound par1NBTTagCompound)
		{
			par1NBTTagCompound.SetByte("Dir", (byte)Direction);
			par1NBTTagCompound.SetString("Motive", Art.Title);
			par1NBTTagCompound.SetInteger("TileX", XPosition);
			par1NBTTagCompound.SetInteger("TileY", YPosition);
			par1NBTTagCompound.SetInteger("TileZ", ZPosition);
		}

		/// <summary>
		/// (abstract) Protected helper method to read subclass entity data from NBT.
		/// </summary>
		public override void ReadEntityFromNBT(NBTTagCompound par1NBTTagCompound)
		{
			Direction = par1NBTTagCompound.GetByte("Dir");
			XPosition = par1NBTTagCompound.GetInteger("TileX");
			YPosition = par1NBTTagCompound.GetInteger("TileY");
			ZPosition = par1NBTTagCompound.GetInteger("TileZ");
			string s = par1NBTTagCompound.GetString("Motive");
			Art[] aenumart = Art.ArtArray;
			int i = aenumart.Length;

			for (int j = 0; j < i; j++)
			{
				Art enumart = aenumart[j];

				if (enumart.Title.Equals(s))
				{
					Art = enumart;
				}
			}

			if (Art == null)
			{
				Art = Art.Kebab;
			}

			Func_412_b(Direction);
		}

		/// <summary>
		/// Tries to moves the entity by the passed in displacement. Args: x, y, z
		/// </summary>
        public override void MoveEntity(float par1, float par3, float par5)
		{
			if (!WorldObj.IsRemote && !IsDead && par1 * par1 + par3 * par3 + par5 * par5 > 0.0F)
			{
				SetDead();
				WorldObj.SpawnEntityInWorld(new EntityItem(WorldObj, PosX, PosY, PosZ, new ItemStack(Item.Painting)));
			}
		}

		/// <summary>
		/// Adds to the current velocity of the entity. Args: x, y, z
		/// </summary>
        public override void AddVelocity(float par1, float par3, float par5)
		{
			if (!WorldObj.IsRemote && !IsDead && par1 * par1 + par3 * par3 + par5 * par5 > 0.0F)
			{
				SetDead();
				WorldObj.SpawnEntityInWorld(new EntityItem(WorldObj, PosX, PosY, PosZ, new ItemStack(Item.Painting)));
			}
		}
	}
}