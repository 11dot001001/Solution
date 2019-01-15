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
            builder.AddField(x => x.Bacteriums, (x, bacteriums) => x.Bacteriums = bacteriums.ToArray(), ReliableBitConverter.GetInstance(IEnumerableVariableLengthBitConverter.GetInstance(BacteriumData.BitConverter.InitializeInstance)));
            BitConverter = builder.Finalize();
        }

        public GameSettings() { }
        public GameSettings(BacteriumData[] bacteriums) => Bacteriums = bacteriums ?? throw new ArgumentNullException(nameof(bacteriums));

        public BacteriumData[] Bacteriums { get; private set; }
    }
}