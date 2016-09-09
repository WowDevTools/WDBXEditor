using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WDBXEditor
{
    public class NamedPipeManager
    {
        public string NamedPipeName = "WDBXEditorPipe";
        public event Action<string> ReceiveString;

        private const string EXIT_STRING = "[EXIT]";
        private List<BackgroundWorker> Workers = new List<BackgroundWorker>();

        /// <summary>
        /// Starts a pipe server on a new backgroundworker
        /// </summary>
        public void StartServer()
        {
            BackgroundWorker bw = new BackgroundWorker();
            Workers.Add(bw);
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += Bw_DoWork;
            bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
            bw.RunWorkerAsync();
        }

        /// <summary>
        /// Creates a new pipe server that awaits a message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            using (var server = new NamedPipeServerStream(NamedPipeName, PipeDirection.InOut, -1))
            {
                try
                {
                    server.WaitForConnection();
                    using (StreamReader reader = new StreamReader(server))
                        e.Result = reader.ReadToEnd();
                }
                catch { e.Result = ""; }
            }
        }

        /// <summary>
        /// Returns the message recieved and spawns a new server disposing of the previous backgroundworker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string result = e.Result as string;
            if (result != EXIT_STRING)
            {
                StartServer(); //If not exit then start a new server instance
                ReceiveString?.Invoke(result); //Send signal we've received a new file to open
            }

            //Dispose background worker
            BackgroundWorker bw = (sender as BackgroundWorker);
            bw.RunWorkerCompleted -= Bw_RunWorkerCompleted;
            bw.DoWork -= Bw_DoWork;
            Task.Run(() => bw.Dispose());
        }

        /// <summary>
        /// Shuts down all the pipe servers
        /// </summary>
        public void StopServer()
        {
            for (int i = 0; i < Workers.Count; i++)
            {
                BackgroundWorker bw = Workers[i];
                Write(EXIT_STRING); //Send all pipes the exit command
            }

            Thread.Sleep(100);
            Workers.Clear();
        }

        public bool Write(string text, int connectTimeout = 250)
        {
            using (var client = new NamedPipeClientStream(NamedPipeName))
            {
                try
                {
                    client.Connect(connectTimeout);
                    if (!client.IsConnected)
                        return false;

                    using (StreamWriter writer = new StreamWriter(client))
                    {
                        writer.Write(text);
                        writer.Flush();
                    }
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }
    }
}
