import websocket
import _thread
import time
import sys

mainLed=0;
remoteLed=0;
command=1

def on_message(ws, message):
    print("Return values are")
    print("Main Leds %d and Remote Leds %d" %(message[1],message[2]))
    ws.close()

def on_error(ws, error):
    print(error)

def on_close(ws):
    print("Connected Closed")

def on_open(ws):
    global mainLed
    global remoteLed
    print("Connected to Server")
    sendCommand(ws)

def sendCommand(ws):
    global cmd
    global mainLed
    global remoteLed

    if(cmd==1):
        ws.send([1,mainLed,remoteLed])
        ws.close()
    elif(cmd==2):
        ws.send([2,0,0])

def connectToServer(ipAddnPort):
    websocket.enableTrace(True)
    ws = websocket.WebSocketApp("ws://"+ipAddnPort+"/",
                              on_message = on_message,
                              on_error = on_error,
                              on_close = on_close)
    ws.on_open = on_open
    ws.run_forever()

def main():
    global cmd
    global mainLed
    global remoteLed
    error=False
    if(len(sys.argv)==3):
        command=sys.argv[1]
        if(command=="getled"):
            cmd=2
            ipAddnPort=sys.argv[2]
            #Connect with Server
            connectToServer(ipAddnPort)
        else:
            error=True
    elif(len(sys.argv)==5):
        command=sys.argv[1]
        if(command=="setled"):
            cmd=1
            ipAddnPort=sys.argv[2]
            mainLed=int(sys.argv[3])
            remoteLed=int(sys.argv[4])
            #Connect with Server
            connectToServer(ipAddnPort)
        else:
            error=True
    else:
        error=True

    if(error):
        print("")
        print("Please enter valid command")
        print("1)setled ipaddr:port data1 data2 OR")
        print("2)getled ipaddr:port")


main()
