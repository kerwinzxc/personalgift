using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DownloadManager;

public class PendingDownloadsManager
{
		public bool initialised = false;
		public PendingDownloadsManager ()
		{
		}

		public enum DownloadState
		{
				Pending=0,
				InProgress=1,
				Complete=2,
				Failed=3
		}

		public IEnumerator Init ()
	{
		yield return 0;
		}
		/*			if (DatabaseManager.GetInstance ().GetDbHelper () != null) {
							this.rescheduleFailedDownloads ();
						this.scheduleDownloads ();
						initialised = true;
						yield break;
				} else
						yield return initialised;
		
		}

		public static void AddToPendingDownloads (IDownloadable downloadable)
		{
				PendingDownload.Add (downloadable.GetBundleName (), downloadable.GetPriority (), (int)DownloadState.Pending);
		}

		private void rescheduleFailedDownloads ()
		{
				string sql = "update PendingDownload set status = " + (int)DownloadState.Pending + " where status in (" + (int)DownloadState.InProgress + "," + (int)DownloadState.Complete + ")";
				DatabaseManager.GetInstance ().GetDbHelper ().ExecuteSql (sql);
		}

		public void scheduleDownloads ()
		{
				string sql = "select * from PendingDownload where status = " + (int)DownloadState.Pending;
				List<PendingDownload> pendingDownloads = DatabaseManager.GetInstance ().GetDbHelper ().Query<PendingDownload> (sql);
				foreach (PendingDownload pendingDownload in pendingDownloads) {
						DaemonDownloadable downloadable = new DaemonDownloadable (pendingDownload.bundleName, pendingDownload.priority);
						AssetBundleManager.GetInstance ().GetAssetBundle (downloadable);
						pendingDownload.status = (int)DownloadState.InProgress;
						DatabaseManager.GetInstance ().GetDbHelper ().Update<PendingDownload> (pendingDownload);
				}
		}

*/


}
