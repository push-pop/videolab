### This Repo attempts to add basic Android support for [VideoLab](https://github.com/teenageengineering/videolab)

With this repo you can connect your Android phone to your OP-Z (as long as it has BLE Support.)  I'm testing on Google Pixel 1. Please let me know your results with other devices.

The Bluetooth connection is currently very unstable. It usually takes a few tries to get the OP-Z fully connected. Once it is connected, it seems to hold the connection fine.

If your phone does not connect on the first try, tyy reconnecting, pushing the connect button on the OP-Z. Praying to whatever higher power you subscribe to.

I've included compiled VideoPaks from [keijiro's brilliant work](https://github.com/keijiro/VideolabTest) in the VideoPak folder. To use these, you need to copy them into the data directory for this app. On my device the path is {PhoneRoot}/Android/data/com.teenageEngineering.videolab/files.

You can also put them in Assets/StreamingAssets to use them in the editor

Any questions, find me and shoot me a message. Feel free to open tickets, give suggestions, PRs, etc...

The main thing on my mind is figuring out how to stabalize the BTLE connection. Unfortunately, it makes it tough to use right now.

Feel free to use what I've done in any way you see fit, just don't be a dick. All VideoLab code is subject to their own licenses.

### teenage engineering videolab

Toolkit for designing midi-controlled video content.

Latest [release](https://github.com/teenageengineering/videolab/releases).

Please read the [wiki](https://github.com/teenageengineering/videolab/wiki) for details.
