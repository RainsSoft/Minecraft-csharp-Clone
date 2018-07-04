using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
    public class EntityDragon : EntityDragonBase
    {
        public float TargetX;
        public float TargetY;
        public float TargetZ;
        public float[][] Field_40162_d;
        public int Field_40164_e;
        public EntityDragonPart[] DragonPartArray;

        /// <summary>
        /// The head bounding box of a dragon </summary>
        public EntityDragonPart DragonPartHead;

        /// <summary>
        /// The body bounding box of a dragon </summary>
        public EntityDragonPart DragonPartBody;
        public EntityDragonPart DragonPartTail1;
        public EntityDragonPart DragonPartTail2;
        public EntityDragonPart DragonPartTail3;
        public EntityDragonPart DragonPartWing1;
        public EntityDragonPart DragonPartWing2;
        public float Field_40173_aw;
        public float Field_40172_ax;
        public bool Field_40163_ay;
        public bool Field_40161_az;
        private Entity Target;
        public int Field_40178_aA;

        /// <summary>
        /// The current endercrystal that is healing this dragon </summary>
        public EntityEnderCrystal HealingEnderCrystal;

        public EntityDragon(World par1World)
            : base(par1World)
        {
            //ORIGINAL LINE: Field_40162_d = new double[64][3];
            //JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
            Field_40162_d = JavaHelper.ReturnRectangularArray<float>(64, 3);
            Field_40164_e = -1;
            Field_40173_aw = 0.0F;
            Field_40172_ax = 0.0F;
            Field_40163_ay = false;
            Field_40161_az = false;
            Field_40178_aA = 0;
            HealingEnderCrystal = null;
            DragonPartArray = (new EntityDragonPart[] { DragonPartHead = new EntityDragonPart(this, "head", 6F, 6F), DragonPartBody = new EntityDragonPart(this, "body", 8F, 8F), DragonPartTail1 = new EntityDragonPart(this, "tail", 4F, 4F), DragonPartTail2 = new EntityDragonPart(this, "tail", 4F, 4F), DragonPartTail3 = new EntityDragonPart(this, "tail", 4F, 4F), DragonPartWing1 = new EntityDragonPart(this, "wing", 4F, 4F), DragonPartWing2 = new EntityDragonPart(this, "wing", 4F, 4F)
		});
            MaxHealth = 200;
            SetEntityHealth(MaxHealth);
            Texture = "/mob/enderdragon/ender.png";
            SetSize(16F, 8F);
            NoClip = true;
            isImmuneToFire_Renamed = true;
            TargetY = 100;
            IgnoreFrustumCheck = true;
        }

        protected override void EntityInit()
        {
            base.EntityInit();
            DataWatcher.AddObject(16, new int?(MaxHealth));
        }

        public virtual float[] Func_40160_a(int par1, float par2)
        {
            if (Health <= 0)
            {
                par2 = 0.0F;
            }

            par2 = 1.0F - par2;
            int i = Field_40164_e - par1 * 1 & 0x3f;
            int j = Field_40164_e - par1 * 1 - 1 & 0x3f;
            float[] ad = new float[3];
            float d = Field_40162_d[i][0];
            float d1;

            for (d1 = Field_40162_d[j][0] - d; d1 < -180D; d1 += 360F)
            {
            }

            for (; d1 >= 180D; d1 -= 360F)
            {
            }

            ad[0] = d + d1 * par2;
            d = Field_40162_d[i][1];
            d1 = Field_40162_d[j][1] - d;
            ad[1] = d + d1 * par2;
            ad[2] = Field_40162_d[i][2] + (Field_40162_d[j][2] - Field_40162_d[i][2]) * par2;
            return ad;
        }

        /// <summary>
        /// Called frequently so the entity can update its state every tick as required. For example, zombies and skeletons
        /// use this to react to sunlight and start to burn.
        /// </summary>
        public override void OnLivingUpdate()
        {
            Field_40173_aw = Field_40172_ax;

            if (!WorldObj.IsRemote)
            {
                DataWatcher.UpdateObject(16, Convert.ToInt32(Health));
            }

            if (Health <= 0)
            {
                float f = (Rand.NextFloat() - 0.5F) * 8F;
                float f2 = (Rand.NextFloat() - 0.5F) * 4F;
                float f4 = (Rand.NextFloat() - 0.5F) * 8F;
                WorldObj.SpawnParticle("largeexplode", PosX + (double)f, PosY + 2D + (double)f2, PosZ + (double)f4, 0.0F, 0.0F, 0.0F);
                return;
            }

            UpdateDragonEnderCrystal();
            float f1 = 0.2F / (MathHelper2.Sqrt_double(MotionX * MotionX + MotionZ * MotionZ) * 10F + 1.0F);
            f1 *= (float)Math.Pow(2D, MotionY);

            if (Field_40161_az)
            {
                Field_40172_ax += f1 * 0.5F;
            }
            else
            {
                Field_40172_ax += f1;
            }

            for (; RotationYaw >= 180F; RotationYaw -= 360F)
            {
            }

            for (; RotationYaw < -180F; RotationYaw += 360F)
            {
            }

            if (Field_40164_e < 0)
            {
                for (int i = 0; i < Field_40162_d.Length; i++)
                {
                    Field_40162_d[i][0] = RotationYaw;
                    Field_40162_d[i][1] = PosY;
                }
            }

            if (++Field_40164_e == Field_40162_d.Length)
            {
                Field_40164_e = 0;
            }

            Field_40162_d[Field_40164_e][0] = RotationYaw;
            Field_40162_d[Field_40164_e][1] = PosY;

            if (WorldObj.IsRemote)
            {
                if (NewPosRotationIncrements > 0)
                {
                    float d = PosX + (NewPosX - PosX) / NewPosRotationIncrements;
                    float d2 = PosY + (NewPosY - PosY) / NewPosRotationIncrements;
                    float d4 = PosZ + (NewPosZ - PosZ) / NewPosRotationIncrements;
                    double d6;

                    for (d6 = NewRotationYaw - (double)RotationYaw; d6 < -180D; d6 += 360D)
                    {
                    }

                    for (; d6 >= 180D; d6 -= 360D)
                    {
                    }

                    RotationYaw += (float)d6 / NewPosRotationIncrements;
                    RotationPitch += (float)(NewRotationPitch - RotationPitch) / NewPosRotationIncrements;
                    NewPosRotationIncrements--;
                    SetPosition(d, d2, d4);
                    SetRotation(RotationYaw, RotationPitch);
                }
            }
            else
            {
                double d1 = TargetX - PosX;
                float d3 = TargetY - PosY;
                double d5 = TargetZ - PosZ;
                double d7 = d1 * d1 + d3 * d3 + d5 * d5;

                if (Target != null)
                {
                    TargetX = Target.PosX;
                    TargetZ = Target.PosZ;
                    float d8 = TargetX - PosX;
                    float d10 = TargetZ - PosZ;
                    float d12 = (float)Math.Sqrt(d8 * d8 + d10 * d10);
                    float d13 = (0.40000000596046448F + d12 / 80F) - 1.0F;

                    if (d13 > 10F)
                    {
                        d13 = 10F;
                    }

                    TargetY = Target.BoundingBox.MinY + d13;
                }
                else
                {
                    TargetX += Rand.NextGaussian() * 2F;
                    TargetZ += Rand.NextGaussian() * 2F;
                }

                if (Field_40163_ay || d7 < 100D || d7 > 22500D || IsCollidedHorizontally || IsCollidedVertically)
                {
                    Func_41006_aA();
                }

                d3 /= MathHelper2.Sqrt_double(d1 * d1 + d5 * d5);
                float f10 = 0.6F;

                if (d3 < (double)(-f10))
                {
                    d3 = -f10;
                }

                if (d3 > (double)f10)
                {
                    d3 = f10;
                }

                MotionY += d3 * 0.10000000149011612F;

                for (; RotationYaw < -180F; RotationYaw += 360F)
                {
                }

                for (; RotationYaw >= 180F; RotationYaw -= 360F)
                {
                }

                double d9 = 180D - (Math.Atan2(d1, d5) * 180D) / Math.PI;
                double d11;

                for (d11 = d9 - (double)RotationYaw; d11 < -180D; d11 += 360D)
                {
                }

                for (; d11 >= 180D; d11 -= 360D)
                {
                }

                if (d11 > 50D)
                {
                    d11 = 50D;
                }

                if (d11 < -50D)
                {
                    d11 = -50D;
                }

                Vec3D vec3d = Vec3D.CreateVector(TargetX - PosX, TargetY - PosY, TargetZ - PosZ).Normalize();
                Vec3D vec3d1 = Vec3D.CreateVector(MathHelper2.Sin((RotationYaw * (float)Math.PI) / 180F), MotionY, -MathHelper2.Cos((RotationYaw * (float)Math.PI) / 180F)).Normalize();
                float f18 = (float)(vec3d1.DotProduct(vec3d) + 0.5D) / 1.5F;

                if (f18 < 0.0F)
                {
                    f18 = 0.0F;
                }

                RandomYawVelocity *= 0.8F;
                float f19 = MathHelper2.Sqrt_double(MotionX * MotionX + MotionZ * MotionZ) * 1.0F + 1.0F;
                double d14 = Math.Sqrt(MotionX * MotionX + MotionZ * MotionZ) * 1.0D + 1.0D;

                if (d14 > 40D)
                {
                    d14 = 40D;
                }

                RandomYawVelocity += (float)(d11 * (0.69999998807907104D / d14 / f19));
                RotationYaw += RandomYawVelocity * 0.1F;
                float f20 = (float)(2D / (d14 + 1.0D));
                float f21 = 0.06F;
                MoveFlying(0.0F, -1F, f21 * (f18 * f20 + (1.0F - f20)));

                if (Field_40161_az)
                {
                    MoveEntity(MotionX * 0.80000001192092896F, MotionY * 0.80000001192092896F, MotionZ * 0.80000001192092896F);
                }
                else
                {
                    MoveEntity(MotionX, MotionY, MotionZ);
                }

                Vec3D vec3d2 = Vec3D.CreateVector(MotionX, MotionY, MotionZ).Normalize();
                float f22 = (float)(vec3d2.DotProduct(vec3d1) + 1.0D) / 2.0F;
                f22 = 0.8F + 0.15F * f22;
                MotionX *= f22;
                MotionZ *= f22;
                MotionY *= 0.9100000262260437F;
            }

            RenderYawOffset = RotationYaw;
            DragonPartHead.Width = DragonPartHead.Height = 3F;
            DragonPartTail1.Width = DragonPartTail1.Height = 2.0F;
            DragonPartTail2.Width = DragonPartTail2.Height = 2.0F;
            DragonPartTail3.Width = DragonPartTail3.Height = 2.0F;
            DragonPartBody.Height = 3F;
            DragonPartBody.Width = 5F;
            DragonPartWing1.Height = 2.0F;
            DragonPartWing1.Width = 4F;
            DragonPartWing2.Height = 3F;
            DragonPartWing2.Width = 4F;
            float f3 = (((float)(Func_40160_a(5, 1.0F)[1] - Func_40160_a(10, 1.0F)[1]) * 10F) / 180F) * (float)Math.PI;
            float f5 = MathHelper2.Cos(f3);
            float f6 = -MathHelper2.Sin(f3);
            float f7 = (RotationYaw * (float)Math.PI) / 180F;
            float f8 = MathHelper2.Sin(f7);
            float f9 = MathHelper2.Cos(f7);
            DragonPartBody.OnUpdate();
            DragonPartBody.SetLocationAndAngles(PosX + (f8 * 0.5F), PosY, PosZ - (f9 * 0.5F), 0.0F, 0.0F);
            DragonPartWing1.OnUpdate();
            DragonPartWing1.SetLocationAndAngles(PosX + (f9 * 4.5F), PosY + 2F, PosZ + (f8 * 4.5F), 0.0F, 0.0F);
            DragonPartWing2.OnUpdate();
            DragonPartWing2.SetLocationAndAngles(PosX - (f9 * 4.5F), PosY + 2F, PosZ - (f8 * 4.5F), 0.0F, 0.0F);

            if (!WorldObj.IsRemote)
            {
                Func_41007_az();
            }

            if (!WorldObj.IsRemote && MaxHurtTime == 0)
            {
                CollideWithEntities(WorldObj.GetEntitiesWithinAABBExcludingEntity(this, DragonPartWing1.BoundingBox.Expand(4, 2, 4).Offset(0.0F, -2F, 0.0F)));
                CollideWithEntities(WorldObj.GetEntitiesWithinAABBExcludingEntity(this, DragonPartWing2.BoundingBox.Expand(4, 2, 4).Offset(0.0F, -2F, 0.0F)));
                AttackEntitiesInList(WorldObj.GetEntitiesWithinAABBExcludingEntity(this, DragonPartHead.BoundingBox.Expand(1.0F, 1.0F, 1.0F)));
            }

            float[] ad = Func_40160_a(5, 1.0F);
            float[] ad1 = Func_40160_a(0, 1.0F);
            float f11 = MathHelper2.Sin((RotationYaw * (float)Math.PI) / 180F - RandomYawVelocity * 0.01F);
            float f12 = MathHelper2.Cos((RotationYaw * (float)Math.PI) / 180F - RandomYawVelocity * 0.01F);
            DragonPartHead.OnUpdate();
            DragonPartHead.SetLocationAndAngles(PosX + (f11 * 5.5F * f5), PosY + (ad1[1] - ad[1]) * 1.0F + (f6 * 5.5F), PosZ - (f12 * 5.5F * f5), 0.0F, 0.0F);

            for (int j = 0; j < 3; j++)
            {
                EntityDragonPart entitydragonpart = null;

                if (j == 0)
                {
                    entitydragonpart = DragonPartTail1;
                }

                if (j == 1)
                {
                    entitydragonpart = DragonPartTail2;
                }

                if (j == 2)
                {
                    entitydragonpart = DragonPartTail3;
                }

                float[] ad2 = Func_40160_a(12 + j * 2, 1.0F);
                float f13 = (RotationYaw * (float)Math.PI) / 180F + ((SimplifyAngle(ad2[0] - ad[0]) * (float)Math.PI) / 180F) * 1.0F;
                float f14 = MathHelper2.Sin(f13);
                float f15 = MathHelper2.Cos(f13);
                float f16 = 1.5F;
                float f17 = (float)(j + 1) * 2.0F;
                entitydragonpart.OnUpdate();
                entitydragonpart.SetLocationAndAngles(PosX - ((f8 * f16 + f14 * f17) * f5), ((PosY + (ad2[1] - ad[1]) * 1.0F) - ((f17 + f16) * f6)) + 1.5F, PosZ + ((f9 * f16 + f15 * f17) * f5), 0.0F, 0.0F);
            }

            if (!WorldObj.IsRemote)
            {
                Field_40161_az = DestroyBlocksInAABB(DragonPartHead.BoundingBox) | DestroyBlocksInAABB(DragonPartBody.BoundingBox);
            }
        }

        /// <summary>
        /// Updates the state of the enderdragon's current endercrystal.
        /// </summary>
        private void UpdateDragonEnderCrystal()
        {
            if (HealingEnderCrystal != null)
            {
                if (HealingEnderCrystal.IsDead)
                {
                    if (!WorldObj.IsRemote)
                    {
                        AttackEntityFromPart(DragonPartHead, DamageSource.Explosion, 10);
                    }

                    HealingEnderCrystal = null;
                }
                else if (TicksExisted % 10 == 0 && Health < MaxHealth)
                {
                    Health++;
                }
            }

            if (Rand.Next(10) == 0)
            {
                float f = 32F;
                List<Entity> list = WorldObj.GetEntitiesWithinAABB(typeof(net.minecraft.src.EntityEnderCrystal), BoundingBox.Expand(f, f, f));
                EntityEnderCrystal entityendercrystal = null;
                double d = double.MaxValue;
                IEnumerator<Entity> iterator = list.GetEnumerator();

                do
                {
                    if (!iterator.MoveNext())
                    {
                        break;
                    }

                    Entity entity = iterator.Current;
                    double d1 = entity.GetDistanceSqToEntity(this);

                    if (d1 < d)
                    {
                        d = d1;
                        entityendercrystal = (EntityEnderCrystal)entity;
                    }
                }
                while (true);

                HealingEnderCrystal = entityendercrystal;
            }
        }

        private void Func_41007_az()
        {
        }

        /// <summary>
        /// Pushes all entities inside the list away from the enderdragon.
        /// </summary>
        private void CollideWithEntities(List<Entity> par1List)
        {
            float d = (DragonPartBody.BoundingBox.MinX + DragonPartBody.BoundingBox.MaxX) / 2F;
            float d1 = (DragonPartBody.BoundingBox.MinZ + DragonPartBody.BoundingBox.MaxZ) / 2F;
            IEnumerator<Entity> iterator = par1List.GetEnumerator();

            do
            {
                if (!iterator.MoveNext())
                {
                    break;
                }

                Entity entity = iterator.Current;

                if (entity is EntityLiving)
                {
                    float d2 = entity.PosX - d;
                    float d3 = entity.PosZ - d1;
                    float d4 = d2 * d2 + d3 * d3;
                    entity.AddVelocity((d2 / d4) * 4F, 0.20000000298023224F, (d3 / d4) * 4F);
                }
            }
            while (true);
        }

        /// <summary>
        /// Attacks all entities inside this list, dealing 5 hearts of damage.
        /// </summary>
        private void AttackEntitiesInList(List<Entity> par1List)
        {
            for (int i = 0; i < par1List.Count; i++)
            {
                Entity entity = par1List[i];

                if (entity is EntityLiving)
                {
                    entity.AttackEntityFrom(DamageSource.CauseMobDamage(this), 10);
                }
            }
        }

        private void Func_41006_aA()
        {
            Field_40163_ay = false;

            if (Rand.Next(2) == 0 && WorldObj.PlayerEntities.Count > 0)
            {
                Target = WorldObj.PlayerEntities[Rand.Next(WorldObj.PlayerEntities.Count)];
            }
            else
            {
                bool flag = false;

                do
                {
                    TargetX = 0.0F;
                    TargetY = 70F + Rand.NextFloat() * 50F;
                    TargetZ = 0.0F;
                    TargetX += Rand.NextFloat() * 120F - 60F;
                    TargetZ += Rand.NextFloat() * 120F - 60F;
                    double d = PosX - TargetX;
                    double d1 = PosY - TargetY;
                    double d2 = PosZ - TargetZ;
                    flag = d * d + d1 * d1 + d2 * d2 > 100D;
                }
                while (!flag);

                Target = null;
            }
        }

        /// <summary>
        /// Simplifies the value of a number by adding/subtracting 180 to the point that the number is between -180 and 180.
        /// </summary>
        private float SimplifyAngle(double par1)
        {
            for (; par1 >= 180D; par1 -= 360D)
            {
            }

            for (; par1 < -180D; par1 += 360D)
            {
            }

            return (float)par1;
        }

        /// <summary>
        /// Destroys all blocks that aren't associated with 'The End' inside the given bounding box.
        /// </summary>
        private bool DestroyBlocksInAABB(AxisAlignedBB par1AxisAlignedBB)
        {
            int i = MathHelper2.Floor_double(par1AxisAlignedBB.MinX);
            int j = MathHelper2.Floor_double(par1AxisAlignedBB.MinY);
            int k = MathHelper2.Floor_double(par1AxisAlignedBB.MinZ);
            int l = MathHelper2.Floor_double(par1AxisAlignedBB.MaxX);
            int i1 = MathHelper2.Floor_double(par1AxisAlignedBB.MaxY);
            int j1 = MathHelper2.Floor_double(par1AxisAlignedBB.MaxZ);
            bool flag = false;
            bool flag1 = false;

            for (int k1 = i; k1 <= l; k1++)
            {
                for (int l1 = j; l1 <= i1; l1++)
                {
                    for (int i2 = k; i2 <= j1; i2++)
                    {
                        int j2 = WorldObj.GetBlockId(k1, l1, i2);

                        if (j2 == 0)
                        {
                            continue;
                        }

                        if (j2 == Block.Obsidian.BlockID || j2 == Block.WhiteStone.BlockID || j2 == Block.Bedrock.BlockID)
                        {
                            flag = true;
                        }
                        else
                        {
                            flag1 = true;
                            WorldObj.SetBlockWithNotify(k1, l1, i2, 0);
                        }
                    }
                }
            }

            if (flag1)
            {
                double d = par1AxisAlignedBB.MinX + (par1AxisAlignedBB.MaxX - par1AxisAlignedBB.MinX) * (double)Rand.NextFloat();
                double d1 = par1AxisAlignedBB.MinY + (par1AxisAlignedBB.MaxY - par1AxisAlignedBB.MinY) * (double)Rand.NextFloat();
                double d2 = par1AxisAlignedBB.MinZ + (par1AxisAlignedBB.MaxZ - par1AxisAlignedBB.MinZ) * (double)Rand.NextFloat();
                WorldObj.SpawnParticle("largeexplode", d, d1, d2, 0.0F, 0.0F, 0.0F);
            }

            return flag;
        }

        public override bool AttackEntityFromPart(EntityDragonPart par1EntityDragonPart, DamageSource par2DamageSource, int par3)
        {
            if (par1EntityDragonPart != DragonPartHead)
            {
                par3 = par3 / 4 + 1;
            }

            float f = (RotationYaw * (float)Math.PI) / 180F;
            float f1 = MathHelper2.Sin(f);
            float f2 = MathHelper2.Cos(f);
            TargetX = PosX + (f1 * 5F) + ((Rand.NextFloat() - 0.5F) * 2.0F);
            TargetY = PosY + (Rand.NextFloat() * 3F) + 1.0F;
            TargetZ = (PosZ - (f2 * 5F)) + ((Rand.NextFloat() - 0.5F) * 2.0F);
            Target = null;

            if ((par2DamageSource.GetEntity() is EntityPlayer) || par2DamageSource == DamageSource.Explosion)
            {
                SuperAttackFrom(par2DamageSource, par3);
            }

            return true;
        }

        /// <summary>
        /// handles entity death timer, experience orb and particle creation
        /// </summary>
        protected override void OnDeathUpdate()
        {
            Field_40178_aA++;

            if (Field_40178_aA >= 180 && Field_40178_aA <= 200)
            {
                float f = (Rand.NextFloat() - 0.5F) * 8F;
                float f1 = (Rand.NextFloat() - 0.5F) * 4F;
                float f2 = (Rand.NextFloat() - 0.5F) * 8F;
                WorldObj.SpawnParticle("hugeexplosion", PosX + (double)f, PosY + 2D + (double)f1, PosZ + (double)f2, 0.0F, 0.0F, 0.0F);
            }

            if (!WorldObj.IsRemote && Field_40178_aA > 150 && Field_40178_aA % 5 == 0)
            {
                for (int i = 1000; i > 0; )
                {
                    int k = EntityXPOrb.GetXPSplit(i);
                    i -= k;
                    WorldObj.SpawnEntityInWorld(new EntityXPOrb(WorldObj, PosX, PosY, PosZ, k));
                }
            }

            MoveEntity(0.0F, 0.10000000149011612F, 0.0F);
            RenderYawOffset = RotationYaw += 20F;

            if (Field_40178_aA == 200)
            {
                for (int j = 10000; j > 0; )
                {
                    int l = EntityXPOrb.GetXPSplit(j);
                    j -= l;
                    WorldObj.SpawnEntityInWorld(new EntityXPOrb(WorldObj, PosX, PosY, PosZ, l));
                }

                CreateEnderPortal(MathHelper2.Floor_double(PosX), MathHelper2.Floor_double(PosZ));
                OnEntityDeath();
                SetDead();
            }
        }

        /// <summary>
        /// Creates the ender portal leading back to the normal world after defeating the enderdragon.
        /// </summary>
        private void CreateEnderPortal(int par1, int par2)
        {
            sbyte byte0 = 64;
            BlockEndPortal.BossDefeated = true;
            int i = 4;

            for (int j = byte0 - 1; j <= byte0 + 32; j++)
            {
                for (int k = par1 - i; k <= par1 + i; k++)
                {
                    for (int l = par2 - i; l <= par2 + i; l++)
                    {
                        double d = k - par1;
                        double d1 = l - par2;
                        double d2 = MathHelper2.Sqrt_double(d * d + d1 * d1);

                        if (d2 > (double)i - 0.5D)
                        {
                            continue;
                        }

                        if (j < byte0)
                        {
                            if (d2 <= (double)(i - 1) - 0.5D)
                            {
                                WorldObj.SetBlockWithNotify(k, j, l, Block.Bedrock.BlockID);
                            }

                            continue;
                        }

                        if (j > byte0)
                        {
                            WorldObj.SetBlockWithNotify(k, j, l, 0);
                            continue;
                        }

                        if (d2 > (double)(i - 1) - 0.5D)
                        {
                            WorldObj.SetBlockWithNotify(k, j, l, Block.Bedrock.BlockID);
                        }
                        else
                        {
                            WorldObj.SetBlockWithNotify(k, j, l, Block.EndPortal.BlockID);
                        }
                    }
                }
            }

            WorldObj.SetBlockWithNotify(par1, byte0 + 0, par2, Block.Bedrock.BlockID);
            WorldObj.SetBlockWithNotify(par1, byte0 + 1, par2, Block.Bedrock.BlockID);
            WorldObj.SetBlockWithNotify(par1, byte0 + 2, par2, Block.Bedrock.BlockID);
            WorldObj.SetBlockWithNotify(par1 - 1, byte0 + 2, par2, Block.TorchWood.BlockID);
            WorldObj.SetBlockWithNotify(par1 + 1, byte0 + 2, par2, Block.TorchWood.BlockID);
            WorldObj.SetBlockWithNotify(par1, byte0 + 2, par2 - 1, Block.TorchWood.BlockID);
            WorldObj.SetBlockWithNotify(par1, byte0 + 2, par2 + 1, Block.TorchWood.BlockID);
            WorldObj.SetBlockWithNotify(par1, byte0 + 3, par2, Block.Bedrock.BlockID);
            WorldObj.SetBlockWithNotify(par1, byte0 + 4, par2, Block.DragonEgg.BlockID);
            BlockEndPortal.BossDefeated = false;
        }

        /// <summary>
        /// Makes the entity despawn if requirements are reached
        /// </summary>
        protected override void DespawnEntity()
        {
        }

        /// <summary>
        /// Return the Entity parts making up this Entity (currently only for dragons)
        /// </summary>
        public override Entity[] GetParts()
        {
            return DragonPartArray;
        }

        /// <summary>
        /// Returns true if other Entities should be prevented from moving through this Entity.
        /// </summary>
        public override bool CanBeCollidedWith()
        {
            return false;
        }

        public virtual int Func_41010_ax()
        {
            return DataWatcher.GetWatchableObjectInt(16);
        }
    }
}