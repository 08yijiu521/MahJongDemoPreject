## WeChat Plugin Android Lite - An easier solution for integrating WeChat share to your games

## What is WeChat Plugin for Android Lite

We Chat plugin for android lite is a Unity3D package to help you using We Chat Sharing images on Android without registering on wechat developer site. you can set up a sharing with no coding involved at all! except to bind your button to our object.

## Main features:
* Extremely easy to set up, includes a easily configurable scene object.
* Supports Share Images and Multiple Images with text.

## Set Up Guide

1. Download and import the WeChatPluginAndroidLite into your project file.
if you already have AndroidManifest in your project. you can exclude that. and add the folling lines instead.
Add in the permissions before the "<application" tag
 
<uses-permission android:name="android.permission.INTERNET" />
    
<uses-permission android:name="android.permission.MODIFY_AUDIO_SETTINGS"/>
    
<uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE"/>

2. Drag the prefab from "WeChatPluginAndroidLite/Prefab/WeChatPluginAndroidLite.prefab" into the scene you desired to share.
3. Configure what you want to share, using the editor of the prefab.
4. Add a reference of the objects WeChatPluginLiteScript in your button's script.
5. call the share. [referenceVariableName].Share();
6. Enjoy! now your we chat share should work properly.

Thats all for setting it up.
Refer to the DemoScene for how to use it.

## Demo Example

FAQ
#Nothing Happens When I Share, Whats Wrong?
*An Texture that you are using might have not been set as Read/Write Enabled. or wrong format. (refer to the texture from the demo)

C. Errors when you sharing Image or thumb using Texture2D.
* Please set there Texture import type of the Image to Advanced ensure the Read/Write Enabled and the Format to RGBA 32 bit.
