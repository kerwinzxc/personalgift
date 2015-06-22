using System;
using UnityEngine;
using System.Collections;
namespace DownloadManager
{
		public class BaseDownloadManager
		{
				private DownloadRequestManager downloadRequestManager;

				public BaseDownloadManager ()
				{
						downloadRequestManager = new DownloadRequestManager ();
				}

				public void LoadBundle (IDownloadable downloadable)
				{
						DownloadProgressObserver downloadProgressObserver = new DownloadProgressObserver (downloadable);
						//Create download Request
						DownloadRequest req = new DownloadRequest ();
						//Instantiate req manager
						req.Init (downloadable, downloadProgressObserver);
						//Schedule download
						downloadRequestManager.EnqueueDownload (req);
				}

				public IEnumerator ExecuteDownloadQueue (MonoBehaviour mbObject)
				{
						yield return mbObject.StartCoroutine (downloadRequestManager.Execute (mbObject));
				}

				public bool ShouldStart ()
				{
						return !downloadRequestManager.running && downloadRequestManager.requestQueue.Count > 0;
				}
		}

}

