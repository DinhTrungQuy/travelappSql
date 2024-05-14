using TravelAppAPI.Infrastructure;

namespace TravelAppAPI.Sevices
{
    public class FileServices
    {

        public async Task<string> SavePlaceFile(IFormFile file, string fileId)
        {
            if (file == null || file.Length == 0)
                return "Invalid file";
            var filePath = Path.Combine("D:\\Publish\\IIS\\quydt.speak.vn_Images\\places", fileId + Path.GetExtension(file.FileName));
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                byte[] byteArray;
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    byteArray = memoryStream.ToArray();
                }
                string imageFormat = ImageDetection.GetImageFormat(byteArray).ToString();
                if (imageFormat == "UNKNOWN")
                {
                    return "Invalid file";
                }
                else
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            return filePath;

        }
        public async Task<string> SaveUserFile(IFormFile file, string fileId)
        {
            if (file == null || file.Length == 0)
                return "Invalid file";
            var filePath = Path.Combine("D:\\Publish\\IIS\\quydt.speak.vn_Images\\users", fileId + Path.GetExtension(file.FileName));
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                byte[] byteArray;
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    byteArray = memoryStream.ToArray();
                }
                string imageFormat = ImageDetection.GetImageFormat(byteArray).ToString();
                if (imageFormat == "UNKNOWN")
                {
                    return "Invalid file";
                }
                else
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            return filePath;

        }
        public void DeletePlaceFile(string path)
        {
            var filePath = Path.Combine("D:\\Publish\\IIS\\quydt.speak.vn_Images\\places", path);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        public void DeleteUserFile(string path)
        {
            var filePath = Path.Combine("D:\\Publish\\IIS\\quydt.speak.vn_Images\\users", path);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        //public void DeleteFile(string fileName)
        //{
        //    var filePath = Path.Combine(_folder, fileName);
        //    if (File.Exists(filePath))
        //    {
        //        File.Delete(filePath);
        //    }
        //}
    }
}
