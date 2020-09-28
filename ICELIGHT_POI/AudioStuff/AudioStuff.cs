using System;
using NAudio.Wave;
using System.Linq;
using NAudio.Codecs;
using System.Diagnostics;
using NAudio;
using NAudio.Wave.Compression;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ICELIGHT_POI
{
    interface IAudioSender : IDisposable
    {
        void Send(byte[] payload);
    }
    interface IAudioReceiver : IDisposable
    {
        void OnReceived(Action<byte[]> handler);
    }
    public interface INetworkChatCodec : IDisposable
    {
        /// <summary>
        /// Friendly Name for this codec
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Tests whether the codec is available on this system
        /// </summary>
        bool IsAvailable { get; }
        /// <summary>
        /// Bitrate
        /// </summary>
        int BitsPerSecond { get; }
        /// <summary>
        /// Preferred PCM format for recording in (usually 8kHz mono 16 bit)
        /// </summary>
        WaveFormat RecordFormat { get; }
        /// <summary>
        /// Encodes a block of audio
        /// </summary>
        byte[] Encode(byte[] data, int offset, int length);
        /// <summary>
        /// Decodes a block of audio
        /// </summary>
        byte[] Decode(byte[] data, int offset, int length);
    }
    class G722ChatCodec : INetworkChatCodec
    {
        private readonly int bitrate;
        private readonly G722CodecState encoderState;
        private readonly G722CodecState decoderState;
        private readonly G722Codec codec;

        public G722ChatCodec()
        {
            bitrate = 64000;
            encoderState = new G722CodecState(bitrate, G722Flags.None);
            decoderState = new G722CodecState(bitrate, G722Flags.None);
            codec = new G722Codec();
            RecordFormat = new WaveFormat(16000, 1);
        }

        public string Name => "G.722 16kHz";

        public int BitsPerSecond => bitrate;

        public WaveFormat RecordFormat { get; }

        public byte[] Encode(byte[] data, int offset, int length)
        {
            if (offset != 0)
            {
                throw new ArgumentException("G722 does not yet support non-zero offsets");
            }
            var wb = new WaveBuffer(data);
            int encodedLength = length / 4;
            var outputBuffer = new byte[encodedLength];
            int encoded = codec.Encode(encoderState, outputBuffer, wb.ShortBuffer, length / 2);
            Debug.Assert(encodedLength == encoded);
            return outputBuffer;
        }

        public byte[] Decode(byte[] data, int offset, int length)
        {
            if (offset != 0)
            {
                throw new ArgumentException("G722 does not yet support non-zero offsets");
            }
            int decodedLength = length * 4;
            var outputBuffer = new byte[decodedLength];
            var wb = new WaveBuffer(outputBuffer);
            int decoded = codec.Decode(decoderState, wb.ShortBuffer, data, length);
            Debug.Assert(decodedLength == decoded * 2);  // because decoded is a number of samples
            return outputBuffer;
        }

        public void Dispose()
        {
            // nothing to do
        }

        public bool IsAvailable => true;
    }
    class Gsm610ChatCodec : AcmChatCodec
    {
        public Gsm610ChatCodec()
            : base(new WaveFormat(8000, 16, 1), new Gsm610WaveFormat())
        {
        }

        public override string Name => "GSM 6.10";
    }
    class AcmALawChatCodec : AcmChatCodec
    {
        public AcmALawChatCodec()
            : base(new WaveFormat(8000, 16, 1), WaveFormat.CreateALawFormat(8000, 1))
        {
        }

        public override string Name => "ACM G.711 a-law";
    }


    class ALawChatCodec : INetworkChatCodec
    {
        public string Name => "G.711 a-law";

        public int BitsPerSecond => RecordFormat.SampleRate * 8;

        public WaveFormat RecordFormat => new WaveFormat(8000, 16, 1);

        public byte[] Encode(byte[] data, int offset, int length)
        {
            byte[] encoded = new byte[length / 2];
            int outIndex = 0;
            for (int n = 0; n < length; n += 2)
            {
                encoded[outIndex++] = ALawEncoder.LinearToALawSample(BitConverter.ToInt16(data, offset + n));
            }
            return encoded;
        }

        public byte[] Decode(byte[] data, int offset, int length)
        {
            byte[] decoded = new byte[length * 2];
            int outIndex = 0;
            for (int n = 0; n < length; n++)
            {
                short decodedSample = ALawDecoder.ALawToLinearSample(data[n + offset]);
                decoded[outIndex++] = (byte)(decodedSample & 0xFF);
                decoded[outIndex++] = (byte)(decodedSample >> 8);
            }
            return decoded;
        }

        public void Dispose()
        {
            // nothing to do
        }

        public bool IsAvailable => true;
    }

    abstract class AcmChatCodec : INetworkChatCodec
    {
        private readonly WaveFormat encodeFormat;
        private AcmStream encodeStream;
        private AcmStream decodeStream;
        private int decodeSourceBytesLeftovers;
        private int encodeSourceBytesLeftovers;

        protected AcmChatCodec(WaveFormat recordFormat, WaveFormat encodeFormat)
        {
            RecordFormat = recordFormat;
            this.encodeFormat = encodeFormat;
        }

        public WaveFormat RecordFormat { get; }

        public byte[] Encode(byte[] data, int offset, int length)
        {
            if (encodeStream == null)
            {
                encodeStream = new AcmStream(RecordFormat, encodeFormat);
            }
            //Debug.WriteLine(String.Format("Encoding {0} + {1} bytes", length, encodeSourceBytesLeftovers));
            return Convert(encodeStream, data, offset, length, ref encodeSourceBytesLeftovers);
        }

        public byte[] Decode(byte[] data, int offset, int length)
        {
            if (decodeStream == null)
            {
                decodeStream = new AcmStream(encodeFormat, RecordFormat);
            }
            //Debug.WriteLine(String.Format("Decoding {0} + {1} bytes", data.Length, decodeSourceBytesLeftovers));
            return Convert(decodeStream, data, offset, length, ref decodeSourceBytesLeftovers);
        }

        private static byte[] Convert(AcmStream conversionStream, byte[] data, int offset, int length, ref int sourceBytesLeftovers)
        {
            int bytesInSourceBuffer = length + sourceBytesLeftovers;
            Array.Copy(data, offset, conversionStream.SourceBuffer, sourceBytesLeftovers, length);
            int bytesConverted = conversionStream.Convert(bytesInSourceBuffer, out var sourceBytesConverted);
            sourceBytesLeftovers = bytesInSourceBuffer - sourceBytesConverted;
            if (sourceBytesLeftovers > 0)
            {
                //Debug.WriteLine(String.Format("Asked for {0}, converted {1}", bytesInSourceBuffer, sourceBytesConverted));
                // shift the leftovers down
                Array.Copy(conversionStream.SourceBuffer, sourceBytesConverted, conversionStream.SourceBuffer, 0, sourceBytesLeftovers);
            }
            byte[] encoded = new byte[bytesConverted];
            Array.Copy(conversionStream.DestBuffer, 0, encoded, 0, bytesConverted);
            return encoded;
        }

        public abstract string Name { get; }

        public int BitsPerSecond => encodeFormat.AverageBytesPerSecond * 8;

        public void Dispose()
        {
            if (encodeStream != null)
            {
                encodeStream.Dispose();
                encodeStream = null;
            }
            if (decodeStream != null)
            {
                decodeStream.Dispose();
                decodeStream = null;
            }
        }

        public bool IsAvailable
        {
            get
            {
                // determine if this codec is installed on this PC
                bool available = true;
                try
                {
                    using (new AcmStream(RecordFormat, encodeFormat)) { }
                    using (new AcmStream(encodeFormat, RecordFormat)) { }
                }
                catch (MmException)
                {
                    available = false;
                }
                return available;
            }
        }
    }

    class MicrosoftAdpcmChatCodec : AcmChatCodec
    {
        public MicrosoftAdpcmChatCodec()
            : base(new WaveFormat(8000, 16, 1), new AdpcmWaveFormat(8000, 1))
        {
        }

        public override string Name => "Microsoft ADPCM";
    }
    class AcmMuLawChatCodec : AcmChatCodec
    {
        public AcmMuLawChatCodec()
            : base(new WaveFormat(8000, 16, 1), WaveFormat.CreateMuLawFormat(8000, 1))
        {
        }

        public override string Name => "ACM G.711 mu-law";
    }


    class MuLawChatCodec : INetworkChatCodec
    {
        public string Name => "G.711 mu-law";

        public int BitsPerSecond => RecordFormat.SampleRate * 8;

        public WaveFormat RecordFormat => new WaveFormat(8000, 16, 1);

        public byte[] Encode(byte[] data, int offset, int length)
        {
            var encoded = new byte[length / 2];
            int outIndex = 0;
            for (int n = 0; n < length; n += 2)
            {
                encoded[outIndex++] = MuLawEncoder.LinearToMuLawSample(BitConverter.ToInt16(data, offset + n));
            }
            return encoded;
        }

        public byte[] Decode(byte[] data, int offset, int length)
        {
            var decoded = new byte[length * 2];
            int outIndex = 0;
            for (int n = 0; n < length; n++)
            {
                short decodedSample = MuLawDecoder.MuLawToLinearSample(data[n + offset]);
                decoded[outIndex++] = (byte)(decodedSample & 0xFF);
                decoded[outIndex++] = (byte)(decodedSample >> 8);
            }
            return decoded;
        }

        public void Dispose()
        {
            // nothing to do
        }

        public bool IsAvailable { get { return true; } }

    }

    class NetworkAudioPlayer : IDisposable
    {
        private readonly INetworkChatCodec codec;
        private readonly IAudioReceiver receiver;
        private readonly IWavePlayer waveOut;
        private readonly BufferedWaveProvider waveProvider;

        public NetworkAudioPlayer(INetworkChatCodec codec, IAudioReceiver receiver)
        {
            this.codec = codec;
            this.receiver = receiver;
            receiver.OnReceived(OnDataReceived);

            waveOut = new WaveOut();
            waveProvider = new BufferedWaveProvider(codec.RecordFormat);
            waveOut.Init(waveProvider);
            waveOut.Play();
        }

        void OnDataReceived(byte[] compressed)
        {
            byte[] decoded = codec.Decode(compressed, 0, compressed.Length);
            waveProvider.AddSamples(decoded, 0, decoded.Length);
        }

        public void Dispose()
        {
            receiver?.Dispose();
            waveOut?.Dispose();
        }
    }

    class NetworkAudioSender : IDisposable
    {
        private readonly INetworkChatCodec codec;
        private readonly IAudioSender audioSender;
        private readonly WaveIn waveIn;

        public NetworkAudioSender(INetworkChatCodec codec, int inputDeviceNumber, IAudioSender audioSender)
        {
            this.codec = codec;
            this.audioSender = audioSender;
            waveIn = new WaveIn();
            waveIn.BufferMilliseconds = 50;
            waveIn.DeviceNumber = inputDeviceNumber;
            waveIn.WaveFormat = codec.RecordFormat;
            waveIn.DataAvailable += OnAudioCaptured;
            waveIn.StartRecording();
        }

        void OnAudioCaptured(object sender, WaveInEventArgs e)
        {
            byte[] encoded = codec.Encode(e.Buffer, 0, e.BytesRecorded);
            audioSender.Send(encoded);
        }

        public void Dispose()
        {
            waveIn.DataAvailable -= OnAudioCaptured;
            waveIn.StopRecording();
            waveIn.Dispose();
            waveIn?.Dispose();
            audioSender?.Dispose();
        }
    }

    class TrueSpeechChatCodec : AcmChatCodec
    {
        public TrueSpeechChatCodec()
            : base(new WaveFormat(8000, 16, 1), new TrueSpeechWaveFormat())
        {
        }

        public override string Name => "DSP Group TrueSpeech";
    }

    class UdpAudioReceiver : IAudioReceiver
    {
        private Action<byte[]> handler;
        private readonly UdpClient udpListener;
        private bool listening;

        public UdpAudioReceiver(int portNumber)
        {
            var endPoint = new IPEndPoint(IPAddress.Loopback, portNumber);

            udpListener = new UdpClient();

            // To allow us to talk to ourselves for test purposes:
            // http://stackoverflow.com/questions/687868/sending-and-receiving-udp-packets-between-two-programs-on-the-same-computer
            udpListener.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            udpListener.Client.Bind(endPoint);

            ThreadPool.QueueUserWorkItem(ListenerThread, endPoint);
            listening = true;
        }

        private void ListenerThread(object state)
        {
            var endPoint = (IPEndPoint)state;
            try
            {
                while (listening)
                {
                    byte[] b = udpListener.Receive(ref endPoint);
                    handler?.Invoke(b);
                }
            }
            catch (SocketException)
            {
                // usually not a problem - just means we have disconnected
            }
        }

        public void Dispose()
        {
            listening = false;
            udpListener?.Close();
        }

        public void OnReceived(Action<byte[]> onAudioReceivedAction)
        {
            handler = onAudioReceivedAction;
        }
    }

    class UdpAudioSender : IAudioSender
    {
        private readonly UdpClient udpSender;
        public UdpAudioSender(IPEndPoint endPoint)
        {
            udpSender = new UdpClient();
            udpSender.Connect(endPoint);
        }

        public void Send(byte[] payload)
        {
            udpSender.Send(payload, payload.Length);
        }

        public void Dispose()
        {
            udpSender?.Close();
        }
    }



    class UncompressedPcmChatCodec : INetworkChatCodec
    {
        public UncompressedPcmChatCodec()
        {
            RecordFormat = new WaveFormat(8000, 16, 1);
        }

        public string Name => "PCM 8kHz 16 bit uncompressed";

        public WaveFormat RecordFormat { get; private set; }

        public byte[] Encode(byte[] data, int offset, int length)
        {
            var encoded = new byte[length];
            Array.Copy(data, offset, encoded, 0, length);
            return encoded;
        }

        public byte[] Decode(byte[] data, int offset, int length)
        {
            var decoded = new byte[length];
            Array.Copy(data, offset, decoded, 0, length);
            return decoded;
        }

        public int BitsPerSecond => RecordFormat.AverageBytesPerSecond * 8;

        public void Dispose() { }

        public bool IsAvailable => true;
    }
}