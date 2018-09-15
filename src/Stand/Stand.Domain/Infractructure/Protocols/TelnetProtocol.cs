﻿using PrimS.Telnet;
using Stand.Domain.Abstract;
using Stand.Domain.Infractructure.EventArgs;
using Stand.Domain.Infractructure.Events;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Stand.Domain.Infractructure.Protocols
{
    public class TelnetProtocol : IProtocol
    {
        public event ReceivedEventHandler AnswerReceived;
        private Client _telnetClient;
        //private Parser _parser;
        //private string _invitation;
        public TelnetProtocol()
        {
            //_parser = new Parser(Properties.Resources.TelnetSwitchCommands);
        }

        public bool Connect(string host, int port)
        {
            try
            {
                _telnetClient = new Client(host, port);
            }
            catch (SocketException ex)
            {
                return false;
            }
            GetAnswer();

            return _telnetClient.IsConnected;
        }

        public void TryPassword(string password)
        {
            if (_telnetClient != null && _telnetClient.IsConnected)
            {
                var bytes = Encoding.Default.GetBytes(password);
                password = Encoding.ASCII.GetString(bytes);
                _telnetClient.WriteLine(password);
                GetAnswer(password);
            }
        }

        public void Disconnect()
        {
            if (_telnetClient != null)
            {
                _telnetClient.Dispose();
                SendAnswer("\nСоединение разорвано.\n");
            }
        }

        public void ExecuteCommand(string command)
        {
            string[] commandSplit = command.Split(':', '#');

            string currentCommand = commandSplit.ElementAtOrDefault(1);

            if (String.IsNullOrEmpty(currentCommand))
            {
                return;
            }

            if (commandSplit[0].ToLower().Contains("password"))
            {
                TryPassword(currentCommand.Trim());
            }
            else
            {
                //_invitation = commandSplit[0];
                ExecutingCommand(currentCommand.Trim());
            }
        }

        private void ExecutingCommand(string command)
        {
            if (_telnetClient != null && _telnetClient.IsConnected)
            {
                var bytes = Encoding.Default.GetBytes(command);
                command = Encoding.ASCII.GetString(bytes);

                _telnetClient.WriteLine(command);
                GetAnswer(command);
            }
        }

        private void GetAnswer(string command = "")
        {
            string answer = string.Empty;
            if (_telnetClient != null)
            {
                Thread.Sleep(100);
                do
                {
                    answer += _telnetClient.Read(new TimeSpan(1000));
                } while (String.IsNullOrEmpty(answer) || answer == command);
            }
            else
            {
                answer = "\nСервер не отвечает.\n";
            }

            answer = answer.TrimStart(command.ToCharArray());

            SendAnswer(answer);
        }

        public void SendAnswer(string message)
        {
            if (AnswerReceived != null)
            {
                var receivedEventArgs = new ReceivedEventArgs { Answer = message };
                AnswerReceived(this, receivedEventArgs);
            }
        }
    }
}
