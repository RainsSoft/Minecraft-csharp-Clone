using System.Text;

namespace net.minecraft.src
{
	public class WorldType
	{
		public static readonly WorldType[] WorldTypes = new WorldType[16];

		/// <summary>
		/// Default world type. </summary>
		public static readonly WorldType DEFAULT = (new WorldType(0, "default", 1)).Func_48631_f();

		/// <summary>
		/// Flat world type. </summary>
		public static readonly WorldType FLAT = new WorldType(1, "flat");

		/// <summary>
		/// Default (1.1) world type. </summary>
		public static readonly WorldType DEFAULT_1_1 = (new WorldType(8, "default_1_1", 0)).SetCanBeCreated(false);

		/// <summary>
		/// 'default' or 'flat' </summary>
		private readonly string WorldType_Renamed;

		/// <summary>
		/// The int version of the ChunkProvider that generated this world. </summary>
		private readonly int GeneratorVersion;

		/// <summary>
		/// Whether this world type can be generated. Normally true; set to false for out-of-date generator versions.
		/// </summary>
		private bool CanBeCreated;
		private bool Field_48638_h;

		private WorldType(int par1, string par2Str) : this(par1, par2Str, 0)
		{
		}

		private WorldType(int par1, string par2Str, int par3)
		{
			WorldType_Renamed = par2Str;
			GeneratorVersion = par3;
			CanBeCreated = true;
			WorldTypes[par1] = this;
		}

		public virtual string Func_48628_a()
		{
			return WorldType_Renamed;
		}

		/// <summary>
		/// Gets the translation key for the name of this world type.
		/// </summary>
		public virtual string GetTranslateName()
		{
			return (new StringBuilder()).Append("generator.").Append(WorldType_Renamed).ToString();
		}

		/// <summary>
		/// Returns generatorVersion.
		/// </summary>
		public virtual int GetGeneratorVersion()
		{
			return GeneratorVersion;
		}

		public virtual WorldType Func_48629_a(int par1)
		{
			if (this == DEFAULT && par1 == 0)
			{
				return DEFAULT_1_1;
			}
			else
			{
				return this;
			}
		}

		/// <summary>
		/// Sets canBeCreated to the provided value, and returns this.
		/// </summary>
		private WorldType SetCanBeCreated(bool par1)
		{
			CanBeCreated = par1;
			return this;
		}

		/// <summary>
		/// Gets whether this WorldType can be used to generate a new world.
		/// </summary>
		public virtual bool GetCanBeCreated()
		{
			return CanBeCreated;
		}

		private WorldType Func_48631_f()
		{
			Field_48638_h = true;
			return this;
		}

		public virtual bool Func_48626_e()
		{
			return Field_48638_h;
		}

		public static WorldType ParseWorldType(string par0Str)
		{
			for (int i = 0; i < WorldTypes.Length; i++)
			{
				if (WorldTypes[i] != null && WorldTypes[i].WorldType_Renamed.ToUpper() == par0Str.ToUpper())
				{
					return WorldTypes[i];
				}
			}

			return null;
		}
	}
}