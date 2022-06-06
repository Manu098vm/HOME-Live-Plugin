using System;
using System.IO;
using System.Windows.Forms;
using PKHeX.Core;
using System.Text;
using System.ComponentModel;

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

        public const int HomeSlotSize = 584;
        public const int HomeBoxes = 200;
        public const int HomeSlots = 30;
        public const int LGPEBoxes = 40;
        public const int LGPESlots = 25;
        public const int LABoxes = 32;
        public const int LASlots = 30;
        public const int BSSBoxes = 40;
        public const int BSSlots = 30;
        public const int SwShBoxes = 32;
        public const int SwShSlots = 30;
        
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

        public void StartDumper(DumpForm frm, BackgroundWorker? bgWorker)
        {
            if (bgWorker != null)
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
                    var sav = SaveFileEditor.SAV;
                    var bot = new PokeSysBotMini(frm.GetConnectionType())
                    {
                        sys = { IP = frm.GetIP(), Port = frm.GetPort() }
                    };
                    bot.sys.Connect();
                    frm.WriteLog("Connected.");

                    var decrypted = frm.GetWantedFormats() == DumpFormat.Decrypted || frm.GetWantedFormats() == DumpFormat.EncAndDec;
                    var encrypted = frm.GetWantedFormats() == DumpFormat.Encrypted || frm.GetWantedFormats() == DumpFormat.EncAndDec;
                    var target = frm.GetDumpTarget();
                    var found = 0;
                    var progress = 0;

                    switch (target)
                    {
                        case DumpTarget.TargetSlot:
                            var data = bot.ReadSlot(frm.GetTargetBox(), frm.GetTargetSlot());
                            HandleDump(data, decrypted, encrypted, frm, bgWorker, ref found);
                            break;
                        default:
                            var completedBoxes = 0;
                            do
                            {
                                var boxData = bot.ReadBox(frm.GetTargetBox() + completedBoxes);
                                for (int i = 0; i < HomeSlots; i++)
                                {
                                    var ekh = ExtractFromBoxData(i, ref boxData!);
                                    HandleDump(ekh, decrypted, encrypted, frm, bgWorker, ref found, i);
                                    progress = (target is DumpTarget.TargetBox) ? (i+1 * HomeBoxes) : i * (completedBoxes+1);
                                }
                                bgWorker.ReportProgress(progress);
                                frm.WriteLog($"Dumping [{(encrypted && decrypted ? found * 2 : found)}] file(s).\nDo note that the Home Data format might change in the future.");
                                completedBoxes++;
                            } while (target is DumpTarget.TargetAll && completedBoxes < HomeBoxes);
                            break;
                    }
                    bgWorker.ReportProgress(HomeBoxes * HomeSlots);
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

        public void StartViewer(ViewForm frm, BackgroundWorker? bgWorker)
        {
            var sav = SaveFileEditor.SAV;
            if (bgWorker != null)
            {
                try
                {
                    GetSavAvalableBoxAndSlots(out int savSlots, out int savBoxes);
                    var bot = new PokeSysBotMini(frm.GetConnectionType())
                    {
                        sys = { IP = frm.GetIP(), Port = frm.GetPort() }
                    };
                    bot.sys.Connect();
                    frm.WriteLog("Connecting...");

                    var selection = frm.GetBoxIndex();
                    var qty = CalcBoxQtyInSelection(selection);
                    for (int i = 0; i < qty; i++)
                    {
                        GetRemoteSizeAndTarget(out int remoteBoxSize, out int remoteBoxTarget, selection);
                        var boxData = bot.ReadBox(remoteBoxTarget + i, remoteBoxSize);

                        if (boxData != null)
                        {
                            for (int j = 0; j < remoteBoxSize / HomeSlotSize; j++)
                            {
                                var data = ExtractFromBoxData(j, ref boxData);
                                if (data != null)
                                {
                                    var pkh = new PKH(data);
                                    PKM? pkm = null;
                                    if (pkh != null && pkh.Species != 0)
                                    {
                                        if (sav is SAV7b && CheckLGPEAvailability(pkh) && (pkh.DataPB7 != null)) //PX8 -> PB7 is not possible
                                            pkm = pkh.ConvertToPB7();
                                        else if (sav is SAV8SWSH && CheckSwShAvailability(pkh) && (pkh.DataPK8 != null || frm.GetForceConversion()))
                                            pkm = pkh.ConvertToPK8();
                                        else if (sav is SAV8BS && CheckBDSPAvailability(pkh) && (pkh.DataPB8 != null || frm.GetForceConversion()))
                                            pkm = pkh.ConvertToPB8();
                                        else if (sav is SAV8LA && CheckPLAAvailability(pkh) && (pkh.DataPA8 != null || frm.GetForceConversion()))
                                            pkm = pkh.ConvertToPA8();

                                        if (pkm != null)
                                            sav.SetBoxSlotAtIndex(pkm, i, j);
                                    }
                                }
                            }
                        }
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

        private void HandleDump(byte[]? ekh, bool decrypted, bool encrypted, DumpForm frm, BackgroundWorker bgWorker, ref int found, int i = 0)
        {
            int version = DataVersion(ekh!);
            if (version == 0)
                frm.WriteLog($"Found an empty slot.");
            if (version > 1)
                throw new ArgumentException($"This plugin currently can handle only DataVersion [1]. PKH DataVersion is [{version}]");
            var pkh = DecryptEH1(ekh);

            if (pkh != null && pkh.Species != 0 && version == 1)
            {
                found++;
                if (decrypted)
                    if (!bgWorker.CancellationPending)
                        SavePKH(pkh.Data, SetFileName(pkh, frm.GetPath(), frm.GetBoxFolderRequested(), false, i));
                if (encrypted)
                    if (!bgWorker.CancellationPending)
                        SavePKH(ekh, SetFileName(pkh, frm.GetPath(), frm.GetBoxFolderRequested(), true, i));
            }
        }

        private byte[]? ExtractFromBoxData(int index, ref byte[] boxData)
        {
            var offset = index * HomeSlotSize;

            var header = boxData.Slice(offset, 0x10);

            if (ReadUInt64LittleEndian(header.AsSpan()[0x02..]) == 0)
                return null;

            var EncodedDataSize = ReadUInt16LittleEndian(header.AsSpan()[0x0E..]);

            return boxData.Slice(offset, 0x10 + EncodedDataSize);
        }

        private int CalcBoxQtyInSelection(int boxIndex)
        {
            GetSavAvalableBoxAndSlots(out int savSlots, out int savBoxes);
            var pastSlots = boxIndex * savSlots * savBoxes;
            var totalSlots = HomeSlots * HomeBoxes;
            var toReadSlots = totalSlots - pastSlots;
            var necessarySavBoxes = toReadSlots / savSlots;
            return necessarySavBoxes > savBoxes ? savBoxes : necessarySavBoxes;
        }

        private void GetRemoteSizeAndTarget(out int remoteBoxSize, out int remoteBoxTarget, int index)
        {
            if (SaveFileEditor.SAV is SAV7b)
            {
                remoteBoxSize = HomeSlotSize * LGPESlots;
                remoteBoxTarget = index * LGPEBoxes;
            }
            else if (SaveFileEditor.SAV is SAV8SWSH)
            {
                remoteBoxSize = HomeSlotSize * SwShSlots;
                remoteBoxTarget = index * SwShBoxes;
            }
            else if (SaveFileEditor.SAV is SAV8BS)
            {
                remoteBoxSize = HomeSlotSize * BSSlots;
                remoteBoxTarget = index * BSSBoxes;
            }
            else if (SaveFileEditor.SAV is SAV8LA)
            {
                remoteBoxSize = HomeSlotSize * LASlots;
                remoteBoxTarget = index * LABoxes;
            }
            else
                throw new ArgumentException($"Unrecognized save file type {SaveFileEditor.SAV.GetType()}");
        }

        private void GetSavAvalableBoxAndSlots(out int savSlots, out int savBoxes)
        {
            if (SaveFileEditor.SAV is SAV7b)
            {
                savSlots = LGPESlots;
                savBoxes = LGPEBoxes;
            }
            else if (SaveFileEditor.SAV is SAV8SWSH)
            {
                savSlots = SwShSlots;
                savBoxes = SwShBoxes;
            }
            else if (SaveFileEditor.SAV is SAV8BS)
            {
                savSlots = BSSlots;
                savBoxes = BSSBoxes;
            }
            else if (SaveFileEditor.SAV is SAV8LA)
            {
                savSlots = LASlots;
                savBoxes = LABoxes;
            }
            else
                throw new ArgumentException($"Unrecognized save file type {SaveFileEditor.SAV.GetType()}");
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

                    if (endingRemainder >= HomeSlots)
                    {
                        endingRemainder -= HomeSlots;
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

        private string SetFileName(PKM? pkm, string path, bool boxFolderReq, bool encrypted, int i = 0)
        {
            var name = $"{path}\\";
            if (boxFolderReq)
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

        private PKH? DecryptEH1(byte[]? ek1)
        {
            if (ek1 != null)
                return new PKH(ek1);
            return null;
        }

        private ushort DataVersion(byte[] ekh)
        {
            ushort version;
            try
            {
                version = ReadUInt16LittleEndian(ekh.AsSpan(0x00));
            }
            catch
            {
                version = 0;
            }
            return version;
        }

        private bool CheckPLAAvailability(PKM pk) => PersonalTable.LA.IsPresentInGame(pk.Species, pk.Form);
        private bool CheckBDSPAvailability(PKM pk) => PersonalTable.BDSP.IsPresentInGame(pk.Species, pk.Form);
        private bool CheckSwShAvailability(PKM pk) => PersonalTable.SWSH.IsPresentInGame(pk.Species, pk.Form);
        private bool CheckLGPEAvailability(PKM pk) => PersonalTable.LG.IsPresentInGame(pk.Species, pk.Form);

        public bool GetNonForceableConvSaveFile() => SaveFileEditor.SAV is SAV7b;

        public string[] CalculateBoxes()
        {
            GetSavAvalableBoxAndSlots(out int savSlots, out int savBoxes);

            var totalRemoteSlots = HomeSlots * HomeBoxes;
            var totalLocalSlots = savSlots * savBoxes;
            var necesaryIndexes = (int)Math.Ceiling((float)totalRemoteSlots / (float)totalLocalSlots);
            var res = new String[necesaryIndexes];

            if (savSlots == HomeSlots)
            {
                var needRemainder = totalRemoteSlots % totalLocalSlots != 0;
                var remainderBoxes = needRemainder ? HomeBoxes - (savBoxes * (necesaryIndexes - 1)) : 0;

                for (int i = 0; i < necesaryIndexes; i++)
                    if (i == necesaryIndexes - 1 && needRemainder)
                        res[i] = $"Box {i * savBoxes + 1} - Box {(i * savBoxes) + remainderBoxes}";
                    else
                        res[i] = $"Box {i * savBoxes + 1} - Box {(i + 1) * savBoxes}";
            }
            else
            {
                var remoteBoxPerIndex = (HomeBoxes / necesaryIndexes);
                var remainderSlots = totalLocalSlots - remoteBoxPerIndex * HomeSlots;

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
