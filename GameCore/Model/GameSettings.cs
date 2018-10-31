using System;
using System.Linq;
using GameCore.Model;
using Noname.BitConversion;
using Noname.BitConversion.System.Collections.Generic;

namespace GameCore.Model
{
    public class GameSettings
    {
        static public readonly VariableLengthBitConverter<GameSettings> BitConverter;

        static GameSettings()
        {
            VariableLengthBitConverterBuilder<GameSettings> builder = new VariableLengthBitConverterBuilder<GameSettings>();
            builder.AddField(x => x._bacteriums, (x, bacteriums) => x._bacteriums = bacteriums.ToArray(), ReliableBitConverter.GetInstance(IEnumerableVariableLengthBitConverter.GetInstance(BacteriumData.BitConverter.Instance)));
            BitConverter = builder.Finalize();
        }

        internal BacteriumData[] _bacteriums;

        public GameSettings() { }
        public GameSettings(BacteriumData[] bacteriums) => _bacteriums = bacteriums ?? throw new ArgumentNullException(nameof(bacteriums));

        public BacteriumData[] Bacteriums => _bacteriums;
    }
}