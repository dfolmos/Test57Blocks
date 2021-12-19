using System.Collections.Generic;

namespace Test57Blocks.DTO
{
    public class TransactionResultDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public List<string> Errors { get; set; }
        public TransactionResultDTO()
        {
            this.Errors = new List<string>();
        }
    }
}
