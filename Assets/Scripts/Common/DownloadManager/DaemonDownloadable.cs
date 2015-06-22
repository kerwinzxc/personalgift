using System;
using UnityEngine;
public class DaemonDownloadable : DownloadableAsset
{
		public DaemonDownloadable (string bundleName, int priority) : base(bundleName, null, priority, OnDownloadComplete, null)
		{
				//Do Nothing
		}

		public static void OnDownloadComplete (string bundleName, string assetName, params System.Object[] options)
		{
				string sql = "update PendingDownload set status = " + (int)PendingDownloadsManager.DownloadState.Complete + " where bundleName = '" + bundleName + "'";
				//FIXME : MANJEET
				//DatabaseManager.GetInstance ().GetDbHelper ().ExecuteSql (sql);
		}

		public bool IsLoadOnComplete ()
		{
				return false;
		}

}

