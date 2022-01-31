# ZteLteMonitor

The ZTE MF283V modem provided by Algérie Télécom frequently looses LTE connectivty and is unable to auto reconnect, a manual reboot is **always** required.
One way of fixing this bug(/feature? by ZTE) is to manually fill in the APN settings. The other way I found is to use a service (available for Windows & Linux), it monitors the router every x second and automatically reboots it in case of LTE disconnection.

The android code is an artefact of my initial attempt to use an Android Tv in order to monitor the router, it's pretty much useless: The OS/Vendor restrict services in order to save battery.

Customize the service behaviour via appsettings.json (Ip address, password...etc.)

Downloads:
- <a href="https://github.com/sferhah/ZteLteMonitor/releases/download/v1/linux-x64.zip">linux-x64.zip</a> (Unzip and use bash install_linux.txt)
- <a href="https://github.com/sferhah/ZteLteMonitor/releases/download/v1/win-x64.zip">win-x64.zip</a> (Unzip and click on install_windows.bat)

See also:
- https://github.com/cosote/R216-Z

