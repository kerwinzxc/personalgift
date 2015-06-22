using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace DownloadManager
{
		public class DownloadRequestManager
		{
				public bool running = false;
				protected static DownloadRequestManager manager;
				public DownloadRequestManager ()
				{

				}

				public SortedDictionary<int, Queue<DownloadRequest>> requestQueue = new SortedDictionary<int, Queue<DownloadRequest>> ();

				public void EnqueueDownload (DownloadRequest request)
				{
						if (!requestQueue.ContainsKey (request.GetPriority ())) {
								requestQueue.Add (request.GetPriority (), new Queue<DownloadRequest> ());
						}
						requestQueue [request.GetPriority ()].Enqueue (request);
				}

				//Loop over the download queue
				public IEnumerator Execute (MonoBehaviour mbObject)
				{
						if (!running && requestQueue.Count > 0) {
								running = true;
								DownloadRequest request = GetPriorityRequest ();
								yield return mbObject.StartCoroutine (request.Download ());
								CleanupQueue ();
								running = false;
						}
						yield break;
				}

				private void CleanupQueue ()
				{
						List<int> removals = new List<int> ();
						foreach (int priorityKey in requestQueue.Keys) {
								if (requestQueue [priorityKey] == null && requestQueue [priorityKey].Count == 0) {
										removals.Add (priorityKey);
								}
						}
						foreach (int emptyKey in removals) {
								requestQueue.Remove (emptyKey);
						}
				}

				private DownloadRequest GetPriorityRequest ()
				{
						foreach (int priorityKey in requestQueue.Keys) {
								if (requestQueue [priorityKey].Count > 0) {
										return requestQueue [priorityKey].Dequeue ();
								}
						}
						return null;
				}
		}
}
