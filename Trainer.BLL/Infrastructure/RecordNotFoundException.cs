using System;

namespace Trainer.BLL.Infrastructure
{
    public class RecordNotFoundException : Exception
    {
        public string Property { get; protected set; }
        public RecordNotFoundException(string message) : base(message)
        {
        }
    }
}
