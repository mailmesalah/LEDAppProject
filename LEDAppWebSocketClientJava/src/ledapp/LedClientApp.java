package ledapp;

import java.util.StringTokenizer;
import java.io.IOException;
import java.net.URI;
import java.nio.ByteBuffer;

import javax.websocket.ClientEndpoint;
import javax.websocket.CloseReason;
import javax.websocket.ContainerProvider;
import javax.websocket.OnClose;
import javax.websocket.OnMessage;
import javax.websocket.OnOpen;
import javax.websocket.SendHandler;
import javax.websocket.Session;
import javax.websocket.WebSocketContainer;

@ClientEndpoint
public class LedClientApp {

	private String serverIPAddress;
	private String serverPortNo;
	Session userSession = null;
	static boolean finished=false;

	public LedClientApp(String serverIPAddress, String serverPortNo) {
		super();
		this.serverIPAddress = serverIPAddress;
		this.serverPortNo = serverPortNo;
	}

	public void connectToServer() {
		// open websocket
		try {
			WebSocketContainer container = ContainerProvider
					.getWebSocketContainer();
			System.out.println("Connecting...");
			container.connectToServer(this, new URI("ws://" + serverIPAddress
					+ ":" + serverPortNo));
			System.out.println("Connected");
		} catch (Exception e) {
			System.out.println("connectToServer() " + e.getMessage());
		}
	}

	/**
	 * Callback hook for Connection open events.
	 *
	 * @param userSession
	 *            the userSession which is opened.
	 */
	@OnOpen
	public void onOpen(Session userSession) {
		System.out.println("opening websocket");
		this.userSession = userSession;
	}

	/**
	 * Callback hook for Connection close events.
	 *
	 * @param userSession
	 *            the userSession which is getting closed.
	 * @param reason
	 *            the reason for connection close
	 */
	@OnClose
	public void onClose(Session userSession, CloseReason reason) {
		System.out.println("closing websocket");
		this.userSession = null;
	}

	/**
	 * Callback hook for Message Events. This method will be invoked when a
	 * client send a message.
	 *
	 * @param message
	 *            The Byte Buffer message
	 */
	@OnMessage
	public void onMessage(ByteBuffer message) {
		System.out.println("Message received");
		byte[] bData = message.array();

		System.out.println("Main Led " + bData[1]);
		System.out.println("Remote Led " + bData[2]);
		//Exits once the data is received
		System.exit(1);		
	}

	/**
	 * Send a message.
	 *
	 * @param message
	 */
	public void sendMessage(ByteBuffer message) {
		try {
			this.userSession.getBasicRemote().getSendStream()
					.write(message.array());
		} catch (IOException e) {
			System.out.println("sendMessage(ByteBuffer message) "
					+ e.getMessage());
		}
	}

	public static void main(String args[]) {

		try {
			boolean wrongCommand = false;
			if (args.length == 4) {
				// Setled Command
				if (args[0].toLowerCase().equals("setled")) {
					StringTokenizer st = new StringTokenizer(args[1], ":");
					if (st.countTokens() == 2) {
						String ipAddress = st.nextToken();
						String portNo = st.nextToken();

						LedClientApp lca = new LedClientApp(ipAddress, portNo);

						byte mainLed = Byte.parseByte(args[2]);
						byte remoteLed = Byte.parseByte(args[3]);

						ByteBuffer bData = ByteBuffer.allocate(3);
						bData.put((byte) 1).put(mainLed).put(remoteLed);
						lca.connectToServer();
						lca.sendMessage(bData);

					} else {
						wrongCommand = true;
					}
				} else {
					wrongCommand = true;
				}

			} else if (args.length == 2) {
				// Getled command
				if (args[0].toLowerCase().equals("getled")) {
					StringTokenizer st = new StringTokenizer(args[1], ":");
					if (st.countTokens() == 2) {
						String ipAddress = st.nextToken();
						String portNo = st.nextToken();

						LedClientApp lca = new LedClientApp(ipAddress, portNo);

						ByteBuffer bData = ByteBuffer.allocate(3);
						bData.put((byte) 2).put((byte) 0).put((byte) 0);
						lca.connectToServer();
						lca.sendMessage(bData);

						//Waits till a message is received back
						while(true);						
						
					} else {
						wrongCommand = true;
					}
				} else {
					wrongCommand = true;
				}
			}else{
				wrongCommand= true;
			}

			if (wrongCommand) {
				System.out.println();
				System.out.println("Please enter valid command");
				System.out.println("1)setled ipaddr:port data1 data2 OR");
				System.out.println("2)getled ipaddr:port");
			}

		} catch (Exception e) {
			System.out.println("Inside main() " + e.getMessage());
			System.out.println();
			System.out.println("Please enter valid command");
			System.out.println("1)setled ipaddr:port data1 data2 OR");
			System.out.println("2)getled ipaddr:port");
		}

	}
}
