using System.Text;

namespace net.minecraft.src
{
    public class Potion
    {
        public static readonly Potion[] PotionTypes = new Potion[32];
        public const Potion Field_35676_b = null;
        public static readonly Potion MoveSpeed = (new Potion(1, false, 0x7cafc6)).SetPotionName("potion.moveSpeed").SetIconIndex(0, 0);
        public static readonly Potion MoveSlowdown = (new Potion(2, true, 0x5a6c81)).SetPotionName("potion.moveSlowdown").SetIconIndex(1, 0);
        public static readonly Potion DigSpeed = (new Potion(3, false, 0xd9c043)).SetPotionName("potion.digSpeed").SetIconIndex(2, 0).SetEffectiveness(1.5D);
        public static readonly Potion DigSlowdown = (new Potion(4, true, 0x4a4217)).SetPotionName("potion.digSlowDown").SetIconIndex(3, 0);
        public static readonly Potion DamageBoost = (new Potion(5, false, 0x932423)).SetPotionName("potion.damageBoost").SetIconIndex(4, 0);
        public static readonly Potion Heal = (new PotionHealth(6, false, 0xf82423)).SetPotionName("potion.heal");
        public static readonly Potion Harm = (new PotionHealth(7, true, 0x430a09)).SetPotionName("potion.harm");
        public static readonly Potion Jump = (new Potion(8, false, 0x786297)).SetPotionName("potion.jump").SetIconIndex(2, 1);
        public static readonly Potion Confusion = (new Potion(9, true, 0x551d4a)).SetPotionName("potion.confusion").SetIconIndex(3, 1).SetEffectiveness(0.25D);

        /// <summary>
        /// The regeneration Potion object. </summary>
        public static readonly Potion Regeneration = (new Potion(10, false, 0xcd5cab)).SetPotionName("potion.regeneration").SetIconIndex(7, 0).SetEffectiveness(0.25D);
        public static readonly Potion Resistance = (new Potion(11, false, 0x99453a)).SetPotionName("potion.resistance").SetIconIndex(6, 1);

        /// <summary>
        /// The fire resistance Potion object. </summary>
        public static readonly Potion FireResistance = (new Potion(12, false, 0xe49a3a)).SetPotionName("potion.fireResistance").SetIconIndex(7, 1);

        /// <summary>
        /// The water breathing Potion object. </summary>
        public static readonly Potion WaterBreathing = (new Potion(13, false, 0x2e5299)).SetPotionName("potion.waterBreathing").SetIconIndex(0, 2);

        /// <summary>
        /// The invisibility Potion object. </summary>
        public static readonly Potion Invisibility = (new Potion(14, false, 0x7f8392)).SetPotionName("potion.invisibility").SetIconIndex(0, 1).SetPotionUnusable();

        /// <summary>
        /// The blindness Potion object. </summary>
        public static readonly Potion Blindness = (new Potion(15, true, 0x1f1f23)).SetPotionName("potion.blindness").SetIconIndex(5, 1).SetEffectiveness(0.25D);

        /// <summary>
        /// The night vision Potion object. </summary>
        public static readonly Potion NightVision = (new Potion(16, false, 0x1f1fa1)).SetPotionName("potion.nightVision").SetIconIndex(4, 1).SetPotionUnusable();

        /// <summary>
        /// The hunger Potion object. </summary>
        public static readonly Potion Hunger = (new Potion(17, true, 0x587653)).SetPotionName("potion.hunger").SetIconIndex(1, 1);

        /// <summary>
        /// The weakness Potion object. </summary>
        public static readonly Potion Weakness = (new Potion(18, true, 0x484d48)).SetPotionName("potion.weakness").SetIconIndex(5, 0);

        /// <summary>
        /// The poison Potion object. </summary>
        public static readonly Potion Poison = (new Potion(19, true, 0x4e9331)).SetPotionName("potion.poison").SetIconIndex(6, 0).SetEffectiveness(0.25D);
        public const Potion Field_35688_v = null;
        public const Potion Field_35687_w = null;
        public const Potion Field_35697_x = null;
        public const Potion Field_35696_y = null;
        public const Potion Field_35695_z = null;
        public const Potion Field_35667_A = null;
        public const Potion Field_35668_B = null;
        public const Potion Field_35669_C = null;
        public const Potion Field_35663_D = null;
        public const Potion Field_35664_E = null;
        public const Potion Field_35665_F = null;
        public const Potion Field_35666_G = null;

        /// <summary>
        /// The Id of a Potion object. </summary>
        public readonly int Id;

        /// <summary>
        /// The name of the Potion. </summary>
        private string Name;

        /// <summary>
        /// The index for the icon displayed when the potion effect is active. </summary>
        private int StatusIconIndex;

        /// <summary>
        /// This field indicated if the effect is 'bad' - negative - for the entity.
        /// </summary>
        private readonly bool isBadEffect_Renamed;
        private double Effectiveness;
        private bool Usable;

        /// <summary>
        /// Is the color of the liquid for this potion. </summary>
        private readonly int LiquidColor;

        protected Potion(int par1, bool par2, int par3)
        {
            Name = "";
            StatusIconIndex = -1;
            Id = par1;
            PotionTypes[par1] = this;
            isBadEffect_Renamed = par2;

            if (par2)
            {
                Effectiveness = 0.5D;
            }
            else
            {
                Effectiveness = 1.0D;
            }

            LiquidColor = par3;
        }

        /// <summary>
        /// Sets the index for the icon displayed in the player's inventory when the status is active.
        /// </summary>
        protected virtual Potion SetIconIndex(int par1, int par2)
        {
            StatusIconIndex = par1 + par2 * 8;
            return this;
        }

        /// <summary>
        /// returns the ID of the potion
        /// </summary>
        public virtual int GetId()
        {
            return Id;
        }

        public virtual void PerformEffect(EntityLiving par1EntityLiving, int par2)
        {
            if (Id == Regeneration.Id)
            {
                if (par1EntityLiving.GetHealth() < par1EntityLiving.GetMaxHealth())
                {
                    par1EntityLiving.Heal(1);
                }
            }
            else if (Id == Poison.Id)
            {
                if (par1EntityLiving.GetHealth() > 1)
                {
                    par1EntityLiving.AttackEntityFrom(DamageSource.Magic, 1);
                }
            }
            else if (Id == Hunger.Id && (par1EntityLiving is EntityPlayer))
            {
                ((EntityPlayer)par1EntityLiving).AddExhaustion(0.025F * (float)(par2 + 1));
            }
            else if (Id == Heal.Id && !par1EntityLiving.IsEntityUndead() || Id == Harm.Id && par1EntityLiving.IsEntityUndead())
            {
                par1EntityLiving.Heal(6 << par2);
            }
            else if (Id == Harm.Id && !par1EntityLiving.IsEntityUndead() || Id == Heal.Id && par1EntityLiving.IsEntityUndead())
            {
                par1EntityLiving.AttackEntityFrom(DamageSource.Magic, 6 << par2);
            }
        }

        /// <summary>
        /// Hits the provided entity with this potion's instant effect.
        /// </summary>
        public virtual void AffectEntity(EntityLiving par1EntityLiving, EntityLiving par2EntityLiving, int par3, double par4)
        {
            if (Id == Heal.Id && !par2EntityLiving.IsEntityUndead() || Id == Harm.Id && par2EntityLiving.IsEntityUndead())
            {
                int i = (int)(par4 * (double)(6 << par3) + 0.5D);
                par2EntityLiving.Heal(i);
            }
            else if (Id == Harm.Id && !par2EntityLiving.IsEntityUndead() || Id == Heal.Id && par2EntityLiving.IsEntityUndead())
            {
                int j = (int)(par4 * (double)(6 << par3) + 0.5D);

                if (par1EntityLiving == null)
                {
                    par2EntityLiving.AttackEntityFrom(DamageSource.Magic, j);
                }
                else
                {
                    par2EntityLiving.AttackEntityFrom(DamageSource.CauseIndirectMagicDamage(par2EntityLiving, par1EntityLiving), j);
                }
            }
        }

        /// <summary>
        /// Returns true if the potion has an instant effect instead of a continuous one (eg Harming)
        /// </summary>
        public virtual bool IsInstant()
        {
            return false;
        }

        /// <summary>
        /// checks if Potion effect is ready to be applied this tick.
        /// </summary>
        public virtual bool IsReady(int par1, int par2)
        {
            if (Id == Regeneration.Id || Id == Poison.Id)
            {
                int i = 25 >> par2;

                if (i > 0)
                {
                    return par1 % i == 0;
                }
                else
                {
                    return true;
                }
            }

            return Id == Hunger.Id;
        }

        /// <summary>
        /// Set the potion name.
        /// </summary>
        public virtual Potion SetPotionName(string par1Str)
        {
            Name = par1Str;
            return this;
        }

        /// <summary>
        /// returns the name of the potion
        /// </summary>
        public virtual string GetName()
        {
            return Name;
        }

        /// <summary>
        /// Returns true if the potion has a associated status icon to display in then inventory when active.
        /// </summary>
        public virtual bool HasStatusIcon()
        {
            return StatusIconIndex >= 0;
        }

        /// <summary>
        /// Returns the index for the icon to display when the potion is active.
        /// </summary>
        public virtual int GetStatusIconIndex()
        {
            return StatusIconIndex;
        }

        /// <summary>
        /// This method returns true if the potion effect is bad - negative - for the entity.
        /// </summary>
        public virtual bool IsBadEffect()
        {
            return isBadEffect_Renamed;
        }

        public static string GetDurationString(PotionEffect par0PotionEffect)
        {
            int i = par0PotionEffect.GetDuration();
            int j = i / 20;
            int k = j / 60;
            j %= 60;

            if (j < 10)
            {
                return (new StringBuilder()).Append(k).Append(":0").Append(j).ToString();
            }
            else
            {
                return (new StringBuilder()).Append(k).Append(":").Append(j).ToString();
            }
        }

        protected virtual Potion SetEffectiveness(double par1)
        {
            Effectiveness = par1;
            return this;
        }

        public virtual double GetEffectiveness()
        {
            return Effectiveness;
        }

        public virtual Potion SetPotionUnusable()
        {
            Usable = true;
            return this;
        }

        public virtual bool IsUsable()
        {
            return Usable;
        }

        /// <summary>
        /// Returns the color of the potion liquid.
        /// </summary>
        public virtual int GetLiquidColor()
        {
            return LiquidColor;
        }
    }
}