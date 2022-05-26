using HOME;

namespace PKHeX.Core.Injection
{
    public class LiveHeXController
    {
        private readonly ISaveFileProvider SAV;
        public readonly IPKMView Editor;
        public PokeSysBotMini Bot;

        public LiveHeXController(ISaveFileProvider boxes, IPKMView editor, ConnectionType con)
        {
            SAV = boxes;
            Editor = editor;
            Bot = new PokeSysBotMini(con, 0);
        }

        //Reminder: Edit the constant value depending on the save file type. TBD when PKHeX will be natively compatible with PKH.
        public void ReadBox(int box)
        {
            var sav = SAV.SAV;
            var len = SAV.SAV.BoxSlotCount * 344;
            var data = Bot.ReadBox(box, len);
            sav.SetBoxBinary(data, box);
            SAV.ReloadSlots();
        }
    }
}