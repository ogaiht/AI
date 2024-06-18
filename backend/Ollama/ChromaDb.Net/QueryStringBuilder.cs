using System.Text;

namespace ChromaDb.Net
{
    public class QueryStringBuilder
    {
        private readonly StringBuilder _builder = new StringBuilder();
        
        public QueryStringBuilder() { }

        public QueryStringBuilder Add(string key, string value) 
        {
            if (_builder.Length > 0) 
            {
                _builder.Append('&');
            }
            _builder
                .Append(key)
                .Append('=')
                .Append(value);
            return this;
        }

        public override string ToString()
        {
            return _builder.ToString();
        }

        public void Clear()
        {
            _builder.Clear();
        }
    }
}
