using System;
using System.IO;
using System.Windows.Forms;
using PKHeX.Core;
using System.Text;
using static System.Buffers.Binary.BinaryPrimitives;

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
            var dumper = new ToolStripMenuItem("Home Live Dumper");
            var viewer = new ToolStripMenuItem("Home Live Viewer");
            tools.DropDownItems.Add(dumper);
            tools.DropDownItems.Add(viewer);
            dumper.Click += (s, e) => new DumpForm(this).Show();
            viewer.Click += (s, e) => new ViewForm(this).Show();
        }

        public void StartDumper(DumpForm frm, System.ComponentModel.BackgroundWorker? bgWorker)
        {
            HomeController? controller = new HomeController(SaveFileEditor, PKMEditor, frm.GetConnectionType());
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

                        if (pkh != null && pkh.Species != 0 && version == 1)
                        {
                            found++;
                            if (decrypted)
                            {
                                string path = SetFileName(pkh, frm.GetPath(), target, frm.GetBoxFolderRequested(), false, i);
                                if(!bgWorker.CancellationPending)
                                    SavePKH(pkh?.Data, path);
                            }
                            if (encrypted)
                            {
                                string path = SetFileName(pkh, frm.GetPath(), target, frm.GetBoxFolderRequested(), true, i);
                                if (!bgWorker.CancellationPending)
                                    SavePKH(ekh, path);
                            }
                        }
                        i++;


                        switch (target)
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

                        frm.WriteLog($"Dumping [{(encrypted && decrypted ? found*2 : found)}] file(s).\nDo note that the Home Data format might change in the future.");
                    } while ((target == DumpTarget.TargetAll && i < 6000) || (target == DumpTarget.TargetBox && i < 30));

                    frm.WriteLog($"Process completed. [{(encrypted && decrypted ? found * 2 : found)}] file(s) dumped.\nDo note that the Home Data format might change in the future.");
                    return;

                } catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    frm.WriteLog("Something went wrong :-( \nCheck your configurations and sys-modules installation.");
                }
            }
        }

        public void StartEncryptorDecryptor(DumpForm frm, DumpFormat originFormat, string file, string path)
        {
            var data = File.ReadAllBytes(file);

            if (DataVersion(data) != 1)
            {
                frm.WriteLog($"{file} is incompatible data.");
                return;
            }

            switch (originFormat)
            {
                case DumpFormat.Encrypted:
                    data = DecryptEH1(data)!.Data;
                    break;
                case DumpFormat.Decrypted:
                    data = HomeCrypto.Encrypt(data);
                    break;
            }

            SavePKH(data, $"{path}\\{Path.GetFileNameWithoutExtension(file)}.{(originFormat == DumpFormat.Encrypted ? "ph1" : "eh1")}");
        }

        public void StartViewer(ViewForm frm, System.ComponentModel.BackgroundWorker? bgWorker)
        {
            HomeController? controller = new HomeController(SaveFileEditor, PKMEditor, frm.GetConnectionType());
            if (controller != null && bgWorker != null)
            {
                try
                {
                    GetSavAvalableBoxAndSlots(out int savSlots, out int savBoxes);
                    controller.Bot = new PokeSysBotMini(frm.GetConnectionType())
                    {
                        sys = { IP = frm.GetIP(), Port = frm.GetPort() }
                    };
                    controller.Bot.sys.Connect();
                    frm.WriteLog("Connecting...");

                    var selection = frm.GetBoxIndex();
                    var qty = CalcBoxQtyInSelection(selection);
                    for (int i = 0; i < qty; i++)
                    {
                        controller.LiveBox(selection, i, frm.GetForceConversion());
                        bgWorker.ReportProgress(100 / savBoxes * i);
                    }
                    frm.WriteLog("Conversion completed.");
                    bgWorker.ReportProgress(100);
                    SaveFileEditor.ReloadSlots();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    frm.WriteLog("Something went wrong :-( \nCheck your configurations and sys-modules installation.");
                }
            }
        }

        private int CalcBoxQtyInSelection(int boxIndex)
        {
            GetSavAvalableBoxAndSlots(out int savSlots, out int savBoxes);
            var pastSlots = boxIndex * savSlots * savBoxes;
            var totalSlots = HomeController.HomeSlots * HomeController.HomeBoxes;
            var toReadSlots = totalSlots - pastSlots;
            var necessarySavBoxes = toReadSlots / savSlots;
            return necessarySavBoxes > savBoxes ? savBoxes : necessarySavBoxes;
        }

        private void GetSavAvalableBoxAndSlots(out int savSlots, out int savBoxes)
        {
            if (SaveFileEditor.SAV is SAV7b)
            {
                savSlots = HomeController.LGPESlots;
                savBoxes = HomeController.LGPEBoxes;
            }
            else if (SaveFileEditor.SAV is SAV8SWSH)
            {
                savSlots = HomeController.SwShSlots;
                savBoxes = HomeController.SwShBoxes;
            }
            else if (SaveFileEditor.SAV is SAV8BS)
            {
                savSlots = HomeController.BSSlots;
                savBoxes = HomeController.BSSBoxes;
            }
            else if (SaveFileEditor.SAV is SAV8LA)
            {
                savSlots = HomeController.LASlots;
                savBoxes = HomeController.LABoxes;
            }
            else
                throw new ArgumentException($"Unrecognized save file type {SaveFileEditor.SAV.GetType()}");
        }

        public string[] CalculateBoxes()
        {
            GetSavAvalableBoxAndSlots(out int savSlots, out int savBoxes);

            var totalRemoteSlots = HomeController.HomeSlots * HomeController.HomeBoxes;
            var totalLocalSlots = savSlots * savBoxes;
            var necesaryIndexes = (int)Math.Ceiling((float)totalRemoteSlots / (float)totalLocalSlots);
            var res = new String[necesaryIndexes];

            if (savSlots == HomeController.HomeSlots)
            {
                var needRemainder = totalRemoteSlots % totalLocalSlots != 0;
                var remainderBoxes = needRemainder ? HomeController.HomeBoxes - (savBoxes * (necesaryIndexes - 1)) : 0;

                for (int i = 0; i < necesaryIndexes; i++)
                    if (i == necesaryIndexes - 1 && needRemainder)
                        res[i] = $"Box {i * savBoxes + 1} - Box {(i * savBoxes) + remainderBoxes}";
                    else
                        res[i] = $"Box {i * savBoxes + 1} - Box {(i + 1) * savBoxes}";
            }
            else
            {
                var remoteBoxPerIndex = (HomeController.HomeBoxes / necesaryIndexes);
                var remainderSlots = totalLocalSlots - remoteBoxPerIndex * HomeController.HomeSlots;

                var lastBox = 0;
                var lastRemainder = 0;
                for (int boxIndex = 0; boxIndex < necesaryIndexes; boxIndex++)
                {
                    var str = GetBoxStrings(remainderSlots, boxIndex, remoteBoxPerIndex, ref lastBox, ref lastRemainder);
                    res[boxIndex] = $"Box {str[0]} - Box {str[1]}";
                }
            }

            return res;
        }

        //Make a different function and do not abbreviate if-else conditions for better logic flow/understanding
        private string[] GetBoxStrings(int remainder, int index, int boxPerIndex, ref int lastBox, ref int lastRemainder)
        {
            int startingBox;
            int endingBox;

            int startingRemainder;
            int endingRemainder;

            string res1, res2;

            if (remainder == 0)
            {
                startingBox = lastBox;
                endingBox = startingBox + boxPerIndex;
                lastBox = endingBox;

                startingRemainder = 0;
                endingRemainder = startingRemainder + remainder;
                lastRemainder = endingRemainder;
            }
            else
            {
                if (index == 0)
                {
                    startingBox = lastBox;
                    endingBox = startingBox + boxPerIndex;
                    lastBox = endingBox;

                    startingRemainder = 0;
                    endingRemainder = startingRemainder + remainder;
                    lastRemainder = endingRemainder;
                }
                else
                {
                    startingBox = lastBox;
                    endingBox = startingBox + boxPerIndex;

                    startingRemainder = lastRemainder;
                    endingRemainder = startingRemainder + remainder;

                    if (endingRemainder >= HomeController.HomeSlots)
                    {
                        endingRemainder -= HomeController.HomeSlots;
                        endingBox += 1;
                    }

                    lastBox = endingBox;
                    lastRemainder = endingRemainder;
                }
            }

            if (startingRemainder == 0)
                res1 = $"{startingBox + 1}";
            else
                res1 = $"{startingBox} Slot {startingRemainder + 1}";

            if (endingRemainder == 0)
                res2 = $"{endingBox}";
            else
                res2 = $"{endingBox} Slot {endingRemainder}";

            return new string[] { res1, res2 };
        }

        private string SetFileName(PKM? pkm, string path, DumpTarget target, bool boxFolderReq, bool encrypted, int i = 0)
        {
            var name = $"{path}\\";
            if (target == DumpTarget.TargetAll && boxFolderReq)
                name += $"Box {((i / 30) + 1):000}\\";
            if (pkm != null && pkm.Species != 0)
            {
                name += $"{pkm.Species:000}";
                if (pkm.Form > 0)
                    name += $"-{pkm.Form:00}";
                if (pkm.IsShiny)
                {
                    if (pkm.ShinyXor == 0 || pkm.FatefulEncounter)
                        name += " ■";
                    else
                        name += " ★";
                }
                name += $" - {NameFilter(pkm.Nickname)}";
                name += $" {pkm.EncryptionConstant:X8}{pkm.PID:X8}";
                if(encrypted)
                    name += $".eh1";
                else
                    name += $".ph1";
            }
            return name;
        }

        private string NameFilter(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
                if (c != '\\' && c != '/' && c != ':' && c != '*' && c != '?' && c != '"' && c != '<' && c != '>' && c != '|')
                    sb.Append(c);
            return sb.ToString();
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

        public PKH? DecryptEH1(byte[]? ek1)
        {
            if (ek1 != null)
                return new PKH(ek1);
            return null;
        }

        private ushort DataVersion(byte[] ekh) => ReadUInt16LittleEndian(ekh.AsSpan(0x00));

        public bool GetNonForceableConvSaveFile() => SaveFileEditor.SAV is SAV7b;

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
