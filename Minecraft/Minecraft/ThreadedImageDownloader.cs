using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace net.minecraft.src
{
    public class ThreadedImageDownloader
    {
		/// <summary>
		/// The image data. </summary>
		public Texture2D Image;

		/// <summary>
		/// Number of open references to this ThreadDownloadImageData </summary>
		public int ReferenceCount;

		/// <summary>
		/// The GL texture name associated with this image, or -1 if the texture has not been allocated
		/// </summary>
		public string TextureName;

		/// <summary>
		/// True if the texture has been allocated and the image copied to the texture.  This is reset if global rendering
		/// settings change, so that setupTexture will be called again.
		/// </summary>
		public bool TextureSetupComplete;

        /// <summary>
        /// The URL of the image to download. </summary>
        readonly string Location;

        /// <summary>
        /// The image buffer to use. </summary>
        readonly ImageBuffer Buffer;

        public ThreadedImageDownloader(string par1Str, ImageBuffer par2ImageBuffer)
		{
			ReferenceCount = 1;
			TextureName = "";
			TextureSetupComplete = false;

            Location = par1Str;
            Buffer = par2ImageBuffer;

            Run();
		}

        public virtual void Run()
        {
            HttpWebRequest httpurlconnection = null;

            try
            {
                httpurlconnection = (HttpWebRequest)WebRequest.Create(Location);
                HttpWebResponse response = (HttpWebResponse)httpurlconnection.GetResponse();

                if ((int)response.StatusCode / 100 == 4)
                {
                    return;
                }

                if (Buffer == null)
                {
                    Image = Texture2D.FromStream(RenderEngine.Instance.GraphicsDevice, response.GetResponseStream());
                }
                else
                {
                    Image = Buffer.ParseUserSkin(Texture2D.FromStream(RenderEngine.Instance.GraphicsDevice, response.GetResponseStream()));
                }
            }
            catch (Exception exception)
            {
                Utilities.LogException(exception);
            }/*
            finally
            {
                httpurlconnection.Disconnect();
            }*/
        }
    }
}
