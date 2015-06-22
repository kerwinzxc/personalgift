using System;
using DownloadManager;

public class DownloadableAsset : IDownloadable
{
		private string assetName;
		private string bundleName; 
		private DownloadProgressObserver.DownloadCallback callback;
		private System.Object[] options;
		private int priority = 1;
		public DownloadableAsset (string bundleName, string assetName, int priority, DownloadProgressObserver.DownloadCallback callback, System.Object[] options)
		{
				this.assetName = assetName;
				this.bundleName = bundleName;
				this.priority = priority;
				this.callback = callback;
				this.options = options;
		}

		
		public string GetBundleName ()
		{
				return this.bundleName;
		}
		
		public string GetAssetName ()
		{
				return this.assetName;
		}
		
		public DownloadProgressObserver.DownloadCallback GetCallback ()
		{
				return this.callback;
		}
		
		public System.Object[] GetParams ()
		{
				return this.options;
		}

		public string GetServerUrl ()
		{
				return "http://static-origin-mc.kiwiup.com/assets/";
		}
		
		public bool IsLoadOnComplete ()
		{
				return true;
		}

		public int GetVersion ()
		{
				return 1;
		}

		public int GetPriority ()
		{
				return this.priority;
		}
}