using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using AtTask.OutlookAddIn.Domain.Model;
using AtTask.OutlookAddIn.Utilities;

namespace AtTask.OutlookAddIn.StreamApi.Connector.Service
{
    public class SolrSearchBuilder<TDomain> where TDomain : EntityBase, new()
    {
        private string _queryString;
        private readonly FieldCollector<TDomain> _fieldCollector;
        private readonly FilterQuery<TDomain> _filterQuery;

        public SolrSearchBuilder()
        {
            _fieldCollector = new FieldCollector<TDomain>();
            _filterQuery = new FilterQuery<TDomain>(_fieldCollector);
        }

        public List<StringPair> GetCriteria()
        {
            var pairs = new List<StringPair>
            {
                new StringPair("query", _queryString),
                new StringPair("searchOptions", GetSearchOptions())
            };

            return pairs;
        }

        public int PageSize { get; set; }
        public int PageNumber { get; set; }

        private string GetSearchOptions()
        {
            var fields = _fieldCollector.GetString();
            var filterQueries = _filterQuery.GetString();
            return "{" + string.Join(",", new[] {fields, filterQueries, PagingString}.Where(s => !string.IsNullOrWhiteSpace(s))) + "}";
        }

        private string PagingString
        {
            get { return PageSize == 0 ? "" : string.Format("pageSize:{0},pageNumber:{1}", PageSize, PageNumber); }
        }

        public FilterQuery<TDomain> QueryString(string queryString)
        {
            queryString = StringUtil.EscapeSolrString(queryString);
            _queryString = WebUtil.EncodeJson(queryString);
            return _filterQuery;
        }
    }

    public class FieldCollector<TDomain> where TDomain : EntityBase, new()
    {
        private readonly List<MemberInfo> _infos;

        public FieldCollector()
        {
            _infos = new List<MemberInfo>();
        }

        internal string GetString()
        {
            var objCode = ReflectionUtil.GetConstValue(typeof (TDomain), "ObjCodeString");
            return "fields:{" + objCode + ":['" + string.Join("','", _infos.Distinct().Select(i => StringUtil.CamelCaseName(i.Name))) + "']}";
        }

        public FieldCollector<TDomain> WithFields<TProperty>(Expression<Func<TDomain, TProperty>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException("");
            }

            var memberInfo = memberExpression.Member;
            _infos.Add(memberInfo);
            return this;
        }
    }

    public class FilterQuery<TDomain> where TDomain : EntityBase, new()
    {
        private readonly FieldCollector<TDomain> _fieldCollector;
        private readonly List<string> _filterQueriesAnd;
        private readonly List<string> _filterQueriesOr;

        internal FilterQuery(FieldCollector<TDomain> fieldCollector)
        {
            _fieldCollector = fieldCollector;
            var objCode = ReflectionUtil.GetConstValue(typeof(TDomain), "ObjCodeString");
            _filterQueriesAnd = new List<string> { "objCode:" + objCode};
            _filterQueriesOr = new List<string>();
        }

        public FilterQuery<TDomain> WithFilter(string fieldName, string filterString, string endWildcard = null)
        {
            if(!string.IsNullOrWhiteSpace(filterString))
            {
                var escapedString = WebUtil.EncodeJson(StringUtil.EscapeSolrString(filterString));
                _filterQueriesOr.Add(fieldName + ":" + escapedString + endWildcard);
            }

            return this;
        }

        public FieldCollector<TDomain> WithFields<TProperty>(Expression<Func<TDomain, TProperty>> expression)
        {
            _fieldCollector.WithFields(expression);
            return _fieldCollector;
        }

        internal string GetString()
        {
            var orQueries = "'" + string.Join(" ", _filterQueriesOr.Where(s => !string.IsNullOrWhiteSpace(s))) + "'";
            var andrQueries = "'" + string.Join("','", _filterQueriesAnd.Where(s => !string.IsNullOrWhiteSpace(s))) + "'";

            return "filterQueries:[" + andrQueries + (orQueries == "''" ? "" : "," + orQueries) + "]";
        }
    }
}