namespace FoodSnap.Web.Envelopes
{
    public class DataEnvelope
    {
        public DataEnvelope(object data)
        {
            Data = data;
        }

        public object Data { get; }
    }
}
