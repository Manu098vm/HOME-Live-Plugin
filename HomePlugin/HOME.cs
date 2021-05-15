using System;
using System.Windows.Forms;
using System.Drawing;
using PKHeX.Core.Injection;
using PKHeX.Core;

namespace HOME
{
    public class HOME : IPlugin
    {
        public string Name => nameof(HOME);
        public int Priority => 1;

        // Initialized on plugin load
        public ISaveFileProvider SaveFileEditor { get; private set; } = null!;
        public IPKMView PKMEditor { get; private set; } = null!;

        public LiveHeXController? controller;

        public void Initialize(params object[] args)
        {
            SaveFileEditor = (ISaveFileProvider)Array.Find(args, z => z is ISaveFileProvider);
            PKMEditor = (IPKMView)Array.Find(args, z => z is IPKMView);
            controller = new LiveHeXController(SaveFileEditor, PKMEditor);
            var menu = (ToolStrip)Array.Find(args, z => z is ToolStrip);
            LoadMenuStrip(menu);
        }

        private void LoadMenuStrip(ToolStrip menuStrip)
        {
            var items = menuStrip.Items;
            if (!(items.Find("Menu_Tools", false)[0] is ToolStripDropDownItem tools))
                throw new ArgumentException(nameof(menuStrip));
            AddPluginControl(tools);
        }

        private void AddPluginControl(ToolStripDropDownItem tools)
        {
            var ctrl = new ToolStripMenuItem(Name);
            tools.DropDownItems.Add(ctrl);
            ctrl.Click += (s, e) => HomeForm().Show();
        }

        private Form HomeForm()
        {
            Label connection_label = new Label()
            {
                Text = "&IP Address:",
                Location = new Point(10, 10),
                TabIndex = 10
            };

            TextBox connection_box = new TextBox()
            {
                Text = (string)Properties.Settings.Default["IP"],
                Size = new Size(260, 100),
                Location = new Point(connection_label.Location.X, connection_label.Bounds.Bottom),
                TabIndex = 11
            };

            Label boxselection_label = new Label()
            {
                Text = "&Select Boxes you want to view:",
                Location = new Point(connection_box.Location.X, connection_box.Bounds.Bottom + 10),
                TabIndex = 10
            };

            ComboBox boxselection_combo = new ComboBox()
            {
                Text = "Box 1-32",
                Size = new Size(260, 100),
                Items = {
                    "Box 1-32",
                    "Box 33-64",
                    "Box 65-96",
                    "Box 97-128",
                    "Box 129-160",
                    "Box 161-192",
                    "Box 193-200",
                },
                Location = new Point(boxselection_label.Location.X, boxselection_label.Bounds.Bottom),
                TabIndex = 11
            };

            Button connect_button = new Button()
            {
                Text = "&Connect!",
                Size = new Size(260, 50),
                Location = new Point(boxselection_combo.Location.X, boxselection_combo.Bounds.Bottom + 10),
                TabIndex = 10
            };

            TextBox log_box = new TextBox()
            {
                Text = "Ensure to have sys-botbase in your console.\nOpen your Home application, enter the console IP Adrress and hit Connect.",
                Location = new Point(connect_button.Location.X, connect_button.Bounds.Bottom + 10),
                TabIndex = 11,
                Size = new Size(260, 60),
                Multiline = true,
                Enabled = false
            };

            Form connectform = new Form
            {
                Text = "Home Live Plugin",
                FormBorderStyle = FormBorderStyle.FixedToolWindow,
                ShowIcon = false
            };

            connect_button.Font = new Font(connect_button.Font.FontFamily, 15);
            log_box.Font = new Font(log_box.Font.FontFamily, 7);
            connectform.Controls.Add(connection_label);
            connectform.Controls.Add(connection_box);
            connectform.Controls.Add(boxselection_label);
            connectform.Controls.Add(boxselection_combo);
            connectform.Controls.Add(connect_button);
            connectform.Controls.Add(log_box);
            if(controller != null)
                connectform.FormClosed += (s, e) => controller.Bot.sys.Disconnect();
            connectform.StartPosition = FormStartPosition.CenterParent;
            connect_button.Click += (s, e) => ModifySaveFile(connectform);
            
            return connectform;
        }

        private void ModifySaveFile(Form connectform)
        {
            if (controller != null)
            {
                try
                {
                    connectform.Controls[5].Font = new Font(connectform.Controls[5].Font.FontFamily, 8);
                    var sav = SaveFileEditor.SAV;
                    if (sav is SAV8SWSH)
                    {
                        var selection = ((ComboBox) connectform.Controls[3]).SelectedIndex;
                        controller.Bot = new PokeSysBotMini(selection == -1 ? 0 : selection)
                        {
                            sys = { IP = connectform.Controls[1].Text, Port = 6000 }
                        };
                        controller.Bot.sys.Connect();
                        var data = controller.Bot.ReadSlot(1, 1);
                        var pkm = sav.GetDecryptedPKM(data);
                        if (pkm.ChecksumValid)
                            connectform.Controls[5].Text = "Connected succesfully!";
                        var limit = selection == 6 ? 8 : 32; // Only read 8 boxes in the last case
                        for (int i = 0; i <= limit - 1; i++)
                            controller.ReadBox(i);
                        SaveFileEditor.ReloadSlots();
                    }
                    else
                    {
                        connectform.Controls[5].Text = "Invalid Save File type. Please use a Sword/Shield save version.";
                    }
                    Properties.Settings.Default["IP"] = connectform.Controls[1].Text;
                    Properties.Settings.Default.Save();
                } catch(Exception)
                {
                    connectform.Controls[5].Font = new Font(connectform.Controls[5].Font.FontFamily, 8);
                    connectform.Controls[5].Text = "Something went wrong :-( \nCheck your sys-botbase installation and input the correct IP address.";
                }
            }
        }

        public void NotifySaveLoaded()
        {
            Console.WriteLine($"{Name} was notified that a Save File was just loaded.");
        }

        public bool TryLoadFile(string filePath)
        {
            Console.WriteLine($"{Name} was provided with the file path, but chose to do nothing with it.");
            return false; // no action taken
        }
    }
}
