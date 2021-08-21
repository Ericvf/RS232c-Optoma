# RS232c Optoma
Control Optoma projectors using RS232 commands over Serial. This code specifically works for a Optoma UHD42 and might need adjustment for other Optoma projectors. 

Different devices with RS232 are most likely *_not_* going to work.

# Setup
## Via Serial
To use this code/program you need to attach the Optoma projector to your PC via Serial. It needs to have a COM port in order to function. 
You can buy a dedicated USB to Serial cable and connect it directly to the projector. Or you can use an Arduino as an Serial to USB device. 
The sketch for this is included, it uses `Serial1` to communicate to an `MAX3232` ic.

## Via Wifi
To use this code/program with Wifi you need an ESP8266 (or variant). It will also use `Serial1` to communicate to an `MAX3232` but communication is initiated over Wifi using Expressif ESP libraries. The sketch for this is included.

## More info here:
https://en.wikipedia.org/wiki/RS-232

