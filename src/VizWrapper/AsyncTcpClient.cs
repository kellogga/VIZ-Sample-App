using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace VizWrapper
{
    public class AsyncTcpClient : IDisposable
    {
        private bool _disposed;
        private TcpClient _tcpClient;
        private Stream _stream;

        private int _bufferSize = 8192;

 
        /// <summary>
        /// Public events
        /// </summary>
        public event EventHandler<byte[]> OnDataReceived;
        public event EventHandler OnDisconnected;

        /// <summary>
        /// Public properties
        /// </summary>
        public int MinBufferSize { get; set; } = 8192;

        public int MaxBufferSize { get; set; } = 15 * 1024 * 1024;

        public bool IsConnected => _tcpClient != null && _tcpClient.Connected;

        public int SendBufferSize
        {
            get {
                return _tcpClient?.SendBufferSize ?? 0;
            }
            set
            {
                if (_tcpClient != null)
                    _tcpClient.SendBufferSize = value;
            }
        }

        private int BufferSize
        {
            get
            {
                return _bufferSize;
            }
            set
            {
                _bufferSize = value;
                if (_tcpClient != null)
                    _tcpClient.ReceiveBufferSize = value;
            }
        }

        /// <summary>
        /// Connects to an external VIZ engine
        /// </summary>
        /// <param name="remoteServerInfo"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task ConnectAsync(RemoteServerInfo remoteServerInfo, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                await Close();
                _tcpClient = new TcpClient();
                cancellationToken.ThrowIfCancellationRequested();
                var result = _tcpClient.BeginConnect(remoteServerInfo.Host, remoteServerInfo.Port, null, null);
                var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(2));
                if (!success)
                {
                    throw new Exception("Failed to connect.");
                }
                await CloseIfCancelled(cancellationToken);
                // get stream and do SSL handshake if applicable

                _stream = _tcpClient.GetStream();
                await CloseIfCancelled(cancellationToken);
                if (remoteServerInfo.Ssl)
                {
                    var sslStream = new SslStream(_stream);
                    sslStream.AuthenticateAsClient(remoteServerInfo.Host);
                    _stream = sslStream;
                    await CloseIfCancelled(cancellationToken);
                }
            }
            catch (Exception)
            {
                CloseIfCancelled(cancellationToken).Wait(cancellationToken);
                throw;
            }
        }

        /// <summary>
        /// Send data to external VIZ engine
        /// </summary>
        /// <param name="data"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task SendAsync(byte[] data, CancellationToken token = default(CancellationToken))
        {
            try
            {
                if (_stream != null)
                {
                    await _stream.WriteAsync(data, 0, data.Length, token);
                    await _stream.FlushAsync(token);
                }
            }
            catch (IOException ex)
            {
                var onDisconected = OnDisconnected;
                if (ex.InnerException is ObjectDisposedException)
                { }
                else onDisconected?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Receive data from VIZ engine
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task ReceiveAsync(CancellationToken token = default(CancellationToken))
        {
            try
            {
                if (IsConnected)
                {
                    var buffer = new byte[_bufferSize];
                    while (IsConnected)
                    {
                        token.ThrowIfCancellationRequested();
                        var bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length, token);
                        if (bytesRead > 0)
                        {
                            if (bytesRead == buffer.Length)
                                BufferSize = Math.Min(BufferSize * 10, MaxBufferSize);
                            else
                            {
                                do
                                {
                                    var reducedBufferSize = Math.Max(BufferSize / 10, MinBufferSize);
                                    if (bytesRead < reducedBufferSize)
                                        BufferSize = reducedBufferSize;

                                }
                                while (bytesRead > MinBufferSize);
                            }
                            var onDataRecieved = OnDataReceived;
                            if (onDataRecieved != null)
                            {
                                var data = new byte[bytesRead];
                                Array.Copy(buffer, data, bytesRead);
                                onDataRecieved(this, data);
                            }
                        }
                        buffer = new byte[_bufferSize];
                    }
                }
            }
            catch (ObjectDisposedException) { }
            catch (IOException ex)
            {
                var evt = OnDisconnected;
                if (ex.InnerException is ObjectDisposedException) // for SSL streams
                {
                    evt?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private async Task Close()
        {
            await Task.Yield();
            if (_tcpClient != null)
            {
                _tcpClient.Close();
                _tcpClient = null;
            }
            if (_stream != null)
            {
                _stream.Dispose();
                _stream = null;
            }
        }

        private async Task CloseIfCancelled(CancellationToken token, Action onClosed = null)
        {
            if (token.IsCancellationRequested)
            {
                await Close();
                onClosed?.Invoke();
                token.ThrowIfCancellationRequested();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources.

                    if (_tcpClient != null)
                    {
                        _tcpClient.Close();
                        _tcpClient = null;
                    }
                }

                // There are no unmanaged resources to release, but
                // if we add them, they need to be released here.
            }

            _disposed = true;

            // If it is available, make the call to the
            // base class's Dispose(Boolean) method
            // base.Dispose(disposing);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
