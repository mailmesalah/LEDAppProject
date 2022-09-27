package ledapp.client.android;

import de.tavendo.autobahn.WebSocketConnection;
import de.tavendo.autobahn.WebSocketHandler;
import android.support.v7.app.ActionBarActivity;
import android.graphics.Color;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

//@ClientEndpoint
public class MainActivity extends ActionBarActivity {

	// Variables
	private String serverIPAddress;
	private String serverPortNo;
	private byte ledMain;
	private byte ledRemote;
	private final WebSocketConnection mConnection = new WebSocketConnection();

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_main);

		// User defined codes

		final Button buttonConnect = (Button) findViewById(R.id.buttonConnect);
		buttonConnect.setOnClickListener(new View.OnClickListener() {

			@Override
			public void onClick(View v) {

				try {

					TextView textIpAdd = (TextView) findViewById(R.id.textIPAdd);
					TextView textPortNo = (TextView) findViewById(R.id.textPortNum);
					serverIPAddress = textIpAdd.getEditableText().toString();
					serverPortNo = textPortNo.getEditableText().toString();

					connectToServerAndRegisterCallbacks();

				} catch (Exception e) {
					Toast.makeText(getApplicationContext(),
							"Connection Failed " + e.getMessage(),
							Toast.LENGTH_SHORT).show();
				}

			}
		});

		final Button buttonRefresh = (Button) findViewById(R.id.buttonRefresh);
		buttonRefresh.setOnClickListener(new View.OnClickListener() {

			@Override
			public void onClick(View v) {
				try {
					// GETLED command
					byte data[] = new byte[3];
					data[0] = 2;
					data[1] = 0;
					data[2] = 0;
					mConnection.sendBinaryMessage(data);
				} catch (Exception e) {
					Toast.makeText(getApplicationContext(),
							"Refresh Failed " + e.getMessage(),
							Toast.LENGTH_SHORT).show();
				}

			}

		});

		final Button led0 = (Button) findViewById(R.id.buttonLed0);
		led0.setOnClickListener(new View.OnClickListener() {

			@Override
			public void onClick(View v) {
				try {
					// Toggle Color and Value
					Button led0 = (Button) findViewById(R.id.buttonLed0);
					if ((ledMain & 0x01) == 0) {
						ledMain = (byte) (ledMain | 0x01);
						led0.setBackgroundColor(Color.RED);
					} else {
						ledMain = (byte) (ledMain & 0x0E);
						led0.setBackgroundColor(Color.BLACK);
					}

					// SETLED command
					byte data[] = new byte[3];
					data[0] = 1;
					data[1] = ledMain;
					data[2] = ledRemote;
					mConnection.sendBinaryMessage(data);
				} catch (Exception e) {
					Toast.makeText(getApplicationContext(),
							"SETLED Failed " + e.getMessage(),
							Toast.LENGTH_SHORT).show();
				}
			}

		});

		final Button led1 = (Button) findViewById(R.id.buttonLed1);
		led1.setOnClickListener(new View.OnClickListener() {

			@Override
			public void onClick(View v) {
				try {
					// Toggle Color and Value
					Button led1 = (Button) findViewById(R.id.buttonLed1);
					if ((ledMain & 0x02) == 0) {
						ledMain = (byte) (ledMain | 0x02);
						led1.setBackgroundColor(Color.BLUE);
					} else {
						ledMain = (byte) (ledMain & 0x0D);
						led1.setBackgroundColor(Color.BLACK);
					}

					// SETLED command
					byte data[] = new byte[3];
					data[0] = 1;
					data[1] = ledMain;
					data[2] = ledRemote;
					mConnection.sendBinaryMessage(data);
				} catch (Exception e) {
					Toast.makeText(getApplicationContext(),
							"SETLED Failed " + e.getMessage(),
							Toast.LENGTH_SHORT).show();
				}
			}

		});

		final Button led2 = (Button) findViewById(R.id.buttonLed2);
		led2.setOnClickListener(new View.OnClickListener() {

			@Override
			public void onClick(View v) {
				try {
					// Toggle Color and Value
					Button led2 = (Button) findViewById(R.id.buttonLed2);
					if ((ledMain & 0x04) == 0) {
						ledMain = (byte) (ledMain | 0x04);
						led2.setBackgroundColor(Color.GREEN);
					} else {
						ledMain = (byte) (ledMain & 0x0B);
						led2.setBackgroundColor(Color.BLACK);
					}

					// SETLED command
					byte data[] = new byte[3];
					data[0] = 1;
					data[1] = ledMain;
					data[2] = ledRemote;
					mConnection.sendBinaryMessage(data);
				} catch (Exception e) {
					Toast.makeText(getApplicationContext(),
							"SETLED Failed " + e.getMessage(),
							Toast.LENGTH_SHORT).show();
				}
			}

		});

		final Button led3 = (Button) findViewById(R.id.buttonLed3);
		led3.setOnClickListener(new View.OnClickListener() {

			@Override
			public void onClick(View v) {
				try {
					// Toggle Color and Value
					Button led3 = (Button) findViewById(R.id.buttonLed3);
					if ((ledMain & 0x08) == 0) {
						ledMain = (byte) (ledMain | 0x08);
						led3.setBackgroundColor(getResources().getColor(R.color.orange));
					} else {
						ledMain = (byte) (ledMain & 0x07);
						led3.setBackgroundColor(Color.BLACK);
					}

					// SETLED command
					byte data[] = new byte[3];
					data[0] = 1;
					data[1] = ledMain;
					data[2] = ledRemote;
					mConnection.sendBinaryMessage(data);
				} catch (Exception e) {
					Toast.makeText(getApplicationContext(),
							"SETLED Failed " + e.getMessage(),
							Toast.LENGTH_SHORT).show();
				}
			}

		});

		final Button led4 = (Button) findViewById(R.id.buttonLed4);
		led4.setOnClickListener(new View.OnClickListener() {

			@Override
			public void onClick(View v) {
				try {
					// Toggle Color and Value
					Button led4 = (Button) findViewById(R.id.buttonLed4);
					if ((ledRemote & 0x01) == 0) {
						ledRemote = (byte) (ledRemote | 0x01);
						led4.setBackgroundColor(Color.RED);
					} else {
						ledRemote = (byte) (ledMain & 0x0E);
						led4.setBackgroundColor(Color.BLACK);
					}

					// SETLED command
					byte data[] = new byte[3];
					data[0] = 1;
					data[1] = ledMain;
					data[2] = ledRemote;
					mConnection.sendBinaryMessage(data);
				} catch (Exception e) {
					Toast.makeText(getApplicationContext(),
							"SETLED Failed " + e.getMessage(),
							Toast.LENGTH_SHORT).show();
				}
			}

		});

		final Button led5 = (Button) findViewById(R.id.buttonLed5);
		led5.setOnClickListener(new View.OnClickListener() {

			@Override
			public void onClick(View v) {
				try {
					// Toggle Color and Value
					Button led5 = (Button) findViewById(R.id.buttonLed5);
					if ((ledRemote & 0x02) == 0) {
						ledRemote = (byte) (ledRemote | 0x02);
						led5.setBackgroundColor(Color.BLUE);
					} else {
						ledRemote = (byte) (ledMain & 0x0D);
						led5.setBackgroundColor(Color.BLACK);
					}

					// SETLED command
					byte data[] = new byte[3];
					data[0] = 1;
					data[1] = ledMain;
					data[2] = ledRemote;
					mConnection.sendBinaryMessage(data);
				} catch (Exception e) {
					Toast.makeText(getApplicationContext(),
							"SETLED Failed " + e.getMessage(),
							Toast.LENGTH_SHORT).show();
				}
			}

		});

		final Button led6 = (Button) findViewById(R.id.buttonLed6);
		led6.setOnClickListener(new View.OnClickListener() {

			@Override
			public void onClick(View v) {
				try {
					// Toggle Color and Value
					Button led6 = (Button) findViewById(R.id.buttonLed6);
					if ((ledRemote & 0x04) == 0) {
						ledRemote = (byte) (ledRemote | 0x04);
						led6.setBackgroundColor(Color.GREEN);
					} else {
						ledRemote = (byte) (ledMain & 0x0B);
						led6.setBackgroundColor(Color.BLACK);
					}

					// SETLED command
					byte data[] = new byte[3];
					data[0] = 1;
					data[1] = ledMain;
					data[2] = ledRemote;
					mConnection.sendBinaryMessage(data);
				} catch (Exception e) {
					Toast.makeText(getApplicationContext(),
							"SETLED Failed " + e.getMessage(),
							Toast.LENGTH_SHORT).show();
				}
			}

		});

		final Button led7 = (Button) findViewById(R.id.buttonLed7);
		led7.setOnClickListener(new View.OnClickListener() {

			@Override
			public void onClick(View v) {
				try {
					// Toggle Color and Value
					Button led7 = (Button) findViewById(R.id.buttonLed7);
					if ((ledRemote & 0x08) == 0) {
						ledRemote = (byte) (ledRemote | 0x08);
						led7.setBackgroundColor(Color.YELLOW);
					} else {
						ledRemote = (byte) (ledMain & 0x07);
						led7.setBackgroundColor(Color.BLACK);
					}

					// SETLED command
					byte data[] = new byte[3];
					data[0] = 1;
					data[1] = ledMain;
					data[2] = ledRemote;
					mConnection.sendBinaryMessage(data);
					// clientWebSocket.send(data);
				} catch (Exception e) {
					Toast.makeText(getApplicationContext(),
							"SETLED Failed " + e.getMessage(),
							Toast.LENGTH_SHORT).show();
				}
			}

		});

		ledMain = 0;
		ledRemote = 0;

		updateLeds();
	}

	// Methods defined
	private void updateLeds() {
		Button led0 = (Button) findViewById(R.id.buttonLed0);
		led0.setBackgroundColor(((ledMain & 0x01) != 0) ? Color.RED
				: Color.BLACK);
		Button led1 = (Button) findViewById(R.id.buttonLed1);
		led1.setBackgroundColor(((ledMain & 0x02) != 0) ? Color.BLUE
				: Color.BLACK);
		Button led2 = (Button) findViewById(R.id.buttonLed2);
		led2.setBackgroundColor(((ledMain & 0x04) != 0) ? Color.GREEN
				: Color.BLACK);
		Button led3 = (Button) findViewById(R.id.buttonLed3);
		led3.setBackgroundColor(((ledMain & 0x08) != 0) ? getResources().getColor(R.color.orange)
				: Color.BLACK);
		Button led4 = (Button) findViewById(R.id.buttonLed4);
		led4.setBackgroundColor(((ledRemote & 0x01) != 0) ? Color.RED
				: Color.BLACK);
		Button led5 = (Button) findViewById(R.id.buttonLed5);
		led5.setBackgroundColor(((ledRemote & 0x02) != 0) ? Color.BLUE
				: Color.BLACK);
		Button led6 = (Button) findViewById(R.id.buttonLed6);
		led6.setBackgroundColor(((ledRemote & 0x04) != 0) ? Color.GREEN
				: Color.BLACK);
		Button led7 = (Button) findViewById(R.id.buttonLed7);
		led7.setBackgroundColor(((ledRemote & 0x08) != 0) ? Color.YELLOW
				: Color.BLACK);
	}

	public void connectToServerAndRegisterCallbacks() {

		try {
			String hostAddress = "ws://" + serverIPAddress + ":" + serverPortNo;

			mConnection.connect(hostAddress, new WebSocketHandler() {
				@Override
				public void onOpen() {
					Toast.makeText(getApplicationContext(), "Connected ",
							Toast.LENGTH_SHORT).show();

					byte data[] = new byte[3];
					data[0] = 2;
					data[1] = 0;
					data[2] = 0;
					mConnection.sendBinaryMessage(data);
				}

				@Override
				public void onBinaryMessage(byte[] payload) {
					try {

						ledMain = payload[1];
						ledRemote = payload[2];
						updateLeds();
					} catch (Exception e) {
						Toast.makeText(getApplicationContext(),
								"Error " + e.getMessage(), Toast.LENGTH_SHORT)
								.show();
					}
				}

				@Override
				public void onClose(int code, String reason) {
					Toast.makeText(getApplicationContext(),
							"Closing Connection", Toast.LENGTH_SHORT).show();
				}

			});
			Toast.makeText(getApplicationContext(), "Connecting to Server ",
					Toast.LENGTH_SHORT).show();

		} catch (Exception e) {
			Toast.makeText(getApplicationContext(),
					"Error in connectToServer() " + e.getMessage(),
					Toast.LENGTH_SHORT).show();
		}
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.main, menu);
		return true;
	}

	@Override
	public boolean onOptionsItemSelected(MenuItem item) {
		// Handle action bar item clicks here. The action bar will
		// automatically handle clicks on the Home/Up button, so long
		// as you specify a parent activity in AndroidManifest.xml.
		int id = item.getItemId();
		if (id == R.id.action_settings) {
			return true;
		}
		return super.onOptionsItemSelected(item);
	}

}
