namespace GameZone.Setting
{
    public  static class FileSettings
    {
        public const string ImagePath = "/assets/images/Games";
        public const string AllowedExtentions = ".jpg,.jpeg,.png";
        public const int MaxFileSizeInMB = 1;
        public const int MaxFileSizeInBytes = MaxFileSizeInMB*1024*1024;
    }
}
