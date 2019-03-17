using UnityEngine;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class PlayerControllerScript: MonoBehaviour 
{
	// 1. Declare Variables
	Thread receiveThread;
	UdpClient client;
	int port;

	public GameObject Player;
	AudioSource jumpSound;
	bool jump;

	// 2. Initialize variables
	void Start(){
		port = 5065;
		jump = false;
		jumpSound = gameObject.GetComponent<AudioSource>();

		InitUDP();
	}

	// 3. InitUDP
	private void InitUDP(){
		print("UDP TERINSTALL");
		receiveThread = new Thread(new ThreadStart(ReceiveData));
		receiveThread.IsBackground = true;
		receiveThread.Start();
	}

	// 4. Receive Data
	private void ReceiveData(){
		client = new UdpClient(port);
		while(true){
			try{
				IPEndPoint anyIP = new IPEndPoint(IPAddress.Parse("0.0.0.0"), port);
				byte[] data = client.Receive(ref anyIP);

				string text = Encoding.UTF8.GetString(data);
				 print(">>" + text);

				 jump = true;
			}
			catch(Exception e){
				print(e.ToString());
			}
		}
	}

	// 5. Make the Player Jump
	private void Jump(){
		Player.GetComponent<Animator>().SetTrigger("Jump");
		jumpSound.PlayDelayed(44100);
	}

	// 6. Check for variable value, and make the Player Jump!
	void  Update(){
		if(jump == true){
			Jump();
			jump = false;
		}
	}
}
