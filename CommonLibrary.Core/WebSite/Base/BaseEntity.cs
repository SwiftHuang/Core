using System.Web;
namespace hwj.CommonLibrary.WebSite.Base
{
    public abstract class BaseEntity<T> where T : class
    {
        public void SetSession()
        {
            HttpContext.Current.Session[SessionKeys] = this as T;
        }
        public void ClearSession()
        {
            HttpContext.Current.Session.Remove(SessionKeys);
        }
        public string SessionKeys
        {
            get
            {
                return this.GetType().ToString();
            }
        }
        public static void GetSessionTo(ref T entity)
        {
            if (HttpContext.Current == null || HttpContext.Current.Session[entity.GetType().ToString()] == null)
                entity = null;
            else
                entity = HttpContext.Current.Session[entity.GetType().ToString()] as T;
        }
    }
}
