
// the backbone of the application: the HTML5 web socket!
var socket;
var ledState= new Uint8Array(2);
/**
 * register callbacks on the socket
 */
function onOpen(e){  
  var data = new Uint8Array(3);
  data[0] = 2;
  data[1] = 0;
  data[2] = 0;
  socket.send(data);  
}

function onClose(e){
  alert("WebSocket Closing");
}

function onMessage(e){    

   var data = new Uint8Array(e.data);
   ledState[0]=data[1];
   ledState[1]=data[2];
   
   updateLEDs();   
}
function onError(e) { 
	alert("Error: "+e); 
}

//Methods

function updateLEDs(){
	//Main
	document.getElementById("led0").style.background = ((ledState[0] & 0x01) != 0) ? '#FF0000' : '#000000';
	document.getElementById("led1").style.background = ((ledState[0] & 0x02) != 0) ? '#0000FF' : '#000000';
	document.getElementById("led2").style.background = ((ledState[0] & 0x04) != 0) ? '#00FF00' : '#000000';
	document.getElementById("led3").style.background = ((ledState[0] & 0x08) != 0) ? '#FFA500' : '#000000';
	
	//Remote
	document.getElementById("led4").style.background = ((ledState[1] & 0x01) != 0) ? '#FF0000' : '#000000';
	document.getElementById("led5").style.background = ((ledState[1] & 0x02) != 0) ? '#0000FF' : '#000000';
	document.getElementById("led6").style.background = ((ledState[1] & 0x04) != 0) ? '#00FF00' : '#000000';
	document.getElementById("led7").style.background = ((ledState[1] & 0x08) != 0) ? '#FFFF00' : '#000000';
}

function connectToServer(){	
	var hostIPAdd=document.getElementById("textIPAdd").value;
	var portNo=document.getElementById("textPortNo").value;
	//Connecting with server
	socket = new WebSocket("ws://"+hostIPAdd+":"+portNo);
	socket.binaryType = 'arraybuffer';
	socket.onopen = function(evt) { onOpen(evt) }; 
	socket.onclose = function(evt) { onClose(evt) }; 
	socket.onmessage = function(evt) { onMessage(evt) }; 
	socket.onerror = function(evt) { onError(evt) };
}
function connectClicked() { 
   connectToServer();
}

function refreshClicked() { 
   var data = new Uint8Array(3);
   data[0] = 2;
   data[1] = 0;
   data[2] = 0;
   socket.send(data);
}

function led0Clicked() { 
   var led0 = document.getElementById("led0");
   if ((ledState[0] & 0x01) == 0) {
     ledState[0] = (ledState[0] | 0x01);
	 led0.style.background='#FF0000';
   } else {
	 ledState[0] = (ledState[0] & 0x0E);
	 led0.style.background='#000000';
   }
   
   var data = new Uint8Array(3);
   data[0] = 1;
   data[1] = ledState[0];
   data[2] = ledState[1];
   socket.send(data);
}

function led1Clicked() { 
   var led1 = document.getElementById("led1");
   if ((ledState[0] & 0x02) == 0) {
     ledState[0] = (ledState[0] | 0x02);
	 led1.style.background='#0000FF';
   } else {
	 ledState[0] = (ledState[0] & 0x0D);
	 led1.style.background='#000000';
   }
   
   var data = new Uint8Array(3);
   data[0] = 1;
   data[1] = ledState[0];
   data[2] = ledState[1];
   socket.send(data);
}

function led2Clicked() { 
   var led2 = document.getElementById("led2");
   if ((ledState[0] & 0x04) == 0) {
     ledState[0] = (ledState[0] | 0x04);
	 led2.style.background='#00FF00';
   } else {
	 ledState[0] = (ledState[0] & 0x0B);
	 led2.style.background='#000000';
   }
   
   var data = new Uint8Array(3);
   data[0] = 1;
   data[1] = ledState[0];
   data[2] = ledState[1];
   socket.send(data);
}

function led3Clicked() { 
   var led3 = document.getElementById("led3");
   if ((ledState[0] & 0x08) == 0) {
     ledState[0] = (ledState[0] | 0x08);
	 led3.style.background='#FFA500';
   } else {
	 ledState[0] = (ledState[0] & 0x07);
	 led3.style.background='#000000';
   }
   
   var data = new Uint8Array(3);
   data[0] = 1;
   data[1] = ledState[0];
   data[2] = ledState[1];
   socket.send(data);
}

function led4Clicked() { 
   var led4 = document.getElementById("led4");
   if ((ledState[1] & 0x01) == 0) {
     ledState[1] = (ledState[1] | 0x01);
	 led4.style.background='#FF0000';
   } else {
	 ledState[1] = (ledState[1] & 0x0E);
	 led4.style.background='#000000';
   }
   
   var data = new Uint8Array(3);
   data[0] = 1;
   data[1] = ledState[0];
   data[2] = ledState[1];
   socket.send(data);
}

function led5Clicked() { 
   var led5 = document.getElementById("led5");
   if ((ledState[1] & 0x02) == 0) {
     ledState[1] = (ledState[1] | 0x02);
	 led5.style.background='#0000FF';
   } else {
	 ledState[1] = (ledState[1] & 0x0D);
	 led5.style.background='#000000';
   }
   
   var data = new Uint8Array(3);
   data[0] = 1;
   data[1] = ledState[0];
   data[2] = ledState[1];
   socket.send(data);
}

function led6Clicked() { 
   var led6 = document.getElementById("led6");
   if ((ledState[1] & 0x04) == 0) {
     ledState[1] = (ledState[1] | 0x04);
	 led6.style.background='#00FF00';
   } else {
	 ledState[1] = (ledState[1] & 0x0B);
	 led6.style.background='#000000';
   }
   
   var data = new Uint8Array(3);
   data[0] = 1;
   data[1] = ledState[0];
   data[2] = ledState[1];
   socket.send(data);
}

function led7Clicked() { 
   var led7 = document.getElementById("led7");
   if ((ledState[1] & 0x08) == 0) {
     ledState[1] = (ledState[1] | 0x08);
	 led7.style.background='#FFFF00';
   } else {
	 ledState[1] = (ledState[1] & 0x07);
	 led7.style.background='#000000';
   }
   
   var data = new Uint8Array(3);
   data[0] = 1;
   data[1] = ledState[0];
   data[2] = ledState[1];
   socket.send(data);
}