namespace CMS.WebApp.Helper
{
    public class FileType
    {
        public static int getFileType(string filePath)
        {
            if (filePath.Contains(".mp4"))
            {
                return 6;
            }
            else if (filePath.Contains(".mp3"))
            {
                return 5;
            }
            else if (filePath.Contains(".png") || filePath.Contains(".jpg") || filePath.Contains(".jpeg") || filePath.Contains(".gif") || filePath.Contains(".bitmap"))
            {
                return 7;
            }
            else if (filePath.Contains(".pptx") || filePath.Contains(".ppt"))
            {
                return 4;
            }
            else if (filePath.Contains(".xlsx") || filePath.Contains(".xls"))
            {
                return 2;
            }
            else if (filePath.Contains(".pdf"))
            {
                return 3;
            }
            else if (filePath.Contains(".docx") || filePath.Contains(".doc"))
            {
                return 2;
            }
            return 0;
        }
        public static int getFeedbackFileType(string filePath)
        {
            if (filePath.Contains(".mp4"))
            {
                return 4;
            }
            else if (filePath.Contains(".mp3"))
            {
                return 3;
            }
            else if (filePath.Contains(".png") || filePath.Contains(".jpg") || filePath.Contains(".jpeg") || filePath.Contains(".gif") || filePath.Contains(".bitmap"))
            {
                return 2;
            }
            else if (filePath.Contains(".pptx") || filePath.Contains(".ppt"))
            {
                return 1;
            }
            else if (filePath.Contains(".xlsx") || filePath.Contains(".xls"))
            {
                return 1;
            }
            else if (filePath.Contains(".pdf"))
            {
                return 1;
            }
            else if (filePath.Contains(".docx") || filePath.Contains(".doc"))
            {
                return 1;
            }
            return 0;
        }
    }
}
