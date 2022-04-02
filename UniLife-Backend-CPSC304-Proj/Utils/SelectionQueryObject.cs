using System.Data.Common;

namespace UniLife_Backend_CPSC304_Proj.Utils
{
    public class SelectionQueryObject<T> : QueryObject
    {
        public string SelectClauseContent { get; set; } = "";
        public string FromClauseContent { get; set; } = "";
        public string? WhereClauseContent { get; set; } = null;
        public string? GroupByClauseContent { get; set; } = null;
        public string? HavingClauseContent { get; set; } = null;
        public string? OrderByClauseContent { get; set; } = null;

        public bool IsDistinct { get; set; } = false;
        public bool IsAscending { get; set; } = false;

        public Func<DbDataReader, T> QueryMapFunction { get; set; }

        public SelectionQueryObject(Func<DbDataReader, T> queryMapFunction,
            string selectClause = "",
            string fromClause = "",
            string ? whereClause = null,
            string ? groupByClause = null,
            string ? havingClause = null,
            string ? orderByClauseContent = null,
            bool isDistinct = false,
            bool isAscending = false) : base(QueryType.Select)
        {
            this.SelectClauseContent = selectClause;
            this.FromClauseContent = fromClause;
            this.WhereClauseContent = whereClause;
            this.GroupByClauseContent = groupByClause;
            this.HavingClauseContent = havingClause;
            this.OrderByClauseContent = orderByClauseContent;
            this.IsDistinct = isDistinct;
            this.IsAscending = isAscending;

            this.QueryMapFunction = queryMapFunction;
        }

        public SelectionQueryObject<T> Select(string selectContent)
        {
            SelectClauseContent = selectContent;
            return this;
        }

        public SelectionQueryObject<T> From(string fromContent)
        {
            FromClauseContent = fromContent;
            return this;
        }

        public SelectionQueryObject<T> Where(string whereContent)
        {
            WhereClauseContent = whereContent;
            return this;
        }

        public SelectionQueryObject<T> GroupBy(string groupByContent)
        {
            GroupByClauseContent = groupByContent;
            return this;
        }

        public SelectionQueryObject<T> Having(string havingContent)
        {
            HavingClauseContent = havingContent;
            return this;
        }

        public SelectionQueryObject<T> OrderBy(string content)
        {
            OrderByClauseContent = content;
            return this;
        }

        public SelectionQueryObject<T> SetIsDistinct(bool isDistinct = true)
        {
            IsDistinct = isDistinct;
            return this;
        }

        public SelectionQueryObject<T> SetIsAscending(bool isAsc = true)
        {
            IsAscending = isAsc;
            return this;
        }

        public void AddToWhereClause(string whereContent)
        {
            if (WhereClauseContent != null)
            {
                WhereClauseContent += " " + whereContent;
            }
            else
            {
                WhereClauseContent = whereContent;
            }
        }

        public string SqlQuery()
        {
            string distinctSrting = IsDistinct ? "Distinct" : "";

            string query = $"Select {distinctSrting} {SelectClauseContent} " +
                $"From {FromClauseContent} ";
            if (WhereClauseContent != null) query += $"Where {WhereClauseContent} ";
            if (GroupByClauseContent != null) query += $"Group By {GroupByClauseContent} ";
            if (HavingClauseContent != null) query += $"Having {HavingClauseContent} ";
            if (OrderByClauseContent != null) query += $"Order By {OrderByClauseContent} ";
            if (OrderByClauseContent != null) query += (IsAscending? "ASC": "DESC") + " ";

            return query;
        }

        public string FormattedSqlQuery()
        {
            string distinctSrting = IsDistinct ? "Distinct" : "";

            string query = $"Select {distinctSrting} {SelectClauseContent}\n" +
                $"From {FromClauseContent}\n";
            if (WhereClauseContent != null) query += $"Where {WhereClauseContent}\n";
            if (GroupByClauseContent != null) query += $"Group By {GroupByClauseContent}\n";
            if (HavingClauseContent != null) query += $"Having {HavingClauseContent}\n";
            if (OrderByClauseContent != null) query += $"Order By {OrderByClauseContent} ";
            if (OrderByClauseContent != null) query += (IsAscending ? "ASC" : "DESC") + "\n";

            return query;
        }

        public new object Clone()
        {
            SelectionQueryObject<T> clone = new SelectionQueryObject<T>(QueryMapFunction);
            clone.SelectClauseContent = SelectClauseContent;
            clone.FromClauseContent = FromClauseContent;
            clone.WhereClauseContent = WhereClauseContent;
            clone.GroupByClauseContent = GroupByClauseContent;
            clone.HavingClauseContent = HavingClauseContent;
            clone.OrderByClauseContent = OrderByClauseContent;
            clone.IsAscending = IsAscending;
            clone.IsDistinct = IsDistinct;
            return clone;
        }

        public SelectionQueryObject<U> CloneTo<U>(Func<DbDataReader, U> newMapFunc)
        {
            SelectionQueryObject<U> clone = new SelectionQueryObject<U>(newMapFunc);
            clone.SelectClauseContent = SelectClauseContent;
            clone.FromClauseContent = FromClauseContent;
            clone.WhereClauseContent = WhereClauseContent;
            clone.GroupByClauseContent = GroupByClauseContent;
            clone.HavingClauseContent = HavingClauseContent;
            clone.OrderByClauseContent = OrderByClauseContent;
            clone.IsAscending = IsAscending;
            clone.IsDistinct = IsDistinct;
            return clone;
        }

        public override string ToString()
        {
            return this.SqlQuery();
        }
    }
}
