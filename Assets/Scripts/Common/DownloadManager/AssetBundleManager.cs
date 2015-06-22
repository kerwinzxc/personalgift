using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DownloadManager;

public class AssetBundleManager : Manager<AssetBundleManager>
{
		static Dictionary<string, AssetBundle> assetBundles = new Dictionary<string, AssetBundle> ();
		private BaseDownloadManager baseDownloadManager;
		PendingDownloadsManager pendingDownloadManager;

		public void OnDownloadComplete (DownloadRequest request, WWW data)
		{
				if (request.loadOnComplete) {
						assetBundles.Add (request.fileName, data.assetBundle);
				} else {
						UnloadAssetBundle (data.assetBundle, request.fileName, request.loadOnComplete);
				}
		}

		private void UnloadAssetBundle (AssetBundle bundle, string bundleName, bool tocache)
		{
				bundle.Unload (true);
				if (tocache && assetBundles.ContainsKey (bundleName))
						assetBundles.Remove (bundleName);
		}

		public AssetBundle GetAssetBundle (IDownloadable downloadable)
		{
				string bundleName = downloadable.GetBundleName ();	
				if (downloadable.IsLoadOnComplete () && assetBundles.ContainsKey (bundleName))
						return assetBundles [bundleName];
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
