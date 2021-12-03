using System.Collections.Generic;

using UnityEngine;

namespace TexPacker
{
    public class TextureInput
    {
        public Texture2D texture;

        private Dictionary<TextureChannel, TextureChannelInput> _inputs = new Dictionary<TextureChannel, TextureChannelInput>();

        public TextureInput()
        {
            _inputs[TextureChannel.Metallic] = new TextureChannelInput();
            _inputs[TextureChannel.AO] = new TextureChannelInput();
            _inputs[TextureChannel.Detail] = new TextureChannelInput();
            _inputs[TextureChannel.Smoothness] = new TextureChannelInput();
        }

        public TextureChannelInput GetChannelInput(TextureChannel channel)
        {
            return _inputs[channel];
        }

        public void SetChannelInput(TextureChannel channel, TextureChannelInput channelInput)
        {
            _inputs[channel] = channelInput;
        }
    }
}