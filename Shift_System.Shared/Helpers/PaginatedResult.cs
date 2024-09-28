
//namespace Shift_System.Shared.Helpers
//{
//   public class PaginatedResult<T> : Result<T>
//   {
//      public PaginatedResult(List<T> data)
//      {
//         Data = data;
//      }
//      public PaginatedResult(bool succeeded, List<T> data = default, List<string> messages = null, int count = 0, int pageNumber = 1, int pageSize = 10)
//      {
//         Data = data;
//         CurrentPage = pageNumber;
//         Succeeded = succeeded;
//         Messages = messages;
//         PageSize = pageSize;
//         TotalPages = (int)Math.Ceiling(count / (double)pageSize);
//         TotalCount = count;
//      }
//      public new List<T> Data { get; set; }
//      public int CurrentPage { get; set; }
//      public int TotalPages { get; set; }
//      public int TotalCount { get; set; }
//      public int PageSize { get; set; }
//      public bool HasPreviousPage => CurrentPage > 1;
//      public bool HasNextPage => CurrentPage < TotalPages;
//      public static PaginatedResult<T> Create(List<T> data, int count, int pageNumber, int pageSize)
//      {
//         return new PaginatedResult<T>(true, data, null, count, pageNumber, pageSize);
//      }
//   }
//}


namespace Shift_System.Shared.Helpers
{
    public class PaginatedResult<T> : Result<T>
    {
        public PaginatedResult(List<T> data)
        {
            Data = data;
            CurrentCount = data.Count; // Mevcut sayfadaki kayıt sayısı
        }

        public PaginatedResult(bool succeeded, List<T> data = default, string message = null, int count = 0, int pageNumber = 1, int pageSize = 10)
        {
            Data = data;
            CurrentPage = pageNumber;
            Succeeded = succeeded;
            Message = message; // Tek bir mesaj alanı
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
            CurrentCount = data?.Count ?? 0; // Mevcut sayfadaki kayıt sayısı
        }

        public new List<T> Data { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentCount { get; set; } // Mevcut sayfadaki kayıt sayısı
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
        public string Message { get; set; } // Tek bir mesaj döndürülüyor

        // Statik SuccessAsync metodu
        public static PaginatedResult<T> SuccessAsync(List<T> data, int totalCount, int pageNumber, int pageSize, string message)
        {
            return new PaginatedResult<T>(true, data, message, totalCount, pageNumber, pageSize);
        }

        public static PaginatedResult<T> Create(List<T> data, int count, int pageNumber, int pageSize)
        {
            return new PaginatedResult<T>(true, data, null, count, pageNumber, pageSize);
        }
    }
}
