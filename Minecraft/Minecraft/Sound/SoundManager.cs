using System;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using OggSharp;

namespace net.minecraft.src
{
	public class SoundManager
	{
		/// <summary>
		/// A reference to the sound system. </summary>
		//private static SoundSystem SndSystem;

		/// <summary>
		/// Sound pool containing sounds. </summary>
		private SoundPool SoundPoolSounds;

		/// <summary>
		/// Sound pool containing streaming audio. </summary>
		private SoundPool SoundPoolStreaming;

		/// <summary>
		/// Sound pool containing music. </summary>
		private SoundPool SoundPoolMusic;

		/// <summary>
		/// The last ID used when a sound is played, passed into SoundSystem to give active sounds a unique ID
		/// </summary>
		private int LatestSoundID;

		/// <summary>
		/// A reference to the game settings. </summary>
		private GameSettings Options;

		/// <summary>
		/// Set to true when the SoundManager has been initialised. </summary>
		private static bool Loaded = false;

		/// <summary>
		/// RNG. </summary>
		private Random Rand;
		private int TicksBeforeMusic;

		public SoundManager()
		{
			SoundPoolSounds = new SoundPool();
			SoundPoolStreaming = new SoundPool();
			SoundPoolMusic = new SoundPool();
			LatestSoundID = 0;
			Rand = new Random();
			TicksBeforeMusic = Rand.Next(12000);
		}

		/// <summary>
		/// Used for loading sound settings from GameSettings
		/// </summary>
		public void LoadSoundSettings(GameSettings par1GameSettings)
		{
			SoundPoolStreaming.IsGetRandomSound = false;
			Options = par1GameSettings;

			if (!Loaded && (par1GameSettings == null || par1GameSettings.SoundVolume != 0.0F || par1GameSettings.MusicVolume != 0.0F))
			{
				TryToSetLibraryAndCodecs();
			}
		}

		/// <summary>
		/// Tries to add the paulscode library and the relevant codecs. If it fails, the volumes (sound and music) will be
		/// set to zero in the options file.
		/// </summary>
		private void TryToSetLibraryAndCodecs()
		{
			try
			{
				float f = Options.SoundVolume;
				float f1 = Options.MusicVolume;
				Options.SoundVolume = 0.0F;
				Options.MusicVolume = 0.0F;
				Options.SaveOptions();/*
				SoundSystemConfig.AddLibrary(typeof(LibraryLWJGLOpenAL));
				SoundSystemConfig.setCodec("ogg", typeof(CodecJOrbis));
				SoundSystemConfig.setCodec("mus", typeof(net.minecraft.src.CodecMus));
				SoundSystemConfig.setCodec("wav", typeof(CodecWav));
				SndSystem = new SoundSystem();*/
				Options.SoundVolume = f;
				Options.MusicVolume = f1;
				Options.SaveOptions();
			}
			catch (Exception throwable)
			{
                Utilities.LogException(throwable);

				Console.Error.WriteLine("error linking with the LibraryJavaSound plug-in");
			}

			Loaded = true;
		}

		/// <summary>
		/// Called when one of the sound level options has changed.
		/// </summary>
		public void OnSoundOptionsChanged()
		{
			if (!Loaded && (Options.SoundVolume != 0.0F || Options.MusicVolume != 0.0F))
			{
				TryToSetLibraryAndCodecs();
			}

			if (Loaded)
			{
				if (Options.MusicVolume == 0.0F)
				{
					//SndSystem.stop("BgMusic");
				}
				else
				{
					//SndSystem.setVolume("BgMusic", Options.MusicVolume);
				}
			}
		}

		/// <summary>
		/// Called when Minecraft is closing down.
		/// </summary>
		public void CloseMinecraft()
		{
			if (Loaded)
			{
				//SndSystem.cleanup();
			}
		}

		/// <summary>
		/// Adds a sounds with the name from the file. Args: name, file
		/// </summary>
		public void AddSound(string par1Str, string par2File)
		{
			SoundPoolSounds.AddSound(par1Str, par2File);
		}

		/// <summary>
		/// Adds an audio file to the streaming SoundPool.
		/// </summary>
		public void AddStreaming(string par1Str, string par2File)
		{
			SoundPoolStreaming.AddSound(par1Str, par2File);
		}

		/// <summary>
		/// Adds an audio file to the music SoundPool.
		/// </summary>
		public void AddMusic(string par1Str, string par2File)
		{
			SoundPoolMusic.AddSound(par1Str, par2File);
		}

		/// <summary>
		/// If its time to play new music it starts it up.
		/// </summary>
		public void PlayRandomMusicIfReady()
		{
			if (!Loaded || Options.MusicVolume == 0.0F)
			{
				return;
			}
            /*
			if (!SndSystem.playing("BgMusic") && !SndSystem.playing("streaming"))
			{
				if (TicksBeforeMusic > 0)
				{
					TicksBeforeMusic--;
					return;
				}

				SoundPoolEntry soundpoolentry = SoundPoolMusic.GetRandomSound();

				if (soundpoolentry != null)
				{
					TicksBeforeMusic = Rand.Next(12000) + 12000;
					SndSystem.backgroundMusic("BgMusic", soundpoolentry.SoundUrl, soundpoolentry.SoundName, false);
					SndSystem.setVolume("BgMusic", Options.MusicVolume);
					SndSystem.play("BgMusic");
				}
			}*/
		}

		/// <summary>
		/// Sets the listener of sounds
		/// </summary>
		public void SetListener(EntityLiving par1EntityLiving, float par2)
		{
			if (!Loaded || Options.SoundVolume == 0.0F)
			{
				return;
			}

			if (par1EntityLiving == null)
			{
				return;
			}
			else
			{
				float f = par1EntityLiving.PrevRotationYaw + (par1EntityLiving.RotationYaw - par1EntityLiving.PrevRotationYaw) * par2;
				double d = par1EntityLiving.PrevPosX + (par1EntityLiving.PosX - par1EntityLiving.PrevPosX) * (double)par2;
				double d1 = par1EntityLiving.PrevPosY + (par1EntityLiving.PosY - par1EntityLiving.PrevPosY) * (double)par2;
				double d2 = par1EntityLiving.PrevPosZ + (par1EntityLiving.PosZ - par1EntityLiving.PrevPosZ) * (double)par2;
				float f1 = MathHelper2.Cos(-f * 0.01745329F - (float)Math.PI);
				float f2 = MathHelper2.Sin(-f * 0.01745329F - (float)Math.PI);
				float f3 = -f2;
				float f4 = 0.0F;
				float f5 = -f1;
				float f6 = 0.0F;
				float f7 = 1.0F;
				float f8 = 0.0F;/*
				SndSystem.setListenerPosition((float)d, (float)d1, (float)d2);
				SndSystem.setListenerOrientation(f3, f4, f5, f6, f7, f8);*/
				return;
			}
		}

		public void PlayStreaming(string par1Str, float par2, float par3, float par4, float par5, float par6)
		{
			if (!Loaded || Options.SoundVolume == 0.0F && par1Str != null)
			{
				return;
			}

			string s = "streaming";
            /*
			if (SndSystem.playing("streaming"))
			{
				SndSystem.stop("streaming");
			}
            */
			if (par1Str == null)
			{
				return;
			}

			SoundPoolEntry soundpoolentry = SoundPoolStreaming.GetRandomSoundFromSoundPool(par1Str);
            /*
			if (soundpoolentry != null && par5 > 0.0F)
			{
				if (SndSystem.playing("BgMusic"))
				{
					SndSystem.stop("BgMusic");
				}

				float f = 16F;
				SndSystem.newStreamingSource(true, s, soundpoolentry.SoundUrl, soundpoolentry.SoundName, false, par2, par3, par4, 2, f * 4F);
				SndSystem.setVolume(s, 0.5F * Options.SoundVolume);
				SndSystem.play(s);
			}*/
		}

		/// <summary>
		/// Plays a sound. Args: soundName, x, y, z, volume, pitch
		/// </summary>
		public void PlaySound(string par1Str, float par2, float par3, float par4, float par5, float par6)
		{
			if (!Loaded || Options.SoundVolume == 0.0F)
			{
				return;
			}

			SoundPoolEntry soundpoolentry = SoundPoolSounds.GetRandomSoundFromSoundPool(par1Str);

			if (soundpoolentry != null && par5 > 0.0F)
			{
				LatestSoundID = (LatestSoundID + 1) % 256;
				string s = (new StringBuilder()).Append("sound_").Append(LatestSoundID).ToString();
				float f = 16F;

				if (par5 > 1.0F)
				{
					f *= par5;
				}
                /*
				SndSystem.newSource(par5 > 1.0F, s, soundpoolentry.SoundUrl, soundpoolentry.SoundName, false, par2, par3, par4, 2, f);
				SndSystem.setPitch(s, par6);
                */
				if (par5 > 1.0F)
				{
					par5 = 1.0F;
				}
                /*
				SndSystem.setVolume(s, par5 * Options.SoundVolume);
				SndSystem.play(s);*/
			}
		}

		/// <summary>
		/// Plays a sound effect with the volume and pitch of the parameters passed. The sound isn't affected by position of
		/// the player (full volume and center balanced)
		/// </summary>
		public void PlaySoundFX(string par1Str, float par2, float par3)
		{
			if (!Loaded || Options.SoundVolume == 0.0F)
			{
				return;
			}

			SoundPoolEntry soundpoolentry = SoundPoolSounds.GetRandomSoundFromSoundPool(par1Str);

			if (soundpoolentry != null)
			{
				LatestSoundID = (LatestSoundID + 1) % 256;
				string s = (new StringBuilder()).Append("sound_").Append(LatestSoundID).ToString();
				//SndSystem.newSource(false, s, soundpoolentry.SoundUrl, soundpoolentry.SoundName, false, 0.0F, 0.0F, 0.0F, 0, 0.0F);
                MemoryStream stream = new MemoryStream();
                using (var fileStream = new FileStream(soundpoolentry.SoundUrl, FileMode.Open))
                {
                    fileStream.CopyTo(stream);
                }
                OggSong song = new OggSong(stream);

				if (par2 > 1.0F)
				{
					par2 = 1.0F;
				}

				par2 *= 0.25F;/*
				SndSystem.setPitch(s, par3);
				SndSystem.setVolume(s, par2 * Options.SoundVolume);
				SndSystem.play(s);*/

                song.Pitch = par3 - 1;
                song.Volume = par2 * Options.SoundVolume;
                song.Play();
			}
		}
	}
}