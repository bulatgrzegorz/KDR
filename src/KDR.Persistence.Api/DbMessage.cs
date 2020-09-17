using System;

namespace KDR.Persistence.Api
{
    public class DbMessage
    {
        //TODO: przejdziemy chyba ostatecznie na long'a generowango jakimś snowflake id - lżej, szybciej i wydajniej
        public Guid Id { get; set; }

        public string Content { get; set; }

        public string Headers { get; set; }

        public DateTime Created { get; set; }

        public DateTime? ExpiresAt { get; set; }

        public DateTime? SendDate { get; set; }

        public int Retries { get; set; }
    }
}