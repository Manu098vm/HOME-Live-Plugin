using System;
using System.IO;
using System.Windows.Forms;
using PKHeX.Core;
using System.Text;
using System.ComponentModel;
using static System.Buffers.Binary.BinaryPrimitives;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HOME
{
    public class HOME : IPlugin
    {
        public const string Version = "2.1.1";

        public string Name => nameof(HOME);
        public int Priority => 1;
        public string Language = null!;

        // Initialized on plugin load
        public ISaveFileProvider SaveFileEditor { get; private set; } = null!;
        public IPKMView PKMEditor { get; private set; } = null!;

        private readonly ToolStripMenuItem Plugin = new("Home Live Plugin");
        private readonly ToolStripMenuItem Dumper = new ToolStripMenuItem("Home Live Dumper");
        private readonly ToolStripMenuItem Viewer = new ToolStripMenuItem("Home Live Viewer");

        private Dictionary<string, string> Strings = null!;

        public const string TitleID = "010015F008C54000";

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
        public const int SVBoxes = 32;
        public const int SVSlots = 30;

        public void Initialize(params object[] args)
        {
            Plugin.Image = Properties.Resources.icon.ToBitmap();
            SaveFileEditor = (ISaveFileProvider)Array.Find(args, z => z is ISaveFileProvider)!;
            PKMEditor = (IPKMView)Array.Find(args, z => z is IPKMView)!;
            var menu = (ToolStrip)Array.Find(args, z => z is ToolStrip)!;
            NotifySaveLoaded();
            Task.Run(async () => { await GitHubUtil.TryUpdate(Language); }).Wait();
            LoadMenuStrip(menu);
            GenerateDictionary();
            TranslateDictionary(Language);
        }

        private void GenerateDictionary()
        {
            Strings = new Dictionary<string, string>
            {
                { "Action.Connected", "" },
                { "Action.Dumping", "" },
                { "Action.Dumped", "" },
                { "Action.Completed", "" },
                { "Action.Files", "" },
                { "Warning.FormatChanges", "" },

                { "Word.Box", "" },
                { "Word.Slot", "" },

                { "Warning.SomethingWrong", "" },
                { "Warning.CheckInstallation", "" },
                { "Warning.IncompatibleData", "" },
                { "Warning.EmptySlot", "" },
                { "Warning.MismatchVersion", "" },
                { "Warning.NoRoutePK9", "" },
                { "Warning.Unrecognized", "" },
                { "Warning.DataNull", "" }
            };
        }

        private void TranslateDictionary(string language) => Strings = Strings.TranslateInnerStrings(language);

        private void LoadMenuStrip(ToolStrip menuStrip)
        {
            var items = menuStrip.Items;
            if (!(items.Find("Menu_Tools", false)[0] is ToolStripDropDownItem tools))
                throw new ArgumentException(nameof(menuStrip));
            AddPluginControl(tools);
        }

        private void AddPluginControl(ToolStripDropDownItem tools)
        {

            Plugin.DropDownItems.Add(Dumper);
            Plugin.DropDownItems.Add(Viewer);
            tools.DropDownItems.Add(Plugin);
            Dumper.Click += (s, e) => new DumpForm(this).Show();
            Viewer.Click += (s, e) => new ViewForm(this).Show();
        }

        private void TranslatePlugins()
        {
            var dic = new Dictionary<string, string>
            {
                { "Plugin.HomeLivePlugin", "Home Live Plugin" },
                { "Plugin.DumperPlugin", "Home Live Dumper" },
                { "Plugin.ViewerPlugin", "Home Live Viewer" }
            }.TranslateInnerStrings(Language);

            Plugin.Text = dic["Plugin.HomeLivePlugin"];
            Dumper.Text = dic["Plugin.DumperPlugin"];
            Viewer.Text = dic["Plugin.ViewerPlugin"];
        }

        public void StartDumper(DumpForm frm, BackgroundWorker? bgWorker)
        {
            if (bgWorker != null)
            {
                try
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
                                    HandleDump(ekh, decrypted, encrypted, frm, bgWorker, ref found, completedBoxes + 1);
                                    progress = (target is DumpTarget.TargetBox) ? (i+1 * HomeBoxes) : i * (completedBoxes+1);
                                }
                                bgWorker.ReportProgress(progress);
                                frm.WriteLog($"{Strings["Action.Dumping"]} [{(encrypted && decrypted ? found * 2 : found)}] {Strings["Action.Files"]}.\n{Strings["Warning.FormatChanges"]}");
                                completedBoxes++;
                            } while (target is DumpTarget.TargetAll && completedBoxes < HomeBoxes);
                            break;
                    }
                    bgWorker.ReportProgress(HomeBoxes * HomeSlots);
                    frm.WriteLog($"{Strings["Action.Completed"]} [{(encrypted && decrypted ? found * 2 : found)}] {Strings["Action.Files"]} {Strings["Action.Dumped"]}.\n{Strings["Warning.FormatChanges"]}");
                    return;
                } catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    frm.WriteLog($"{Strings["Warning.SomethingWrong"]}\n{Strings["Warning.CheckInstallation"]}");
                }
            }
        }

        public void StartEncryptorDecryptor(DumpForm frm, DumpFormat originFormat, string file, string path)
        {
            var data = File.ReadAllBytes(file);
            
            if (DataVersion(data) != 1)
            {
                frm.WriteLog($"{file} {Strings["Warning.IncompatibleData"]}");
                return;
            }

            switch (originFormat)
            {
                case DumpFormat.Encrypted:
                    if(HomeCrypto.GetIsEncrypted1(data.AsSpan()))
                        data = DecryptEH1(data)!.Data;
                    break;
                case DumpFormat.Decrypted:
                    if(!HomeCrypto.GetIsEncrypted1(data.AsSpan()))
                    data = HomeCrypto.Encrypt(data.AsSpan());
                    break;
            }

            SavePKH(data, $"{path}\\{Path.GetFileNameWithoutExtension(file)}.{(originFormat == DumpFormat.Encrypted ? "ph1" : "eh1")}");
        }

        public void StartViewer(ViewForm frm, BackgroundWorker? bgWorker)
        {
            if (bgWorker is not null)
            {
                try
                {
                    GetSavAvalableBoxAndSlots(out int savSlots, out int savBoxes);
                    var bot = new PokeSysBotMini(frm.GetConnectionType())
                    {
                        sys = { IP = frm.GetIP(), Port = frm.GetPort() }
                    };
                    bot.sys.Connect();
                    frm.WriteLog(Strings["Action.Connected"]);

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
                                    PKM? pkm = null;
                                    try
                                    {
                                        pkm = ConvertToPKM(new PKH(data), frm.GetConversionType());
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show($"{ex.Message}");
                                        return;
                                    }

                                    if (pkm != null)
                                    {
                                        LegalityHelper.CheckAndFixLegality(pkm);
                                        SaveFileEditor.SAV.SetBoxSlotAtIndex(pkm, i, j);
                                    }
                                }
                            }
                        }
                        bgWorker.ReportProgress(100 / savBoxes * i);
                    }
                    frm.WriteLog(Strings["Action.Completed"]);
                    bgWorker.ReportProgress(100);

                    if (SaveFileEditor is UserControl u && u.InvokeRequired)
                        u.Invoke(SaveFileEditor.ReloadSlots);
                    else
                        SaveFileEditor.ReloadSlots();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    frm.WriteLog($"{Strings["Warning.SomethingWrong"]}\n{Strings["Warning.CheckInstallation"]}");
                }
            }
        }

        public bool StartLoader(DumpForm frm, string file, bool toBoxes, int box = 0, int slot = 0)
        {
            var data = File.ReadAllBytes(file);
            if (DataVersion(data) != 1)
            {
                frm.WriteLog($"{file} {Strings["Warning.IncompatibleData"]}");
                return false;
            }

            PKM? pkm = null;
            try
            {
                pkm = ConvertToPKM(DecryptEH1(data)!, ConversionType.AnyData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}");
                if (SaveFileEditor.SAV is SAV9SV)
                    return false;
            }

            if (pkm != null)
            {
                LegalityHelper.CheckAndFixLegality(pkm);
                if (toBoxes)
                    SaveFileEditor.SAV.SetBoxSlotAtIndex(pkm, box, slot);
                else
                {
                    SaveFileEditor.SAV.AdaptPKM(pkm);
                    if (PKMEditor is UserControl u && u.InvokeRequired)
                        u.Invoke(() => { PKMEditor.PopulateFields(pkm, true); });
                    else
                        PKMEditor.PopulateFields(pkm, true);
                }
                return true;
            }

            return false;
        }

        private void HandleDump(byte[]? ekh, bool decrypted, bool encrypted, DumpForm frm, BackgroundWorker bgWorker, ref int found, int box = 0)
        {
            int version = DataVersion(ekh!);
            if (version == 0)
                frm.WriteLog($"{Strings["Warning.EmptySlot"]}");
            if (version > 1)
                throw new ArgumentException($"{Strings["Warning.MismatchVersion"]}[{version}]");
            var pkh = DecryptEH1(ekh);

            if (pkh != null && pkh.Species != 0 && version == 1)
            {
                found++;
                if (decrypted)
                    if (!bgWorker.CancellationPending)
                        SavePKH(pkh.Data, SetFileName(pkh, frm.GetPath(), frm.GetBoxFolderRequested(), false, box));
                if (encrypted)
                    if (!bgWorker.CancellationPending)
                        SavePKH(ekh, SetFileName(pkh, frm.GetPath(), frm.GetBoxFolderRequested(), true, box));
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

        private PKM? ConvertToPKM(PKH? pkh, ConversionType conv)
        {
            if (pkh != null && CanConvert(pkh, conv))
                return SimulateTransferAndConvert(pkh);
            return null;
        }

        private PKM? SimulateTransferAndConvert(PKH pkh)
        {
            var sav = SaveFileEditor.SAV;
            if (sav is SAV7b)
            {
                return pkh.ConvertToPB7();
            }
            else if (sav is SAV8SWSH)
            {
                if (pkh.DataPK8 is not null)
                    return pkh.ConvertToPK8();
                else
                {
                    EntityConverter.RejuvenateHOME = EntityRejuvenationSetting.MissingDataHOME;
                    var pk = EntityConverter.ConvertToType(GetAvailablePkmData(pkh)!, typeof(PK8), out var success);
                    if ((int)success < 4)
                        return pk;
                    else
                        return pkh.ConvertToPK8();
                }
            }
            else if (sav is SAV8BS)
            {
                if (pkh.DataPB8 is not null)
                    return pkh.ConvertToPB8();
                else
                {
                    EntityConverter.RejuvenateHOME = EntityRejuvenationSetting.MissingDataHOME;
                    var pk = EntityConverter.ConvertToType(GetAvailablePkmData(pkh)!, typeof(PB8), out var success);
                    if ((int)success < 4)
                        return pk;
                    else
                        return pkh.ConvertToPB8();
                }
            }
            else if (sav is SAV8LA)
            {
                if (pkh.DataPA8 is not null)
                    return pkh.ConvertToPA8();
                else
                {
                    EntityConverter.RejuvenateHOME = EntityRejuvenationSetting.MissingDataHOME;
                    var pk = EntityConverter.ConvertToType(GetAvailablePkmData(pkh)!, typeof(PA8), out var success);
                    if ((int)success < 4)
                        return pk;
                    else
                        return pkh.ConvertToPA8();
                }
            }
            else if (sav is SAV9SV)
            {
                if (pkh.DataPK9 is not null)
                    return pkh.ConvertToPK9();
                else
                {
                    //EntityConverter.RejuvenateHOME = EntityRejuvenationSetting.MissingDataHOME;
                    //var pk = EntityConverter.ConvertToType(GetAvailablePkmData(pkh)!, typeof(PK9), out var success);
                    //if ((int)success < 4)
                        //return pk;
                    //else
                        //return pkh.ConvertToPK9();
                    throw new Exception($"{Strings["Warning.NoRoutePK9"]}\n");
                }
            }
            else 
                throw new ArgumentOutOfRangeException(nameof(sav));
        }

        private PKM? GetAvailablePkmData(PKH pkh)
        {
            if (pkh.DataPK8 is not null)
                return pkh.ConvertToPK8();
            else if (pkh.DataPB8 is not null)
                return pkh.ConvertToPB8();
            else if(pkh.DataPA8 is not null)
                return pkh.ConvertToPA8();
            else if(pkh.DataPK9 is not null)
                return pkh.ConvertToPK9();
            else if(pkh.DataPB7 is not null)
                return pkh.ConvertToPB7();
            else 
                throw new ArgumentOutOfRangeException();
        }

        private bool CanConvert(PKH? pkh, ConversionType conv)
        {
            if (pkh != null && pkh.Species > 0)
            {
                var sav = SaveFileEditor.SAV;
                if (sav is SAV7b)
                {
                    if (conv is ConversionType.SpecificData && CheckLGPEAvailability(pkh) && pkh.DataPB7 is not null)
                        return true;
                }
                else if(sav is SAV8SWSH)
                {
                    if (conv is ConversionType.SpecificData && CheckSwShAvailability(pkh) && pkh.DataPK8 is not null)
                        return true;
                    else if (conv is ConversionType.CompatibleData && CheckSwShAvailability(pkh))
                        return true;
                    else if (conv is ConversionType.AnyData)
                        return true;
                }
                else if(sav is SAV8BS)
                {
                    if (conv is ConversionType.SpecificData && CheckBDSPAvailability(pkh) && pkh.DataPB8 is not null)
                        return true;
                    else if (conv is ConversionType.CompatibleData && CheckBDSPAvailability(pkh))
                        return true;
                    else if (conv is ConversionType.AnyData)
                        return true;
                }
                else if (sav is SAV8LA)
                {
                    if (conv is ConversionType.SpecificData && CheckPLAAvailability(pkh) && pkh.DataPA8 is not null)
                        return true;
                    else if (conv is ConversionType.CompatibleData && CheckPLAAvailability(pkh))
                        return true;
                    else if (conv is ConversionType.AnyData)
                        return true;
                }
                else if(sav is SAV9SV)
                {
                    if (conv is ConversionType.SpecificData && CheckSVAvailability(pkh) && pkh.DataPK9 is not null)
                        return true;
                    else if (conv is ConversionType.CompatibleData && CheckSVAvailability(pkh))
                        return true;
                    else if (conv is ConversionType.AnyData)
                        return true;
                }
            }
            return false;
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
            else if(SaveFileEditor.SAV is SAV9SV)
            {
                remoteBoxSize= HomeSlotSize * SVSlots;
                remoteBoxTarget = index * SVBoxes;
            }
            else
                throw new ArgumentException($"{Strings["Warning.Unrecognized"]} {SaveFileEditor.SAV.GetType()}");
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
                res1 = $"{startingBox} {Strings["Word.Slot"]} { startingRemainder + 1}";

            if (endingRemainder == 0)
                res2 = $"{endingBox}";
            else
                res2 = $"{endingBox} {Strings["Word.Slot"]} {endingRemainder}";

            return new string[] { res1, res2 };
        }

        private string SetFileName(PKH? pkm, string path, bool boxFolderReq, bool encrypted, int box = 0)
        {
            var name = $"{path}\\";
            if (boxFolderReq)
                name += $"{Strings["Word.Box"]} {box:000}\\";
            if (pkm != null && pkm.Species != 0)
            {
                name += $"{pkm.Species:000}";
                if (pkm.Form > 0 || pkm.Species == 869)
                    name += $"-{pkm.Form:00}";
                if (pkm.Species == 869 && pkm is IFormArgument f)
                    name += $"-{f.FormArgument:00}";
                if (pkm.IsShiny)
                {
                    if (pkm.ShinyXor == 0 || pkm.FatefulEncounter)
                        name += " ■";
                    else
                        name += " ★";
                }
                if (pkm.ConvertToPK8() is IGigantamax g)
                    if (g.CanGigantamax)
                        name += " (GMax)";
                if (pkm.ConvertToPA8() is IAlpha a)
                    if (a.IsAlpha)
                        name += " (Alpha)";
                if (pkm.ConvertToPA8() is INoble n)
                    if (n.IsNoble)
                        name += " (Noble)";
                name += $" - {NameFilter(pkm.Nickname)}";
                name += $" {pkm.Tracker:X16}";
                if (encrypted)
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
        
        private void SavePKH(byte[]? data, string path)
        {
            if (data != null)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path)!);
                File.WriteAllBytes(path, data);
            }
            else
                throw new ArgumentException(Strings["Warning.DataNull"]);
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

        private bool CheckSVAvailability(PKM pk) => PersonalTable.SV.IsPresentInGame(pk.Species, pk.Form);
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
                        res[i] = $"{Strings["Word.Box"]} {i * savBoxes + 1} - {Strings["Word.Box"]} {(i * savBoxes) + remainderBoxes}";
                    else
                        res[i] = $"{Strings["Word.Box"]} {i * savBoxes + 1} - {Strings["Word.Box"]} {(i + 1) * savBoxes}";
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
                    res[boxIndex] = $"{Strings["Word.Box"]} {str[0]} - {Strings["Word.Box"]} {str[1]}";
                }
            }
            return res;
        }

        public void GetSavAvalableBoxAndSlots(out int savSlots, out int savBoxes)
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
            else if (SaveFileEditor.SAV is SAV9SV)
            {
                savSlots = SVSlots;
                savBoxes = SVBoxes;
            }
            else
                throw new ArgumentException($"{Strings["Warning.Unrecognized"]} {SaveFileEditor.SAV.GetType()}");
        }

        public void ReloadSav()
        {
            if (SaveFileEditor is UserControl u && u.InvokeRequired)
                u.Invoke(SaveFileEditor.ReloadSlots);
            else
                SaveFileEditor.ReloadSlots();
        }

        public void NotifySaveLoaded()
        {
            Language = GameInfo.CurrentLanguage;
            TranslatePlugins();
            if (SaveFileEditor.SAV is SAV9SV or SAV8LA or SAV8BS or SAV8SWSH or SAV7b)
                Plugin.Enabled = true;
            else
                Plugin.Enabled = false;
        }

        public bool TryLoadFile(string filePath)
        {
            var pkhlist = new List<PKH>();

            if (Directory.Exists(filePath))
            {
                var files = Directory.EnumerateFiles(filePath, "*.*", SearchOption.AllDirectories);
                foreach (var file in files)
                    TryAddPkh(file, pkhlist);
            }

            else if(File.Exists(filePath))
                TryAddPkh(filePath, pkhlist);

            var pklist = new List<PKM>();
            foreach (var pkh in pkhlist)
            {
                PKM? pkm = null;
                try
                {
                    pkm = ConvertToPKM(pkh, ConversionType.AnyData);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}");
                    return false;
                }
                if (pkm != null)
                {
                    SaveFileEditor.SAV.AdaptPKM(pkm);
                    LegalityHelper.CheckAndFixLegality(pkm);
                    pklist.Add(pkm);
                }
            }

            if (pklist.Count > 1)
            {
                if(SaveFileEditor is UserControl u && u.InvokeRequired)
                    u.Invoke(() => { SaveFileEditor.SAV.LoadBoxes(pklist, out _, SaveFileEditor.CurrentBox, false, true); });
                else
                    SaveFileEditor.SAV.LoadBoxes(pklist, out _, SaveFileEditor.CurrentBox, false, true);

                ReloadSav();
                return true;
            }

            else if (pklist.Count == 1)
            {
                if (PKMEditor is UserControl u && u.InvokeRequired)
                    u.Invoke(() => { PKMEditor.PopulateFields(pklist[0]); });
                else
                    PKMEditor.PopulateFields(pklist[0]);
                return true;
            }

            return false;
        }

        private void TryAddPkh(string file, List<PKH> pkhlist)
        {
            if (Path.GetExtension(file).Equals(".pkh") ||
                Path.GetExtension(file).Equals(".ekh") ||
                Path.GetExtension(file).Equals(".ph1") || 
                Path.GetExtension(file).Equals(".eh1"))
            {
                try
                {
                    var data = File.ReadAllBytes(file);
                    if (DataVersion(data) != 1)
                        return;
                    var pkh = DecryptEH1(data);
                    if (pkh!.Valid)
                        pkhlist.Add(pkh);
                }
                catch { }
            }
        }
    }
}
