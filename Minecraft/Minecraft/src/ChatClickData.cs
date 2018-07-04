using System.Text;

namespace net.minecraft.src
{
	public class ChatClickData
	{
		public static readonly string Field_50097_a = "^(?:(https?)://)?([-\\w_\\.]{2,}\\.[a-z]{2,3})(/\\S*)?$";
		private readonly FontRendererOld Field_50095_b;
		private readonly ChatLine Field_50096_c;
		private readonly int Field_50093_d;
		private readonly int Field_50094_e;
		private readonly string Field_50091_f;
		private readonly string Field_50092_g;

		public ChatClickData(FontRendererOld par1FontRenderer, ChatLine par2ChatLine, int par3, int par4)
		{
			Field_50095_b = par1FontRenderer;
			Field_50096_c = par2ChatLine;
			Field_50093_d = par3;
			Field_50094_e = par4;
			Field_50091_f = par1FontRenderer.Func_50107_a(par2ChatLine.Message, par3);
			Field_50092_g = Func_50090_c();
		}

		public virtual string Func_50088_a()
		{
			return Field_50092_g;
		}

		public virtual string Func_50089_b()
		{
			string s = Func_50088_a();

			if (s == null)
			{
				return null;
			}
            /*
			Matcher matcher = Field_50097_a.matcher(s);

			if (matcher.matches())
			{
				try
				{
					string s1 = matcher.group(0);

					if (matcher.group(1) == null)
					{
						s1 = (new StringBuilder()).Append("http://").Append(s1).ToString();
					}

					return s1;
				}
				catch (URISyntaxException urisyntaxexception)
				{
					Logger.getLogger("Minecraft").log(Level.SEVERE, "Couldn't create URI from chat", urisyntaxexception);
				}
			}*/

			return null;
		}

		private string Func_50090_c()
		{
			int i = Field_50091_f.LastIndexOf(" ", Field_50091_f.Length) + 1;

			if (i < 0)
			{
				i = 0;
			}

			int j = Field_50096_c.Message.IndexOf(" ", i);

			if (j < 0)
			{
				j = Field_50096_c.Message.Length;
			}

			FontRendererOld _tmp = Field_50095_b;
			return FontRendererOld.Func_52014_d(Field_50096_c.Message.Substring(i, j - i));
		}
	}
}