﻿namespace Common.BL
{
    public interface IUserAccessService
    {
        void CheckUserAccess(int entityTypeId, PermissionType permission, long? entityId = null);
    }
}