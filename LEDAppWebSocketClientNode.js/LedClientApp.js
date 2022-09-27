
var WebSocketClient = require('websocket').client;
var client = new WebSocketClient();
var setL=false;
var getL=false;
var mainLed;
var remoteLed;

client.on('connectFailed', function(error) {
    console.log('Connect Error: ' + error.toString());
});

client.on('connect', function(connection) {
    console.log('Connected to Server');	
    connection.on('error', function(error) {
        console.log("Connection Error: " + error.toString());
    });
    connection.on('close', function() {
        console.log('echo-protocol Connection Closed');
    });
    connection.on('message', function(message) {
		if(getL){
			var data = new Uint8Array(message.binaryData);
			console.log("Main: %s  Remote: %s" , data[1],data[1] );        
		}
		
		process.exit(0);
    });
    
	if (connection.connected) {
		if(setL){					
			var data = new Buffer(3);
			data[0] = 1;
			data[1] = mainLed;
			data[2] = remoteLed;
			connection.sendBytes(data)													
		}else if(getL){
			var data = new Buffer(3);
			data[0] = 2;
			data[1] = 0;
			data[2] = 0;		
			connection.sendBytes(data);
		}
	}
});


(function(){
	var error=false;
	
	if(process.argv.length==4){
		var command=process.argv[2];
		if(command.toLowerCase()==="getled"){
			var args=process.argv[3].split(":");
			var ipAdd=args[0];
			var portN=args[1];
			getL=true;
			//Connect with Server
			client.connect('ws://'+ipAdd+':'+portN);
			client.binaryType="arraybuffer";
		}else{
			error=true;
		}
	}else if(process.argv.length==6){
		var command=process.argv[2];
		if(command.toLowerCase()==="setled"){
			var args=process.argv[3].split(":");
			var ipAdd=args[0];
			var portN=args[1];
			mainLed=process.argv[4];
			remoteLed=process.argv[5];
			setL=true;
			//Connect with Server
			client.connect('ws://'+ipAdd+':'+portN);
			client.binaryType="arraybuffer";
			
		}
		else{
			error=true;
		}
	}else{
		error=true;
	}
	
	if(error){
		console.log("");
		console.log("Please enter valid command");
		console.log("1)setled ipaddr:port data1 data2 OR");
		console.log("2)getled ipaddr:port");	
	}
	
}());














