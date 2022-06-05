using System;
using static System.Buffers.Binary.BinaryPrimitives;
using HOME;
using System.Windows.Forms;

namespace PKHeX.Core.Injection
{
    public class HomeController
    {
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

        private readonly ISaveFileProvider SAV;
        public readonly IPKMView Editor;
        public PokeSysBotMini Bot;

        public HomeController(ISaveFileProvider boxes, IPKMView editor, ConnectionType con)
        {
            SAV = boxes;
            Editor = editor;
            Bot = new PokeSysBotMini(con);
        }

        public void LiveBox(int selectedIndex, int currBox, bool forceConversion)
        {
            var sav = SAV.SAV;

            var remoteBoxSize = 0;
            var remoteBoxTarget = 0;

            if (sav is SAV7b) {
                remoteBoxSize = HomeSlotSize * LGPESlots;
                remoteBoxTarget = selectedIndex * LGPEBoxes;
            }
            else if (sav is SAV8SWSH) {
                remoteBoxSize = HomeSlotSize * SwShSlots;
                remoteBoxTarget = selectedIndex * SwShBoxes;
            }
            else if (sav is SAV8BS) {
                remoteBoxSize = HomeSlotSize * BSSlots;
                remoteBoxTarget = selectedIndex * BSSBoxes;
            }
            else if (sav is SAV8LA) {
                remoteBoxSize = HomeSlotSize * LASlots;
                remoteBoxTarget = selectedIndex * LABoxes;
            }
            else
                throw new ArgumentException($"Unrecognized save file type {sav.GetType()}");

            var boxData = Bot.ReadBox(remoteBoxTarget + currBox, remoteBoxSize);

            if (boxData != null)
            {
                for (int i = 0; i < remoteBoxSize/HomeSlotSize; i++)
                {
                    var data = ExtractFromBoxData(i, ref boxData);
                    if (data != null)
                    {
                        var pkh = new PKH(data);
                        PKM? pkm = null;
                        if(pkh != null && pkh.Species != 0)
                        {
                            if (sav is SAV7b && CheckLGPEAvailability(pkh) && (pkh.DataPB7 != null)) //PX8 -> PB7 is not possible
                                pkm = pkh.ConvertToPB7();
                            else if (sav is SAV8SWSH && CheckSwShAvailability(pkh) && (pkh.DataPK8 != null || forceConversion))
                                pkm = pkh.ConvertToPK8();
                            else if (sav is SAV8BS && CheckBDSPAvailability(pkh) && (pkh.DataPB8 != null || forceConversion))
                                pkm = pkh.ConvertToPB8();
                            else if (sav is SAV8LA && CheckPLAAvailability(pkh) && (pkh.DataPA8 != null || forceConversion))
                                pkm = pkh.ConvertToPA8();

                            if(pkm != null)
                                sav.SetBoxSlotAtIndex(pkm, currBox, i);
                        }
                    }
                }
            }
        }

        private bool CheckPLAAvailability(PKM pk) => PersonalTable.LA.IsPresentInGame(pk.Species, pk.Form);
        private bool CheckBDSPAvailability(PKM pk) => PersonalTable.BDSP.IsPresentInGame(pk.Species, pk.Form);
        private bool CheckSwShAvailability(PKM pk) => PersonalTable.SWSH.IsPresentInGame(pk.Species, pk.Form);
        private bool CheckLGPEAvailability(PKM pk) => PersonalTable.LG.IsPresentInGame(pk.Species, pk.Form);


        private byte[]? ExtractFromBoxData(int index, ref byte[] boxData)
        {
            var offset = index * HomeSlotSize;

            var header = boxData.Slice(offset, 0x10);

            if (ReadUInt64LittleEndian(header.AsSpan()[0x02..]) == 0)
                return null;

            var EncodedDataSize = ReadUInt16LittleEndian(header.AsSpan()[0x0E..]);

            return boxData.Slice(offset, 0x10 + EncodedDataSize);
        }
    }
}