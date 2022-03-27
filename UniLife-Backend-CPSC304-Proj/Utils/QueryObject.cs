namespace UniLife_Backend_CPSC304_Proj.Utils
{
    public class QueryObject: ICloneable
    {
        public enum QueryType
        {
            Delete,
            Update,
            Insert,
            Select
        }

        QueryType queryType;
        QueryType ObjQueryType { get { return queryType; } }

        public QueryObject(QueryType queryType)
        {
            this.queryType = queryType;
        }

        public object Clone()
        {
            return new QueryObject(queryType);
        }
    }
}
