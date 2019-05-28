namespace ReportBuilderAPI.Helpers
{
    [System.Serializable]
    public class PermissionManager
    {
        private readonly long _perm;
        public PermissionManager(long perm) { _perm = perm; }
        public virtual bool Contains(object perm) { return (_perm & (long)perm) == (long)perm; }
        public long Value => _perm;
    }
}
