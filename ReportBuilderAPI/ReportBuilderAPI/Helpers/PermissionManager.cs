using System;
using System.Collections.Generic;
using System.Text;

namespace ReportBuilderAPI.Helpers
{
    [System.Serializable]
    public class PermissionManager
    {
        long _perm;
        public PermissionManager(long perm) { _perm = perm; }
        public virtual bool Contains(object perm) { return (_perm & (long)perm) == (long)perm; }
        public long Value
        {
            get { return _perm; }
        }
    }
}
