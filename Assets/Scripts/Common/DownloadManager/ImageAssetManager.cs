using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DownloadManager;

public class ImageAssetManager : Manager<ImageAssetManager>
{
		static Dictionary<string, Texture2D> imageAssets = new Dictionary<string, Texture2D> ();
		private BaseDownloadManager baseDownloadManager;
		PendingDownloadsManager pendingDownloadManager;

		public void OnDownloadComplete (DownloadRequest request, WWW data)
		{
				if (request.loadOnComplete) {
					imageAssets.Add (request.fileName, data.texture);
				} else {
						UnloadAssetBundle (data.assetBundle, request.fileName, request.loadOnComplete);
				}
		}

		private void UnloadAssetBundle (AssetBundle bundle, string bundleName, bool tocache)
		{
				bundle.Unload (true);
				if (tocache && imageAssets.ContainsKey (bundleName))
					imageAssets.Remove (bundleName);
		}

		public Texture2D GetAssetBundle (IDownloadable downloadable)
		{
				string bundleName = downloadable.GetBundleName ();	
			if (downloadable.IsLoadOnComplete () && imageAssets.ContainsKey (bundleName))
				return imageAssets [bundleName];
				else
						baseDownloadManager.LoadBundle (downloadable);
				return null;
		}

		public bool IsBundleCached (String url, int version)
		{
				return !Caching.IsVersionCached (url, version);
		}

		void Start ()
		{
				DontDestroyOnLoad (this.gameObject);
		}

		override public void StartInit ()
		{
				baseDownloadManager = new BaseDownloadManager ();
				pendingDownloadManager = new PendingDownloadsManager ();
		}

		override public void PopulateDependencies() {
				dependencies = new List<ManagerDependency>();
				dependencies.Add(ManagerDependency.DATABASE_INITIALIZED);
				dependencies.Add(ManagerDependency.DATA_LOADED);
		}
	
		void Update ()
		{
				if (!initialised)
						return;
				if (baseDownloadManager.ShouldStart ())
						StartCoroutine (baseDownloadManager.ExecuteDownloadQueue (this));
				if (!pendingDownloadManager.initialised) {
						StartCoroutine (pendingDownloadManager.Init ());
				}
		}

}
