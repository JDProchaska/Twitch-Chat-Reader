using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;

namespace Twitch_Chat_Reader.TextToSpeech
{
    internal class TextToSpeech
    {
        SpeechSynthesizer synthesizer;

        public TextToSpeech()
        {
            synthesizer = new SpeechSynthesizer();
            synthesizer.SetOutputToDefaultAudioDevice();
        }

        public void ReadText(string msg)
        {
            synthesizer.Speak(msg);
        }
    }
}
