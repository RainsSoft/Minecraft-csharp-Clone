using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace net.minecraft.src
{
    using IOPath = System.IO.Path;

	public class StatsSyncher
	{
		private volatile bool IsBusy;
        private volatile Dictionary<StatBase, int> Field_27437_b;
        private volatile Dictionary<StatBase, int> Field_27436_c;

		/// <summary>
		/// The StatFileWriter object, presumably used to write to the statistics files
		/// </summary>
		private StatFileWriter StatFileWriter;

		/// <summary>
		/// A file named 'stats_' [lower case username] '_unsent.dat' </summary>
		private string UnsentDataFile;

		/// <summary>
		/// A file named 'stats_' [lower case username] '.dat' </summary>
		private string DataFile;

		/// <summary>
		/// A file named 'stats_' [lower case username] '_unsent.tmp' </summary>
		private string UnsentTempFile;

		/// <summary>
		/// A file named 'stats_' [lower case username] '.tmp' </summary>
		private string TempFile;

		/// <summary>
		/// A file named 'stats_' [lower case username] '_unsent.old' </summary>
		private string UnsentOldFile;

		/// <summary>
		/// A file named 'stats_' [lower case username] '.old' </summary>
		private string OldFile;

		/// <summary>
		/// The Session object </summary>
		private Session TheSession;
		private int Field_27427_l;
		private int Field_27426_m;

		public StatsSyncher(Session par1Session, StatFileWriter par2StatFileWriter, string par3File)
		{
			IsBusy = false;
			Field_27437_b = null;
			Field_27436_c = null;
			Field_27427_l = 0;
			Field_27426_m = 0;
			UnsentDataFile = IOPath.Combine(par3File, (new StringBuilder()).Append("stats_").Append(par1Session.Username.ToLower()).Append("_unsent.dat").ToString());
            DataFile = IOPath.Combine(par3File, (new StringBuilder()).Append("stats_").Append(par1Session.Username.ToLower()).Append(".dat").ToString());
            UnsentOldFile = IOPath.Combine(par3File, (new StringBuilder()).Append("stats_").Append(par1Session.Username.ToLower()).Append("_unsent.old").ToString());
            OldFile = IOPath.Combine(par3File, (new StringBuilder()).Append("stats_").Append(par1Session.Username.ToLower()).Append(".old").ToString());
            UnsentTempFile = IOPath.Combine(par3File, (new StringBuilder()).Append("stats_").Append(par1Session.Username.ToLower()).Append("_unsent.tmp").ToString());
            TempFile = IOPath.Combine(par3File, (new StringBuilder()).Append("stats_").Append(par1Session.Username.ToLower()).Append(".tmp").ToString());

			if (!par1Session.Username.ToLower().Equals(par1Session.Username))
			{
				Func_28214_a(par3File, (new StringBuilder()).Append("stats_").Append(par1Session.Username).Append("_unsent.dat").ToString(), UnsentDataFile);
				Func_28214_a(par3File, (new StringBuilder()).Append("stats_").Append(par1Session.Username).Append(".dat").ToString(), DataFile);
				Func_28214_a(par3File, (new StringBuilder()).Append("stats_").Append(par1Session.Username).Append("_unsent.old").ToString(), UnsentOldFile);
				Func_28214_a(par3File, (new StringBuilder()).Append("stats_").Append(par1Session.Username).Append(".old").ToString(), OldFile);
				Func_28214_a(par3File, (new StringBuilder()).Append("stats_").Append(par1Session.Username).Append("_unsent.tmp").ToString(), UnsentTempFile);
				Func_28214_a(par3File, (new StringBuilder()).Append("stats_").Append(par1Session.Username).Append(".tmp").ToString(), TempFile);
			}

			StatFileWriter = par2StatFileWriter;
			TheSession = par1Session;

			if (File.Exists(UnsentDataFile))
			{
				par2StatFileWriter.Func_27179_a(Func_27415_a(UnsentDataFile, UnsentTempFile, UnsentOldFile));
			}

			BeginReceiveStats();
		}

		private void Func_28214_a(string par1File, string par2Str, string par3File)
		{
			string file = IOPath.Combine(par1File, par2Str);

			if (File.Exists(file) && !Directory.Exists(file) && !File.Exists(par3File))
			{
				File.Move(file, par3File);
			}
		}

        private Dictionary<StatBase, int> Func_27415_a(string par1File, string par2File, string par3File)
		{
			if (File.Exists(par1File))
			{
				return Func_27408_a(par1File);
			}

			if (File.Exists(par3File))
			{
				return Func_27408_a(par3File);
			}

			if (File.Exists(par2File))
			{
				return Func_27408_a(par2File);
			}
			else
			{
				return null;
			}
		}

        private Dictionary<StatBase, int> Func_27408_a(string par1File)
		{
			StreamReader bufferedreader = null;

			try
			{
				bufferedreader = new StreamReader(par1File);
				string s = "";
				StringBuilder stringbuilder = new StringBuilder();

				while ((s = bufferedreader.ReadLine()) != null)
				{
					stringbuilder.Append(s);
				}

                Dictionary<StatBase, int> map = StatFileWriter.Func_27177_a(stringbuilder.ToString());
				return map;
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.ToString());
				Console.Write(exception.StackTrace);
			}
			finally
			{
				if (bufferedreader != null)
				{
					try
					{
						bufferedreader.Close();
					}
					catch (Exception exception2)
					{
						Console.WriteLine(exception2.ToString());
						Console.Write(exception2.StackTrace);
					}
				}
			}

			return null;
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: private void Func_27410_a(java.util.Map par1Map, File par2File, File par3File, File par4File) throws IOException
        private void Func_27410_a(Dictionary<StatBase, int> par1Map, string par2File, string par3File, string par4File)
		{
			StreamWriter printwriter = new StreamWriter(par3File, false);

			try
			{
				printwriter.Write(StatFileWriter.Func_27185_a(TheSession.Username, "local", par1Map));
			}
			finally
			{
				printwriter.Close();
			}

			if (File.Exists(par4File))
			{
				File.Delete(par4File);
			}

			if (File.Exists(par2File))
			{
				File.Move(par2File, par4File);
			}

			File.Move(par3File, par2File);
		}

		/// <summary>
		/// Attempts to begin receiving stats from the server. Will throw an IllegalStateException if the syncher is already
		/// busy.
		/// </summary>
		public virtual void BeginReceiveStats()
		{
			if (IsBusy)
			{
				throw new InvalidOperationException("Can't get stats from server while StatsSyncher is busy!");
			}
			else
			{
				Field_27427_l = 100;
				IsBusy = true;

                Action statSyncherReceive = () =>
                {
                    try
                    {
                        if (StatsSyncher.Func_27422_a(this) != null)
                        {
                            StatsSyncher.Func_27412_a(this, StatsSyncher.Func_27422_a(this), StatsSyncher.Func_27423_b(this), StatsSyncher.Func_27411_c(this), StatsSyncher.Func_27413_d(this));
                        }
                        else if (File.Exists(StatsSyncher.Func_27423_b(this)))
                        {
                            StatsSyncher.Func_27421_a(this, StatsSyncher.Func_27409_a(this, StatsSyncher.Func_27423_b(this), StatsSyncher.Func_27411_c(this), StatsSyncher.Func_27413_d(this)));
                        }
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception.ToString());
                        Console.Write(exception.StackTrace);
                    }
                    finally
                    {
                        StatsSyncher.SetBusy(this, false);
                    }
                };

                new Thread(new ThreadStart(statSyncherReceive)).Start();
				return;
			}
		}

		/// <summary>
		/// Attempts to begin sending stats to the server. Will throw an IllegalStateException if the syncher is already
		/// busy.
		/// </summary>
        public virtual void BeginSendStats(Dictionary<StatBase, int> par1Map)
		{
			if (IsBusy)
			{
				throw new InvalidOperationException("Can't save stats while StatsSyncher is busy!");
			}
			else
			{
				Field_27427_l = 100;
				IsBusy = true;

                Action statSyncerSend = () =>
                {
                    try
                    {
                        StatsSyncher.Func_27412_a(this, par1Map, StatsSyncher.GetUnsentDataFile(this), StatsSyncher.GetUnsentTempFile(this), StatsSyncher.GetUnsentOldFile(this));
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception.ToString());
                        Console.Write(exception.StackTrace);
                    }
                    finally
                    {
                        StatsSyncher.SetBusy(this, false);
                    }
                };

                new Thread(new ThreadStart(statSyncerSend)).Start();
				return;
			}
		}

		public virtual void SyncStatsFileWithMap(Dictionary<StatBase, int> par1Map)
		{
			for (int i = 30; IsBusy && --i > 0;)
			{
				try
				{
					Thread.Sleep(100);
				}
				catch (ThreadInterruptedException interruptedexception)
				{
					Console.WriteLine(interruptedexception.ToString());
					Console.Write(interruptedexception.StackTrace);
				}
			}

			IsBusy = true;

			try
			{
				Func_27410_a(par1Map, UnsentDataFile, UnsentTempFile, UnsentOldFile);
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.ToString());
				Console.Write(exception.StackTrace);
			}
			finally
			{
				IsBusy = false;
			}
		}

		public virtual bool Func_27420_b()
		{
			return Field_27427_l <= 0 && !IsBusy && Field_27436_c == null;
		}

		public virtual void Func_27425_c()
		{
			if (Field_27427_l > 0)
			{
				Field_27427_l--;
			}

			if (Field_27426_m > 0)
			{
				Field_27426_m--;
			}

			if (Field_27436_c != null)
			{
				StatFileWriter.Func_27187_c(Field_27436_c);
				Field_27436_c = null;
			}

			if (Field_27437_b != null)
			{
				StatFileWriter.Func_27180_b(Field_27437_b);
				Field_27437_b = null;
			}
		}

        static Dictionary<StatBase, int> Func_27422_a(StatsSyncher par0StatsSyncher)
		{
			return par0StatsSyncher.Field_27437_b;
		}

        static string Func_27423_b(StatsSyncher par0StatsSyncher)
		{
			return par0StatsSyncher.DataFile;
		}

        static string Func_27411_c(StatsSyncher par0StatsSyncher)
		{
			return par0StatsSyncher.TempFile;
		}

        static string Func_27413_d(StatsSyncher par0StatsSyncher)
		{
			return par0StatsSyncher.OldFile;
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: static void Func_27412_a(StatsSyncher par0StatsSyncher, java.util.Map par1Map, File par2File, File par3File, File par4File) throws IOException
        static void Func_27412_a(StatsSyncher par0StatsSyncher, Dictionary<StatBase, int> par1Map, string par2File, string par3File, string par4File)
		{
			par0StatsSyncher.Func_27410_a(par1Map, par2File, par3File, par4File);
		}

        static Dictionary<StatBase, int> Func_27421_a(StatsSyncher par0StatsSyncher, Dictionary<StatBase, int> par1Map)
		{
			return par0StatsSyncher.Field_27437_b = par1Map;
		}

        static Dictionary<StatBase, int> Func_27409_a(StatsSyncher par0StatsSyncher, string par1File, string par2File, string par3File)
		{
			return par0StatsSyncher.Func_27415_a(par1File, par2File, par3File);
		}

		static bool SetBusy(StatsSyncher par0StatsSyncher, bool par1)
		{
			return par0StatsSyncher.IsBusy = par1;
		}

        static string GetUnsentDataFile(StatsSyncher par0StatsSyncher)
		{
			return par0StatsSyncher.UnsentDataFile;
		}

        static string GetUnsentTempFile(StatsSyncher par0StatsSyncher)
		{
			return par0StatsSyncher.UnsentTempFile;
		}

        static string GetUnsentOldFile(StatsSyncher par0StatsSyncher)
		{
			return par0StatsSyncher.UnsentOldFile;
		}
	}
}