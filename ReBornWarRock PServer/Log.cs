using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace ReBornWarRock_PServer
{
    class Log
    {
        ~Log()
        {
            GC.Collect();
        }
        private static StreamWriter _LogFile;
        private static StreamWriter _LogPackets;
        private static StreamWriter _GSetting;

        public static bool setupPackets(string logFile)
        {
            try
            {
                _LogPackets = new StreamWriter(logFile, true);
                _LogPackets.WriteLine("Start up: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString());
                _LogPackets.Flush();
            }
            catch { }

            return false;
        }

        public static bool setup(string logFile)
        {
            try
            {
                _LogFile = new StreamWriter(logFile, true);
                _LogFile.WriteLine("Start up: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString());
                _LogFile.Flush();
            }
            catch { }

            return false;
        }
        public static bool GSetup(string logFile)
        {
            try
            {
                _GSetting = new StreamWriter(logFile, true);
                _GSetting.Flush();
            }
            catch { }

            return false;
        }

        public static void setTitle(string sTitle)
        {
            Console.Title = sTitle;
        }

        public static void closeLog()
        {
            try { if (_LogFile != null) { _LogFile.Close(); } }
            catch { }
        }


        public static void AppendText(String text)
        {
            try
            {
                //Program.mainForm.textBox1.AppendText(text.ToString() + Environment.NewLine);
                DateTime _DTN = DateTime.Now;
                StackFrame _SF = new StackTrace().GetFrame(2);
                //FormCalling.frm1.richTextBox1.SelectionColor = System.Drawing.Color.Black;
                FormCalling.frm1.AppendTextBox(text);
                if (_LogFile != null)
                {
                    _LogFile.WriteLine("[" + _DTN.ToShortDateString() + " " + _DTN.ToLongTimeString() + "] [" + _SF.GetMethod().ReflectedType.Name + "." + _SF.GetMethod().Name + "] » " + text);
                    _LogFile.Flush();
                }
            }
            catch (Exception ex)
            {
                Log.AppendError(ex.Message);
            }

        }
        public static void AppendError(String text)
        {
            try
            {
                //Program.mainForm.textBox1.AppendText(text.ToString() + Environment.NewLine);
                DateTime _DTN = DateTime.Now;
                StackFrame _SF = new StackTrace().GetFrame(2);
                FormCalling.frm1.richTextBox1.SelectionColor = System.Drawing.Color.Red;
                FormCalling.frm1.AppendTextBox(text);
                if (_LogFile != null)
                {
                    _LogFile.WriteLine("[" + _DTN.ToShortDateString() + " " + _DTN.ToLongTimeString() + "] [" + _SF.GetMethod().ReflectedType.Name + "." + _SF.GetMethod().Name + "] » " + text);
                    _LogFile.Flush();
                }
            }
            catch (Exception ex)
            {
                Log.AppendError(ex.Message);
            }

        }
        public static void WritePackets(String text)
        {
            try
            {
                //Program.mainForm.textBox1.AppendText(text.ToString() + Environment.NewLine);
                DateTime _DTN = DateTime.Now;
                StackFrame _SF = new StackTrace().GetFrame(2);
                FormCalling.frm6.AppendTextBox(text);
                if (_LogPackets != null)
                {
                    _LogPackets.WriteLine("[" + _DTN.ToShortDateString() + " " + _DTN.ToLongTimeString() + "] [" + _SF.GetMethod().ReflectedType.Name + "." + _SF.GetMethod().Name + "] » " + text);
                    _LogPackets.Flush();
                }
            }
            catch (Exception ex)
            {
                Log.AppendError(ex.Message);
            }

        }

        public static void WriteSetting(String text)
        {
            try
            {
                //Program.mainForm.textBox1.AppendText(text.ToString() + Environment.NewLine);
                DateTime _DTN = DateTime.Now;
                StackFrame _SF = new StackTrace().GetFrame(2);
                FormCalling.frm6.AppendTextBox(text);
                if (_GSetting != null)
                {
                    _GSetting.WriteLine(text);
                    _GSetting.Flush();
                }
            }
            catch (Exception ex)
            {
                Log.AppendError(ex.Message);
            }
        }

        public static void WriteDebug(string sMsg)
        {
            try
            {
                //Program.mainForm.textBox1.AppendText(text.ToString() + Environment.NewLine);
                DateTime _DTN = DateTime.Now;
                StackFrame _SF = new StackTrace().GetFrame(2);
                FormCalling.frm1.richTextBox1.SelectionColor = System.Drawing.Color.DarkMagenta;
                FormCalling.frm1.AppendTextBox(sMsg);
                if (_LogFile != null)
                {
                    _LogFile.WriteLine("[" + _DTN.ToShortDateString() + " " + _DTN.ToLongTimeString() + "] [" + _SF.GetMethod().ReflectedType.Name + "." + _SF.GetMethod().Name + "] » " + sMsg);
                    _LogFile.Flush();
                }
            }
            catch (Exception ex)
            {
                Log.AppendError(ex.Message);
            }
        }

        public static void WriteBlank()
        {
            FormCalling.frm1.AppendTextBox("");
        }


        public static void WriteDoss(string sMsg)
        {
            DateTime now = DateTime.Now;
            //FormCalling.frm1.richTextBox1.SelectionColor = System.Drawing.Color.DarkMagenta;
            FormCalling.frm1.AppendTextBox("[ANTI-DDOS] » " + sMsg);
            try
            {
                if (Log._LogFile != null)
                {
                    Log._LogFile.WriteLine("[" + now.ToShortDateString() + " " + now.ToLongTimeString() + "] [ANTI-DDOS] » " + sMsg);
                    ((TextWriter)Log._LogFile).Flush();
                }
            }
            catch
            {
            }
            Log.WriteBlank();
        }

        public static void WriteFile(string sMsg)
        {
            try
            {
                if (_LogFile != null)
                {
                    _LogFile.WriteLine(sMsg);
                    _LogFile.Flush();
                }
            }
            catch (Exception ex)
            {
                Log.AppendError(ex.Message);
            }
        }
    }
}

