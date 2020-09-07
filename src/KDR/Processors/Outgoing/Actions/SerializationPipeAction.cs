using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KDR.Abstractions.Messages;
using KDR.Messages;
using KDR.Persistence.Api;
using KDR.Serialization;
using Newtonsoft.Json;

namespace KDR.Processors.Outgoing.Actions
{
    public class SerializationPipeAction : IOutgoingPipeAction
    {
        private readonly ISerializer _serializer;

        public SerializationPipeAction(ISerializerFactory serialization)
        {
            //TODO: Tutaj pewnie będzie potrzebna jakaś konfiguracja ogólna albo na okreslone eventy jak mają być serializowane
            //jeśli taki event będzie wysyłany od nas i szedł do KCF to może być potrzeba innej serializacji
            _serializer = serialization.Create(ContentTypes.JsonUtf8ContentType);
        }

        public async Task ExecuteAsync(OutgoingPipelineContext ctx, Func<Task> next)
        {
            var message = ctx.Load<Message>();

            ctx.Save(new DbMessage()
            {
                Content = JsonConvert.SerializeObject(message)
            });

            await next();
        }
    }
}