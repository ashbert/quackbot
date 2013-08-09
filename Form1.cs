using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Messenger;
using System.Net;

namespace ashbot
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox textBox1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		
		private System.ComponentModel.Container components = null;
		private Messenger.MsgrObject msgobj = new MsgrObject();
		private Messenger.IMsgrService mserv;
		private System.Windows.Forms.TextBox textBox2;
		private Messenger.IMsgrUser imuser;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Random rand = new Random();
		public System.IO.StreamReader Stream;
		public int[] list = new int[24] {322,134,374,272,213,162,113,91,271,22,15,105,192,52,98,359,30,216,331,147,65,102,56,13};

		private static string header = 
			"MIME-Version: 1.0\r\nContent-Type: text/plain;" +
			" charset=UTF-8\r\nX-MMS-IM-Format: " +
			"FN=MS%20Shell%20Dlg; EF=; CO=FF; CS=0; PF=0\r\n\r\n";
		//For the word
		private static string header1 = 
			"MIME-Version: 1.0\r\nContent-Type: text/plain;" +
			" charset=UTF-8\r\nX-MMS-IM-Format: " +
			"FN=MS%20Shell%20Dlg; EF=B; CO=666600; CS=0; PF=0\r\n\r\n";
		//For the meaning
		private static string header2 = 
			"MIME-Version: 1.0\r\nContent-Type: text/plain;" +
			" charset=UTF-8\r\nX-MMS-IM-Format: " +
			"FN=MS%20Shell%20Dlg; EF=; CO=336600; CS=0; PF=0\r\n\r\n";
		//For the sentence
		private static string header3 = 
			"MIME-Version: 1.0\r\nContent-Type: text/plain;" +
			" charset=UTF-8\r\nX-MMS-IM-Format: " +
			"FN=MS%20Shell%20Dlg; EF=; CO=330000; CS=0; PF=0\r\n\r\n";

		public string m_strSite;
		public string htm = ".htm";		
		public string path;
		public string ext;
		
		
		int nIndexStart = 0;
		int nIndexEnd = 0;
		int nIndex = 0;
		int randomno = 0;
		int randomno2 = 0;


		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			msgobj.OnNewSessionRequest += new DMsgrObjectEvents_OnNewSessionRequestEventHandler(this.newreq);
			msgobj.OnUserJoin += new DMsgrObjectEvents_OnUserJoinEventHandler(this.userjoin);
			msgobj.OnTextReceived += new DMsgrObjectEvents_OnTextReceivedEventHandler(this.ontextrecv);				
			
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(56, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(248, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "Messages recieved";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(56, 192);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(248, 24);
			this.label2.TabIndex = 3;
			this.label2.Text = "Server responses";
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(56, 224);
			this.textBox2.Multiline = true;
			this.textBox2.Name = "textBox2";
			this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBox2.Size = new System.Drawing.Size(384, 136);
			this.textBox2.TabIndex = 1;
			this.textBox2.Text = "";
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(56, 56);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBox1.Size = new System.Drawing.Size(384, 128);
			this.textBox1.TabIndex = 0;
			this.textBox1.Text = "";
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(504, 397);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.label2,
																		  this.label1,
																		  this.textBox2,
																		  this.textBox1});
			this.Name = "Form1";
			this.Text = "Quack Bot Engine";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
			
			msgobj.Logon("ashbott@hotmail.com","freakass",mserv);						
		}

		private void newreq(IMsgrUser User, IMsgrIMSession Session, ref bool enabled)
		{
			textBox1.Text += "session from:--  " + User.FriendlyName.ToString()+ "\r\n";			
			string msg = "Hey <" + User.FriendlyName + ">! I'm QuackBot - the erudite psycho!! try me.";
			string msg2 = "Available commands: GRE (caps on)";
			User.SendText(header,msg,Messenger.MMSGTYPE.MMSGTYPE_ALL_RESULTS);						
			User.SendText(header,msg2,Messenger.MMSGTYPE.MMSGTYPE_ALL_RESULTS);						
		}
		
		private void ontextrecv(IMsgrIMSession Session, IMsgrUser User, string Header, string MsgText, ref bool Enabled)
		{
			if(Header.IndexOf("Content-Type: text/plain") != -1 && User.FriendlyName != "Hotmail")
			{			
				textBox2.Text += "ASS header says:--  " + Header.ToString()+ "\r\n";
				textBox1.Text += User.FriendlyName.ToString() +" says :--" + MsgText.ToString() + "\r\n";
				imuser = User;
				Enabled = false;				
				if(MsgText.IndexOf("fuck",0)!= -1)
				{
					imuser.SendText(header,"Same to you jackass !!",Messenger.MMSGTYPE.MMSGTYPE_ALL_RESULTS);
				}
				else if(MsgText.IndexOf("GRE",0) != -1) 
				{
					//imuser.SendText(header,"Please specify a wordlist: ListA /B /C..../XYZ (Check syntax)",Messenger.MMSGTYPE.MMSGTYPE_ALL_RESULTS);
					calllist(imuser);
				}
					/*else if(MsgText.IndexOf("List",0)!= -1)					
					{
						calllist(MsgText,imuser);
					}*/
				else
					imuser.SendText(header,"I'm not programmed to chat yet!",Messenger.MMSGTYPE.MMSGTYPE_ALL_RESULTS);					
			}
		}
		private void calllist(IMsgrUser usr)
		{
			path = "D:\\Wordlist\\";
			/*mtext = String.Concat(mtext,htm);
			path = String.Concat(path,mtext);
			textBox2.Text+= path;*/
			randomno = rand.Next(0,23);			
			int num;
			num = list[randomno];
			ext = String.Concat(num.ToString(),htm);
			path = String.Concat(path,ext);

			try
			{
				WebRequest req = WebRequest.Create(path);
								
				System.IO.StreamReader stream = new System.IO.StreamReader(req.GetResponse().GetResponseStream());
						
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				string strLine;
				// Read the stream a line at a time and place each one
				// into the stringbuilder
				while( (strLine = stream.ReadLine()) != null )
				{
					// Ignore blank lines
					if(strLine.Length > 0 )
						sb.Append(strLine);
				}
				// Finished with the stream so close it now
				stream.Close();

				// Cache the streamed site now so it can be used
				// without reconnecting later
				m_strSite = sb.ToString();
				//textBox1.Text = m_strSite;
				string temp;
				
				randomno2 = rand.Next(1,num);
				temp = randomno2.ToString();
				temp = String.Concat(temp,".");

				nIndex = m_strSite.IndexOf(temp,0);
			
				//getting the word
				nIndexStart = m_strSite.IndexOf("<b>",nIndex);
				nIndexEnd = m_strSite.IndexOf("</b>",nIndexStart);				
				usr.SendText(header1,m_strSite.Substring(nIndexStart+3,(nIndexEnd - 3) - nIndexStart),Messenger.MMSGTYPE.MMSGTYPE_ALL_RESULTS);
				
				//getting the meaning
				nIndexStart = m_strSite.IndexOf("</a>",nIndex);
				nIndexEnd = m_strSite.IndexOf("<br>",nIndexStart);
				imuser.SendText(header2,m_strSite.Substring(nIndexStart+4,(nIndexEnd - 4) - nIndexStart),Messenger.MMSGTYPE.MMSGTYPE_ALL_RESULTS);
				
				int newindex = 0;
				//getting the sentence
				newindex = m_strSite.IndexOf("<br>",nIndexEnd + 4);
				imuser.SendText(header3,m_strSite.Substring(nIndexEnd+4,(newindex - 4) - nIndexEnd),Messenger.MMSGTYPE.MMSGTYPE_ALL_RESULTS);
				

			}
			catch(Exception e)
			{
				imuser.SendText(header,"Wooops (try again).",Messenger.MMSGTYPE.MMSGTYPE_ALL_RESULTS);
			}
		}


		private void userjoin(IMsgrUser User, IMsgrIMSession Session)
		{
			//User.SendText(header,"Hey <" + User.FriendlyName + ">! whaazup !!",Messenger.MMSGTYPE.MMSGTYPE_ALL_RESULTS);
		}
	}
}
