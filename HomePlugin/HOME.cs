using System;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using static System.Buffers.Binary.BinaryPrimitives;
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

        public void Initialize(params object[] args)
        {
            SaveFileEditor = (ISaveFileProvider)Array.Find(args, z => z is ISaveFileProvider);
            PKMEditor = (IPKMView)Array.Find(args, z => z is IPKMView);
            
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
            ctrl.Click += (s, e) => new MainForm(this).Show();
        }

        public void ProcessRemote(MainForm frm, System.ComponentModel.BackgroundWorker? bgWorker)
        {
            //TBD If PKHeX will be natively compatible with PKH, let the data be converted to the current save format and reload SAV boxes
            LiveHeXController? controller = new LiveHeXController(SaveFileEditor, PKMEditor, frm.GetConnectionType());
            if (controller != null && bgWorker != null)
            {
                try
                {
                    var sav = SaveFileEditor.SAV;

                    controller.Bot = new PokeSysBotMini(frm.GetConnectionType())
                    {
                        sys = { IP = frm.GetIP(), Port = frm.GetPort() }
                    };
                    controller.Bot.sys.Connect();

                    frm.WriteLog("Connected.");

                    var encrypted = frm.GetWantedFormats() == DumpFormat.Encrypted || frm.GetWantedFormats() == DumpFormat.EncAndDec;
                    var decrypted = frm.GetWantedFormats() == DumpFormat.Decrypted || frm.GetWantedFormats() == DumpFormat.EncAndDec;

                    var target = frm.GetDumpTarget();
                    var offset = target switch
                    {
                        DumpTarget.TargetBox => controller.Bot.GetBoxOffset(frm.GetTargetBox()),
                        DumpTarget.TargetSlot => controller.Bot.GetSlotOffset(frm.GetTargetBox(), frm.GetTargetSlot()),
                        DumpTarget.TargetAll => controller.Bot.GetB1S1Offset(),
                        _ => controller.Bot.GetB1S1Offset(),
                    };
                    var i = 0;
                    var found = 0;

                    do
                    {
                        var ekh = controller.Bot.ReadBytesPKH(offset);

                        ushort version;
                        try
                        {
                            version = DataVersion(ekh!);
                        }
                        catch
                        {
                            version = 0;
                            frm.WriteLog($"Found an empty slot.");
                        }

                        if (version > 1)
                            frm.WriteLog($"This plugin currently can handle only DataVersion [1]. PKH DataVersion is [{version}]");


                        var pkh = DecryptEH1(ekh);
                        i++;

                        if (pkh != null && pkh.Species != 0 && version == 1)
                        {
                            found++;
                            if (decrypted)
                            {
                                string path = SetFileName(pkh, frm.GetPath(), target, false, i);
                                SavePKH(pkh?.Data, path);
                            }
                            if (encrypted)
                            {
                                string path = SetFileName(pkh, frm.GetPath(), target, true, i);
                                SavePKH(ekh, path);
                            }
                        }

                        switch(target)
                        {
                            case DumpTarget.TargetAll:
                                bgWorker.ReportProgress(i);
                                offset += controller.Bot.GetSlotSize();
                                break;
                            case DumpTarget.TargetBox:
                                bgWorker.ReportProgress(i * controller.Bot.GetBoxCount());
                                offset += controller.Bot.GetSlotSize();
                                break;
                            case DumpTarget.TargetSlot:
                                bgWorker.ReportProgress(controller.Bot.GetSlotCount() * controller.Bot.GetBoxCount());
                                break;
                        };

                        frm.WriteLog($"Dumping [{(encrypted && decrypted ? found*2 : found)}] file(s).");
                    } while ((target == DumpTarget.TargetAll && i < 6000) || (target == DumpTarget.TargetBox && i < 30));

                    frm.WriteLog($"Process completed. [{(encrypted && decrypted ? found * 2 : found)}] file(s) dumped.");
                    return;

                } catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    frm.WriteLog("Something went wrong :-( \nCheck your sys-botbase installation and input the correct IP address.");
                }
            }
        }

        public void ProcessLocal(MainForm frm, System.ComponentModel.BackgroundWorker? bgWorker, string file, string path)
        {
            var data = File.ReadAllBytes(file);
            if (DataVersion(data) != 1)
            {
                frm.WriteLog($"{file} is incompatible data.");
                return;
            }
            SavePKH(DecryptEH1(data)!.Data, $"{path}/{Path.GetFileNameWithoutExtension(file)}.ph1");
        }

        private string SetFileName(PKM? pkm, string path, DumpTarget target, bool encrypted, int i = 0)
        {
            var name = $"{path}/";
            if (target == DumpTarget.TargetAll)
                name += $"Box {(i / 30) + 1}/";
            if (pkm != null && pkm.Species != 0)
            {
                name += $"{pkm.Species}";
                if (pkm.Form > 0)
                    name += $"-{pkm.Form:00}";
                name += $" - {Regex.Replace(pkm.Nickname, @"[^0-9a-zA-Z\._ ]", "")}";
                name += $" {pkm.EncryptionConstant:X8}{pkm.PID:X8}";
                if(encrypted)
                    name += $".eh1";
                else
                    name += $".ph1";
            }
            return name;
        }

        private ushort DataVersion(byte[] ekh) => ReadUInt16LittleEndian(ekh.AsSpan(0x00));

        private PKH? DecryptEH1(byte[]? ek1)
        {
            if (ek1 != null)
            {
                if (HomeCrypto.GetIsEncrypted1(ek1))
                    return new PKH(ek1);
            }
            return null;
        }
        
        private static void SavePKH(byte[]? data, string path)
        {
            if (data != null)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                File.WriteAllBytes(path, data);
            }
            else
                throw new ArgumentException("Data is null.");
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
