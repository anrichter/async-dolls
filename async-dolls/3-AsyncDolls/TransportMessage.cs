using System;
using System.Collections.Generic;
using System.IO;

namespace AsyncDolls
{
    public class TransportMessage
    {
        Stream body;

        public TransportMessage()
        {
            var id = CombGuid.Generate().ToString();

            Headers = new Dictionary<string, string>
            {
                {HeaderKeys.MessageId, id},
                {HeaderKeys.CorrelationId, id},
                {HeaderKeys.ContentType, null},
                {HeaderKeys.ReplyTo, null},
                {HeaderKeys.MessageType, null},
                {HeaderKeys.MessageIntent, null}
            };
        }

        //public TransportMessage(BrokeredMessage message)
        //{
        //    Headers = new Dictionary<string, string>
        //    {
        //        {HeaderKeys.MessageId, message.MessageId},
        //        {HeaderKeys.CorrelationId, message.CorrelationId},
        //        {HeaderKeys.MessageType, message.ContentType},
        //        {HeaderKeys.ReplyTo, message.ReplyTo}
        //    };

        //    this.message = message;

        //    foreach (var pair in message.Properties)
        //    {
        //        if (!Headers.ContainsKey(pair.Key))
        //        {
        //            Headers.Add(pair.Key, (string) pair.Value);
        //        }
        //    }
        //}

        public string Id
        {
            get { return Headers[HeaderKeys.MessageId]; }
        }

        public string CorrelationId
        {
            get { return Headers[HeaderKeys.CorrelationId]; }
            set { Headers[HeaderKeys.CorrelationId] = value; }
        }

        public string ContentType
        {
            get { return Headers[HeaderKeys.ContentType]; }
            set { Headers[HeaderKeys.ContentType] = value; }
        }

        public string MessageType
        {
            get { return Headers[HeaderKeys.MessageType]; }
            set { Headers[HeaderKeys.MessageType] = value; }
        }

        public MessageIntent MessageIntent
        {
            get
            {
                MessageIntent messageIntent;
                string messageIntentString = Headers[HeaderKeys.MessageIntent];
                Enum.TryParse(messageIntentString, true, out messageIntent);
                return messageIntent;
            }

            set { Headers[HeaderKeys.MessageIntent] = value.ToString(); }
        }

        public Queue ReplyTo
        {
            get { return (Queue) Headers[HeaderKeys.ReplyTo].Parse(); }
            set { Headers[HeaderKeys.ReplyTo] = value.ToString(); }
        }

        public virtual int DeliveryCount
        {
            get { return 0; }
        }

        public IDictionary<string, string> Headers { get; private set; }

        public Stream Body
        {
            get { return body ?? (body = new MemoryStream()); }
        }

        public void SetBody(Stream body)
        {
            if (this.body != null)
            {
                throw new InvalidOperationException("Body is already set.");
            }

            this.body = body;
        }

        //public BrokeredMessage ToBrokeredMessage()
        //{
        //    var brokeredMessage = new BrokeredMessage(body, false)
        //    {
        //        ContentType = MessageType,
        //        MessageId = Id,
        //        CorrelationId = CorrelationId,
        //        ReplyTo = ReplyTo != null ? ReplyTo.ToString() : null
        //    };

        //    foreach (KeyValuePair<string, string> pair in Headers)
        //    {
        //        brokeredMessage.Properties.Add(pair.Key, pair.Value);
        //    }

        //    return brokeredMessage;
        //}

        //public Task DeadLetterAsync()
        //{
        //    var deadLetterHeaders = Headers.Where(x => x.Key.StartsWith(HeaderKeys.FailurePrefix, StringComparison.InvariantCultureIgnoreCase))
        //        .Select(x => x)
        //        .ToDictionary(x => x.Key, x => (object) x.Value);

        //    return DeadLetterAsyncInternal(deadLetterHeaders);
        //}

        //protected virtual Task DeadLetterAsyncInternal(IDictionary<string, object> deadLetterHeaders)
        //{
        //    return message.DeadLetterAsync(deadLetterHeaders);
        //}
    }
}