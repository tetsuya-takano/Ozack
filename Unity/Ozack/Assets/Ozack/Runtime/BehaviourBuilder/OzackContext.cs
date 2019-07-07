using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozack
{
    public interface IOzackContext : IDisposable
    {
        void Add<T>( T obj );
        T Get<T>();
    }
    /// <summary>
    /// データコンテナ
    /// </summary>
    public class OzackContext : IOzackContext
    {
        //=========================================
        // 変数
        //=========================================
        private HashSet<object> m_context = new HashSet<object>();

        //=========================================
        // 関数
        //=========================================

        public OzackContext( params object[] contexts )
        {
            foreach (var o in contexts)
            {
                m_context.Add(o);
            }
        }

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
        public T Get<T>()
        {
            foreach (var c in m_context)
            {
                if (c is T v)
                {
                    return v;
                }
            }
            return default;
        }

        public void Add<T>(T obj)
        {
            m_context.Add(obj);
        }
    }
}
