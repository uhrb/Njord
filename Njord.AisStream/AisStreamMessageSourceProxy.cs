﻿using Njord.Ais.MessageProcessing;

namespace Njord.AisStream
{
    public class AisStreamMessageSourceProxy : IMessageSourceProxy<RawAisMessage>
    {
        private Func<RawAisMessage, CancellationToken, Task<bool>>? _receiver;

        public async Task ReceiveAsync(RawAisMessage message, CancellationToken token)
        {
            if (_receiver == null)
            {
                return;
            }
            await _receiver(message, token);
        }

        public void SetReceiver(Func<RawAisMessage, CancellationToken, Task<bool>> receiver)
        {
            _receiver = receiver;
        }
    }
}
