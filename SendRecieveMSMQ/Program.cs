using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;
using Experimental.System.Messaging;
using SendRecieveMSMQ;

namespace SendReceiveMSMQ
{
   
    class Program
    {
        static void Main(string[] args)
        {
            //path of queue
            const string queueName = @".\private$\Mail";

            //create event Messagelister object 
            MSMQListner msmq = new MSMQListner(queueName);

            //message formatterType is string
            msmq.FormatterTypes = new Type[] { typeof(string) };
            
            //call to clickEvent MessaggeReceivedEventHandler method  
            msmq.MessageReceived += new MessageReceivedEventHandler(listener_MessageReceived);
            msmq.Start();
          
            //FireCommand(queueName);
            Console.ReadLine();
        }

        static void listener_MessageReceived(object sender, MessageEventArgs args)
        {
            Console.WriteLine("received");
        }
    }

    //delegate method MessageReceivedEventHandler
    public delegate void MessageReceivedEventHandler(object sender, MessageEventArgs args);

    public class MSMQListner
    {
        private bool _listen;
        private Type[] _types;
        private MessageQueue _queue;

        public event MessageReceivedEventHandler MessageReceived;

        public Type[] FormatterTypes
        {
            get { return _types; }
            set { _types = value; }
        }

        public MSMQListner(string queuePath)
        {
            _queue = new MessageQueue(queuePath);
        }

        public void Start()
        {
            _listen = true;

            if (_types!=null && _types.Length>0)
            {
                //Using only the XmlMessageFormatter. You can use other formatter as well
                _queue.Formatter = new XmlMessageFormatter(_types);
            }

            //_queue.PeekCompleted += new PeekCompletedEventHandler(OnPeekCompleted);
            _queue.ReceiveCompleted += new ReceiveCompletedEventHandler(OnReceiveCompleted);

            StartListening();
        }

        public void Stop()
        {
            _listen = false;
            _queue.ReceiveCompleted -= new ReceiveCompletedEventHandler(OnReceiveCompleted);

        }
    
        private void StartListening()
        {
            if (!_listen)
            {
                return;
            }

            // The MSMQ class does not have a BeginRecieve method that can take in a 
            // MSMQ transaction object. This is a   – we do a BeginPeek and then 
            // recieve the message synchronously in a transaction.
            // Check documentation for more details
            if (_queue.Transactional)
            {
                _queue.BeginPeek();
            }
            else
            {
                _queue.BeginReceive();
            }
        }

        private void FireRecieveEvent(object body)
        {
            if (MessageReceived != null)
            {
                MessageReceived(this, new MessageEventArgs(body));
            }
        }
       
            private void OnReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
            {
                Message msg = _queue.EndReceive(e.AsyncResult);
            
                Console.WriteLine("MEssage:::" + msg.Body);
                EmilService emailService = new EmilService();
                emailService.EmailService(msg.Body.ToString(), msg.Label);

                StartListening();

                FireRecieveEvent(msg.Body);
            }
        }
    }

    public class MessageEventArgs : EventArgs
    {
        private object _messageBody;

        public object MessageBody
        {
            get { return _messageBody; }
        }

        public MessageEventArgs(object body)
        {
            _messageBody = body;

        }
    }