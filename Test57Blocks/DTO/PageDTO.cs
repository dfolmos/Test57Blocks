namespace Test57Blocks.DTO
{
    public class PageDTO
    {
        public int Page { get; set; } = 1;
        private int recordsByPage = 10;
        private readonly int maxRecordsByPage = 50;
        public int RecordsByPage
        {
            get
            {
                return recordsByPage;
            }
            set
            {
                recordsByPage = (value > maxRecordsByPage) ? maxRecordsByPage : value;
            }
        }
    }
}
