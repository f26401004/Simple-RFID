using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using PCSC;
using System.Windows.Forms;

namespace RFID_ReaderGUI
{
    class SmartCardReaderHelper
    {
        public int status;
        public bool Enabled {
            get { return _timer.Enabled; }
            set
            {
                if (value)
                {
                    _timer.Start();
                }
                else
                {
                    _timer.Stop();
                }
            }
        }
        // the hexadecimal code to control the LED.
        private byte[] Led_Blue_On = new byte[] { 0xFF, 0xE1, 0x02, 0x01, 0x0C, 0xA0, 0xFC, 0x10, 0xA0, 0xFD, 0x04, 0xA0, 0xFE, 0x04, 0xA0, 0xFF, 0x04 };
        //private byte[] Led_Blue_Blink = new byte[] { 0xFF, 0xE1, 0x02, 0x01, 0x0C, 0xA0, 0xFC, 0x18, 0xA0, 0xFD, 0x04, 0xA0, 0xFE, 0x04, 0xA0, 0xFF, 0x04 };
        //private byte[] Led_Yellow_On = new byte[] { 0xFF, 0xE1, 0x02, 0x01, 0x0C, 0xA0, 0xFC, 0x20, 0xA0, 0xFD, 0x04, 0xA0, 0xFE, 0x04, 0xA0, 0xFF, 0x04 };
        //private byte[] Led_Yellow_Blink = new byte[] { 0xFF, 0xE1, 0x02, 0x01, 0x0C, 0xA0, 0xFC, 0x28, 0xA0, 0xFD, 0x04, 0xA0, 0xFE, 0x04, 0xA0, 0xFF, 0x04 };
        //private byte[] Led_Green_On = new byte[] { 0xFF, 0xE1, 0x02, 0x01, 0x0C, 0xA0, 0xFC, 0x40, 0xA0, 0xFD, 0x04, 0xA0, 0xFE, 0x04, 0xA0, 0xFF, 0x04 };
        private byte[] Led_Green_On_SE = new byte[] { 0xFF, 0xE1, 0x02, 0x01, 0x0C, 0xA0, 0xFC, 0x44, 0xA0, 0xFD, 0x04, 0xA0, 0xFE, 0x04, 0xA0, 0xFF, 0x01 };
        //private byte[] Led_Green_Blink = new byte[] { 0xFF, 0xE1, 0x02, 0x01, 0x0C, 0xA0, 0xFC, 0x48, 0xA0, 0xFD, 0x04, 0xA0, 0xFE, 0x04, 0xA0, 0xFF, 0x04 };
        //private byte[] Led_Red_On = new byte[] { 0xFF, 0xE1, 0x02, 0x01, 0x0C, 0xA0, 0xFC, 0x80, 0xA0, 0xFD, 0x04, 0xA0, 0xFE, 0x04, 0xA0, 0xFF, 0x04 };
        //private byte[] Led_Red_Blink = new byte[] { 0xFF, 0xE1, 0x02, 0x01, 0x0C, 0xA0, 0xFC, 0x88, 0xA0, 0xFD, 0x04, 0xA0, 0xFE, 0x04, 0xA0, 0xFF, 0x04 };
        //private byte[] Led_Off = new byte[] { 0xFF, 0xE1, 0x02, 0x01, 0x0C, 0xA0, 0xFC, 0x00, 0xA0, 0xFD, 0x04, 0xA0, 0xFE, 0x04, 0xA0, 0xFF, 0x04 };
        private byte[] Get_ID = new byte[] { 0xFF, 0xCA, 0x00, 0x00, 0x00 };
        //private byte[] Led_All_On = new byte[] { 0xFF, 0xE1, 0x02, 0x01, 0x0C, 0xA0, 0xFC, 0xFC, 0xA0, 0xFD, 0x04, 0xA0, 0xFE, 0x04, 0xA0, 0xFF, 0x04 };
        //private byte[] test_2 = new byte[] { 0xFF, 0xE1, 0x02, 0x01, 0x0C, 0xA0, 0xFC, 0x20, 0xA0, 0xFD, 0x04, 0xA0, 0xFE, 0x01, 0xA0, 0xFF, 0x01 };
        // the hexadecimal code to check card type.
        private byte TAG_CARD_TYPE = 0X50;
        private byte[] Cmd_Select_Aid_VISA = new byte[] { 0x00, 0xA4, 0x04, 0x00, 0x07, 0xA0, 0x00, 0x00, 0x00, 0x03, 0x10, 0x10, 0x00 };
        private byte[] Cmd_Select_Aid_MASTER = new byte[] { 0x00, 0xA4, 0x04, 0x00, 0x07, 0xA0, 0x00, 0x00, 0x00, 0x04, 0x10, 0x10, 0x00 };
        // the context of a card.
        private SCardContext _context = new SCardContext();
        private static int controlCode = 0x003136b0;
        private IntPtr IOCTL_SCL2000_CTL = new IntPtr(controlCode);
        // the port of smart card reader.
        private string _port;
        private string _uid;
        // the timer to keep detect card.
        private System.Timers.Timer _timer;
        private object locker = new object();

        public SmartCardReaderHelper()
        {
            // get the smart card reader.
            _context.Establish(SCardScope.System);
            string[] _adpuPorts = _context.GetReaders();
            _port = _adpuPorts[0];
            _context.Release();
            Thread.Sleep(1000);
            // change the smart card reader LED status to LED blue on.
            _dcmd(Led_Blue_On);
            status = 0;
            // initialize the timer
            _timer = new System.Timers.Timer();
            _timer.Interval = 50;
            _timer.Elapsed += _timerElapsed;
            
        }
        public void Dispose()
        {
            _context.Release();
            _context.Dispose();

        }

        private void _timerElapsed(object _sender, System.Timers.ElapsedEventArgs _e)
        {
            // syncronized the detect code.
            lock (locker)
            {
                try
                {
                    // get the real smart card reader reads.
                    _uid = ReadCardUID();
                    // if the LED is blue and there is card access reader.
                    if (status == 0 && _uid != string.Empty)
                    {
                        // change the smart card reader LED status to LED Green on.
                        if (status != 1)
                        {
                            // set the status to LED_Green_On_SE
                            status = 1;
                            Console.WriteLine(_uid);
                            _dcmd(Led_Green_On_SE);
                            // thread sleep to wait for reader connection closed
                            Thread.Sleep(150);
                        }
                        
                    }
                    // if the card removed from reader.
                    else if (_uid == string.Empty)
                    {
                        // set the status to LED_Blue_On
                        status = 0;
                        Console.WriteLine("No Card!!");
                        // change the smart card reader LED status to LED blue on.
                        _dcmd(Led_Blue_On);
                        // thread sleep to wait for reader connection closed
                        Thread.Sleep(150);
                        
                    }
                }
                catch (PCSCException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public static bool CheckError(SCardError _error)
        {
            // if there is error, then return true.
            if (_error != SCardError.Success)
            {
                return true;
            }
            return false;
        }
        public static string ToHexString(byte[] _target)
        {
            string _hexString = string.Empty;
            // if target is not null.
            if (_target != null)
            {
                // use string builder to transfer the hexadecimal to decimal.
                StringBuilder _sb = new StringBuilder();
                for (int i = 0; i < _target.Length; ++i)
                {
                    _sb.Append(_target[i].ToString("X2"));
                }
                _hexString = _sb.ToString();
            }
            return _hexString;
        }

        public string ReadCardUID()
        {
            // initialize the reader.
            SCardReader _reader = new SCardReader(_context);
            // connect to the reader, protocol not limited.
            SCardError _error = _reader.Connect(_port,
                SCardShareMode.Shared,
                SCardProtocol.T0 | SCardProtocol.T1);
            if (CheckError(_error))
            {
                return string.Empty;
            }
            IntPtr _pioSendPci;
            // according to the active protocol to set the request structure.
            switch (_reader.ActiveProtocol)
            {
                case SCardProtocol.T0:
                    _pioSendPci = SCardPCI.T0;
                    break;
                case SCardProtocol.T1:
                    _pioSendPci = SCardPCI.T1;
                    break;
                default:
                    throw new PCSCException(SCardError.ProtocolMismatch, 
                        "Protocol Not Supported: " + _reader.ActiveProtocol.ToString());
            }
            byte[] _pbRecvBuffer = new byte[256];
            // transmit the data fron reader.
            _error = _reader.Transmit(_pioSendPci, Get_ID, ref _pbRecvBuffer);
            string _result = ToHexString(_pbRecvBuffer);
            if (CheckError(_error))
            {
                return string.Empty;
            }
            return _result;
        }

        private void _dcmd(byte[] _name)
        {
            try
            {
                // initialize the reader.
                SCardReader _reader = new SCardReader(_context);
                _context.Establish(SCardScope.System);
                // connect to the reader, protocol not limited.
                SCardError _error = _reader.Connect(_port,
                    SCardShareMode.Direct,
                    SCardProtocol.Unset);
                if (CheckError(_error))
                {
                    throw new PCSCException(_error);
                }
                byte[] _pbRecvBuffer = new byte[256];
                // use the hexadecimal code to control the LED.
                _error = _reader.Control(IOCTL_SCL2000_CTL, _name, ref _pbRecvBuffer);
                if (CheckError(_error))
                {
                    throw new PCSCException(_error);
                }
                string _result = ToHexString(_pbRecvBuffer);
                // disconnect the reader.
                _reader.Disconnect(SCardReaderDisposition.Leave);
            }
            catch (PCSCException ex)
            {
                Console.WriteLine(ex.Message + " (" + ex.SCardError.ToString() + ") ");
                
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
