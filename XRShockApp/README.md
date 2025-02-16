# ShockCollar
Desktop GUI for controlling OSC based IO Devices

This Program listens for the OSC Address "/avatar/parameters/RemotePressed" and once it reads a true there, it will activate the Collar with the selected settings.

It listens on Port 9001 and sends on Port 9000.

In addition all read and not used udp Messages get repeated on Ports 9011 and 9012.

The Gigarusk menus get activated by pressing "Test".
