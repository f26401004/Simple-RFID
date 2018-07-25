using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.IO;

namespace RFID_ReaderGUI
{
    class SerialPortHelper
    {

        private SerialPort _serialPort = null;
        private int _currentPortNumber = -1;


        private int _baudRate = 4800;
        private Parity _parity = Parity.None;
        private Handshake _handshake = Handshake.None;
        private bool _dtrEnable = false;
        private int _dataBits = 8;
        private StopBits _stopBits = StopBits.One;
        private Encoding _encoding = Encoding.ASCII;
        private int _readTimeout = 500;
        private int _writeTimeout = 200;
        private int _readBufferSize = 4096;
        private int _writeBufferSize = 2048;

        public event SerialDataReceivedEventHandler DataReceived;
        public event SerialErrorReceivedEventHandler ErrorReceived;
        public event SerialPinChangedEventHandler PinChanged;
        public event System.EventHandler Disposed;

        [System.NonSerializedAttribute]
        public string Buffer;


        public bool IsOpen()
        {
            if (_currentPortNumber == -1)
            {
                return false;
            }
            return _serialPort.IsOpen;
        }

        public string GetPortName()
        {
            if (this.IsOpen() && _currentPortNumber != -1)
            {
                return "COM" + _currentPortNumber.ToString();
            }
            return "undefined";
        }

        public bool SetPort(int _port)
        {
            if (this.IsOpen())
            {
                string _com = this.GetPortName();
                Console.WriteLine("When Init \"" + _com + "\" Still Connecting ...");
                try
                {
                    _serialPort.DiscardInBuffer();
                    _serialPort.DiscardOutBuffer();
                    _serialPort.Dispose();
                    this.Close();
                    Console.WriteLine("\t> " + _com + " Connection Closing!!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\t> " + _com + " Can Not Being Close!!");
                }
            }
            string _connectionSetupRef = "";
            try
            {
                // initialize the serial port.
                _serialPort = new SerialPort(("COM" + _port.ToString()),
                    _baudRate,
                    _parity,
                    _dataBits,
                    _stopBits);
                _serialPort.Handshake = _handshake;
                _serialPort.Encoding = _encoding;
                _serialPort.ReadBufferSize = _readBufferSize;
                _serialPort.ReadTimeout = _readTimeout;
                _serialPort.WriteBufferSize = _writeBufferSize;
                _serialPort.WriteTimeout = _writeTimeout;
                // add event
                _serialPort.DataReceived += this.DataReceived;
                _serialPort.ErrorReceived += this.ErrorReceived;
                // open the serial port.
                _serialPort.Open();
                _currentPortNumber = _port;

                _connectionSetupRef += "Port \t:" + this.GetPortName().ToString() + "\n";
                _connectionSetupRef += "BaudRate \t:" + _baudRate + "\n";
                _connectionSetupRef += "Parity \t:" + _parity.ToString() + "\n";
                _connectionSetupRef += "DataBits \t:" + _dataBits + "\n";
                _connectionSetupRef += "StopBits \t:" + _stopBits + "\n";
                _connectionSetupRef += "Handshake \t:" + _handshake.ToString() + "\n";
                _connectionSetupRef += "Encoding \t:" + _encoding.ToString() + "\n";
                _connectionSetupRef += "ReadBufferSize \t:" + _readBufferSize + "\n";
                _connectionSetupRef += "ReadTimeout \t:" + _readTimeout + "\n";
                _connectionSetupRef += "WriteBufferSize \t:" + _writeBufferSize + "\n";
                _connectionSetupRef += "WriteTimeout \t:" + _writeTimeout + "\n";
                Console.Write("COM" + _port + " Connected!!\n" + _connectionSetupRef);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("COM" + _port + " Not Exist\n\t" + ex.Message);
                _serialPort = null;
                _currentPortNumber = -1;
            }
            return false;
        }

        public bool SearchComPort()
        {
            Console.WriteLine("Serial Port Searching ...");
            for (int i = 1; i < 13; ++i)
            {
                Console.WriteLine("Try To Connecting COM" + i.ToString() + " Port ...");
                if (SetPort(i))
                {
                    return true;
                }
            }
            Console.WriteLine("Serial Port Searching Failed!!");
            return false;
        }

        public void DataReceivedHandle(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort _sp = (SerialPort)sender;
            Buffer = _sp.ReadExisting();
            if (Buffer.Length > 0)
            {
                Console.WriteLine("SerialPort : Data Received \n\t" + Buffer + "\n\tEvent - " + e.ToString());
            }
            if (this.DataReceived != null)
            {
                this.DataReceived(sender, e);
            }
        }

        public void ErrorReceivedHandle(object sender, SerialErrorReceivedEventArgs e)
        {
            Console.WriteLine("SerialPort : Received Error - \n" + e.ToString());
            if (this.ErrorReceived != null)
            {
                this.ErrorReceived(sender, e);
            }
        }

        public void PinChangedHandle(object sender, SerialPinChangedEventArgs e)
        {
            Console.WriteLine("SerialPort : PinChange - \n" + e.ToString());
            if (this.PinChanged != null)
            {
                PinChanged(sender, e);
            }
        }

        public void DisposedHandle(object sender, EventArgs e)
        {
            Console.WriteLine("SerialPort : Disposed - \n" + e.ToString());
            if (this.Disposed != null)
            {
                this.Disposed(sender, e);
            }
        }
        public void Open()
        {
            _serialPort.Open();
            Console.WriteLine("SerialPort :" + this.GetPortName() + " Opened");
        }

        public void Close()
        {
            Console.WriteLine("SerialPort :" + this.GetPortName() + "Closed");
            _serialPort.Close();
        }

        public void Write(string _text)
        {
            _serialPort.Write(_text);
            Console.WriteLine("Write : " + _text);
        }

        public int Read(ref byte[] _buffer, int _offset, int _count)
        {
            try
            {
                int _result = _serialPort.Read(_buffer, _offset, _count);
                Console.WriteLine("Read char -Char[" + _count + "] : " + _buffer.ToString());
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
            return -1;
        }

        public int ReadByte()
        {
            try
            {
                int _result = _serialPort.ReadByte();
                Console.WriteLine("Read byte =[" + _result + "]");
                return _result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
            return -1;
        }
        public int ReadChar()
        {
            try
            {
                int _result = _serialPort.ReadChar();
                Console.WriteLine("Read byte =[" + _result + "]");
                return _result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }
        public string ReadLine()
        {
            try
            {
                string _result = _serialPort.ReadLine();
                Console.WriteLine("Read Line : " + _result);
                return _result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }

        public string ReadExsiting()
        {
            try
            {
                string _result = _serialPort.ReadExisting();
                Console.WriteLine("Read Exsiting : " + _result);
                return _result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }

        public Stream BaseStream()
        {
            return _serialPort.BaseStream;
        }
        public void DiscardInBuffer()
        {
            _serialPort.DiscardInBuffer();
            Console.WriteLine("Discard In Buffer!!");
        }

        public void DiscardOutBuffer()
        {
            _serialPort.DiscardOutBuffer();
            Console.WriteLine("Discard Out Buffer!!");
        }
        public void Dispose()
        {
            _serialPort.Dispose();
        }

        public int BytesToRead()
        {
            return _serialPort.BytesToRead;
        }
    }
}
