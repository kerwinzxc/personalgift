using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace DownloadManager
{
		public class DownloadProgressObserver
		{
	
				private string assetName;
				private DownloadRequest downloadRequest;
				private DownloadCallback callback;
				public delegate void DownloadCallback (string bundleName,string assetName,params System.Object[] options);
				private  string bundleName;
				private System.Object[] options;


				public DownloadProgressObserver (IDownloadable downloadable)
				{
						this.callback = downloadable.GetCallback ();
						this.bundleName = downloadable.GetBundleName ();
						this.assetName = downloadable.GetAssetName ();
						this.options = downloadable.GetParams ();
				}

				public void OnComplete ()
				{
						this.callback (this.bundleName, this.assetName, this.options);
				}

				public void OnFailure ()
				{
						//Post failure notification
				}
		}
}
