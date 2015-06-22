using UnityEngine;
using System;
using UnityEngine;

using System.Collections;

using System;

using System.Net;

using System.Net.Mail;

using System.Net.Security;

using System.Security.Cryptography.X509Certificates;



 
public class SendMail : MonoBehaviour {

	
	void Update() {
		if(Input.GetKeyUp(KeyCode.D)) {
		//	interactiveConsole.CallFBInit();
			SendEmail ();
			//SendEmailFromMe();
		}
	}

	void SendEmail ()
	{	

		string email = "msudhanshu@kiwiup.com";
		
		string subject = MyEscapeURL("My Subject");
		
		string body = MyEscapeURL("My Body\r\nFull of non-escaped chars");
		
		Debug.Log ("sending email "+email+",subject="+subject);
		Application.OpenURL ("mailto:" + email + "?subject=" + subject + "&body=" + body);
		
	}  

	string MyEscapeURL (string url)
		
	{
		
		return WWW.EscapeURL(url).Replace("+","%20");
		
	}




		void SendEmailFromMe ()
			
		{
			
			MailMessage mail = new MailMessage();
			
			
			
			mail.From = new MailAddress("sudhanshu.manjeet@gmail.com");
			
			mail.To.Add("msudhanshu@kiwiup.com");
			
			mail.Subject = "Test Mail";
			
			mail.Body = "This is for testing SMTP mail from GMAIL";
			
			
			
			SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
			
			smtpServer.Port = 587;
			
			smtpServer.Credentials = new System.Net.NetworkCredential("sudhanshu.manjeet@gmail.com", "prematma") as ICredentialsByHost;
			
			smtpServer.EnableSsl = true;
			
			ServicePointManager.ServerCertificateValidationCallback =
				
				delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
					
			{ return true; };
			
			smtpServer.Send(mail);
			
			Debug.Log("success");
			
		}
		




}