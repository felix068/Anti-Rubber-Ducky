using System;
using System.Collections.Generic;
using System.Management;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

class Program
{
    public static string del_keyboard = "";

    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        Icon appIcon = new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream("keyboard.logo.ico"));

        NotifyIcon notifyIcon = new NotifyIcon
        {
            Visible = true,
            Icon = appIcon,
            BalloonTipTitle = @"⚠️Périphérique HID connecté⚠️",
            BalloonTipText = "Un nouveau Clavier a été connecté. Il a été déconnecté automatiquement. Si vous en êtes l'auteur, veuillez autoriser ce périphérique, Sinon il s'agit peut-être d'une attaque (rubber ducky).",
        };

        ContextMenuStrip contextMenuStrip = new ContextMenuStrip();

        ToolStripMenuItem quitMenuItem = new ToolStripMenuItem
        {
            Text = "Quitter"
        };

        ToolStripMenuItem trustMenuItem = new ToolStripMenuItem
        {
            Text = "Ajouter les HID actuels à la liste de confiance"
        };

        quitMenuItem.Click += (sender, e) =>
        {
            Application.Exit();
        };

        trustMenuItem.Click += (sender, e) =>
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "trusted_hid_devices");

            File.AppendAllLines(path, new List<string> { del_keyboard });

            notifyIcon.BalloonTipText = "Veuillez débrancher et rebrancher le clavier à ajouter à la liste de confiance";
            notifyIcon.BalloonTipTitle = @"ℹ️Ajout d'un clavier à la liste de confianceℹ️";
            notifyIcon.ShowBalloonTip(6500);
            notifyIcon.BalloonTipTitle = @"⚠️Périphérique HID connecté⚠️";
            notifyIcon.BalloonTipText = "Un nouveau Clavier a été connecté. Il a été déconnecté automatiquement. Si vous en êtes l'auteur, veuillez autoriser ce périphérique, Sinon il s'agit peut-être d'une attaque (rubber ducky).";
            
        };

        contextMenuStrip.Items.Add(trustMenuItem);

        contextMenuStrip.Items.Add(quitMenuItem);

        notifyIcon.ContextMenuStrip = contextMenuStrip;

        List<string> initialHidDevices = GetConnectedHidKeyboards();

        Console.WriteLine("Périphériques HID connectés au lancement :");
        foreach (string deviceId in initialHidDevices)
        {
            Console.WriteLine("PNPDeviceID: {0}", deviceId);
        }

        WqlEventQuery query = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2");
        ManagementEventWatcher watcher = new ManagementEventWatcher(query);

        Console.WriteLine("En attente de nouveaux périphériques HID...");

        watcher.EventArrived += (sender, args) =>
        {
            List<string> currentHidDevices = GetConnectedHidKeyboards();

            string[] lines = File.ReadAllLines(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "trusted_hid_devices"));
            if (lines.Length > 0)
            {
                foreach (string line in lines)
                {
                    initialHidDevices.Add(line);
                }
            }

            foreach (string deviceId in currentHidDevices)
            {
                if (!initialHidDevices.Contains(deviceId))
                {
                    Console.WriteLine("Un nouveau périphérique HID a été connecté :");
                    Console.WriteLine("PNPDeviceID: {0}", deviceId);
                    initialHidDevices.Add(deviceId);
                    
                    ProcessStartInfo startInfo = new ProcessStartInfo("pnputil.exe", "/remove-device \"" + deviceId + "\"");
                    startInfo.Verb = "runas";
                    startInfo.UseShellExecute = false;
                    startInfo.CreateNoWindow = true;
                    Process process = Process.Start(startInfo);
                    process.WaitForExit();

                    initialHidDevices.Remove(deviceId);

                    del_keyboard = deviceId;

                    notifyIcon.ShowBalloonTip(6500);
                }
            }
        };

        watcher.Start();
        Application.Run();
    }

    static List<string> GetConnectedHidKeyboards()
    {
        List<string> hidKeyboards = new List<string>();

        ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE DeviceID LIKE 'HID%'");
        foreach (ManagementObject hidDevice in searcher.Get())
        {
            string deviceId = hidDevice.GetPropertyValue("DeviceID").ToString();
            if (deviceId.Contains("VID") && deviceId.Contains("PID") && deviceId.Contains("&MI_00"))
            {
                hidKeyboards.Add(deviceId);
            }
        }

        return hidKeyboards;
    }
}