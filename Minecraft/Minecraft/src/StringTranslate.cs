using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;

namespace net.minecraft.src
{
    public class StringTranslate
    {
        /// <summary>
        /// Is the private singleton instance of StringTranslate. </summary>
        private static StringTranslate instance = new StringTranslate();

        /// <summary>
        /// Contains all key/value pairs to be translated - is loaded from '/lang/en_US.lang' when the StringTranslate is
        /// created.
        /// </summary>
        private Dictionary<string, string> translateTable;
        private SortedDictionary<string, string> languageList;
        private string currentLanguage;
        private bool isUnicode;

        private StringTranslate()
        {
            translateTable = new Dictionary<string, string>();
            LoadLanguageList();
            SetLanguage("en_US");
        }

        /// <summary>
        /// Return the StringTranslate singleton instance
        /// </summary>
        public static StringTranslate GetInstance()
        {
            return instance;
        }

        private void LoadLanguageList()
        {
            SortedDictionary<string, string> treemap = new SortedDictionary<string, string>();

            try
            {
                StreamReader bufferedreader = new StreamReader(Minecraft.GetResourceStream("lang.languages.txt"), Encoding.UTF8);

                for (string s = bufferedreader.ReadLine(); s != null; s = bufferedreader.ReadLine())
                {
                    string[] @as = StringHelperClass.StringSplit(s, "=", true);

                    if (@as != null && @as.Length == 2)
                    {
                        treemap[@as[0]] = @as[1];
                    }
                }
            }
            catch (IOException ioexception)
            {
                Utilities.LogException(ioexception);
                return;
            }

            languageList = treemap;
        }

        public virtual SortedDictionary<string, string> GetLanguageList()
        {
            return languageList;
        }

        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
        //ORIGINAL LINE: private void loadLanguage(Properties par1Properties, String par2Str) throws IOException
        private void LoadLanguage(Dictionary<string, string> par1Properties, string par2Str)
        {
            StreamReader bufferedreader = new StreamReader(Minecraft.GetResourceStream(new StringBuilder().Append("lang.").Append(par2Str).Append(".lang").ToString()), Encoding.UTF8);

            for (string s = bufferedreader.ReadLine(); s != null; s = bufferedreader.ReadLine())
            {
                s = s.Trim();

                if (s.StartsWith("#"))
                {
                    continue;
                }

                string[] @as = s.Split('=');

                if (@as != null && @as.Length == 2)
                {
                    par1Properties[@as[0]] = @as[1];
                }
            }
        }

        public virtual void SetLanguage(string par1Str)
        {
            if (!par1Str.Equals(this.currentLanguage))
            {
                Dictionary<string, string> var2 = new Dictionary<string, string>();

                try
                {
                    LoadLanguage(var2, "en_US");
                }
                catch (IOException var8)
                {
                    Utilities.LogException(var8);
                }

                isUnicode = false;

                if (!"en_US".Equals(par1Str))
                {
                    try
                    {
                        LoadLanguage(var2, par1Str);
                        IEnumerator<string> var3 = var2.Keys.GetEnumerator();

                        while (var3.MoveNext() && !this.isUnicode)
                        {
                            object var5 = var2[var3.Current];

                            if (var5 != null)
                            {
                                string var6 = var5.ToString();

                                for (int var7 = 0; var7 < var6.Length; ++var7)
                                {
                                    if (var6[var7] >= 256)
                                    {
                                        this.isUnicode = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    catch (IOException var9)
                    {
                        Utilities.LogException(var9);
                        return;
                    }
                }

                this.currentLanguage = par1Str;
                this.translateTable = var2;
            }
        }

        public virtual string GetCurrentLanguage()
        {
            return currentLanguage;
        }

        public virtual bool IsUnicode()
        {
            return isUnicode;
        }

        /// <summary>
        /// Translate a key to current language.
        /// </summary>
        public virtual string TranslateKey(string par1Str)
        {
            return (translateTable.ContainsKey(par1Str) ? translateTable[par1Str] : par1Str);
        }

        /// <summary>
        /// Translate a key to current language applying String.format()
        /// </summary>
        public virtual string TranslateKeyFormat(string par1Str, object[] par2ArrayOfObj)
        {
            string s = (translateTable.ContainsKey(par1Str) ? translateTable[par1Str] : par1Str);
            return string.Format(s, par2ArrayOfObj);
        }

        /// <summary>
        /// Translate a key with a extra '.name' at end added, is used by blocks and items.
        /// </summary>
        public virtual string TranslateNamedKey(string par1Str)
        {
            var key = new StringBuilder().Append(par1Str).Append(".name").ToString();
            return (translateTable.ContainsKey(key) ? translateTable[key] : "");
        }

        public static bool IsBidrectional(string par0Str)
        {
            return "ar_SA".Equals(par0Str) || "he_IL".Equals(par0Str);
        }
    }
}