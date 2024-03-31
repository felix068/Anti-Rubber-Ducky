# Rubber Ducky Protection

This C# program aims to automatically detect and disconnect any new Human Interface Device (HID) keyboard devices connected to your computer. This helps protect against "Rubber Ducky" attacks, which involve injecting malicious commands via a USB keyboard disguised as a storage device.

## Features

- Real-time detection of newly connected HID devices
- Automatic disconnection of new, unapproved HID devices
- Graphical user interface with a system tray icon
- Ability to add HID devices to a trusted list

## Usage

1. Run the program
2. An icon will appear in the system tray
3. If a new HID device is connected, it will be automatically disconnected, and a notification will be displayed
4. To approve an HID device, right-click the icon and select "Add current HIDs to the trusted list"
5. Unplug and replug the HID device you want to approve
6. The device will be added to the trusted list and will no longer be disconnected

## Configuration

The program stores the list of approved HID devices in a `trusted_hid_devices` file located in the current user's `%AppData%` folder.

## Notes

- This program requires administrative privileges to disconnect devices
- It is recommended to run this program continuously for ongoing protection

## Contribution

Contributions to improve this program are welcome. Feel free to submit pull requests or report issues.
