using System;
namespace DownloadManager
{
		public interface IDownloadable
		{
				String GetBundleName ();
				String GetAssetName ();
				DownloadProgressObserver.DownloadCallback GetCallback ();
				String GetServerUrl ();
				bool IsLoadOnComplete ();
				int GetVersion ();
				System.Object[] GetParams ();
				int GetPriority ();
		}
}
