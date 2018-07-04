using System.Collections.Generic;
using System.Text;
using net.minecraft.src;

namespace net.minecraft.src
{
    abstract class GuiSlotStats : GuiSlot
    {
        protected int Field_27268_b;
        protected List<StatCrafting> Field_27273_c;
        protected IComparer<StatCrafting> Field_27272_d;
        public int Field_27271_e;
        public int Field_27270_f;
        readonly GuiStats Field_27269_g;

        protected GuiSlotStats(GuiStats par1GuiStats)
            : base(GuiStats.GetMinecraft(par1GuiStats), par1GuiStats.Width, par1GuiStats.Height, 32, par1GuiStats.Height - 64, 20)
        {
            Field_27269_g = par1GuiStats;
            Field_27268_b = -1;
            Field_27271_e = -1;
            Field_27270_f = 0;
            Func_27258_a(false);
            Func_27259_a(true, 20);
        }

        /// <summary>
        /// the element in the slot that was clicked, bool for wether it was double clicked or not
        /// </summary>
        protected override void ElementClicked(int i, bool flag)
        {
        }

        /// <summary>
        /// returns true if the element passed in is currently selected
        /// </summary>
        protected override bool IsSelected(int par1)
        {
            return false;
        }

        protected override void DrawBackground()
        {
            Field_27269_g.DrawDefaultBackground();
        }

        protected override void Func_27260_a(int par1, int par2, Tessellator par3Tessellator)
        {/*
            if (Mouse.GetState().LeftButton != ButtonState.Pressed)
            {
                Field_27268_b = -1;
            }
            */
            if (Field_27268_b == 0)
            {
                GuiStats.DrawSprite(Field_27269_g, (par1 + 115) - 18, par2 + 1, 0, 0);
            }
            else
            {
                GuiStats.DrawSprite(Field_27269_g, (par1 + 115) - 18, par2 + 1, 0, 18);
            }

            if (Field_27268_b == 1)
            {
                GuiStats.DrawSprite(Field_27269_g, (par1 + 165) - 18, par2 + 1, 0, 0);
            }
            else
            {
                GuiStats.DrawSprite(Field_27269_g, (par1 + 165) - 18, par2 + 1, 0, 18);
            }

            if (Field_27268_b == 2)
            {
                GuiStats.DrawSprite(Field_27269_g, (par1 + 215) - 18, par2 + 1, 0, 0);
            }
            else
            {
                GuiStats.DrawSprite(Field_27269_g, (par1 + 215) - 18, par2 + 1, 0, 18);
            }

            if (Field_27271_e != -1)
            {
                char c = 'O';
                sbyte byte0 = 18;

                if (Field_27271_e == 1)
                {
                    c = '\u0201';
                }
                else if (Field_27271_e == 2)
                {
                    c = '\u0263';
                }
                if (Field_27270_f == 1)
                {
                    byte0 = 36;
                }
                GuiStats.DrawSprite(Field_27269_g, par1 + c, par2 + 1, byte0, 0);
            }
        }

        protected override void Func_27255_a(int par1, int par2)
        {
            Field_27268_b = -1;
            if (par1 >= 79 && par1 < 115)
            {
                Field_27268_b = 0;
            }
            else if (par1 >= 129 && par1 < 165)
            {
                Field_27268_b = 1;
            }
            else if (par1 >= 179 && par1 < 215)
            {
                Field_27268_b = 2;
            }
            if (Field_27268_b >= 0)
            {
                Func_27266_c(Field_27268_b);
                GuiStats.GetMinecraft(Field_27269_g).SndManager.PlaySoundFX("random.click", 1.0F, 1.0F);
            }
        }

        /// <summary>
        /// Gets the size of the current slot list.
        /// </summary>
        public override int GetSize()
        {
            return Field_27273_c.Count;
        }
        
        protected StatCrafting Func_27264_b(int par1)
        {
            return Field_27273_c[par1];
        }
        
        protected abstract string Func_27263_a(int i);

        protected void Func_27265_a(StatCrafting par1StatCrafting, int par2, int par3, bool par4)
        {
            if (par1StatCrafting != null)
            {
                string s = par1StatCrafting.Func_27084_a(GuiStats.GetStatsFileWriter(Field_27269_g).WriteStat(par1StatCrafting));
                Field_27269_g.DrawString(GuiStats.GetFontRenderer(Field_27269_g), s, par2 - (int)GuiStats.GetFontRenderer(Field_27269_g).GetStringWidth(s), par3 + 5, par4 ? 0xffffff : 0x909090);
            }
            else
            {
                string s1 = "-";
                Field_27269_g.DrawString(GuiStats.GetFontRenderer(Field_27269_g), s1, par2 - (int)GuiStats.GetFontRenderer(Field_27269_g).GetStringWidth(s1), par3 + 5, par4 ? 0xffffff : 0x909090);
            }
        }
        
        protected override void Func_27257_b(int par1, int par2)
        {
            if (par2 < Top || par2 > Bottom)
            {
                return;
            }
            int i = Func_27256_c(par1, par2);
            int j = Field_27269_g.Width / 2 - 92 - 16;

            if (i >= 0)
            {
                if (par1 < j + 40 || par1 > j + 40 + 20)
                {
                    return;
                }
                StatCrafting statcrafting = Func_27264_b(i);
                Func_27267_a(statcrafting, par1, par2);
            }
            else
            {
                string s = "";
                if (par1 >= (j + 115) - 18 && par1 <= j + 115)
                {
                    s = Func_27263_a(0);
                }
                else if (par1 >= (j + 165) - 18 && par1 <= j + 165)
                {
                    s = Func_27263_a(1);
                }
                else if (par1 >= (j + 215) - 18 && par1 <= j + 215)
                {
                    s = Func_27263_a(2);
                }
                else
                {
                    return;
                }
                s = (new StringBuilder()).Append("").Append(StringTranslate.GetInstance().TranslateKey(s)).ToString().Trim();
                if (s.Length > 0)
                {
                    int k = par1 + 12;
                    int l = par2 - 12;
                    int i1 = GuiStats.GetFontRenderer(Field_27269_g).GetStringWidth(s);
                    GuiStats.DrawGradientRect(Field_27269_g, k - 3, l - 3, k + i1 + 3, l + 8 + 3, 0xc000000, 0xc000000);
                    GuiStats.GetFontRenderer(Field_27269_g).DrawStringWithShadow(s, k, l, -1);
                }
            }
        }
        
        protected void Func_27267_a(StatCrafting par1StatCrafting, int par2, int par3)
        {
            if (par1StatCrafting == null)
            {
                return;
            }
            Item item = Item.ItemsList[par1StatCrafting.GetItemID()];
            string s = new StringBuilder().Append("").Append(StringTranslate.GetInstance().TranslateNamedKey(item.GetItemName())).ToString().Trim();
            if (s.Length > 0)
            {
                int i = par2 + 12;
                int j = par3 - 12;
                int k = GuiStats.GetFontRenderer(Field_27269_g).GetStringWidth(s);
                GuiStats.DrawGradientRect(Field_27269_g, i - 3, j - 3, i + k + 3, j + 8 + 3, 0xc000000, 0xc000000);
                GuiStats.GetFontRenderer(Field_27269_g).DrawStringWithShadow(s, i, j, -1);
            }
        }
        
        protected void Func_27266_c(int par1)
        { 
            if (par1 != Field_27271_e)
            {
                Field_27271_e = par1; Field_27270_f = -1;
            }
            else if (Field_27270_f == -1)
            {
                Field_27270_f = 1;
            }
            else
            {
                Field_27271_e = -1; Field_27270_f = 0;
            }
            Field_27273_c.Sort(Field_27272_d);
        }
    }
}