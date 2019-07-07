using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozack
{
    public interface IOzackContext : IDisposable
    {
        void Add<T>(string key, T obj);
        T Get<T>( string key );
    }
    /// <summary>
    /// データコンテナ
    /// </summary>
    public class OzackContext : IOzackContext
    {
        //=========================================
        // 変数
        //=========================================
        private Dictionary<string,object> m_context = new Dictionary<string, object>();

        //=========================================
        // 関数
        //=========================================

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            m_context.Clear();
            DoDispsoe();
        }
        protected virtual void DoDispsoe() { }

        /// <summary>
        /// 取得
        /// </summary>
        public T Get<T>( string key )
        {
            if (m_context.TryGetValue(key, out var obj))
            {
                if (obj is T v)
                {
                    return v;
                }
            }
            return default;
        }

        public void Add<T>(string key, T obj)
        {
            m_context.Add(key, obj);
        }
    }
}
