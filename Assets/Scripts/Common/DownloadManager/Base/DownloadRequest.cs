using System;
using UnityEngine;
using System.Collections;
namespace DownloadManager
{
		public class DownloadRequest
		{
				public String fileName;
				public String serverUrl;
				public DownloadProgressObserver observer;
				public int status;
				public int priority = 1;
				public bool loadOnComplete;
				public int downloadVersion;
				public const string FileExt = ".unity3d";
		
				public DownloadRequest ()
				{
				}
		
				public void Init (IDownloadable downloadable, DownloadProgressObserver observer)
				{
						this.fileName = downloadable.GetBundleName ();
						this.serverUrl = downloadable.GetServerUrl ();
						this.observer = observer;
						this.loadOnComplete = downloadable.IsLoadOnComplete ();
						this.downloadVersion = downloadable.GetVersion ();
						this.priority = downloadable.GetPriority ();
				}

				public void OnComplete (WWW download)
				{
						if (!string.IsNullOrEmpty (download.error)) {
								OnFailure (download);
						} else {
								OnSuccess (download);
						}
				}

				public String GetDownloadUrl ()
				{
						return serverUrl + fileName + FileExt;
				}

				public void OnFailure (WWW download)
				{
						Debug.LogError ("Download failed " + download.error + " " + download.url);
						observer.OnFailure ();
						//Post failure handling
				}

				public void OnSuccess (WWW download)
				{
						if (download != null)
								AssetBundleManager.GetInstance ().OnDownloadComplete (this, download);
						observer.OnComplete ();
				}

				public IEnumerator Download ()
				{
						if (!AssetBundleManager.GetInstance ().IsBundleCached (GetDownloadUrl (), this.downloadVersion) || this.loadOnComplete) {
								WWW download = WWW.LoadFromCacheOrDownload (GetDownloadUrl (), this.downloadVersion);
								yield return download;
								OnComplete (download);
						} else
								OnSuccess (null);
						yield break;
				}

				public int GetPriority ()
				{
						return priority;
				}
		}
}
